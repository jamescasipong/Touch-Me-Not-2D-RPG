using UnityEngine;
using System.Collections; // Required for IEnumerator
using System.Collections.Generic;
using UnityEngine.Playables;

public class NPCController : MonoBehaviour, Interactable, ISavable
{
    [SerializeField] Dialog dialog;
    //[SerializeField] GameObject interactButton;
    //[SerializeField] public Text interactText;

    [Header("Quest Start for Quest1")]
    [SerializeField] public QuestBase quesToStart;
    [Header("Quest Early Completion for Quest1")]
    [SerializeField] QuestBase questToComplete;

    [Header("Quest Start for Quest2")]
    [SerializeField] public QuestBase2 questToStart2;
    [Header("Quest Early Completion for Quest2")]
    [SerializeField] QuestBase2 questToComplete2;

    [Header("Disable or Enable Status")]
    [SerializeField] GameObject questStatusIcon;

    
    [Header("Disable HideNotif")]
    [SerializeField] bool disableHideNotif = false;

    [Header("NPC Movement")]
    [SerializeField] List<Vector2> movementPattern;
    [SerializeField] float timeBetweenPattern;

    [Header("Dis/Enable Fade with String")]
    //[SerializeField] string toDoFinish;
    public string questDone;
    public bool showFade = false;

    [Header("Disable Dialogs and StatusInteract")]
    public bool disableDialog;
    [SerializeField] bool disableStatusInteract;

    

    public PlayableDirector completionTimeline;

    public bool hasInteracted = false;
    [SerializeField] NPCController npcController;
    [SerializeField] bool dialogEnable = false;

    private bool isQuestStatusIconActive;
    float idleTimer = 0f;
    NPCState state;
    int currentPattern = 0;

    Quest activeQuest { get; set; }


    //private bool shouldCheckNotification = false;


    
    public NPCController(Quest _base)
    {
        activeQuest = _base;

    }

    Quest2 activeQuest2 { get; set; }

    //[SerializeField] List<Sprite> sprites;
    SpriteAnimator spriteAnimator;

    ItemGiver itemGiver;

    Character character;
    public QuestBase Base { get; private set; }



    private bool isNotificationHidden = false;


    TutorialManager tutorialManagers;

    public NPCController(QuestBase _base)
    {
        Base = _base;
    }


    /*public IEnumerator CheckAndHideNotification()
    {
        if (DialogManager.Instance != null)
        {
            var inventory = Inventory.GetInventory();
            if (activeQuest != null && activeQuest.Status == QuestStatus.Started)
            {
                if (inventory.HasItem(activeQuest.Base.RequiredItem, activeQuest.Base.RequiredItemCount))
                {
                    if (!isNotificationHidden)
                    {
                        isNotificationHidden = true;

                        yield return DialogManager.Instance.ShowNotifText(toDoFinish);
                    }
                }
                else
                {
                    isNotificationHidden = false;
                }
            }
        }
        else
        {
            Debug.LogWarning("DialogManager.Instance is null in CheckAndHideNotification.");
        }
    }*/



    private void Awake()
    {
        itemGiver = GetComponent<ItemGiver>();

        character = GetComponent<Character>();

        

    }
    public void Start()
    {
        ButtonHandlers.Instance.HideInteractObjects();


        GameObject tutorial = GameObject.FindGameObjectWithTag("Tutorial");
        if (tutorial != null)
        {
            tutorialManagers = tutorial.GetComponent<TutorialManager>();
        }
        else
        {
            Debug.LogWarning("No GameObject tagged as 'Tutorial' found.");
        }

        /*spriteAnimator = new SpriteAnimator(sprites, GetComponent<SpriteRenderer>());
        spriteAnimator.Start();*/

    }

    private void Update()
    {
        if (state == NPCState.Idle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > timeBetweenPattern)
            {
                idleTimer = 0f;

                // Check if there is a movement pattern and it's not empty
                if (movementPattern != null && movementPattern.Count > 0)
                {
                    StartCoroutine(Walk());
                }
            }
        }

        // Check if the character reference is not null before calling HandleUpdate
        if (character != null)
        {
            character.HandleUpdate();
        }
        if (activeQuest != null)
        {
            if (activeQuest.Status == QuestStatus.Started)
            {
                activeQuest.SetNotificationCheckFlag(); // Set the flag to check for notifications.
            }
        }

        if (activeQuest != null)
        {
            if (activeQuest.Status == QuestStatus.Started)
            {
                HandleNotificationCheck();
            }
        }

