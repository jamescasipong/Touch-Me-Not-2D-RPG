using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class StartQuestAfterAQuest : MonoBehaviour, ISavable
{
    [SerializeField] List<QuestBase> questsToCheck;
    public Transform spawnPoint;
    

    [Header("Pick a Quest")]
    [SerializeField] StartQuestObjectActions pickQuest;

    [Header("Different Type Of Quest")]
    [SerializeField] QuestBase questGetItems;
    [SerializeField] QuestBase2 questToStart2;
    [SerializeField] public QuestBase2 questScene;
    [SerializeField] float waitDelay = 0f;

    [Header("Show status for a next quest and if quest is completed")]
    [SerializeField] public string showNext;
    [SerializeField] GameObject enabledStatusIcon;
    [SerializeField] public QuestBase questCompleted;

    [Header("Changing Time if a quest is completed (Add PlayableDirctor - Optional)")]
    public float time;
    public string narrationTextforchangingtime;
    [SerializeField] PlayableDirector playableDirector;

    [Header("Play Timeline Quest")]
    [SerializeField] PlayableDirector timeLine;
    public string timelineText;


    [Header("AddQuest")]
    [SerializeField] NPCController addQuestToNPC;
    [SerializeField] string questName;

    [Header("QuestInteract - Fade")]
    public string textFade;
    public bool enableFade;

    [Header("QuestGetItms - Disable dialogs")]
    [SerializeField] bool disableDialog;

    public int addQuestIndex = 0;



    [SerializeField] Dialog dialog;

    [HideInInspector] public QuestList questList;


    private bool questStarted = false;
    public bool enableDialog = false;


    public static StartQuestAfterAQuest instance;

    [Header("Text for Fade")]
    public string questDone;

    [Header("Check to Show Dialog")]
    public bool checkToShowDialog = false;

    private QuestTrigger activeQuestTrigger;

    Transform initiator;
    Transform initiator2;

    PlayerController player;

    public float fadeInDuration;


    public Quest2 activeQuest2 { get; set; }

    Quest2 activeQuestScene { get; set; }

    QuestTrigger questTrigger { get; set; }


    //public bool disableDialog;
    public bool disableFade = false;
    public bool disableHideNotif = false;
    Quest activeQuest { get; set; }

    public PlayableDirector completionTimeline;

    private void Awake()
    {
        instance = this;

        
    }

    public void Start()
    {
        questList = QuestList.GetQuestList();
        questList.OnUpdated += UpdateObjectStatus;
        UpdateObjectStatus();
    }


    private void OnDestroy()
    {
        if (questList != null)
            questList.OnUpdated -= UpdateObjectStatus;
    }

    public void UpdateObjectStatus()
    {

        StartCoroutine(EnableDisableObjectsAfterDelay());
    }

    private IEnumerator EnableDisableObjectsAfterDelay()
    {
        
        bool allQuestsCompleted = true;

        if (questsToCheck != null)
        {
            foreach (QuestBase quest in questsToCheck)
            {
                if (!questList.IsCompleted(quest.Name))
                {
                    allQuestsCompleted = false;
                    break;
                    // Exit the loop as soon as one quest is not completed
                }
            }
        }




        if (allQuestsCompleted)
        {
            if (gameObject.activeInHierarchy || !gameObject.activeInHierarchy) {
                if (pickQuest == StartQuestObjectActions.QuestInteract)
                {
                    if (!questStarted) // You may need to add this variable to the Quest class.
                    {
                        
                        StartCoroutine(DelayedStartQuest(waitDelay, questDone, enableFade));
                        //questStarted = true;
                        //yield return null;
                    }
                }

                else if (pickQuest == StartQuestObjectActions.QuestGetItems)
                {
                    if (!questStarted) // You may need to add this variable to the Quest class.
                    {
                        
                        StartCoroutine(DelayedStartQuest(waitDelay, disableDialog));
                        
                        

                    }

                }

                else if (pickQuest == StartQuestObjectActions.QuestScene)
                {
                    if (!questStarted) // You may need to add this variable to the Quest class.
                    {

                        StartCoroutine(DelayedStartQuestForScene(waitDelay));
                        questStarted = true;
                        yield return null;

                    }
                }


                else if (pickQuest == StartQuestObjectActions.ShowNext)
                {
                    if (!questStarted) // You may need to add this variable to the Quest class.
                    {
                        questStarted = true;
                        StartCoroutine(DialogManager.Instance.ShowNotifText(showNext));

                        if (enabledStatusIcon != null)
                        {
                            enabledStatusIcon.SetActive(true);


                        }

                        if (enableDialog)
                        {
                            yield return new WaitForSeconds(waitDelay);

                            StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
                        }

                        if (questCompleted != null)
                        {
                            StartCoroutine(IfQuestCompleted());
                        }



                        questsToCheck = null;

                        yield return null;
                        //questStarted = true;
                    }
                }

                else if (pickQuest == StartQuestObjectActions.ChangeTime)
                {
                    if (!questStarted) // You may need to add this variable to the Quest class.
                    {
                        questStarted = true;

                        CutScene.instance.StartCutsceneWithFadeIn(questDone, 1.0f);

                        yield return new WaitForSeconds(1.5f);


                        Lighting.instance.SetTime(time);


                        if (playableDirector != null)
                        {
                            yield return new WaitForSeconds(2.5f);

                            playableDirector.Play();
                        }

                        if (narrationTextforchangingtime != null)

                            yield return new WaitForSeconds((float)playableDirector.duration);
                        StartCoroutine(DialogManager.Instance.ShowNotifText(narrationTextforchangingtime));

                        questsToCheck = null;
                    }
                }

                else if (pickQuest == StartQuestObjectActions.TimeLine)
                {
                    if (!questStarted)
                    {
                        

                        if (timeLine != null)
                        {
                            yield return new WaitForSeconds(1f);
                            
                            UIsManager.instance.DisableUIS();
                            timeLine.Play();



                            yield return new WaitForSeconds((float)timeLine.duration);

                           
                            UIsManager.instance.EnableUIS();

                            if (DialogManager.Instance != null || timelineText.ToString() != null)
                                yield return DialogManager.Instance.ShowNotifText(timelineText);

                            timeLine = null;
                            questsToCheck = null;

                            questStarted = true;
                        }
                    }
                }

                else if (pickQuest == StartQuestObjectActions.AddQuest)
                {
                    Debug.Log(addQuestIndex);

                    if (addQuestIndex == 0)
                    {


                        //QuestBase questToAssign = Resources.Load<QuestBase>(questName);

                        if (questsToCheck != null)
                        {
                            // Ensure addQuestToNPC references the correct GameObject with NPCController attached
                            NPCController npcController = addQuestToNPC.GetComponent<NPCController>();
                            Debug.Log("questToCheck: " + questsToCheck[0].Name);
                            if (npcController != null)
                            {
                                if (npcController.gameObject.activeInHierarchy)
                                {

                                    npcController.quesToStart = QuestDB.GetObjectByName(questName);

                                    Debug.Log("Added " + questName);


                                    questsToCheck = null;

                                    addQuestIndex++;
                                    Debug.Log(questName + addQuestIndex);
                                }
                            }
                            else
                            {
                                Debug.LogError("NPCController script not found on addQuestToNPC GameObject.");
                            }
                        }
                        else
                        {
                            Debug.LogError("Unable to load QuestBase 'Chap1Chef' from Resources.");
                        }
                    }
                }
            }
        }
    }

  
    public IEnumerator IfQuestCompleted()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f); // Wait for 1 second before executing the action.

            if (questList.IsCompleted(questCompleted.Name))
            {
                yield return DialogManager.Instance.HideNotifText(showNext);
            }
        }
    }

    public void OnQuestTriggered(QuestTrigger questTrigger)
    {
        if (questTrigger != null)
        {
            Debug.Log("Exists");
            activeQuestTrigger = questTrigger;

            if (activeQuestTrigger.IsTriggered())
            {
                
                HandleQuestCompletion();
            }
        }
        else
        {
        }
    }

    private void HandleQuestCompletion()
    {
        if (activeQuestTrigger != null)
        {
            // For example, you can call CompleteQuestInitiatorForScene.
            StartCoroutine(CompleteQuestInitiatorForScene());
            activeQuestTrigger = null;
        }
    }

    public IEnumerator CompleteQuestInitiatorForScene()
    {
        if (activeQuestScene != null)
        {
            yield return activeQuestScene.CompleteQuestInitiatorForScene();
        }
    }


    public IEnumerator CompleteQuestInitiator()
    {
        yield return activeQuest2.CompleteQuestInitiator(initiator);
    }




    private IEnumerator CheckQuestCompletion(string questDone, bool enableFade)
    {
        while (true)
        {
            //yield return new WaitForSeconds(0.1f); // Adjust the delay as needed

            if (activeQuest2 != null)
            {

                if (activeQuest2.CanBeCompleted())
                {
                    if (enableFade)
                    {
                        CutScene.instance.StartCutsceneWithFadeIn(questDone, fadeInDuration);
                    }
                    yield return CompleteQuestInitiator();
                    activeQuest2 = null;
                    break;
                }
            }
        }
    }


    private IEnumerator CheckQuestCompletionForScene()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f); // Adjust the delay as needed


            if (questTrigger != null && activeQuestScene != null)
            {

                if (questTrigger.IsTriggered() && activeQuestScene != null)
                {
                    CutScene.instance.StartCutsceneWithFadeIn(questDone, fadeInDuration);
                    yield return CompleteQuestInitiatorForScene();
                    activeQuestScene = null;
                }
            }
            else
            {
            }
        }
    }

    public IEnumerator DelayedStartQuest(float waitToStartQuest, string questDone, bool enableFade)
    {
        yield return new WaitForSeconds(waitToStartQuest);


        StartCoroutine(CheckIfQuestToStart2());

        initiator = FindObjectOfType<PlayerController>().transform;

        StartCoroutine(CheckQuestCompletion(questDone, enableFade));

    }

    public IEnumerator DelayedStartQuestForScene(float waitToStartQuest)
    {
        yield return new WaitForSeconds(waitToStartQuest);


        StartCoroutine(StartQuestCoroutineScene());

        initiator = FindObjectOfType<PlayerController>().transform;

        StartCoroutine(CheckQuestCompletionForScene());

    }


  

    public IEnumerator CheckIfQuestToStart2()
    {
        if (questToStart2 != null)
        {
            yield return StartCoroutine(StartQuestCoroutine2());
        }


    }

   

    public IEnumerator StartQuestCoroutine2()
    {

        activeQuest2 = new Quest2(questToStart2);
        yield return activeQuest2.StartQuestInitiator();
        questToStart2 = null;
    }

    public IEnumerator StartQuestCoroutineScene()
    {
        activeQuestScene = new Quest2(questScene);
        yield return activeQuestScene.StartQuestInitiatorForScene();
        questScene = null;
    }


    public IEnumerator CompleteQuestInitiator(bool disableDialog)
    {
        yield return activeQuest.CompleteQuest(initiator, disableDialog, completionTimeline, disableHideNotif);
    }



    private IEnumerator CheckQuestCompletion(bool disableDialog)
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        while (activeQuest != null && !activeQuest.CanBecompleted())
        {
            yield return wait;
        }

        if (activeQuest != null)
        {
            if (disableFade)
            {
                CutScene.instance.StartCutsceneWithFadeIn(questDone, fadeInDuration);
            }
            yield return CompleteQuestInitiator(disableDialog);

            activeQuest = null;
        }
    }



    public IEnumerator DelayedStartQuest(float waitToStartQuest, bool disableDialog)
    {
        yield return new WaitForSeconds(waitToStartQuest);


        StartCoroutine(StartQuestCoroutine(disableDialog));

        initiator = FindObjectOfType<PlayerController>().transform;

        StartCoroutine(CheckQuestCompletion(disableDialog));

    }


 



    public IEnumerator StartQuestCoroutine(bool disableDialog)
    {
        if (questGetItems != null)
        {
            activeQuest = new Quest(questGetItems);
            yield return activeQuest.StartQuest(disableDialog);
            questGetItems = null;
        }

    }

    public object CaptureState()
    {
        var saveData = new NPCQuestSaveData();

        if (questToStart2 != null)
            saveData.questToStart2 = (new Quest2(questToStart2)).GetSaveData();

        if (questGetItems != null)
            saveData.questToStart = new Quest(questGetItems).GetSaveData();

        if (activeQuest != null)
            saveData.activeQuest = activeQuest.GetSaveData();

        if (activeQuest2 != null)
            saveData.activeQuest2 = activeQuest2.GetSaveData();

        if (questScene != null)
            saveData.sceneQuest = (new Quest2(questScene)).GetSaveData();

        if (activeQuestScene != null)
            saveData.activeQuestScene = activeQuestScene.GetSaveData();

        saveData.questStarted = questStarted;
        saveData.addQuestIndex = addQuestIndex;

        return saveData;
    }

    public void RestoreState(object state)
    {
        var saveData = state as NPCQuestSaveData;

        if (saveData != null)
        {

            activeQuest2 = (saveData.activeQuest2 != null) ? new Quest2(saveData.activeQuest2) : null;
            activeQuestScene = (saveData.activeQuestScene != null) ? new Quest2(saveData.activeQuestScene) : null;
            questToStart2 = (saveData.questToStart2 != null) ? new Quest2(saveData.questToStart2).Base : null;
            questScene = (saveData.sceneQuest != null) ? new Quest2(saveData.sceneQuest).Base : null;
            activeQuest = (saveData.activeQuest != null) ? new Quest(saveData.activeQuest) : null;
            questGetItems = (saveData.questToStart != null) ? new Quest(saveData.questToStart).Base : null;
            questStarted = saveData.questStarted;
            addQuestIndex = saveData.addQuestIndex;
        }
    }
    // ... (previous code)



    //QuestInitiator

}



public enum StartQuestObjectActions { QuestGetItems, QuestInteract, QuestScene, ShowNext, ChangeTime, TimeLine, AddQuest }
