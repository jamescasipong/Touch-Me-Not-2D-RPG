using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Text = TMPro.TMP_Text;

public class InventoryUIState : MonoBehaviour
{
    [SerializeField] GameObject itemList;
    [SerializeField] ItemSlotUI slots;
    [SerializeField] Image ItemIcon;
    [SerializeField] Text itemDescription;
    public Sprite yourBlankOrEmptySprite;



    public int SelectedItem { get; set; }
    InventoryUIState state;

    List<ItemSlotUI> slotUIList;
    Inventory inventory;

    private void Awake()
    {
        inventory = Inventory.GetInventory();
    }

    private void Start()
    {
        UpdateItemList();
        // Select the first item by default
        SelectedItem = 0;
        UpdateItemSelection();

        inventory.onUpdated += UpdateItemList;
    }

    void UpdateItemList()
    {
        foreach (Transform child in itemList.transform)
            Destroy(child.gameObject);

        slotUIList = new List<ItemSlotUI>();
        int index = 0;
        foreach (var itemSlot in inventory.Slots)
        {
            var slotUIObj = Instantiate(slots, itemList.transform);
            slotUIObj.setData(itemSlot);


            // Attach an event listener for mouse click
            slotUIObj.gameObject.AddComponent<SlotClickHandler>().Initialize(index, this);

            slotUIList.Add(slotUIObj);
            index++;
        }

        // Update the selected item UI (if there are items in the inventory)
        if (inventory.Slots.Count > 0)
        {
            var selectedItem = inventory.Slots[Mathf.Clamp(SelectedItem, 0, inventory.Slots.Count - 1)];
            ItemIcon.sprite = selectedItem.Item.Icon;
            itemDescription.text = selectedItem.Item.Description;
        }
        else
        {
            // If the inventory is empty, display appropriate UI
            ItemIcon.sprite = yourBlankOrEmptySprite; // Clear the icon
            itemDescription.text = "Sa sandaling buksan mo ang iyong imbentaryo, walang mabubuksang mga bagay na nakatago. Walang laman, walang tanging kalakip na anuman.\r\n\r\n\r\n\r\n\r\n\r\n"; 
        }
    }




    public void HandleUpdate(Action onBack)
    {
        int prevSelection = SelectedItem;

        if (Input.GetKeyDown(KeyCode.X))
        {
            onBack?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ++SelectedItem;
            UpdateItemSelection();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            --SelectedItem;
            UpdateItemSelection();
        }
    }

    public void UpdateItemSelection()
    {
        if (inventory.Slots.Count == 0)
        {
            // Handle the case where the inventory is empty
            SelectedItem = -1;
            ItemIcon.sprite = yourBlankOrEmptySprite;
            itemDescription.text = "Sa sandaling buksan mo ang iyong imbentaryo, walang mabubuksang mga bagay na nakatago. Walang laman, walang tanging kalakip na anuman.";
            return;
        }

        SelectedItem = Mathf.Clamp(SelectedItem, 0, inventory.Slots.Count - 1);

        for (int i = 0; i < slotUIList.Count; i++)
        {
            if (i == SelectedItem)
            {
                slotUIList[i].NameText.color = GlobalSettings.i.HighlightedColor;
            }
            else
            {
                slotUIList[i].NameText.color = Color.black;
            }

            // Check if the item is new and update UI accordingly
            if (inventory.Slots[i].IsNew)
            {
                // Set the item as seen and update the UI
                inventory.Slots[i].IsNew = false;
                UpdateItemSelection(); // Recursive call to ensure all new items are marked as seen
            }


        }

        var item = inventory.Slots[SelectedItem];
        ItemIcon.sprite = item.Item.Icon;
        itemDescription.text = item.Item.Description;

    }





    // This class handles mouse clicks on inventory slots
    public class SlotClickHandler : MonoBehaviour, IPointerClickHandler
    {
        private int slotIndex;
        private InventoryUIState inventoryUI;

        public void Initialize(int index, InventoryUIState ui)
        {
            slotIndex = index;
            inventoryUI = ui;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                // Left mouse button click selects the item
                inventoryUI.SelectedItem = slotIndex;
                inventoryUI.UpdateItemSelection();
            }
        }
    }
}
