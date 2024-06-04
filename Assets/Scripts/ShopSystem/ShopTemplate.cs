using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TMP_Text;

public class ShopTemplate : MonoBehaviour
{
    [SerializeField] public Text titleText;
    
    [SerializeField] public Text descriptionText;
    [SerializeField] public Text costText;
    [SerializeField] public Image icon;


    [SerializeField] public int count;
    [SerializeField] public Text countText;

}
