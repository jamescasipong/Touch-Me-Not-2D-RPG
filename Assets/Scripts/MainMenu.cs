using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour, ISavable
{
    [SerializeField] public Button continueBtn;
    [SerializeField] public Button chap1Button;
    [SerializeField] public Button chap2Button;
    [SerializeField] public Button chap3Button;
    [SerializeField] public Button chap4Button;
    [SerializeField] public Button chap5Button;
    [SerializeField] public Button chap6Button;
    [SerializeField] public Button chap7Button;
    [SerializeField] public Button chap8Button;
    [SerializeField] public Button chap9Button;
    [SerializeField] public Button chap10Button;

    public bool transportoNewGame;
    public TMP_InputField playerInput;
    public GenderState genderState;

    public Button maleBtn;
    public Button femaleBtn;

    public GameObject tagNameConfirmation;

    public GameObject iconConfirmation;

    public static MainMenu instance { get; private set; }


    public void Male()
    {

        genderState = GenderState.Male;
    }

    public void Female()
    {

        genderState = GenderState.Female;
    }

    // Add a method to update the UI or any related elements when gender changes


    public void Update()
    {
        if (genderState == GenderState.Male)
        {
            maleBtn.interactable = false;
            femaleBtn.interactable = true;
        }
        else if (genderState == GenderState.Female)
        {

            femaleBtn.interactable = false;
            maleBtn.interactable = true;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            IntroCutScene();
        }
    }

    private void Awake()
    {
        genderState = GenderState.NonBinary;

        instance = this;
        string saveFile = "saveSlot2";
        continueBtn.interactable = File.Exists(SavingSystem.i.GetPath($"Gameplay_{saveFile}"));
        chap1Button.interactable = File.Exists(SavingSystem.i.GetPath("Gameplay_Chap1"));
        chap2Button.interactable = File.Exists(SavingSystem.i.GetPath("Gameplay_Chap2"));
        chap3Button.interactable = File.Exists(SavingSystem.i.GetPath("Gameplay_Chap3"));
        /*chap4Button.interactable = File.Exists(SavingSystem.i.GetPath("Gameplay_Chap4"));
        chap5Button.interactable = File.Exists(SavingSystem.i.GetPath("Gameplay_Chap5"));
        chap6Button.interactable = File.Exists(SavingSystem.i.GetPath("Gameplay_Chap6"));
        chap7Button.interactable = File.Exists(SavingSystem.i.GetPath("Gameplay_Chap7"));
        chap8Button.interactable = File.Exists(SavingSystem.i.GetPath("Gameplay_Chap8"));
        chap9Button.interactable = File.Exists(SavingSystem.i.GetPath("Gameplay_Chap9"));
        chap10Button.interactable = File.Exists(SavingSystem.i.GetPath("Gameplay_Chap10"));*/

        
    }


    public void Chapters(bool loadNewData, bool loadSavedData, bool loadChap1, bool loadChap2, bool loadChap3, bool loadChap4, bool loadChap5, bool loadChap6, bool loadChap7, bool loadChap8, bool loadChap9, bool loadChap10)
    {
        
        SavingSystem.i.loadNewData = loadNewData;
        SavingSystem.i.loadSavedData = loadSavedData;
        //Chap1
        SavingSystem.i.loadChap1 = loadChap1;
        //Chap2
        SavingSystem.i.loadChap2 = loadChap2;
        //Chap3
        SavingSystem.i.loadChap3 = loadChap3;
        //Chap4
        SavingSystem.i.loadChap4 = loadChap4;
        //Chap5
        SavingSystem.i.loadChap5 = loadChap5;
        //Chap6
        SavingSystem.i.loadChap6 = loadChap6;
        //Chap7
        SavingSystem.i.loadChap7 = loadChap7;
        //Chap8
        SavingSystem.i.loadChap8 = loadChap8;
        //Chap9
        SavingSystem.i.loadChap9 = loadChap9;
        //Chap10
        SavingSystem.i.loadChap10 = loadChap10;

        SceneManager.LoadScene("Gameplay");
    }

    public void NewGame()
    {
        SavingSystem.i.Save("state");


        if (genderState == GenderState.NonBinary)
        {
            Debug.Log("Can't");
        }
        else if (string.IsNullOrEmpty(playerInput.text))
        {
            Debug.Log("Can't");
        }

        else //(genderState == GenderState.Female || genderState == GenderState.Male && playerInput.text != null)
        {
            Chapters(true, false, false, false, false, false, false, false, false, false, false, false);
        }
    }


    public void IntroCutScene()
    {
        SavingSystem.i.Save("state");


        if (genderState == GenderState.NonBinary)
        {
            iconConfirmation.SetActive(true);
        }
        else if (string.IsNullOrEmpty(playerInput.text))
        {
            tagNameConfirmation.SetActive(true);
        }
        else if (genderState == GenderState.Female || genderState == GenderState.Male && playerInput.text != null)
        {
            if (!transportoNewGame)
            {
                SceneManager.LoadScene("CutSceneHouse");
            }
            else
            {
                NewGame();
            }
        }
    }

    public void LoadData()
    {
        SavingSystem.i.Load("state");
        Chapters(false, true, false, false, false, false, false, false, false, false, false, false);
    }

    public void LoadChap1()
    {
        SavingSystem.i.Load("state");

        Chapters(false, false, true, false, false, false, false, false, false, false, false, false);
    }

    // Methods for loading other chapters (Chap2 to Chap10)
    public void LoadChap2()
    {
        SavingSystem.i.Load("state");

        Chapters(false, false, false, true, false, false, false, false, false, false, false, false);
    }

    public void LoadChap3()
    {
        SavingSystem.i.Load("state");

        Chapters(false, false, false, false, true, false, false, false, false, false, false, false);
    }

    public void LoadChap4()
    {
        Chapters(false, false, false, false, false, true, false, false, false, false, false, false);
    }

    public void LoadChap5()
    {
        Chapters(false, false, false, false, false, false, true, false, false, false, false, false);
    }

    public void LoadChap6()
    {
        Chapters(false, false, false, false, false, false, false, true, false, false, false, false);
    }

    public void LoadChap7()
    {
        Chapters(false, false, false, false, false, false, false, false, true, false, false, false);
    }

    public void LoadChap8()
    {
        Chapters(false, false, false, false, false, false, false, false, false, true, false, false);
    }

    public void LoadChap9()
    {
        Chapters(false, false, false, false, false, false, false, false, false, false, true, false);
    }

    public void LoadChap10()
    {
        Chapters(false, false, false, false, false, false, false, false, false, false, false, true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ShowJoystick()
    {
        if (ButtonHandlers.Instance != null)
            ButtonHandlers.Instance.ShowJoystick();
    }

    public object CaptureState()
    {
        MainMenuState state = new MainMenuState
        {
            GenderState = genderState,
            PlayerInput = playerInput != null ? playerInput.text : "" // Save the playerInput text
        };
        return state;
    }

    // Add a method to restore the state
    public void RestoreState(object state)
    {
        if (state is MainMenuState mainMenuState)
        {
            genderState = mainMenuState.GenderState;
            if (playerInput != null)
                playerInput.text = mainMenuState.PlayerInput; // Restore the playerInput text if the field exists
        }
    }
}

// Define a serializable data structure to capture the MainMenu state
[System.Serializable]
public class MainMenuState
{
    public GenderState GenderState;
    public string PlayerInput;
}
