using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObjectsInstance : MonoBehaviour
{
    public static TargetObjectsInstance instance;
    public Transform[] targetNPCS;
    public Transform[] targetKapitanHouse;
    public Transform[] targetHotel;
    public Transform[] targetShop;


    private void Awake()
    {
        instance = this;
    }
}
