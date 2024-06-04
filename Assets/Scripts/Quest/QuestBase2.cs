using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Quests2/Create a New Quest2")]
public class QuestBase2 : QuestBase
{
    [SerializeField] public List<QuestBase> questsToCheck;
}
