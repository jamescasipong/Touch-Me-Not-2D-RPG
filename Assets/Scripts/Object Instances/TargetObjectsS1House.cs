using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObjectsS1House : MonoBehaviour
{
    public static TargetObjectsS1House instance;
    public Transform[] targetGlass;

    private void Awake()
    {
        instance = this;
    }
}
