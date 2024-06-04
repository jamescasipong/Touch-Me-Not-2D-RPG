using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialObjectx : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
