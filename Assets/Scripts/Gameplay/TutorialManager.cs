using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;


public class TutorialManager : MonoBehaviour, Interactable, ISavable
{
    public GameObject[] tutorialPopups;
    public int currentPopupIndex = -1;
    public GameObject statusIcon; 
    public GameObject canvasTutorial;
    [SerializeField] Dialog dialog;
    [SerializeField] public Dialog closeBag;
    [SerializeField] public Dialog closeMainMenu;
    [SerializeField] public Dialog claimRewards;
    [SerializeField] public Dialog startDialog;
    [SerializeField] Dialog walk;
    [SerializeField] GameObject locationPortal;
    private bool dialogState;
    public TutorialManager tutorialManager;
    public NPCController addQuesToNPC;

    Transform initiator;

    private bool isWalkingDown = false;
    private bool isWalkingUp = false;
    private bool isWalkingLeft = false;
    private bool isWalkingRight = false;

    private float walkDownTimer = 0f;
    private float walkingUpTimer = 0f;
    private float walkLeftTimer = 0f;
    private float walkRightTimer = 0f;
    private float requiredWalkingTime = 0.3f; // Time required to walk up to proceed

    public static TutorialManager instance;

    bool stopRepeat;
    bool stopSaveRepeat;
    private void Awake()
    {
        instance = this;
    }
    public IEnumerator DialogLauncher(Dialog dialog, Action onComplete = null)
    {
        yield return DialogManager.Instance.ShowDialog(dialog);
        onComplete?.Invoke(); // Invoke the onComplete callback when the dialog is finished
    }


    public IEnumerator Interact(Transform initiator)
    {
        PlayerController playerController = initiator.GetComponent<PlayerController>();

        if (playerController != null && playerController.Character != null && playerController.Character.Animator != null)
        {
            playerController.Character.Animator.IsMoving = false;
        }

        if (!dialogState)
        {
            yield return DialogManager.Instance.AutomaticallyShowDialog(dialog);
            currentPopupIndex++;
            ShowCurrentPopup();
            dialogState = true;
            
        }
    }

    IEnumerator StartDialog(Dialog dialog)
    {
        yield return new WaitForSeconds(4f);
        yield return DialogManager.Instance.AutomaticallyShowDialog(dialog);

    }
    void Update()
    {
        if (currentPopupIndex == -1)
        {
            statusIcon.SetActive(true);

            if (!stopRepeat)
            {
                stopRepeat = true;
                StartCoroutine(StartDialog(startDialog));
            }
        }
        else
        {
            statusIcon.SetActive(false);
        }

        if (currentPopupIndex == 0)
        {
            GameObject initiator = GameObject.FindGameObjectWithTag("Player");
            PlayerController playerController = initiator.GetComponent<PlayerController>();
            ButtonHandlers.Instance.arrowIndicatorJoystick.SetActive(true);


            isWalkingUp = playerController.input.y > 0;
            isWalkingDown = playerController.input.y < 0;
            isWalkingLeft = playerController.input.x < 0;
            isWalkingRight = playerController.input.x > 0;


            if (isWalkingUp)
            {
                walkingUpTimer += Time.deltaTime;
            }


            if (isWalkingDown)
            {
                walkDownTimer += Time.deltaTime;
            }

            if (isWalkingLeft)
            {
                walkLeftTimer += Time.deltaTime;
            }

            if (isWalkingRight)
            {
                walkRightTimer += Time.deltaTime;
            }


            if (walkDownTimer >= requiredWalkingTime && walkingUpTimer >= requiredWalkingTime && walkLeftTimer >= requiredWalkingTime && walkRightTimer >= requiredWalkingTime)
            {
                playerController.Character.Animator.IsMoving = false;
                ButtonHandlers.Instance.arrowIndicatorJoystick.SetActive(false);
                StartCoroutine(DialogManager.Instance.AutomaticallyShowDialog(walk));
                currentPopupIndex++;
                ShowCurrentPopup();
            }
        }

        else if (currentPopupIndex == 1)
        {
            
            GameController.Instance.SelfActiveInventory();

            
        }
        else if (currentPopupIndex == 2)
        {
            GameController.Instance.SelfInActiveInventory();

            
        }

        else if (currentPopupIndex == 3)
        {
            ButtonHandlers.Instance.MenuPanelSelfActive();
        }

        else if (currentPopupIndex == 4)
        {
            ButtonHandlers.Instance.MenuPanelInSelfActive();
        }

        else if (currentPopupIndex == 5)
        {
            ButtonHandlers.Instance.QuestMenuSelfActive();
        }

        else if (currentPopupIndex == 6)
        {
            ButtonHandlers.Instance.QuestMenuInSelfActive();
        }


        else if (currentPopupIndex == 7 /*6*/)
        {

            NPCController npcController = addQuesToNPC.GetComponent<NPCController>();

            if (npcController != null)
            {

                npcController.questToStart2 = Quest2DB.GetObjectByName("Kabanata");
            }

            if (!stopSaveRepeat)
            {
                SavingSystem.i.Save("Chap1");
                stopSaveRepeat = true;
            }

            if (npcController.hasInteracted == true)
            {
                Debug.Log("works");
                currentPopupIndex++;
                ShowCurrentPopup();
            }
        }

    }

    

    public void ShowCurrentPopup()
    {
        

        if (currentPopupIndex < tutorialPopups.Length)
        {
            for (int i = 0; i < tutorialPopups.Length; i++)
            {
                if (i == currentPopupIndex)
                {
                    tutorialPopups[i].SetActive(true);

                }

                else
                   
                {
                    tutorialPopups[i].SetActive(false);

                }
            }
        }
        else
        {
            // Tutorial finished
            Debug.Log("Tutorial Finished");
            canvasTutorial.SetActive(false);

            
            locationPortal.SetActive(true);
            tutorialManager.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            if (dialog != null)
            {
                ButtonHandlers.Instance.InteractObjects();
            }

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ButtonHandlers.Instance.HideInteractObjects();
        }
    }

    public object CaptureState()
    {
        Dictionary<string, object> state = new Dictionary<string, object>();
        state.Add("currentPopupIndex", currentPopupIndex);
        state.Add("dialogState", dialogState);
        // Add more variables to 'state' as needed

        return state;
    }

    public void RestoreState(object state)
    {
        if (state is Dictionary<string, object> tutorialState)
        {
            if (tutorialState.ContainsKey("currentPopupIndex"))
            {
                currentPopupIndex = (int)tutorialState["currentPopupIndex"];
                ShowCurrentPopup(); // Update UI based on restored index
            }

            if (tutorialState.ContainsKey("dialogState"))
            {
                dialogState = (bool)tutorialState["dialogState"];
            }

            // Restore more variables as needed
        }
    }
}
