using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTrigger : MonoBehaviour, ISavable
{
    // The quest you want to complete.

    [SerializeField] private bool isTriggered = false;
    public StartQuestAfterAQuest startQuest;
    
    public QuestBase questCene;
    public int array;
    StartQuestAfterAQuest SQ;
    public QuestList questList;

    public void Start()
    {
        questList = QuestList.GetQuestList();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isTriggered)
            {
                //Debug.Log("works");
                //Debug.Log("QuestType: " + StartQuestAfterAQuest.instance.questType); // Add this line
                //Debug.Log("QuestCene Name: " + questCene.Name);

                if (questList != null && questCene != null && questList.IsStarted(questCene.Name))
                {
                    Debug.Log("triggered works");
                    isTriggered = true;

                    // Debug.Log("Triggered quest");
                    StartQuestAfterAQuest startQuests = StartQuestInstance.instance.startQuest[array].GetComponent<StartQuestAfterAQuest>();

                    startQuests.OnQuestTriggered(this);

                    questCene = null;

                }
                
            }
            else
            {
                //Debug.Log("different dimension mahboi");
            }
        }

    }




    public bool IsTriggered()
    {
        return isTriggered;
    }

    public object CaptureState()
    {
        return isTriggered;
    }


    public void RestoreState(object state)
    {
        if (state is bool isTriggeredState)
        {
            isTriggered = isTriggeredState;
        }
    }

}

public enum QuestType
{
    Quest1,
    Quest2,
    Quest3,
    Quest4,
    Quest5,
    Quest6,
    Quest7,
    Quest8,
    Quest9,
    Quest10,
    // Add more quest types as needed
}

