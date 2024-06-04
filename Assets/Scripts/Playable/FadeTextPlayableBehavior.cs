using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class FadeTextPlayableBehavior : PlayableBehaviour
{
    public string dialogueText;
    private TextMeshProUGUI typingText;
    private Color initialColor;
    private bool hasFaded = false;
    public float fadeDuration = 0.5f; // Duration for the fade effect in seconds

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        typingText = playerData as TextMeshProUGUI;

        if (typingText != null && !hasFaded)
        {
            initialColor = typingText.color;

            float elapsedTime = (float)playable.GetTime();
            float normalizedTime = elapsedTime / (float)playable.GetDuration();

            if (normalizedTime < 6f)
            {
                float alpha = Mathf.Lerp(0f, 6f, normalizedTime);
                Color newColor = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
                typingText.color = newColor;
            }
            else
            {
                typingText.color = initialColor;
                hasFaded = true;
            }

            typingText.text = dialogueText;
        }
    }
}
