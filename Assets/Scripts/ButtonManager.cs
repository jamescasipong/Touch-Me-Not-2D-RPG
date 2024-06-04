using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonManager : MonoBehaviour, ISavable
{
    public static ButtonManager instance;
    [SerializeField] GameObject notif;
    [SerializeField] GameObject notifQuest;
    [SerializeField] public GameObject arrowIndicatorBag;
    [SerializeField] public GameObject arrowMainMenu;
    [SerializeField] public GameObject arrowClaimMenu;

    public TMP_Text coinText2;
    public TMP_Text clockTimeText;

    private bool isNotifActive; // Variable to store the state of the notif GameObject

    private void Awake()
    {
        notif.SetActive(false);
        notifQuest.SetActive(false);
        instance = this;
    }

    public void QuestInstance()
    {
        ButtonHandlers.Instance.EnableQuestMenu();
    }

    public void ShowMenuAcrosses()
    {
        //InventoryAnimation.instance.OpenInventory();
        GameController.Instance.ShowMenuAcross();
    }

    public void ShowMenuPanel()
    {
        ButtonHandlers.Instance.ShowMenuPanel();
    }

    public void ShowNotif()
    {
        notif.SetActive(true);
        isNotifActive = true; // Update the state when the notif is shown
    }

    public void ShowNotifQuest()
    {
        notifQuest.SetActive(true);
        //isNotifActive = true; // Update the state when the notif is shown
    }

    public void HideNotifQuest()
    {
        notifQuest.SetActive(false);
        //isNotifActive = true; // Update the state when the notif is shown
    }

    public void HideNotif()
    {
        notif.SetActive(false);
        isNotifActive = false; // Update the state when the notif is hidden
    }

    public object CaptureState()
    {
        // Capture the state of the notif GameObject
        return isNotifActive;
    }


    public void RestoreState(object state)
    {
        if (state is bool)
        {
            bool savedState = (bool)state;
            // Restore the state of the notif GameObject based on the saved state
            if (savedState)
            {
                ShowNotif();
            }
            else
            {
                HideNotif();
            }
        }
    }

    public void Coin2Instance(int coins)
    {
        coinText2.text = coins.ToString();
    }

    public void ClockInstance(string formattime)
    {
        clockTimeText.text = formattime;
    }
}
