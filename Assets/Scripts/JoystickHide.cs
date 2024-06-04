using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickHide : MonoBehaviour
{
    [SerializeField] private GameObject joystickhide;

    public static JoystickHide instance;
    
    void Awake()
    {
        instance = this;

        if (Application.isMobilePlatform)
        {
            // If it's a mobile platform, set the button text to "Interact"
            joystickhide.SetActive(true);
        }
        else
        {
            // If it's not a mobile platform (e.g., PC), set the button text to "Interact (F)"
            joystickhide.SetActive(false);
        }
    }
}
