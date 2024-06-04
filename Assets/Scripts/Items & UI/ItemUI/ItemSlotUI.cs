using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TMP_Text;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] Text countText;
    [SerializeField] Text nameText;


    public Text NameText => nameText;
    public Text CountText => countText;
    public void setData(ItemSlot itemSlot)
    {
        nameText.text = itemSlot.Item.Name;
        countText.text = $"X {itemSlot.Count}";

    }
}
