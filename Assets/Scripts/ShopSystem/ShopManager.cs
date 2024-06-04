using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour, ISavable
{
    public GameObject Shop;

    public int coins = 0;
    public TMP_Text coinUI;

    public ShopItemSO[] shopItemSO;
    public GameObject[] shopPanelsGO;
    public ShopTemplate[] shopPanels;
    public Button[] myPurchaseBtns;
    public RectTransform shopPanelsContainer;

    public int[] countBuy;
    public TMP_Text[] countBuyTxt;
    public GameObject[] confirmation;

    [SerializeField] AudioSource purchaseSFX;
    public static ShopManager Instance;
    public void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        CheckPurchaseable();
        DisplayCountBuy();
        /*Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "MainMenu")
        {
            coinText2.gameObject.SetActive(false);
        }
        else
        {
            coinText2.gameObject.SetActive(true);
        }*/
    }
    public void OpenShop()
    {
        Shop.SetActive(true);
    }

  

    public static ShopManager GetShopManager()
    {
        return FindAnyObjectByType<PlayerController>().GetComponent<ShopManager>();
    }

    private void Start()
    {

        for (int i = 0; i < countBuyTxt.Length; i++)
        {
            countBuy[i] = 0;
        }

        DisplayCountBuy();

        for (int i = 0; i < shopItemSO.Length; i++)
            shopPanelsGO[i].SetActive(true);

        coinUI.text = coins.ToString();

        if (ButtonManager.instance != null)
            ButtonManager.instance.Coin2Instance(coins);

        LoadPanels(); // Call the LoadPanels method here
        CheckPurchaseable();
    }

    public void DisplayCountBuy()
    {
        for (int i = 0; i < countBuyTxt.Length; i++)
        {
            countBuyTxt[i].text = "Count: " + countBuy[i];
        }
    }


    public void AddCoins(int coinsz)
    {
        coins = coinsz + coins;
        coinUI.text = coins.ToString();

        if (ButtonManager.instance != null)
            ButtonManager.instance.Coin2Instance(coins);

        CheckPurchaseable();
    }

    public void RemoveCoins(int coinsToRemove)
    {
        if (coinsToRemove <= coins)
        {
            coins -= coinsToRemove;
        }
        else
        {
            Debug.LogWarning("Attempting to remove more coins than available!");
            coins = 0; // Set coins to zero to avoid negative values
        }
        coinUI.text = coins.ToString();

        if (ButtonManager.instance != null)
            ButtonManager.instance.Coin2Instance(coins);

        CheckPurchaseable();
    }

    public void CheckPurchaseable()
    {

        for (int i = 0; i < shopItemSO.Length; i++)
        {
            if (coins >= shopItemSO[i].baseCost)
            {
                myPurchaseBtns[i].interactable = true;
            }
            else
            {
                myPurchaseBtns[i].interactable = false;
            }
            if (shopPanels[i].count == 0)
            {
                myPurchaseBtns[i].interactable = false;
                shopPanelsGO[i].transform.SetAsLastSibling();
                shopPanelsGO[i].GetComponent<Image>().color = new Color(0xB0 / 255.0f, 0xB0 / 255.0f, 0xB0 / 255.0f);

            }
            else
            {
                myPurchaseBtns[i].interactable = true;
                shopPanelsGO[i].transform.SetAsFirstSibling();
                shopPanelsGO[i].GetComponent<Image>().color = Color.white;
            }
        }
    }

    public void CountBuyIncreased(int index)
    {
        countBuy[index]++;
    }

    public void CountBuyDecreased(int index)
    {
        if (countBuy[index] > 0)
        countBuy[index]--;
    }

    public void PurchaseItem(int btnNo)
    {
        purchaseSFX.Play();

        var inventory = Inventory.GetInventory();


        if (coins >= shopItemSO[btnNo].baseCost * countBuy[btnNo] /*&& shopPanels[btnNo].count > 0*/ && countBuy[btnNo] <= shopPanels[btnNo].count)
        {
            coins -= (shopItemSO[btnNo].baseCost * countBuy[btnNo]);
            coinUI.text = coins.ToString();
            ButtonManager.instance.Coin2Instance(coins);
            CheckPurchaseable();

            
            shopPanels[btnNo].count -= countBuy[btnNo];

            // Add the item to the player's inventory
            ItemBase itemToPurchase = shopItemSO[btnNo];
            inventory.AddItem(itemToPurchase, countBuy[btnNo]);

            // Update the UI count for the purchased item
            shopPanels[btnNo].countText.text = "x" + shopPanels[btnNo].count;

            countBuy[btnNo] = 0;

            confirmation[btnNo].SetActive(false);
        }

        else if (countBuy[btnNo] >= shopPanels[btnNo].count || countBuy[btnNo] == 0)
        {
            Debug.Log("can't bro");
        }
    }




    public void LoadPanels()
    {
        for (int i = 0; i < shopItemSO.Length; i++)
        {
            //Debug.Log("Loading panel: " + i);
            //Debug.Log("Name: " + shopItemSO[i].name);
            //Debug.Log("Description: " + shopItemSO[i].Description);
            //Debug.Log("Base Cost: " + shopItemSO[i].baseCost);

            shopPanels[i].titleText.text = shopItemSO[i].Name;
            shopPanels[i].descriptionText.text = shopItemSO[i].Description;
            shopPanels[i].costText.text = "Coins " + shopItemSO[i].baseCost;
            shopPanels[i].icon.sprite = shopItemSO[i].Icon;
            shopPanels[i].countText.text = "x" + shopPanels[i].count;
        }
    }

    public object CaptureState()
    {
        ShopManagerState state = new ShopManagerState
        {
            coins = coins,
            counts = new int[shopPanels.Length]
        };

        for (int i = 0; i < shopPanels.Length; i++)
        {
            state.counts[i] = shopPanels[i].count;
        }
        //Debug.Log(coins);
        return state;
    }

    public void RestoreState(object stateObj)
    {
        if (stateObj is ShopManagerState state)
        {
            coins = state.coins;

            for (int i = 0; i < shopPanels.Length; i++)
            {
                if (shopPanels[i] != null)
                {
                    shopPanels[i].count = state.counts[i];

                    if (shopPanels[i].countText != null)
                    {
                        // Update the UI count for the purchased item
                        shopPanels[i].countText.text = "x" + shopPanels[i].count;
                    }
                }
            }

            if (coinUI != null)
            {
                coinUI.text = coins.ToString();
                ButtonManager.instance.Coin2Instance(coins);
            }

            CheckPurchaseable();
        }
    }
}


    [Serializable]
public class ShopManagerState
{
    public int coins;
    public int[] counts;
}