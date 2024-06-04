using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using TMPro;


[Serializable]
public struct LightingSaveData
{
    public float currentTime;
    public float globalLightColorR;
    public float globalLightColorG;
    public float globalLightColorB;
    public float globalLightColorA;
    public float dayDuration;
}

public class Lighting : MonoBehaviour, ISavable
{
    public Light2D globalLight;
    public float dayDuration = 1440.0f; // Change dayDuration to 180 seconds
    private float currentTime = 0.0f;

    public Color dayColor = new Color(0.467f, 0.831f, 1.0f);
    public Color nightColor = Color.white;

    public static Lighting instance;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject); // Ensures only one instance exists
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); // Keeps the instance between scene changes
        }

    }

    private void Start()
    {
        //SetTime(0.25f);

    }

    void Update()
    {
        currentTime += Time.deltaTime / dayDuration;
        if (currentTime >= 1.0f)
        {
            currentTime = 0.0f;
        }

        UpdateLighting();
    }

    public void SetTime(float time)
    {
        // Ensure the time value is within the range of 0 to 1  
        time = Mathf.Clamp01(time);

        // Calculate the total minutes in a day
        int totalMinutesInDay = 24 * 60;

        // Calculate the total minutes for the specified time
        int minutes = Mathf.RoundToInt(time * totalMinutesInDay);

        // Calculate the hours and minutes
        int hours = minutes / 60;
        int minutesPart = minutes % 60;

        // Determine whether it's AM or PM
        string amPm = (hours < 12) ? "AM" : "PM";

        // Convert 0 AM and 12 PM to 12 AM and 12 PM
        if (hours == 0)
        {
            hours = 12;
        }

        // Update the current time
        currentTime = time;

        // Calculate the time in 24-hour format
        float timeIn24HourFormat = (hours % 12) + (minutesPart / 60.0f) + (amPm == "PM" ? 12.0f : 0.0f);

        // Calculate the day duration based on the new time (assuming a full day is 24 hours)
        dayDuration = 24 * 60; // 24 hours in minutes

        // Update the lighting based on the new time
        UpdateLighting();

        // Adjust the clock time text to match the new time
        string formattedTime = string.Format("{0:00}:{1:00} {2}", hours, minutesPart, amPm);
        if (ButtonManager.instance != null)
            ButtonManager.instance.ClockInstance(formattedTime);
    }




    private void UpdateLighting()
    {
        float lightIntensity = Mathf.Lerp(0.25f, 1.0f, Mathf.Abs(currentTime - 0.5f) * 2);
        globalLight.intensity = lightIntensity;

        Color targetColor;

        // Check if the time is around 6:00 AM
        if (currentTime >= 0.25f && currentTime < 0.35f)
        {
            // Transition from night to day
            float t = (currentTime - 0.25f) / 0.1f;
            targetColor = Color.Lerp(nightColor, dayColor, t);
        }
        else if (lightIntensity > 0.6f)
        {
            // Night
            targetColor = nightColor;
        }
        else
        {
            // Transition from day to night
            float t = (lightIntensity - 0.25f) / 0.35f;
            targetColor = Color.Lerp(dayColor, nightColor, t);
        }

        globalLight.color = Color.Lerp(globalLight.color, targetColor, Time.deltaTime * 0.5f);

        // Calculate hours and minutes
        int totalMinutes = Mathf.FloorToInt(currentTime * 24 * (dayDuration / 1440.0f) * 60);
        int hours = totalMinutes / 60;
        int minutes = totalMinutes % 60;

        // Determine whether it's AM or PM
        string amPm = (hours < 12) ? "PM" : "AM";

        // Convert 0 AM and 12 PM to 12 AM and 12 PM
        if (hours == 0)
        {
            hours = 12;
        }

        // Adjust the clock time text to match the new dayDuration
        string formattedTime = string.Format("{0:00}:{1:00} {2}", hours, minutes, amPm);
        if (ButtonManager.instance != null)
            ButtonManager.instance.ClockInstance(formattedTime);

        GameObject[] spotlights = GameObject.FindGameObjectsWithTag("Spotlights");


        if (lightIntensity > 0.6f)
        {
            if (HomeTown3Instance.instance != null)
                HomeTown3Instance.instance.DisableLights();
            if (HomeTown2Instance.instance != null)
                HomeTown2Instance.instance.DisableLights();
            if (HomeTown1Instance.instance != null)
                HomeTown1Instance.instance.DisableLights();

            if (ButtonManager.instance != null)
                PlayerController.instance.PlayerLightDisable();
        }
        else
        {
            if (HomeTown3Instance.instance != null)
                HomeTown3Instance.instance.EnableLights();
            if (HomeTown2Instance.instance != null)
                HomeTown2Instance.instance.EnableLights();
            if (HomeTown1Instance.instance != null)
                HomeTown1Instance.instance.EnableLights();

            if (ButtonManager.instance != null)
                PlayerController.instance.PlayerLightEnable();
        }

    }





    public object CaptureState()
    {
        LightingSaveData saveData;
        saveData.currentTime = currentTime;
        saveData.globalLightColorR = globalLight.color.r;
        saveData.globalLightColorG = globalLight.color.g;
        saveData.globalLightColorB = globalLight.color.b;
        saveData.globalLightColorA = globalLight.color.a;
        saveData.dayDuration = dayDuration;
        return saveData;
    }

    public void RestoreState(object state)
    {
        if (state is LightingSaveData saveData)
        {
            currentTime = saveData.currentTime;
            globalLight.color = new Color(saveData.globalLightColorR, saveData.globalLightColorG, saveData.globalLightColorB, saveData.globalLightColorA);
            dayDuration = saveData.dayDuration;
            UpdateLighting();
        }
    }
}