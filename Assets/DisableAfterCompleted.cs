using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DisableAfterCompleted : MonoBehaviour
{
    QuestList questList;

    public QuestBase questBase;

    public QuestBase questChef;

    //public bool trigger;
    //public PlayerController playerController;

    private void Start()
    {
        questList = QuestList.GetQuestList();
    }
    private void Update()
    {
        if (questChef == null)
        {
            if (questBase != null)
            {
                if (questList.IsCompleted(questBase.Name))
                {
                    foreach (Transform child in transform)
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }
        }
        if (questBase == null)
        {
            if (questChef != null && questList.IsStarted(questChef.Name))
            {
                
                    if (ShopManager.Instance.Shop.activeInHierarchy)
                    {
                        foreach (Transform child in transform)
                        {
                            child.gameObject.SetActive(false);
                        }

                    }

                if (questList.IsCompleted(questChef.Name))
                {

                    foreach (Transform child in transform)
                    {
                        child.gameObject.SetActive(false);
                    }

                    Debug.Log("disabling");
                }

            }
            
        }
    }

}
