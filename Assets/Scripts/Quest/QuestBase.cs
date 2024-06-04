using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Quests/Create a New Quest")]

public class QuestBase : ScriptableObject
{
    [SerializeField] string name;
    [SerializeField] string description;


    [Header("Dialouges")]
    [SerializeField] Dialog startDialogue;
    [SerializeField] Dialog inProgressDialogue;
    [SerializeField] Dialog completedDialogue;

    [Header("Indicator")]
    [SerializeField] public string textGuideString = "";

    [SerializeField] public string toDoToFinishQuest = "";

    /*[Header("Ink")]
    [SerializeField] TextAsset inkStartDialogue;
    [SerializeField] TextAsset inkInProgressDialogue;
    [SerializeField] TextAsset inkCompletedDialogue;*/

    [Header("Required Item")]
    [SerializeField] ItemBase requiredItem;
    [SerializeField] int requiredItemCount;

    [Header("Reward Item")]
    [SerializeField] ItemBase rewardItem;
    [SerializeField] int rewardItemCount;

    [SerializeField] int coins;

    [SerializeField] int giveCoins;

    /*public TextAsset InkStartDialogue => inkStartDialogue;
    public TextAsset InkInProgressDialogue => inkCompletedDialogue;
    public TextAsset InkCompletedDialouge => inkCompletedDialogue;*/

    public int GiveCoins => giveCoins;
    public int Coins => coins;
    public string ToDotoFinishQuest => toDoToFinishQuest;

    public string Name => name;
    public string Description => description;
    public string TextGuideString => textGuideString;
    public int RequiredItemCount => requiredItemCount;

    public int RewardItemCount => rewardItemCount;

    public Dialog StartDialogue => startDialogue;
    public Dialog InProgressDialogue => inProgressDialogue?.NPCLines?.Count > 0 ? inProgressDialogue : startDialogue;
    public Dialog CompletedDialouge => completedDialogue;

    public ItemBase RequiredItem => requiredItem;
    public ItemBase RewardItem => rewardItem;

}
