using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest2
{
    public QuestStatus Status { get; set; }
    public QuestBase2 Base { get; private set; }
    // List of quests to check for completion
    private bool isNotificationHidden = false;

    //QuestInitiator questInitiator {  get; set; }

    private bool shouldCheckNotification = false;
    private bool isNotificationCheckedAndHidden = false;

    public void SetNotificationCheckFlag()
    {
        shouldCheckNotification = true;
    }

    public string questName { get; set; }
    public Quest2(QuestBase2 _base)
    {
        Base = _base;
    }

    public Quest2(Quest2SaveData saveData)
    {
        Base = Quest2DB.GetObjectByName(saveData.name);
        Status = saveData.status;
    }

    public Quest2SaveData GetSaveData()
    {
        var saveData = new Quest2SaveData()
        {
            name = Base.name,
            status = Status
        };
        return saveData;
    }

    public IEnumerator CheckAndHideNotification()
    {


        if (DialogManager.Instance != null)
        {
            bool allQuestsCompleted = true;
            QuestList questList = QuestList.GetQuestList();
            foreach (QuestBase quest in Base.questsToCheck)
            {
                if (!questList.IsCompleted(quest.Name))
                {
                    allQuestsCompleted = false;
                    break;
                }
            }

            if (allQuestsCompleted)
            {
                if (!isNotificationHidden)
                {
                    if (isNotificationCheckedAndHidden)
                    {
                        isNotificationHidden = true;
                        Debug.Log("Before ShowNotifText");
                        yield return DialogManager.Instance.ShowNotifText(Base.toDoToFinishQuest);
                        yield break;
                    }
                }
            }
            else
            {
                isNotificationHidden = false;
            }
        }

        // Set the flag to indicate that the notification has been checked and hidden.
        isNotificationCheckedAndHidden = true;
    }



    public IEnumerator StartQuest(bool disableDialog)
    {

        Status = QuestStatus.Started;
        var questList = QuestList.GetQuestList();
        questList.AddQuest2(this);

        if (!disableDialog)
        {
            yield return DialogManager.Instance.AutomaticallyShowDialog(Base.StartDialogue);
        }
        yield return DialogManager.Instance.ShowNotifText(Base.TextGuideString);
    }

    public IEnumerator CompleteQuest(Transform player, bool disableDialog)
    {
        Status = QuestStatus.Completed;

        QuestList questList = QuestList.GetQuestList();
        var inventory = Inventory.GetInventory();

        // Check if all quests in questsToCheck are completed
        bool allQuestsCompleted = true;
        foreach (QuestBase quest in Base.questsToCheck)
        {
            if (!questList.IsCompleted(quest.Name))
            {
                allQuestsCompleted = false;
                break;
            }
        }

        if (allQuestsCompleted)
        {
            if (!disableDialog)
            {
                yield return DialogManager.Instance.AutomaticallyShowDialog(Base.CompletedDialouge);
            }

            yield return DialogManager.Instance.HideNotifText(Base.TextGuideString);
            //var shopManager = ShopManager.GetShopManager();
            if (Base.RewardItem != null && Base.RewardItemCount > 0)
            {

                inventory.AddItem(Base.RewardItem, Base.RewardItemCount);


                //shopManager.AddCoins(Base.Coins);
                string playerName = player.GetComponent<PlayerController>().Name;
                DialogManager.Instance.ShowDialogText($"{playerName} received {Base.RewardItemCount} x {Base.RewardItem.Name}");
            }
        }

        questList.AddQuest2(this);
    }

    public IEnumerator CompleteQuestWithoutDialog(string name)
    {

        Status = QuestStatus.Started;
        var questList = QuestList.GetQuestList();
        questList.AddQuest2(this);
        yield return DialogManager.Instance.ShowNotifText(Base.TextGuideString);
    }




    public IEnumerator StartQuestInitiator()
    {

        Status = QuestStatus.Started;
        var questList = QuestList.GetQuestList();
        questList.AddQuest2(this);
        yield return DialogManager.Instance.ShowNotifText(Base.TextGuideString);
    }


    public IEnumerator CompleteQuestInitiator(Transform player)
    {
        Status = QuestStatus.Completed;

        QuestList questList = QuestList.GetQuestList();
        var inventory = Inventory.GetInventory();

        // Check if all quests in questsToCheck are completed
        bool allQuestsCompleted = true;
        foreach (QuestBase quest in Base.questsToCheck)
        {
            if (!questList.IsCompleted(quest.Name))
            {
                allQuestsCompleted = false;
                break;
            }
        }

        if (allQuestsCompleted)
        {
            yield return DialogManager.Instance.HideNotifText(Base.TextGuideString);
            //var shopManager = ShopManager.GetShopManager();
            if (Base.RewardItem != null && Base.RewardItemCount > 0)
            {

                inventory.AddItem(Base.RewardItem, Base.RewardItemCount);


                //shopManager.AddCoins(Base.Coins);
                string playerName = player.GetComponent<PlayerController>().Name;
            }
        }

        questList.AddQuest2(this);
    }

    public bool CanBeCompleted()
    {
        // Quest2 can be completed if all quests in questsToCheck are completed
        QuestList questList = QuestList.GetQuestList();
        foreach (QuestBase quest in Base.questsToCheck)
        {
            if (!questList.IsCompleted(quest.Name))
            {
                return false;
            }
        }
        return true;
    }


    public IEnumerator StartQuestInitiatorForScene()
    {
        Debug.Log("Quest started");
        Status = QuestStatus.Started;
        var questList = QuestList.GetQuestList();
        questList.AddQuest2(this);
        yield return DialogManager.Instance.ShowNotifText(Base.TextGuideString);

    }

    public IEnumerator CompleteQuestInitiatorForScene()
    {
        if (Status != QuestStatus.Started)
        {
            Debug.Log("Quest is not in the correct status to complete.");
            yield break;
        }

        Status = QuestStatus.Completed;
        var questList = QuestList.GetQuestList();
        var inventory = Inventory.GetInventory();
        yield return DialogManager.Instance.HideNotifText(Base.TextGuideString);

        //var shopManager = ShopManager.GetShopManager();
        if (Base.RewardItem != null && Base.RewardItemCount > 0)
        {
            inventory.AddItem(Base.RewardItem, Base.RewardItemCount);
            //shopManager.AddCoins(Base.Coins);
        }
        Debug.Log("Quest completed");


        questList.AddQuest2(this);
    }

    /*public bool IfSceneCanBeCompleted()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        // Replace with your player-finding logic.
        if (player != null)
        {
            string currentSceneName = player.gameObject.scene.name;
            return currentSceneName == Base.sceneName;


        }
        return false;
    }*/



}

[System.Serializable]
public class Quest2SaveData
{
    public string name;
    public QuestStatus status;
}