        if (activeQuest2 != null)
        {
            if (activeQuest2.Status == QuestStatus.Started)
            {
                activeQuest2.SetNotificationCheckFlag(); // Set the flag to check for notifications.
            }
        }

        if (activeQuest2 != null)
        {
            if (activeQuest2.Status == QuestStatus.Started)
            {
                StartCoroutine(HandleNotificationCheck2());
            }
        }

        if (quesToStart != null || questToStart2 != null || activeQuest != null || activeQuest2 != null || questToComplete != null || questToComplete2 != null)
        {
            if (questStatusIcon != null)
                questStatusIcon.SetActive(true);
        }
        else
        {
            if (questStatusIcon != null)
                questStatusIcon.SetActive(false);
        }
    }

    public void HandleNotificationCheck()
    {
        if (activeQuest != null)
        {
            StartCoroutine(activeQuest.CheckAndHideNotification());
        }
    }

    public IEnumerator HandleNotificationCheck2()
    {
        if (activeQuest2 != null)
        {
            yield return activeQuest2.CheckAndHideNotification();
        }
    }


    IEnumerator Walk()
    {
        state = NPCState.Walking;

        var oldPos = transform.position;

        yield return character.Move(movementPattern[currentPattern]);

        if (transform.position != oldPos)
            currentPattern = (currentPattern + 1) % movementPattern.Count;

        state = NPCState.Idle;
    }
    public StartQuestAfterAQuest startQuestScript;
    bool startQuest;
    public IEnumerator Interact(Transform initiator)
    {
        

        if (tutorialManagers != null)
        {
            if (tutorialManagers.currentPopupIndex == 7)
            {
                Debug.Log("hasInteracted");
                hasInteracted = true;
            }
        }

        PlayerController playerController = initiator.GetComponent<PlayerController>();

        if (playerController != null && playerController.Character != null && playerController.Character.Animator != null)
        {
            playerController.Character.Animator.IsMoving = false;
        }

        ButtonHandlers.Instance.HideInteractObjects();

        if (state == NPCState.Idle)
        {
            if (character != null)
            character.LookTowards(initiator.position);
            state = NPCState.Dialog;

            if (questToComplete != null)
            {
                

                var quest = new Quest(questToComplete);

                yield return quest.CompleteQuest(initiator, disableDialog, completionTimeline, disableHideNotif);

                if (showFade)
                {
                    CutScene.instance.StartCutsceneWithFadeIn(questDone, 0.5f);
                }

                if (npcController != null)
                {
                    var shopManager = ShopManager.GetShopManager();

                    shopManager.RemoveCoins(100);
                }


                questToComplete = null;
                



                Debug.Log($"{quest.Base.Name} completed");
            }

            else if (questToComplete2 != null)
            {
                if (disableStatusInteract)
                {
                    
                }

                var quest = new Quest2(questToComplete2);

                yield return quest.CompleteQuest(initiator, disableDialog);

                if (showFade)
                {
                    CutScene.instance.StartCutsceneWithFadeIn(questDone, 0.5f);
                }

                questToComplete2 = null;


                Debug.Log($"{quest.Base.Name} completed");
            }

            else if (itemGiver != null && itemGiver.CanBeGiven())
            {
                yield return itemGiver.GiveItem(initiator.GetComponent<PlayerController>());
            }
            else if (questToStart2 != null)
            {
                yield return StartQuestCoroutine2();

                if (activeQuest2.CanBeCompleted())
                {
                    
                    yield return activeQuest2.CompleteQuest(initiator, disableDialog);
                    if (showFade)
                    {
                        CutScene.instance.StartCutsceneWithFadeIn(questDone, 0.5f);
                    }
                    activeQuest2 = null;
                }
            }
            else if (activeQuest2 != null)
            {
                if (activeQuest2.CanBeCompleted())
                {
                    
                   
                    yield return activeQuest2.CompleteQuest(initiator, disableDialog);
                    if (showFade)
                    {
                        CutScene.instance.StartCutsceneWithFadeIn(questDone, 0.5f);
                    }
                    activeQuest2 = null;
                }
                else
                {
                    yield return DialogManager.Instance.ShowDialog(activeQuest2.Base.InProgressDialogue);
                }
            }
            else if (quesToStart != null)
            {
                yield return StartQuestCoroutine();

                if (activeQuest.CanBecompleted())
                {
                    
                    yield return activeQuest.CompleteQuest(initiator, disableDialog, completionTimeline, disableHideNotif);
                    if (showFade)
                    {
                        CutScene.instance.StartCutsceneWithFadeIn(questDone, 0.5f);
                    }
                    activeQuest = null;
                }
            }
            else if (activeQuest != null)
            {
                if (activeQuest.CanBecompleted())
                {
                    
                    yield return activeQuest.CompleteQuest(initiator, disableDialog, completionTimeline, disableHideNotif);

                    if (showFade)
                    {
                        CutScene.instance.StartCutsceneWithFadeIn(questDone, 0.5f);
                    }
                    activeQuest = null;
                }
                else
                {
                    yield return DialogManager.Instance.ShowDialog(activeQuest.Base.InProgressDialogue);
                }
            }
            else
            {
                if (dialogEnable)
                {
                    yield return DialogManager.Instance.ShowDialog(dialog);
                }
            }

            // Reset state and timer
            state = NPCState.Idle;
            idleTimer = 0f;
        }
    }


    private IEnumerator StartQuestCoroutine2()
    {
        activeQuest2 = new Quest2(questToStart2);
        yield return activeQuest2.StartQuest(disableDialog);
        questToStart2 = null;
    }


    //edit
    private IEnumerator StartQuestCoroutine()
    {
        activeQuest = new Quest(quesToStart);
        yield return activeQuest.StartQuest(disableDialog);
        quesToStart = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            if (questToComplete != null || questToStart2 != null || quesToStart != null || questToComplete2 || activeQuest != null || activeQuest2 != null || dialogEnable == true) 
            {
            //Debug.Log("collider is working");
                if (Application.isMobilePlatform)
                {
                    ButtonHandlers.Instance.InteractObjects();
                }
                else
                {
                    ButtonHandlers.Instance.InteractObjects();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (questToComplete != null || questToStart2 != null || quesToStart != null || questToComplete2 || activeQuest != null || activeQuest2 != null || dialogEnable == true)
        {
            if (other.CompareTag("Player"))
            {
                ButtonHandlers.Instance.HideInteractObjects();
            }
        }
    }

    public object CaptureState()
    {
        var saveData = new NPCQuestSaveData();
        if (activeQuest != null)
            saveData.activeQuest = activeQuest.GetSaveData();

        if (quesToStart != null)
            saveData.questToStart = (new Quest(quesToStart)).GetSaveData();


        if (questToComplete != null)
            saveData.questToComplete = new Quest(questToComplete).GetSaveData();

        if (questToStart2 != null)
            saveData.questToStart2 = (new Quest2(questToStart2)).GetSaveData();

        if (activeQuest2 != null)
            saveData.activeQuest2 = activeQuest2.GetSaveData();

        if (questToComplete2 != null)
            saveData.questToComplete2 = new Quest2(questToComplete2).GetSaveData();

        if (questStatusIcon != null)
            saveData.isQuestStatusIconActive = questStatusIcon.activeSelf;

        return saveData;

    }

    public void RestoreState(object state)
    {
        var saveData = state as NPCQuestSaveData;
        if (saveData != null)
        {
            activeQuest = (saveData.activeQuest != null) ? new Quest(saveData.activeQuest) : null;
            quesToStart = (saveData.questToStart != null) ? new Quest(saveData.questToStart).Base : null;
            questToComplete = (saveData.questToComplete != null) ? new Quest(saveData.questToComplete).Base : null;

            activeQuest2 = (saveData.activeQuest2 != null) ? new Quest2(saveData.activeQuest2) : null;
            questToStart2 = (saveData.questToStart2 != null) ? new Quest2(saveData.questToStart2).Base : null;
            questToComplete2 = (saveData.questToComplete2 != null) ? new Quest2(saveData.questToComplete2).Base : null;

            if (questStatusIcon != null)
                isQuestStatusIconActive = saveData.isQuestStatusIconActive;

            if (questStatusIcon != null)
                questStatusIcon.SetActive(isQuestStatusIconActive);
        }
    }
}

[System.Serializable]
public class NPCQuestSaveData
{
    public QuestSaveData activeQuest;
    public QuestSaveData questToStart;
    public QuestSaveData questToComplete;
    public Quest2SaveData questToComplete2;
    public bool isQuestStatusIconActive;
    public Quest2SaveData questToStart2;
    public Quest2SaveData activeQuest2;
    public Quest2SaveData activeQuestScene;
    public Quest2SaveData sceneQuest;
    public bool questStarted;
    public int addQuestIndex;

}


public enum NPCState { Idle, Walking, Dialog }