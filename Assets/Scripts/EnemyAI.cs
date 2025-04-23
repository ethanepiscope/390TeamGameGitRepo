using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    // Controls the enemy AI's state (chase, patrol, attack)
    [SerializeField] private NavMeshAgent agent; 
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerCamera; 
    [SerializeField] private LayerMask whatIsGround, whatIsPlayer; 
    [SerializeField] private float fieldOfView; // Defines the field of view of the enemy AI in degrees 
    
    // Patrolling variables
    [SerializeField] private Vector3 walkPoint; // A random point that the enemy walks to 
    bool walkPointSet; 
    [SerializeField] private float walkPointRange; // Range of where the random point could be  

    // Attacking; to modify (would need to call death method) 
    [SerializeField] private float timeBetweenAttacks;
    private bool alreadyAttacked; 

    [SerializeField] private float sightRange, attackRange; 
    private bool playerInSightRange, playerInAttackRange;
    private float timeSincePlayerLastSeen = 10f; 
    private Vector3 playerLastSeenAt; 
    private PlayerInteraction playerInteraction; 
    private bool isIdle; 
    [SerializeField] private float idleTime; 
    [SerializeField] private Animator animator; 
    private bool isInRageAnim=false; 
    private bool isInAttackAnim=false;
    private GameOverLogic gameOverLogic; 
    private Candle candle; 
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; if (player==null) Debug.LogWarning("Player not found on " + transform.name);    
        agent = GetComponent<NavMeshAgent>(); if (agent==null) Debug.LogWarning("NavMeshAgent not found on " + transform.name); 
        playerInteraction = player.GetComponent<PlayerInteraction>(); if (playerInteraction==null) Debug.LogError("Player interaction null for enemy AI"); 
        Debug.LogWarning("Make sure whatIsGround, whatIsPlayer layer mask is set");
        gameOverLogic = FindFirstObjectByType<GameOverLogic>(); if (gameOverLogic==null) Debug.LogError("Game over logic found null in enemy ai"); 
        agent.updateRotation = true; 
        candle = FindFirstObjectByType<Candle>(); if (candle==null) Debug.LogError("Candle null in Enemy Ai!");
    }
    
    private void Update() { 
        if (alreadyAttacked) return; // Game over. 
        timeSincePlayerLastSeen += Time.deltaTime; 
        CheckAnims(); 
        DecideState();

    }

    void CheckAnims() { 
        isInRageAnim = animator.GetCurrentAnimatorStateInfo(0).IsName("Rage");
    }

    private void DecideState() { 
        if (isInRageAnim) { 
            agent.isStopped = true; 
            return; // Don't let any other state override movement 
        }
        else { agent.isStopped = false; }
        CheckIfPlayerVisible(); // Helper determines playerInSightRange variable 
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer); 

        // If player was recently seen (ex briefly disappeared behind a wall), enemy AI remembers last seen position 
        // and goes for that 
        bool playerRecentlySeen = timeSincePlayerLastSeen < 5f; 
        if (playerInSightRange || playerRecentlySeen && !alreadyAttacked) { 
            transform.LookAt(player); 
            if (!playerInAttackRange) ChasePlayer();
            else if (playerInSightRange) AttackPlayer(); 
        }        
        else Patrol(); 
        
    }

    /* Checks two spheres around enemy AI, checks if an object
        with layer mask player is in distance, and sets the 
        playerInSight and playerInAttackRange bools accordingly
        */
    private void CheckIfPlayerVisible() { 
        // If player is hiding, they are always invisible 
        if (playerInteraction.GetIsHiding()==true) { 
            playerInSightRange=false; 
            return; 
        }
        // Checks if player is in visible range, if not, return 
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        if (!playerInSightRange) return; // Player is not in visible range 

        // Checks if player is in enemy's field of view in front of the enemy 
        Vector3 forward = transform.forward; 
        Vector3 toPlayer = (player.position - transform.position).normalized;
        float playerAtAngle = Vector3.Angle(forward, toPlayer); 
        // forward = center line from enemy; enemy can see fieldOfView/2f to the left and right of this line 
        // if angle (always positive) is <= this, the player can be seen. 
        if (playerAtAngle <= fieldOfView/2f) playerInSightRange = true;
        else { 
            playerInSightRange=false; 
            return; 
        }     
        // Finally, raycast from enemy to player and see if there are any objects in the way (like a wall)
        RaycastHit hit; 
        if (Physics.Raycast(transform.position, toPlayer, out hit, sightRange)) { 
            if (!hit.transform.CompareTag("Player")) playerInSightRange=false;  
        }     
        if (playerInSightRange) { 
            timeSincePlayerLastSeen = 0f; 
            playerLastSeenAt = player.transform.position; 
        }
    }
    private void Patrol() {
        if (isIdle) return; // Don't patrol

        animator.SetBool("Chasing", false); 
        animator.SetBool("Idle", false); 

        animator.SetBool("Patrolling", true); 

        if (!walkPointSet) SearchWalkPoint(); 

        if (walkPointSet) { 
            // transform.LookAt(walkPoint); 
            agent.SetDestination(walkPoint);
        } 

        Vector3 distanceToWalkPoint = transform.position - walkPoint; 
        if (distanceToWalkPoint.magnitude < 0.4f) { 
            Debug.Log("Walkpoint close, walkpoint set to false"); 
            walkPointSet = false; 
            Idle(); 
        }
    }

    private void Idle() { 
        isIdle = true; 
        animator.SetBool("Chasing", false); 
        animator.SetBool("Patrolling", false);  
        animator.SetBool("Idle", true); 
        Invoke(nameof(ResetIdle), idleTime); 
        // Want to do: Idle state, play "idle" after patrol is finished - stay for 3 seconds 
            // but must be interrupted by chase or attack  

    }

    private void ResetIdle() { 
        Debug.Log("Idle reset"); 
        isIdle = false; 
        animator.SetBool("Idle", false); 
    }
    
    private void ChasePlayer() { 
        animator.SetBool("Patrolling", false); 
        animator.SetBool("Idle", false);  

        animator.SetBool("Chasing", true); 
        if (playerInSightRange) agent.SetDestination(player.position);
        else { 
            Debug.Log("Player out of sight but chasing last seen position");
            agent.SetDestination(playerLastSeenAt); 
        }
    }

    private void AttackPlayer() {
        // Debug.Log("Attacking player called");
        // Ensure enemy isn't moving
        agent.SetDestination(transform.position); 
        // Checks if they have just recently attacked; will modify later 
        if (!alreadyAttacked) { 
            // To do: insert attack code 
            Debug.Log("Attacking=true"); 
            animator.SetBool("Chasing", false); 
            animator.SetBool("Patrolling", false); 
            animator.SetBool("Idle", false); 
            animator.SetTrigger("Attacking");
            alreadyAttacked = true; 
            player.LookAt(transform); 
            playerCamera.LookAt(transform.position + Vector3.up*0.7f); 
            gameOverLogic.GameOver(); 
            // Invoke(nameof(ResetAttack), timeBetweenAttacks); 
        }
    }

    private void ResetAttack() { 
        Debug.Log("Attack reset"); 
        alreadyAttacked = false; 
    }

    /* This function searches for a random point to walk to that's within the enemy 
    AI's range, verifying if it's actually in the map 
    */
    void SearchWalkPoint() { 
        // Debug.Log("Searching for walk point"); 
        // Calculates random point in range 
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ); 
        // Raycasts from the walkpoint down to see if it hits the ground; if not, don't walk here 
        if (Physics.Raycast(walkPoint, -transform.up, 100f, whatIsGround)) {
            // Walk point is a valid point on the map 
            NavMeshPath path = new NavMeshPath();
            // Checks if there is a path to this walkpoint (might be blocked by door) 
            if (agent.CalculatePath(walkPoint, path) && path.status == NavMeshPathStatus.PathComplete){
                walkPointSet = true;
            }
        }
    }
    // Draws attack and sight range in inspector view 
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange); 
        Debug.DrawRay(walkPoint, -transform.up * 100f, Color.red); 
    }
}
