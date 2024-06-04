using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> quests; // List of all the quests (Scriptable Objects)
    public GameObject completionPanel; // Reference to the UI panel to enable when all quests are completed

    private void Awake()
    {
        // Initialize the completionPanel as inactive
    }

    // Check if all quests are completed
    private bool AreAllQuestsCompleted()
    {
        foreach (Quest quest in quests)
        {
            if (quest.Status != QuestStatus.Completed)
            {
                return false; // At least one quest is not completed
            }
        }
        return true; // All quests are completed
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if all quests are completed
        if (AreAllQuestsCompleted())
        {
            // Enable the UI panel
            completionPanel.SetActive(true);
        }
    }
}
