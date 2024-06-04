using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialObjects : MonoBehaviour
{
    private static EssentialObjects instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
