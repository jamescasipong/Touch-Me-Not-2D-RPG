using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TMP_Text;

public class PickUp : MonoBehaviour, Interactable, ISavable
{
    [SerializeField] ItemBase item;
    //[SerializeField] GameObject interactButton;
    //[SerializeField] public Text interactText;
    public bool Used { get; set; } = false;

    [SerializeField] public bool spriteRenderer = false;



    [SerializeField] int count = 1;

    [SerializeField] public bool showDialouge;

    private void Start()
    {
        
    }
    public IEnumerator Interact(Transform initiator)
    {
        if (!Used)
        {
            
            
            initiator.GetComponent<Inventory>().AddItem(item, count);


            Used = true;
            // Destroy the item GameObject when picked up
            spriteRenderer = GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;


            if (showDialouge) 
            {
                yield return DialogManager.Instance.ShowDialogText($"Player Found {item.Name}");
            }
        }
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
        return Used;
    }

    public void RestoreState(object state)
    {
        Used = (bool)state;

        if (Used)
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
