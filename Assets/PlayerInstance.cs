using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstance : MonoBehaviour
{
    public GameObject[] playerNCamera;


    public static PlayerInstance instance;

    public void Awake()
    {
        instance = this;
    }

    public void DisablePlayerNCamera()
    {
        foreach (GameObject playerNCamera in playerNCamera)
        {
            playerNCamera.SetActive(false);
        }
    }

    public void EnablePlayerNCamera()
    {
        foreach (GameObject playerNCamera in playerNCamera)
        {
            playerNCamera.SetActive(true);
        }
    }
}
