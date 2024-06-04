using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class PickUpCoins : MonoBehaviour, Interactable, ISavable
{
    int coins;
    bool disable;
    bool pickUpState = false;

    

    private void Start()
    {
        coins = Random.Range(30, 50);
    }

    public IEnumerator Interact(Transform initiator)
    {
        if (!pickUpState)
        {
            disable = false;


            ShopManager.GetShopManager().AddCoins(coins);
            Debug.Log("the value of coins is " + coins);
            GetComponent<SpriteRenderer>().enabled = disable;
            GetComponent<BoxCollider2D>().enabled = disable;

            yield return null;
        }
    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("works");

            
             ButtonHandlers.Instance.InteractObjects();
            
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
        return pickUpState;
    }

    public void RestoreState(object state)
    {
        pickUpState = (bool)state;

        if (pickUpState)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
