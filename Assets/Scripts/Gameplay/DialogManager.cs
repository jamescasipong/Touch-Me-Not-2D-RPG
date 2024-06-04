using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Text = TMPro.TMP_Text;


public class DialogManager : MonoBehaviour, ISavable
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] int letterPerSecond;
    [SerializeField] private Text skipText;
    [SerializeField] public GameObject joystick;
    public bool joystickBool;
    public bool dialogShowing;
    private bool sceneChangeHandled = false;
    private bool skipDialog = false;
    public bool controlStateofJoystick = false;
    public event Action OnShowDialog;
    public event Action OnCloseDialog;

    private bool isDialogShowing;
    [SerializeField] private AudioSource typingAudioSource;
    [SerializeField] private AudioClip typingSound;
    [SerializeField] private Text characterNameText;
    [SerializeField] private Image characterIconImage;


    
    // Other variables and methods...


    public bool DialogShowing
    {
        get { return isDialogShowing; }
        set
        {
            isDialogShowing = value;
            HandleDialogShowingChanged();
        }
    }

    private void HandleDialogShowingChanged()
    {
        //if (Application.isMobilePlatform)
        //{
            joystick.SetActive(!DialogShowing);
        //}

    }


    public TMP_Text notificationText; // Reference to the Text UI element to display notifications.

    private void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "MainMenu" || currentScene.name == "CutSceneHouse" || currentScene.name == "CutSceneRoom" || currentScene.name == "CutsceneHallway")
        {
            notificationText.gameObject.SetActive(false);


                joystick.SetActive(false);


        }
        else
        {
            if (!controlStateofJoystick)
            {
                if (notificationText != null)
                    notificationText.gameObject.SetActive(true);

  
                joystick.SetActive(!DialogShowing);
            }

        }

        // Check for input to skip the dialog when it's showing.
        if (DialogShowing)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                skipDialog = true;
            }
        }



    }





    Action onDialogFinished;

    public QuestBase Base { get; private set; }
    public DialogManager(QuestBase _base)
    {
        Base = _base;
    }

    public static DialogManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /*public void HideText(string text)
    {
        StartCoroutine(HideNotifText(text));
    }*/


    public bool IsShowing { get; private set; }


    public IEnumerator ShowDialogText(string text, bool waitForInput = true, bool autoClose = true)
    {
        DialogShowing = true;
        OnShowDialog?.Invoke();
        IsShowing = true;
        dialogBox.SetActive(true);

        yield return TypeDialog(text);
        if (waitForInput)
        {
            yield return new WaitUntil(() => Input.GetKey(KeyCode.E) || Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began));
        }
        if (autoClose)
        {
            CloseDialog();
        }
        DialogShowing = false;
    }

    


    public IEnumerator ShowNotifText(string text)
    {
        IsShowing = true;
        notificationText.text = ""; // Clear the existing text
        notificationText.enabled = true;
        yield return TypeNotif(text);
        //ButtonManager.instance.ShowNotifQuest();
    }


    public void HideTextNotif()
    {
        StartCoroutine(HideNotifText(notificationText.text));
    }
    public IEnumerator HideNotifText(string text)
    {
        IsShowing = false;
        notificationText.enabled = false;
        yield return TypeNotif(text);
    }

    public IEnumerator ShowDialog(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();

        DialogShowing = true; // Set DialogShowing to true when a dialog is being shown
        OnShowDialog?.Invoke();

        IsShowing = true;


        dialogBox.SetActive(true);
        if (skipText != null)
            skipText.gameObject.SetActive(true);

        foreach (var line in dialog.NPCLines)
        {
            if (line.CharacterName == "Player")
            {
                characterNameText.text = MainMenu.instance.playerInput.text;

                if (MainMenu.instance.genderState == GenderState.Female)
                {
                    characterIconImage.sprite = Resources.Load<Sprite>("Girl");
                }
                else if (MainMenu.instance.genderState == GenderState.Male)
                {
                    characterIconImage.sprite = Resources.Load<Sprite>("Boy");
                }
            }
            else
            {
                characterNameText.text = line.CharacterName;
                characterIconImage.sprite = line.CharacterIcon;
            }

            yield return TypeDialog(line.Text);

            

            // Wait for player input to proceed to the next NPC line
            bool continueDialog = false;
            while (!continueDialog)
            {
                // Check for keyboard input
                if (Input.GetKeyDown(KeyCode.E))
                {
                    continueDialog = true;
                }

                // Check for mouse click or touch input
                if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
                {
                    continueDialog = true;
                }

                if (skipDialog)
                {
                    dialogText.text = line.Text;
                    skipDialog = false;
                    break;
                }

                yield return null;
            }
        }




        // Close the dialog box or perform other closing actions here
        dialogBox.SetActive(false);
        IsShowing = false;
        DialogShowing = false; // Set DialogShowing to false when the dialog is closed

        // Invoke finishing actions if there are any


        OnCloseDialog?.Invoke();
    }
    public IEnumerator AutomaticallyShowDialog(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();

        DialogShowing = true; // Set DialogShowing to true when a dialog is being shown
        OnShowDialog?.Invoke();

        IsShowing = true;

        dialogBox.SetActive(true);
        if (skipText != null)
            skipText.gameObject.SetActive(true);

        foreach (var line in dialog.NPCLines)
        {
            if (line.CharacterName == "Player")
            {
                characterNameText.text = MainMenu.instance.playerInput.text;

                if (MainMenu.instance.genderState == GenderState.Female)
                {
                    characterIconImage.sprite = Resources.Load<Sprite>("Girl");
                }
                else if (MainMenu.instance.genderState == GenderState.Male)
                {
                    characterIconImage.sprite = Resources.Load<Sprite>("Boy");
                }
            }
            else
            {
                characterNameText.text = line.CharacterName;
                characterIconImage.sprite = line.CharacterIcon;
            }

            yield return TypeDialog(line.Text);

            float elapsedTime = 0f;
            const float timeToWait = 2f; // Wait for 2 seconds after text completion
            while (elapsedTime < timeToWait)
            {
                // Check for skip conditions
                if (Input.GetKeyDown(KeyCode.E) ||
                    Input.GetMouseButtonDown(0) ||
                    (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) ||
                    skipDialog)
                {
                    skipDialog = false;
                    break;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(0.1f); // Small delay before moving to the next line
        }

        // Close the dialog box or perform other closing actions here
        dialogBox.SetActive(false);
        IsShowing = false;
        DialogShowing = false; // Set DialogShowing to false when the dialog is closed

        OnCloseDialog?.Invoke();
    }

    public void CloseDialog()
    {
        DialogShowing = false; // Set DialogShowing to false when the dialog is closed
        dialogBox.SetActive(false);
        IsShowing = false;
        OnCloseDialog?.Invoke();
        // ... rest of the method
    }



    public void HandleUpdate()
    {

    }



    public IEnumerator TypeDialog(string line)
{
    dialogText.text = "";

    // If skipDialog is true, instantly display the entire text
    if (skipDialog)
    {
        dialogText.text = line;
    }
    else
    {
        typingAudioSource.PlayOneShot(typingSound); // Play typing sound at the beginning

        foreach (var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / letterPerSecond);

            // Check if skipDialog flag is set while typing
            if (skipDialog)
            {
                // Display the entire text and exit the loop
                dialogText.text = line;
                break;
            }
        }

        typingAudioSource.Stop(); // Stop typing sound when typing is finished
    }
}



    public IEnumerator TypeNotif(string line)
    {
        notificationText.text = "";
        //foreach (var letter in line.ToCharArray())
        //{
        notificationText.text = line;//+= letter;
                                     //yield return new WaitForSeconds(1f / letterPerSecond);
                                     //}
        yield return null;
    }

    public object CaptureState()
    {
        // Create a dictionary to capture the state information.
        Dictionary<string, object> state = new Dictionary<string, object>();

        // Capture the text displayed in the notificationText.
        state["notificationText"] = notificationText.text;

        // Capture whether the notificationText is enabled or disabled.
        state["isNotificationTextEnabled"] = notificationText.enabled;

        return state;
    }


    public void RestoreState(object state)
    {
        if (state is Dictionary<string, object> stateDict)
        {
            // Restore the text displayed in the notificationText.
            if (stateDict.ContainsKey("notificationText"))
            {
                notificationText.text = "";
                notificationText.text = stateDict["notificationText"].ToString();
            }

            // Restore whether the notificationText is enabled or disabled.
            if (stateDict.ContainsKey("isNotificationTextEnabled"))
            {
                bool isNotificationTextEnabled = (bool)stateDict["isNotificationTextEnabled"];
                notificationText.enabled = isNotificationTextEnabled;
            }
        }
    }
}