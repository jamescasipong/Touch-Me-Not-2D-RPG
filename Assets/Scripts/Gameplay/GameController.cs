using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using JetBrains.Annotations;

public enum GameState { Dialog, FreeRoam, Bag, Paused }

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Camera worldCamera;
    [SerializeField] public GameObject Inventorysz;
    [SerializeField] InventoryUIState inventoryUI;
    public GameObject arrowInBag;


    public SceneDetails CurrentScene { get; private set; }
    public SceneDetails PrevScene { get; private set; }

    private Inventory inventory;

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        GameObject inventoryManager = GameObject.Find("InventoryManager");
        if (inventoryManager != null)
        {
            inventory = inventoryManager.GetComponent<Inventory>();
        }

        ItemDB.Init();
        QuestDB.Init();
        Quest2DB.Init();
    }
    GameState statebeforePaused;

    GameState state;

    private void Start()
    {
        state = GameState.FreeRoam;
        DialogManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
        };

        DialogManager.Instance.OnCloseDialog += () =>
        {
            if (state == GameState.Dialog)
                state = GameState.FreeRoam;
        };
    }

    public void SelfActiveInventory()
    {
        GameObject tutorial = GameObject.FindGameObjectWithTag("Tutorial");
        TutorialManager tutorialManagers = tutorial.GetComponent<TutorialManager>();
        ButtonManager.instance.arrowIndicatorBag.SetActive(true);

        if (Inventorysz.activeInHierarchy)
        {
            ButtonManager.instance.arrowIndicatorBag.SetActive(false);
            tutorialManagers.currentPopupIndex++;
            tutorialManagers.ShowCurrentPopup();

        }
    }

    public void SelfInActiveInventory()
    {

        GameObject tutorial = GameObject.FindGameObjectWithTag("Tutorial");
        TutorialManager tutorialManagers = tutorial.GetComponent<TutorialManager>();
        arrowInBag.SetActive(true);

        if (!Inventorysz.activeInHierarchy)
        {
            arrowInBag.SetActive(false);
            StartCoroutine(WaitInventory(tutorialManagers));
        }
    }

    bool stopRepeating;
    IEnumerator WaitInventory(TutorialManager tutorialInstance)
    {
        if (!stopRepeating)
        {
            stopRepeating = true;
            yield return DialogManager.Instance.AutomaticallyShowDialog(tutorialInstance.closeBag);

            tutorialInstance.currentPopupIndex++;
            tutorialInstance.ShowCurrentPopup();
        }
    }
    public void ShowMenuAcross()
    {
        Inventorysz.SetActive(true);
    }


    public void PauseGame(bool pause)
    {
        if (pause)
        {
            statebeforePaused = state;
            state = GameState.Paused;
        }
        else
        {
            state = statebeforePaused;
        }
    }

    private void Update()
    {
        if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
        else if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();

            /*if (SavingSystem.i != null)
            {
                if (Input.GetKey(KeyCode.C))
                {
                    SavingSystem.i.Save("saveSlot2");
                }
                if (Input.GetKey(KeyCode.L))
                {
                    SavingSystem.i.Load("saveSlot2");
                }
            }*/

            if (Input.GetKey(KeyCode.B))
            {
                Inventorysz.SetActive(true);
                Debug.Log("It's being clicked");
            }
            else if (Input.GetKey(KeyCode.V))
            {
                Inventorysz.SetActive(false);
                Debug.Log("It's being clicked");
            }
        }
        else if (state == GameState.Bag)
        {
            Action onBack = () =>
            {
                inventoryUI.gameObject.SetActive(false);
                state = GameState.FreeRoam;
            };
            inventoryUI.HandleUpdate(onBack);
        }
    }

    public void SetCurrentScene(SceneDetails currScene)
    {
        PrevScene = CurrentScene;
        CurrentScene = currScene;
    }
}
