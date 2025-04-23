using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class SlideshowController : MonoBehaviour
{
    [Header("Slide Content")]
    public Sprite[] slides;
    public string[] captions;
    public Vector2[] captionPositions;
    public Color[] captionColors;
    public AudioClip[] slideSounds;
    public bool[] slideLoops;

    [Header("Audio")]
    public AudioSource musicSource; // plays background music continuously
    public AudioSource sfxSource;   // plays slide-specific SFX
    public AudioClip backgroundMusic;

    [Header("UI References")]
    public Image displayImage;
    public TMP_Text captionText;

    [Header("Effects")]
    public float typingSpeed = 0.04f;
    public float imageFadeDuration = 1f;

    private int currentSlide = 0;
    private bool isTyping = false;

    void Start()
    {
        // Start the looping background music
        if (backgroundMusic != null && musicSource != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }

        ShowSlide(currentSlide);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                captionText.text = captions[currentSlide];
                isTyping = false;
                return;
            }

            currentSlide++;

            if (currentSlide < slides.Length &&
                currentSlide < captions.Length &&
                currentSlide < captionPositions.Length &&
                currentSlide < captionColors.Length &&
                currentSlide < slideSounds.Length &&
                currentSlide < slideLoops.Length)
            {
                ShowSlide(currentSlide);
            }
            else
            {
                SceneManager.LoadScene("StartScreen"); // Replace with your actual scene
            }
        }
    }

    void ShowSlide(int index)
    {
        if (index >= slides.Length || index >= captions.Length ||
            index >= captionPositions.Length || index >= captionColors.Length ||
            index >= slideSounds.Length || index >= slideLoops.Length)
        {
            Debug.LogWarning("Slide index out of bounds!");
            return;
        }

        // Fade in the slide image
        displayImage.sprite = slides[index];
        Color imageColor = displayImage.color;
        imageColor.a = 0;
        displayImage.color = imageColor;
        StartCoroutine(FadeInImage());

        // Play slide-specific sound effect
        if (slideSounds[index] != null && sfxSource != null)
        {
            sfxSource.Stop();
            sfxSource.clip = slideSounds[index];
            sfxSource.loop = slideLoops[index];
            sfxSource.Play();
        }

        // Set caption style
        captionText.rectTransform.anchoredPosition = captionPositions[index];
        captionText.color = captionColors[index];

        if (isTyping)
        {
            StopAllCoroutines();
            isTyping = false;
        }

        StartCoroutine(TypeCaption(captions[index]));
    }

    IEnumerator FadeInImage()
    {
        float timer = 0f;
        Color imgColor = displayImage.color;
        imgColor.a = 0;

        while (timer < imageFadeDuration)
        {
            float alpha = timer / imageFadeDuration;
            displayImage.color = new Color(imgColor.r, imgColor.g, imgColor.b, alpha);
            timer += Time.deltaTime;
            yield return null;
        }

        displayImage.color = new Color(imgColor.r, imgColor.g, imgColor.b, 1f);
    }

    IEnumerator TypeCaption(string fullText)
    {
        isTyping = true;
        captionText.text = "";

        foreach (char c in fullText)
        {
            captionText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}
