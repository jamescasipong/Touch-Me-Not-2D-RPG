using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class QuestList : MonoBehaviour, ISavable
{
    public List<Quest> quests = new List<Quest>();
    public List<Quest2> quests2 = new List<Quest2>();

    public event Action OnUpdated;

    public void AddQuest(Quest quest)
    {
        // Check if a quest with the same name is already in the list
        var existingQuest = quests.FirstOrDefault(q => q.Base.Name == quest.Base.Name);

        if (existingQuest != null)
        {
            // Update the status of the existing quest to "Completed"
            existingQuest.Status = QuestStatus.Completed;
        }
        else
        {
            // Add the quest to the list if it's not already there
            quests.Add(quest);
        }

        OnUpdated?.Invoke();
    }

    public void AddQuest2(Quest2 quest2)
    {
        // Check if a quest with the same name is already in the list
        var existingQuest = quests2.FirstOrDefault(q => q.Base.Name == quest2.Base.Name);

        if (existingQuest != null)
        {
            // Update the status of the existing quest to "Completed"
            existingQuest.Status = QuestStatus.Completed;
        }
        else
        {
            // Add the quest to the list if it's not already there
            quests2.Add(quest2);
        }

        OnUpdated?.Invoke();
    }

    public bool IsStarted(string questName)
    {
        var questStatus1 = quests.FirstOrDefault(q => q.Base.Name == questName)?.Status;
        var questStatus2 = quests2.FirstOrDefault(q => q.Base.Name == questName)?.Status;
        return questStatus1 == QuestStatus.Started || questStatus1 == QuestStatus.Completed ||
               questStatus2 == QuestStatus.Started || questStatus2 == QuestStatus.Completed;
    }

    public bool IsCompleted(string questName)
    {
        var questStatus1 = quests.FirstOrDefault(q => q.Base.Name == questName)?.Status;
        var questStatus2 = quests2.FirstOrDefault(q => q.Base.Name == questName)?.Status;
        return questStatus1 == QuestStatus.Completed || questStatus2 == QuestStatus.Completed;
    }


    public static QuestList GetQuestList()
    {
        return FindObjectOfType<PlayerController>().GetComponent<QuestList>();
    }

    public object CaptureState()
    {
        var questSaveData = quests.Select(q => q.GetSaveData()).ToList();
        var quest2SaveData = quests2.Select(q => q.GetSaveData()).ToList();
        return new QuestListSaveData { QuestSaveDataList = questSaveData, Quest2SaveDataList = quest2SaveData };
    }

    public void RestoreState(object state)
    {
        var saveData = state as QuestListSaveData;
        if (saveData != null)
        {
            quests = saveData.QuestSaveDataList.Select(q => new Quest(q)).ToList();
            quests2 = saveData.Quest2SaveDataList.Select(q => new Quest2(q)).ToList();
            OnUpdated?.Invoke();
        }
    }
}

[Serializable]
public class QuestListSaveData
{
    public List<QuestSaveData> QuestSaveDataList;
    public List<Quest2SaveData> Quest2SaveDataList;
}
