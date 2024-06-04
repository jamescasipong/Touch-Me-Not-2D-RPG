using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ink.Runtime;

public class QuizGame : MonoBehaviour
{
    private Story inkStory;
    private int score;

    void Start()
    {
        TextAsset inkJSON = Resources.Load<TextAsset>("Quiz1");
        inkStory = new Story(inkJSON.text);
        score = 0; // Initialize the score
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (inkStory.canContinue)
            {
                string text = inkStory.Continue();
            }
            else if (inkStory.currentChoices.Count > 0)
            {
                // Handle player choices
                foreach (Choice choice in inkStory.currentChoices)
                {
                    if (choice.text == "Correct Answer") // Replace with the actual correct answer
                    {
                        score++;
                    }
                }

                if (score == 3) // Assuming there are 3 questions
                {
                    // Player answered all questions correctly, so switch to a new scene
                    SceneManager.LoadScene("NewSceneName"); // Replace with the name of your new scene
                }
            }
        }
    }
}
