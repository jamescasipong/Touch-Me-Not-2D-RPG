using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartQuestInstance : MonoBehaviour
{
    public static StartQuestInstance instance;
    public StartQuestAfterAQuest[] startQuest;

    private void Awake()
    {
        instance = this;
    }
}
