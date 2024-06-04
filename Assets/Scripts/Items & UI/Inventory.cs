using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TMP_Text;


public enum ItemCategory { Items }
public class Inventory : MonoBehaviour, ISavable
{
    [SerializeField] List<ItemSlot> slots;

    public Text notificationText; // Reference to the Text UI element to display notifications.
    public float fadeDuration = 1.0f; // Duration for fading in and out.
    [SerializeField] float typingSpeed;
    public Text keyText;

    private Queue<string> notificationQueue = new Queue<string>();
    private bool isDisplayingNotification = false;

    public event Action onUpdated;

    public List<ItemSlot> Slots => slots;

    public static Inventory instance;

    private void Awake()
    {
        instance = this;
    }

    public void AddItem(ItemBase item, int count = 1)
    {
        // Check if the item already exists in the inventory
        ItemSlot existingSlot = slots.Find(slot => slot.Item == item);

        if (notificationText != null)
        {
            notificationQueue.Enqueue("Nakakuha ka ng " + item.name + " x" + count);
            if (!isDisplayingNotification)
            {
                StartCoroutine(DisplayNextNotification());
            }
        }


        if (existingSlot != null)
        {
            // Item already exists, increase the count
            existingSlot.Count += count;
        }
        else
        {
            // Item doesn't exist, create a new slot
            ItemSlot newSlot = new ItemSlot(item);
            newSlot.Count = count;

            // Mark the new item as seen
            newSlot.IsNew = true;

            if (ButtonManager.instance != null)
            {
                ButtonManager.instance.ShowNotif();
            }
            slots.Add(newSlot);

            // Display a notification for the new item

        }

        // Notify that the inventory has been updated
        onUpdated?.Invoke();
    }


    // Rest of your Inventory class remains the same...

    private IEnumerator DisplayNextNotification()
    {
        isDisplayingNotification = true;

        while (notificationQueue.Count > 0)
        {
            string notification = notificationQueue.Dequeue();
            notificationText.enabled = true;
            notificationText.text = ""; // Clear the text initially

            foreach (char letter in notification)
            {
                notificationText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }

            yield return new WaitForSeconds(fadeDuration);

            notificationText.enabled = false;
        }

        isDisplayingNotification = false;
    }






    public ItemBase useItem(int itemIndex)
    {
        var item = slots[itemIndex].Item;
        bool itemUsed = item.Use();
        if (itemUsed)
        {
            RemoveItem(item);
            return item;
        }
        return null;
    }



    public void RemoveItem(ItemBase item, int count = 1)
    {
        var itemSlot = slots.FirstOrDefault(slot => slot.Item == item);
        if (itemSlot != null)
        {
            itemSlot.Count -= count;
            if (itemSlot.Count <= 0)
            {
                slots.Remove(itemSlot);
            }
        }
        onUpdated?.Invoke();
    }



    public bool HasItem(ItemBase item, int count)
    {
        // Find the first slot that matches the item
        var slot = slots.FirstOrDefault(slot => slot.Item == item);

        // Check if the slot exists and has enough count
        if (slot != null && slot.Count >= count)
        {
            return true;
        }

        return false;
    }



    public static Inventory GetInventory()
    {
        return FindAnyObjectByType<PlayerController>().GetComponent<Inventory>();
    }


    public object CaptureState()
    {
        var saveData = new InventorySaveData()
        {
            items = slots.Select(i => i.GetSaveData()).ToList()

        };

        return saveData;

    }

    public void RestoreState(object state)
    {
        var saveData = state as InventorySaveData;

        slots = saveData.items.Select(i => new ItemSlot(i)).ToList();


        onUpdated?.Invoke();


    }
}



[Serializable]
public class ItemSlot
{
    [SerializeField] ItemBase item;
    [SerializeField] int count;
    [SerializeField] bool isNew; // Add this line

    public ItemSlot(ItemSaveData saveData)
    {
        item = ItemDB.GetObjectByName(saveData.name);
        count = saveData.count;
        isNew = saveData.isNew;
    }

    public ItemSaveData GetSaveData()
    {
        //Debug.Log("Before creating QuestSaveData: Base=" + item + ", Status=" + count);
        var saveData = new ItemSaveData()
        {
            name = item.name,
            count = count,
            isNew = isNew
        };
        //Debug.Log("After creating ItemSaveData: saveData=" + saveData);
        return saveData;
    }

    public ItemBase Item
    {
        get => item;
        set => item = value;
    }

    public ItemSlot(ItemBase item)
    {
        Item = item;
        count = 0;
        isNew = false; // Initialize isNew to false
    }

    public int Count
    {
        get => count;
        set => count = value;
    }

    public bool IsNew // Add this property
    {
        get => isNew;
        set => isNew = value;
    }
}

[Serializable]
public class ItemSaveData
{
    public string name;
    public int count;
    public bool isNew;
}

[Serializable]
public class InventorySaveData
{
    public List<ItemSaveData> items;

}

