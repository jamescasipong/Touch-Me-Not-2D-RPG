using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIsManager : MonoBehaviour
{
    public GameObject[] uIs;

    public static UIsManager instance;

    private void Awake()
    {
        instance = this;
    }
    public void DisableUIS()
    {
        foreach (GameObject ui in uIs)
        {
            ui.SetActive(false);
        }

        DialogManager.Instance.joystick.SetActive(false);
        DialogManager.Instance.notificationText.enabled = false;
    }

    public void EnableUIS()
    {
        foreach (GameObject ui in uIs)
        {
            ui.SetActive(true);
        }

        DialogManager.Instance.joystick.SetActive(true);
        DialogManager.Instance.notificationText.enabled = true;
    }
}
