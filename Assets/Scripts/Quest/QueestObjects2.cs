using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestObject2 : MonoBehaviour
{
    [SerializeField] List<QuestBase2> questsToCheck; // Change to a list of QuestBase
    [SerializeField] ObjectActions2 onStart;
    [SerializeField] ObjectActions2 onComplete;

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
        bool allQuestsCompleted = true;
        foreach (QuestBase2 quest in questsToCheck)
        {
            if (!questList.IsCompleted(quest.Name))
            {
                allQuestsCompleted = false;
                break;
                // Exit the loop as soon as one quest is not completed
            }
        }

        foreach (Transform child in transform)
        {
            if (allQuestsCompleted)
            {

                if (onComplete == ObjectActions2.Enable)
                    child.gameObject.SetActive(true);
                else if (onComplete == ObjectActions2.Disable)
                    child.gameObject.SetActive(false);
            }
            else
            {
                if (onStart == ObjectActions2.Enable)
                    child.gameObject.SetActive(true);
                else if (onStart == ObjectActions2.Disable)
                    child.gameObject.SetActive(false);
            }
        }
    }
}

public enum ObjectActions2 { DoNothing, Enable, Disable }
