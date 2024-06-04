using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] string sceneToLoad = "MainMenu";

    public AudioSource clickSoundEffect;
    public void ClickSoundEffect()
    {
        clickSoundEffect.Play();
    }

    private void Start()
    {
        
    }
    //Pause Menu
    public void Save()
    {
        if (SavingSystem.i != null)
            SavingSystem.i.Save("saveSlot2");

    }

    public void Load()
    {
        if (SavingSystem.i != null)
            SavingSystem.i.loadNewData = false;
        SavingSystem.i.Load("saveSlot2");
        SavingSystem.i.Load("saveSlot2");
    }

    public void LoadBackToMainMenu()
    {
        SceneManager.LoadScene(sceneToLoad);
        if (SavingSystem.i != null)
        {
            SavingSystem.i.loadNewData = false;
            SavingSystem.i.loadSavedData = false;
        }
    }

    //Second MainMenu
    public void LoadNewGame()
    {
        if (SavingSystem.i != null)
            SavingSystem.i.Load("NewGame");
    }
}