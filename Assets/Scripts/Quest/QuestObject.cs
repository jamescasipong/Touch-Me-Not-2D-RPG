using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestObject : MonoBehaviour, ISavable
{
    [SerializeField] List<QuestBase> questsToCheck; // Change to a list of QuestBase
    [SerializeField] ObjectActions onStart;
    [SerializeField] ObjectActions onComplete;
    [SerializeField] float waitDelay = 0f;
    [SerializeField] bool disableWait;

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
        StartCoroutine(EnableDisableObjectsAfterDelay(waitDelay));
    }

    private IEnumerator EnableDisableObjectsAfterDelay(float delay)
    {
        
        bool allQuestsCompleted = true;
        foreach (QuestBase quest in questsToCheck)
        {
            if (!questList.IsCompleted(quest.Name))
            {
                allQuestsCompleted = false;
                break;
                // Exit the loop as soon as one quest is not completed
            }
        }
        if (!disableWait) 
        {
            yield return new WaitForSeconds(delay);
        }

        foreach (Transform child in transform)
        {
            if (allQuestsCompleted)
            {
                if (onComplete == ObjectActions.Enable)
                {
                    
                    child.gameObject.SetActive(true);
                }

                else if (onComplete == ObjectActions.Disable)
                {
                    child.gameObject.SetActive(false);
                }

            }
            else
            {
                
                if (onStart == ObjectActions.Enable)
                {
                    child.gameObject.SetActive(true);
                }

                else if (onStart == ObjectActions.Disable)
                {
                    
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    public object CaptureState()
    {
        return disableWait;
    }

    public void RestoreState(object state)
    {
        if (state is bool)
        {
            disableWait = (bool)state;
        }
    }

}

public enum ObjectActions { DoNothing, Enable, Disable }
