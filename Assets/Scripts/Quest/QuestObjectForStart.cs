using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjectForStart : MonoBehaviour, ISavable
{
    [SerializeField] List<QuestBase> questsToCheck; // Change to a list of QuestBase
    [SerializeField] ObjectActionsz onStart;
    [SerializeField] ObjectActionsz onComplete;

    QuestList questList;

    public void Start()
    {
        questList = QuestList.GetQuestList();
        questList.OnUpdated += UpdateObjectStatus;

        UpdateObjectStatus();
    }

    private void OnDestroy()
    {
        questList.OnUpdated -= UpdateObjectStatus;
    }

    public void UpdateObjectStatus()
    {
        bool allQuestsStarted = true;

        foreach (QuestBase quest in questsToCheck)
        {
            if (!questList.IsStarted(quest.Name))
            {
                allQuestsStarted = false;
                break; // Exit the loop as soon as one quest is not completed
            }
        }

        foreach (Transform child in transform)
        {
            if (allQuestsStarted)
            {
                if (onComplete == ObjectActionsz.Enable)
                    child.gameObject.SetActive(true);
                else if (onComplete == ObjectActionsz.Disable)
                    child.gameObject.SetActive(false);
            }
            else
            {
                if (onStart == ObjectActionsz.Enable)
                    child.gameObject.SetActive(true);
                else if (onStart == ObjectActionsz.Disable)
                    child.gameObject.SetActive(false);
            }
        }
    }

    public object CaptureState()
    {
        QuestObjectForStartSaveData saveData = new QuestObjectForStartSaveData();

        // Saving quest names from questsToCheck
        saveData.questNames = new List<string>();
        foreach (QuestBase quest in questsToCheck)
        {
            saveData.questNames.Add(quest.Name);
        }

        // Saving onStart and onComplete states
        saveData.onStartState = onStart;
        saveData.onCompleteState = onComplete;

        return saveData;
    }

    public void RestoreState(object state)
    {
        if (state is QuestObjectForStartSaveData saveData)
        {
            // Restoring quest names to questsToCheck
            questsToCheck.Clear();
            foreach (string questName in saveData.questNames)
            {
                // Retrieve quests by name and add them back to questsToCheck
                // Assuming there's a way to retrieve quests by name
                // Add your logic here to retrieve quests based on their names
            }

            // Restoring onStart and onComplete states
            onStart = saveData.onStartState;
            onComplete = saveData.onCompleteState;

            // After restoring the state, you might want to update the object status
            UpdateObjectStatus();
        }
    }
}

[System.Serializable]
public class QuestObjectForStartSaveData
{
    public List<string> questNames; // Saving quest names as strings
    public ObjectActionsz onStartState;
    public ObjectActionsz onCompleteState;
}


public enum ObjectActionsz { DoNothing, Enable, Disable }