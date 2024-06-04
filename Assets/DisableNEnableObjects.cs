using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableNEnableObjects : MonoBehaviour, ISavable
{
    public QuestBase questBase;
    QuestList questList;

    public GameObject portal;
    bool doIt = false;

    void Start()
    {
        questList = QuestList.GetQuestList();
        doIt = false;
    }

    void Update()
    {
        if (!doIt)
        {
            if (questList.IsStarted(questBase.Name))
            {
                portal.SetActive(false);

                if (questList.IsCompleted(questBase.Name))
                {
                    portal.SetActive(true);
                    doIt = true;
                }
            }
        }
    }

    public object CaptureState()
    {
        return doIt; // Saving the state of 'doIt' variable
    }

    public void RestoreState(object state)
    {
        doIt = (bool)state; // Restoring the state of 'doIt' variable
    }
}
