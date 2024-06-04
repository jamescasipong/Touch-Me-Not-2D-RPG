using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HasKey : MonoBehaviour, Interactable, ISavable
{
    public ItemBase key;
    public int quantity;

    public bool disabledTrigger;

    [SerializeField] float typingSpeed;

    private Queue<string> notificationQueue = new Queue<string>();
    private bool isDisplayingNotification = false;


    private void Start()
    {
        disabledTrigger = true;


    }

    public IEnumerator Interact(Transform initiator)
    {
        var door = Inventory.GetInventory();
        if (disabledTrigger)
        {
            if (door.HasItem(key, quantity))
            {
                disabledTrigger = false;
                door.RemoveItem(key, quantity);
                Debug.Log("works");




                GetComponent<SpriteRenderer>().enabled = disabledTrigger;
                GetComponent<BoxCollider2D>().enabled = disabledTrigger;

            }
            else
            {
                if (Inventory.instance.keyText != null)
                {
                    notificationQueue.Enqueue("Kailangan mo ng " + key.Name + " ng " + quantity);
                    if (!isDisplayingNotification)
                    {
                        StartCoroutine(DisplayNextNotification());
                    }
                }
            }
        }

        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("works");

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

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ButtonHandlers.Instance.HideInteractObjects();
        }
    }

    public object CaptureState()
    {
        return disabledTrigger; // Save the state of disabledTrigger
    }

    public void RestoreState(object state)
    {
        disabledTrigger = (bool)state; // Restore the state of disabledTrigger
        GetComponent<SpriteRenderer>().enabled = disabledTrigger;
        GetComponent<BoxCollider2D>().enabled = disabledTrigger;
    }

    private IEnumerator DisplayNextNotification()
    {
        isDisplayingNotification = true;

        while (notificationQueue.Count > 0)
        {
            string notification = notificationQueue.Dequeue();
            Inventory.instance.keyText.enabled = true;
            Inventory.instance.keyText.text = ""; // Clear the text initially

            foreach (char letter in notification)
            {
                Inventory.instance.keyText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }

            yield return new WaitForSeconds(1.0f);

            Inventory.instance.keyText.enabled = false;
        }

        isDisplayingNotification = false;
    }
}
