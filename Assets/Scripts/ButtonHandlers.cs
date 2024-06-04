using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Text = TMPro.TMP_Text;

public class ButtonHandlers : MonoBehaviour
{
    [SerializeField] GameObject interactObject;
    [SerializeField] GameObject MenuPanel;
    [SerializeField] public GameObject Joystick;
    [SerializeField] GameObject questMenu;
    [SerializeField] public GameObject arrowIndicatorJoystick;
    [SerializeField] public GameObject[] arrowPointers;
    public GameObject arrowInMenu;
    public GameObject arrowQuestInactive;


    public static ButtonHandlers Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void InteractObjects()
    {
        interactObject.SetActive(true);
    }

    public void EnableQuestMenu()
    {
        questMenu.SetActive(true);
    }

    public void DisableQuestMenu()
    {
        questMenu.SetActive(false);
    }

    public void HideInteractObjects()
    {
        interactObject.SetActive(false);
    }

    public void MenuPanelSelfActive()
    {
        GameObject tutorial = GameObject.FindGameObjectWithTag("Tutorial");
        TutorialManager tutorialManagers = tutorial.GetComponent<TutorialManager>();
        ButtonManager.instance.arrowMainMenu.SetActive(true);


        if (MenuPanel.activeInHierarchy)
        {
            ButtonManager.instance.arrowMainMenu.SetActive(false);
            tutorialManagers.currentPopupIndex++;
            tutorialManagers.ShowCurrentPopup();
        }
    }

    public void MenuPanelInSelfActive()
    {
        GameObject tutorial = GameObject.FindGameObjectWithTag("Tutorial");
        TutorialManager tutorialManagers = tutorial.GetComponent<TutorialManager>();
        arrowInMenu.SetActive(true);

        if (!MenuPanel.activeInHierarchy)
        {
            arrowInMenu.SetActive(false);
            StartCoroutine(Wait(tutorialManagers, tutorialManagers.closeMainMenu));
        }
    }

    public void QuestMenuSelfActive()
    {

        GameObject tutorial = GameObject.FindGameObjectWithTag("Tutorial");
        TutorialManager tutorialManagers = tutorial.GetComponent<TutorialManager>();
        ButtonManager.instance.arrowClaimMenu.SetActive(true);

        if (questMenu.activeInHierarchy)
        {
            ButtonManager.instance.arrowClaimMenu.SetActive(false);
            tutorialManagers.currentPopupIndex++;
            tutorialManagers.ShowCurrentPopup();
        }
    }
    bool stopRepeat;
    bool stopRepeat2;
    IEnumerator Wait(TutorialManager tutorial, Dialog dialog)
    {
        if (!stopRepeat)
        {
            stopRepeat = true;
            yield return DialogManager.Instance.AutomaticallyShowDialog(dialog);

            tutorial.currentPopupIndex++;
            tutorial.ShowCurrentPopup();
        }
    }

    IEnumerator Wait2(TutorialManager tutorial, Dialog dialog)
    {
        if (!stopRepeat2)
        {
            stopRepeat2 = true;
            yield return DialogManager.Instance.AutomaticallyShowDialog(dialog);

            tutorial.currentPopupIndex++;
            tutorial.ShowCurrentPopup();
        }
    }

    public void QuestMenuInSelfActive()
    {

        GameObject tutorial = GameObject.FindGameObjectWithTag("Tutorial");
        TutorialManager tutorialManagers = tutorial.GetComponent<TutorialManager>();
        arrowQuestInactive.SetActive(true);

        if (!questMenu.activeInHierarchy)
        {
            arrowQuestInactive.SetActive(false);
            StartCoroutine(Wait2(tutorialManagers, tutorialManagers.claimRewards));
            
        }
    }

    public void ShowMenuPanel()
    {
        MenuPanel.SetActive(true);
    }

    public void ShowJoystick()
    {
        
        Joystick.SetActive(true);
        
    }

    public void Save()
    {
        SavingSystem.i.Save("saveSlot2");
    }

    public void Load()
    {
        SavingSystem.i.Load("saveSlot2");
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
