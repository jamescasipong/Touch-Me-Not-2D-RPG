using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Playables;


[System.Serializable]
public class Quest
{
    public static Quest instance { get; private set; }
    public QuestStatus Status { get; set; }
    public QuestBase Base { get; private set; }
    private bool isNotificationHidden = false;

    public ShopManager Shop { get; private set; }

    private bool shouldCheckNotification = false;

    private NPCController npcController;
    public Quest(QuestBase _base)
    {
        Base = _base;
    }

    public void SetNotificationCheckFlag()
    {
        shouldCheckNotification = true;
    }
    public Quest(QuestSaveData saveData)
    {
        Base = QuestDB.GetObjectByName(saveData.name);
        Status = saveData.status;
    }



    public QuestSaveData GetSaveData()
    {
        var saveData = new QuestSaveData()
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
            var inventory = Inventory.GetInventory();
            if (Status == QuestStatus.Started)
            {
                if (inventory.HasItem(Base.RequiredItem, Base.RequiredItemCount))
                {
                    if (!isNotificationHidden) // You may need to add this variable to the Quest class.
                    {

                        isNotificationHidden = true;
                        yield return DialogManager.Instance.ShowNotifText(Base.toDoToFinishQuest); // You may need to add this variable to the Quest class.
                    }
                }
                else
                {
                    isNotificationHidden = false;
                }
            }
        }
        else
        {
            Debug.LogWarning("DialogManager.Instance is null in CheckAndHideNotification.");
        }
    }



    public IEnumerator StartQuest(bool disableDialog)
    {
        
        Status = QuestStatus.Started;

        if (!disableDialog) { 
            yield return DialogManager.Instance.AutomaticallyShowDialog(Base.StartDialogue);
        }

        //var shopManager = ShopManager.GetShopManager();

        //shopManager.AddCoins(Base.Coins);

        var questList = QuestList.GetQuestList();
        questList.AddQuest(this);
        
        yield return DialogManager.Instance.ShowNotifText(Base.TextGuideString);

    }


    public IEnumerator CompleteQuest(Transform player, bool disableDialog, PlayableDirector completionTimeline, bool disableHideText)
    {

        Status = QuestStatus.Completed;
        if (!disableDialog)
        {
            yield return DialogManager.Instance.AutomaticallyShowDialog(Base.CompletedDialouge);
        }
        //var shopManager = ShopManager.GetShopManager();

        if (!disableHideText)
        {
            yield return DialogManager.Instance.HideNotifText(Base.TextGuideString);
        }

        var inventory = Inventory.GetInventory();


        if (Base.RequiredItem != null && Base.RequiredItemCount > 0)
        {
            
            var itemInInventory = inventory.Slots.FirstOrDefault(slot => slot.Item == Base.RequiredItem);
            if (itemInInventory != null)
            {
                int itemsToRemove = Mathf.Min(Base.RequiredItemCount, itemInInventory.Count);
                inventory.RemoveItem(Base.RequiredItem, Base.RequiredItemCount); // Remove one instance of the required item
            }
        }

        if (Base.RewardItem != null && Base.RewardItemCount > 0)
        {
            
            inventory.AddItem(Base.RewardItem, Base.RewardItemCount);
            string playerName = player.GetComponent<PlayerController>().Name;
            DialogManager.Instance.ShowDialogText($"{playerName} received {Base.RewardItemCount} x {Base.RewardItem.Name}");
        }

        if (completionTimeline != null) 
        {
            GameController.Instance.PauseGame(true);

            completionTimeline.Play();

            yield return new WaitForSeconds((float)completionTimeline.duration);

            GameController.Instance.PauseGame(false);
        }

        var questList = QuestList.GetQuestList();
        questList.AddQuest(this);

    }

    public IEnumerator StartQuestWithoutDialog()
    {

        Status = QuestStatus.Started;




        var questList = QuestList.GetQuestList();
        questList.AddQuest(this);
        yield return DialogManager.Instance.ShowNotifText(Base.TextGuideString);

    }


    public IEnumerator CompleteQuestWithoutDialog(Transform player)
    {

        Status = QuestStatus.Completed;

        //var shopManager = ShopManager.GetShopManager();

        //shopManager.AddCoins(Base.Coins);

        var inventory = Inventory.GetInventory();
        if (Base.RequiredItem != null && Base.RequiredItemCount > 0)
        {
            yield return DialogManager.Instance.HideNotifText(Base.TextGuideString);
            var itemInInventory = inventory.Slots.FirstOrDefault(slot => slot.Item == Base.RequiredItem);
            if (itemInInventory != null)
            {
                int itemsToRemove = Mathf.Min(Base.RequiredItemCount, itemInInventory.Count);
                inventory.RemoveItem(Base.RequiredItem, Base.RequiredItemCount); // Remove one instance of the required item
            }
        }

        if (Base.RewardItem != null && Base.RewardItemCount > 0)
        {

            inventory.AddItem(Base.RewardItem, Base.RewardItemCount);
            string playerName = player.GetComponent<PlayerController>().Name;
            DialogManager.Instance.ShowDialogText($"{playerName} received {Base.RewardItemCount} x {Base.RewardItem.Name}");
        }

        var questList = QuestList.GetQuestList();
        questList.AddQuest(this);

    }




    public bool CanBecompleted()
    {
        var inventory = Inventory.GetInventory();
        if (Base.RequiredItem != null && Base.RequiredItemCount > 0)
        {
            var itemInInventory = inventory.Slots.FirstOrDefault(slot => slot.Item == Base.RequiredItem);
            if (itemInInventory != null && itemInInventory.Count >= Base.RequiredItemCount)
            {

                return true;
            }
            return false;
        }
        return true; // Quest can be completed if there's no required item
    }
}

[System.Serializable]
public class QuestSaveData
{
    public string name;
    public QuestStatus status;
}

public enum QuestStatus { None, Started, Completed }
