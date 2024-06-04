using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SFXMainMenu : MonoBehaviour, ISavable
{
    public AudioSource clickSoundEffect;
    public AudioSource bgm;
    public Slider volumeSlider;
    public Toggle bgmToggle;
    public Toggle seToggle;

    private bool bgmMuted;
    private bool seMuted;
    private float savedVolume;

    private void Start()
    {
        volumeSlider.value = bgm.volume;

        // Add a listener to the slider that calls a method when the value changes
        volumeSlider.onValueChanged.AddListener(ChangeVolume);

        // Add listeners to the toggle buttons for enabling/disabling the audio source
        bgmToggle.onValueChanged.AddListener(ToggleBGM);
        seToggle.onValueChanged.AddListener(ToggleSE);



        if (File.Exists(SavingSystem.i.GetPath("MainMenu_Sounds")))
        {
            SavingSystem.i.Load("Sounds");

            Debug.Log("Loaded");
        }

    }


    public void SaveSoundState()
    {
        SavingSystem.i.Save("Sounds");

    }

    // Method to change the volume of the AudioSource
    public void ChangeVolume(float volume)
    {
        // Update the volume of the AudioSource based on the slider value
        bgm.volume = volume;

        
        clickSoundEffect.volume = volume;
        savedVolume = volume; // Save the current volume
    }

    // Method to toggle the background music
    public void ToggleBGM(bool isOn)
    {
        
        bgm.mute = !isOn;
        bgmMuted = !isOn; // Save the mute state of BGM
    }

    // Method to toggle the sound effects
    public void ToggleSE(bool isOn)
    {
        //if (AudioSinglenton.instance != null)
        //AudioSinglenton.GetAudio().bgmEffects.mute = !isOn;
        clickSoundEffect.mute = !isOn;
        seMuted = !isOn; // Save the mute state of sound effects
    }

    public object CaptureState()
    {
        // Capture the necessary state information
        return new SavedState
        {
            BgmMuted = bgmMuted,
            SeMuted = seMuted,
            Volume = savedVolume
        };
    }

    public void RestoreState(object state)
    {
        if (state is SavedState savedState)
        {
            // Restore the state based on the provided savedState object
            bgmMuted = savedState.BgmMuted;
            seMuted = savedState.SeMuted;
            savedVolume = savedState.Volume;

            // Apply the restored state to the audio sources and UI elements
            bgm.mute = bgmMuted;
            clickSoundEffect.mute = seMuted;
            volumeSlider.value = savedVolume;
            bgm.volume = savedVolume;
            clickSoundEffect.volume = savedVolume;
        }
    }

    // Define a custom class to hold the saved state information
    
}

[System.Serializable]
public class SavedState
{
    public bool BgmMuted;
    public bool SeMuted;
    public float Volume;
}

