using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopeMenu", menuName = "Scriptable objects/New Shop Item", order = 1)]
public class ShopItemSO : ItemBase
{
    public int baseCost;
    public int Cost => baseCost;

}
