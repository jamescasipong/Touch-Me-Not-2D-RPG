using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class TypingTextPlayable : PlayableBehaviour
{
    public string dialogueText;
    private TextMeshProUGUI typingText;
    private int currentCharacterIndex = 0;
    private float timePerCharacter = 0.01f; //0.0005f; // Adjust the typing speed as needed                                              // Adjust the typing speed as needed
    private float timeSinceLastCharacter;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        typingText = playerData as TextMeshProUGUI;

        if (typingText != null)
        {
            timeSinceLastCharacter += Time.deltaTime;

            if (timeSinceLastCharacter >= timePerCharacter)
            {
                timeSinceLastCharacter = 0f;
                currentCharacterIndex = Mathf.Min(currentCharacterIndex + 1, dialogueText.Length);
                string displayedText = dialogueText.Substring(0, currentCharacterIndex);
                typingText.text = displayedText;
            }
        }
    }
}
