using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ClaimRewardsMenuState
{
    public bool[] ParentClaimActiveStates;
    public bool[] ClaimBtnActiveStates;
}

public class ClaimRewardsMenu : MonoBehaviour, ISavable
{
    public GameObject[] ParentClaim;
    public GameObject[] claimBtn;
    public QuestList questList;
    public QuestBase[] questBase; // Initialize the list to avoid null reference
    public RewardsTemplate[] rewardsTemplates;
    public string[] indicator;

    void Start()
    {
        questList = QuestList.GetQuestList();
        if (questList == null)
        {
            Debug.LogError("QuestList reference not set in ClaimRewardsMenu!");
            return;
        }


        DisplayQuestRewards();
    }

    private void Update()
    {
        for (int i = 0; i < questBase.Length; i++)
        {
            if (questList.IsCompleted(questBase[i].Name))
            {
                if (!claimBtn[i].activeSelf) // Check if rewards for this quest have been claimed
                {
                    if (!ParentClaim[i].activeSelf) // Check if ParentClaim has been set to false
                    {
                        Debug.Log("Quest completed and rewards claimed.");
                        ButtonManager.instance.HideNotifQuest();
                    }
                    else
                    {
                        claimBtn[i].SetActive(true);
                        Debug.Log("Quest completed. Rewards available to claim.");
                        ButtonManager.instance.ShowNotifQuest();
                    }
                }
            }
        }
    }


    void DisplayQuestRewards()
    {
        for (int i = 0; i < rewardsTemplates.Length; i++)
        {
            if (rewardsTemplates[i].titleText != null || questBase[i] != null)
                rewardsTemplates[i].titleText.text = indicator[i] + " - Coins: " + questBase[i].Coins;

            for (int s = 0; s < questBase.Length; s++)
            {
                ParentClaim[i].SetActive(true);
                claimBtn[i].SetActive(false);
            }
        }
    }



    // Method to be called when the claim button is clicked for a specific quest
    public void ClaimRewards(int Btn)
    {
        var shopManager = ShopManager.GetShopManager();
        shopManager.AddCoins(questBase[Btn].Coins);
        ParentClaim[Btn].SetActive(false);


        ButtonManager.instance.HideNotifQuest();

    }

    public object CaptureState()
    {
        ClaimRewardsMenuState state = new ClaimRewardsMenuState();

        // Capture active states of ParentClaim and claimBtn arrays
        state.ParentClaimActiveStates = new bool[ParentClaim.Length];
        state.ClaimBtnActiveStates = new bool[claimBtn.Length];

        for (int i = 0; i < ParentClaim.Length; i++)
        {
            state.ParentClaimActiveStates[i] = ParentClaim[i].activeSelf;
            state.ClaimBtnActiveStates[i] = claimBtn[i].activeSelf;
        }

        return state;
    }

    public void RestoreState(object state)
    {
        if (state is ClaimRewardsMenuState menuState)
        {
            // Restore active states of ParentClaim and claimBtn arrays
            for (int i = 0; i < ParentClaim.Length && i < menuState.ParentClaimActiveStates.Length; i++)
            {
                ParentClaim[i].SetActive(menuState.ParentClaimActiveStates[i]);
                claimBtn[i].SetActive(menuState.ClaimBtnActiveStates[i]);
            }
        }
    }
}
