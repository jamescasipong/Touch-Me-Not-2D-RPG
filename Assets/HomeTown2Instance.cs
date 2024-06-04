using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeTown2Instance : MonoBehaviour
{
    [SerializeField]public GameObject[] lights2d;
    public static HomeTown2Instance instance;

    private void Awake()
    {
        instance = this;
    }

    public void DisableLights()
    {
        foreach (GameObject light in lights2d)
        {
            light.gameObject.SetActive(false);
        }
    }

    public void EnableLights()
    {
        foreach (GameObject light in lights2d)
        {
            light.gameObject.SetActive(true);
        }
    }
}
