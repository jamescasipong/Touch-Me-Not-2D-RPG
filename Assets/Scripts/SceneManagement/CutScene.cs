using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CutScene : MonoBehaviour
{
    public float fadeInDuration = 2.0f;
    public float textFadeInDuration = 1.0f;
    public float visibleDuration = 2.0f;
    public float fadeOutDuration = 0.8f;

    public string cutsceneTextContent; // Public field to set the cutscene text in the Inspector.
    
    public Image blackImage;
    public TMP_Text cutsceneText;

    public static CutScene instance;

    private int currentIndex = 0;

    public void Awake()
    {
        // Ensure there's only one instance of this script in the project.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Set the cutscene text from the Inspector field.
        

        // Initialize the alpha values for the black image and text.
        blackImage.color = new Color(0, 0, 0, 0);
        cutsceneText.color = new Color(1, 1, 1, 0);
    }

    public void StartCutsceneWithFadeIn(string name, float fadeInDuration)
    {
        cutsceneText.text = name;
        Sequence sequence = DOTween.Sequence();

        // Fade in the black image
        sequence.Append(blackImage.DOFade(1, fadeInDuration));

        // Fade in the text after the black image has finished fading in
        sequence.AppendCallback(() => {
            cutsceneText.DOFade(1, textFadeInDuration).OnComplete(() => {
                // After the text is fully visible, wait for 'visibleDuration' seconds, and then start the fade-out.
                Invoke("StartFadeOut", visibleDuration);
            });
        });
    }

    public void StartCutsceneWithPopInAndFadeOut(string name)
    {
        cutsceneText.text = name;

        blackImage.color = new Color(0, 0, 0, 1);

        // Set the text to fully opaque
        cutsceneText.color = new Color(1, 1, 1, 1);

        // Start the cutscene with the text and black image fully visible
        Invoke("StartFadeOut", visibleDuration);
    }

    private void StartFadeOut()
    {
        // Sequence the fade-out animations
        Sequence sequence = DOTween.Sequence();

        // Fade out the text
        sequence.Append(cutsceneText.DOFade(0, fadeOutDuration));

        // Fade out the black image after the text has finished fading out
        sequence.AppendCallback(() => {
            blackImage.DOFade(0, fadeOutDuration);
        });
    }
}
