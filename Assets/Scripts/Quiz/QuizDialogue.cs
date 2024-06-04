using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.Playables;
using System.Linq;

public class QuizDialogue : MonoBehaviour, ISavable
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private TMP_Text tagName;
    [SerializeField] public Image icon;
    [SerializeField] private Sprite quizzerIcon;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    [Header("Text Typing Settings")]
    [SerializeField] private float textTypeSpeed = 0.03f; // Adjust the speed as needed


    

    private Coroutine textTypingCoroutine;
    private string currentSentence;

    private Story currentStory;

    public bool dialogueIsPlaying { get; private set; }
    public bool dialogueIsDisplaying;

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";


    public static QuizDialogue instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            //Debug.Log("Quiz works");
        }
        instance = this;

         
    }

    public static QuizDialogue GetInstance()
    {
        return instance;
    }
    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        continueButton.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    public void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ContinuStory();
            DisplayChoices();
        }

        if (dialoguePanel.activeInHierarchy)
        {
            ButtonHandlers.Instance.Joystick.SetActive(false);
        }

        else
        {
            ButtonHandlers.Instance.Joystick.SetActive(true);
        }
        // Toggle "Continue" button visibility based on the dialogue state
        ToggleContinueButtonVisibility();
    }




    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);



        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        GameController.Instance.PauseGame(true);
        ButtonHandlers.Instance.HideInteractObjects();
        ContinuStory();
    }

    public IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        GameController.Instance.PauseGame(false);
    }

    public void ContinuStory()
    {
        if (currentStory.canContinue)
        {
            currentSentence = currentStory.Continue();
            if (textTypingCoroutine != null)
            {
                StopCoroutine(textTypingCoroutine);
            }
            textTypingCoroutine = StartCoroutine(TypeText(currentSentence));
            DisplayChoices();

            HandleTags(currentStory.currentTags);

            if (!currentStory.canContinue)
            {
                if (currentStory.variablesState["initialize"] is int initialize)
                {
                    if (initialize == 1)
                    {
                        StartCoroutine(ExitDialogueMode());
                        CheckAndSwitchScene();
                        dialoguePanel.SetActive(false);
                    }
                }
            }
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
            CheckAndSwitchScene();
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 1)
            {
                Debug.Log("Tag inapproate " + tag);
            }

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            if (tagKey == SPEAKER_TAG)
            {
                tagName.text = tagValue;


                if (tagValue == "Player")
                {
                    Debug.Log("speaker=" + tagValue);
                    tagName.text = MainMenu.instance.playerInput.text;
                }
                else
                {
                    tagName.text = tagValue;
                }
            }
            
            else if (tagKey == PORTRAIT_TAG)
            {
                Debug.Log("portrait=" + tagValue);
                if ((MainMenu.instance.genderState == GenderState.Male && tagValue == "Player"))
                {
                    icon.sprite = Resources.Load<Sprite>("Boy");
                }
                else if ((MainMenu.instance.genderState == GenderState.Female && tagValue == "Player"))
                {
                    icon.sprite = Resources.Load<Sprite>("Girl");
                }
                /*else if ((MainMenu.instance.genderState == GenderState.Female))
                {
                    icon.sprite = Resources.Load<Sprite>("Girl");
                }
                else if ((MainMenu.instance.genderState == GenderState.Male))
                {
                    icon.sprite = quizzerIcon;
                }*/
            }

        }
    }


    private IEnumerator TypeText(string text)
    {
        dialogueText.text = "";
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textTypeSpeed);
        }
    }


    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices" + currentChoices.Count);
        }

        int index = 0;

        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (choiceIndex >= 0 && choiceIndex < currentChoices.Count)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);

            if (textTypingCoroutine != null)
            {
                StopCoroutine(textTypingCoroutine);
                dialogueText.text = currentSentence; // Show full sentence immediately
            }

            ContinuStory();
        }
        else
        {
            Debug.LogError("Invalid choice index: " + choiceIndex);
            Debug.Log("Available choices:");
            for (int i = 0; i < currentChoices.Count; i++)
            {
                Debug.Log("Choice " + i + ": " + currentChoices[i].text);
            }
        }
    }


    public void ToggleContinueButtonVisibility()
    {
        bool areChoicesVisible = AreChoicesVisible();
        continueButton.SetActive(!areChoicesVisible);
    }

    private bool AreChoicesVisible()
    {
        foreach (GameObject choice in choices)
        {
            if (choice.activeSelf)
            {
                return true;
            }
        }
        return false;
    }


    // Add a function to check the player's score and switch scenes if the score is 3
    public void CheckAndSwitchScene()
    {
        if (currentStory.variablesState["initialize"] is int initialize)
        {
            if (initialize == 2)
            {
                if (currentStory.variablesState["score"] is int score)
                {
                    if (score == 10)
                    {
                        Debug.Log("5");
                        if (DialogueTrigger.Instance.enablePlayDirector == true)
                        {
                            StartCoroutine(StartCutScene());
                            SavingSystem.i.Save("Chap2");
                        }
                        else
                        {
                            DialogueTrigger.Instance.ShowPortal();
                        }
                    }
                    else
                    {
                        Debug.Log("Not 5");
                        //DialogueTrigger.Instance.BacktoChap();
                        dialoguePanel.SetActive(false);
                    }
                }
            }
        }
    }

    IEnumerator StartCutScene()
    {
        yield return new WaitForSeconds(1f);
        UIsManager.instance.DisableUIS();


        DialogueTrigger.Instance.playerDirector.Play();

        yield return new WaitForSeconds((float)DialogueTrigger.Instance.playerDirector.duration);

        UIsManager.instance.EnableUIS();
        TeleportPlayerSample.Instance.TeleportPlayer();

        DialogueTrigger.Instance.playerDirector = null;
    }
    public object CaptureState()
    {
        Dictionary<string, object> state = new Dictionary<string, object>();

        // Save dialogue progress
        state["currentSentence"] = currentSentence;
        state["currentChoices"] = currentStory.currentChoices.Select(c => c.text).ToList();
        state["tagName"] = tagName.text;
        // ... save other relevant data

        return state;
    }

    public void RestoreState(object state)
    {
        if (state is Dictionary<string, object> dict)
        {
            if (dict.ContainsKey("currentSentence"))
            {
                currentSentence = (string)dict["currentSentence"];
                // Restore other relevant data
                dialogueText.text = currentSentence; // Show restored sentence
            }
            if (dict.ContainsKey("currentChoices"))
            {
                List<string> choices = (List<string>)dict["currentChoices"];
                // Restore choices...
            }
            if (dict.ContainsKey("tagName"))
            {
                tagName.text = (string)dict["tagName"];
                // Restore other relevant data related to speaker...
            }
            // Restore other saved data
        }
    }

}