using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestObjectAssign : MonoBehaviour, ISavable
{
    [SerializeField] List<QuestBase> questsToCheck;
    [SerializeField] List<GameObject> objectsZ; // Change to a list of QuestBase
    [SerializeField] ObjectActionsAssigner onStart;
    [SerializeField] ObjectActionsAssigner onComplete;

    QuestList questList;

    public void Start()
    {
        questList = QuestList.GetQuestList();
        questList.OnUpdated += UpdateObjectStatus;

        UpdateObjectStatus();
    }


    private void OnDestroy()
    {
        if (questList != null)
        {
            questList.OnUpdated -= UpdateObjectStatus;
        }
    }

    public void UpdateObjectStatus()
    {
        bool allQuestsCompleted = true;


        foreach (QuestBase quest in questsToCheck)
        {
            if (!questList.IsCompleted(quest.Name))
            {
                allQuestsCompleted = false;
                break; // Exit the loop as soon as one quest is not completed
            }
        }

        foreach (GameObject obj in objectsZ)
        {
            if (allQuestsCompleted)
            {
                if (onComplete == ObjectActionsAssigner.Enable)
                {
                    obj.SetActive(true);
                    //Debug.Log("Object Enabled: " + obj.name);
                }
                else if (onComplete == ObjectActionsAssigner.Disable)
                {
                    obj.SetActive(false);
                    //Debug.Log("Object Disabled: " + obj.name);
                }

                else if (onComplete == ObjectActionsAssigner.DisableText)
                {
                    StartCoroutine(DialogManager.Instance.ShowNotifText(""));
                }
            }
            else
            {
                if (onStart == ObjectActionsAssigner.Enable)
                {
                    obj.SetActive(true);
                    Debug.Log("Object Enabled: " + obj.name);
                }
                else if (onStart == ObjectActionsAssigner.Disable)
                {
                    obj.SetActive(false);
                    Debug.Log("Object Disabled: " + obj.name);
                }
            }
        }
    }

    public object CaptureState()
    {
        Dictionary<string, List<string>> state = new Dictionary<string, List<string>>();

        List<string> questNames = new List<string>();
        foreach (QuestBase quest in questsToCheck)
        {
            questNames.Add(quest.Name);
        }
        state["QuestsToCheck"] = questNames;

        List<string> objectNames = new List<string>();
        foreach (GameObject obj in objectsZ)
        {
            objectNames.Add(obj.name);
        }
        state["ObjectsZ"] = objectNames;

        return state;
    }

    public void RestoreState(object state)
    {
        if (state is Dictionary<string, List<string>>)
        {
            var dictState = (Dictionary<string, List<string>>)state;

            if (dictState.ContainsKey("QuestsToCheck"))
            {
                List<string> questNames = dictState["QuestsToCheck"];
                questsToCheck.Clear();
                foreach (string questName in questNames)
                {
                    // Assuming you have a method to find QuestBase by name
                    QuestBase quest = FindQuestByName(questName);
                    if (quest != null)
                    {
                        questsToCheck.Add(quest);
                    }
                }
            }

            if (dictState.ContainsKey("ObjectsZ"))
            {
                List<string> objectNames = dictState["ObjectsZ"];
                objectsZ.Clear();
                foreach (string objName in objectNames)
                {
                    GameObject obj = GameObject.Find(objName);
                    if (obj != null)
                    {
                        objectsZ.Add(obj);
                    }
                }
            }
        }
    }

    // Method to find a QuestBase by name (You need to implement this)
    private QuestBase FindQuestByName(string questName)
    {
        // Assuming questsToSearch is a list of QuestBase objects accessible in your context
        foreach (QuestBase quest in questsToCheck)
        {
            if (quest.Name == questName)
            {
                return quest;
            }
        }

        // Return null if the quest with the given name is not found
        return null;
    }


}

public enum ObjectActionsAssigner { DoNothing, Enable, Disable, DisableText }
