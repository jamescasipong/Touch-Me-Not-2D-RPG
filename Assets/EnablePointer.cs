using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablePointer : MonoBehaviour
{
    QuestList questList;
    public QuestBase quest;
    void Start()
    {
        questList = QuestList.GetQuestList();

    }

    // Update is called once per frame
    void Update()
    {


        if (questList.IsStarted(quest.Name))
        {
            SetActive(true);

        }

        else if (questList.IsCompleted(quest.Name))
        {
            SetActive(false);
        }
        else
        {
            SetActive(false);
        }
        
        
    }

    void SetActive(bool active)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(active);
        }
    }
}
