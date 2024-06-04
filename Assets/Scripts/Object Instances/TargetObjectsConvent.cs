using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObjectsConvent : MonoBehaviour
{
    public static TargetObjectsConvent instance;
    public Transform[] targetConvent;

    private void Awake()
    {
        instance = this;
    }
}
