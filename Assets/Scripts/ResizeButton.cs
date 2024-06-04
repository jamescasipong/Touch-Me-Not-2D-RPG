using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ResizeButtons : MonoBehaviour
{
    public List<Button> buttons; // List of buttons
    public float paddingValue = 20f; // Padding value for extra space around the text

    void Update()
    {
        ResizeAllButtons();
    }

    void ResizeAllButtons()
    {
        foreach (Button button in buttons)
        {
            ResizeButton(button);
        }
    }

    void ResizeButton(Button button)
    {
        RectTransform rectTransform = button.GetComponent<RectTransform>();
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();

        if (buttonText != null)
        {
            
            float preferredWidth = buttonText.preferredWidth;
            float preferredHeight = buttonText.preferredHeight;

            // Get the maximum allowed width and height based on screen size
            float maxWidth = Screen.width * 0.8f; // 80% of screen width
            float maxHeight = Screen.height * 0.8f; // 80% of screen height

            // Calculate the final width and height considering the padding
            float finalWidth = Mathf.Min(preferredWidth + paddingValue, maxWidth);
            float finalHeight = Mathf.Min(preferredHeight + paddingValue, maxHeight);

            // Check if the text height exceeds the screen height
            if (preferredHeight > maxHeight)
            {
                // Adjust the height to fit within the screen
                finalHeight = maxHeight;
            }

            // Adjust the size of the button's RectTransform
            rectTransform.sizeDelta = new Vector2(finalWidth, finalHeight);
        }
    }
}
