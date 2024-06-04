using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCShop : MonoBehaviour, Interactable
{
    public GameObject iconStatus;


    public void Awake()
    {
        iconStatus.SetActive(false);
    }

    public IEnumerator Interact(Transform initiator)
    {
        PlayerController playerController = initiator.GetComponent<PlayerController>();
        if (playerController != null && playerController.Character != null && playerController.Character.Animator != null)
        {
            playerController.Character.Animator.IsMoving = false;
        }

        ShopManager.Instance.OpenShop();

        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            iconStatus.SetActive(true);
            ButtonHandlers.Instance.InteractObjects();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            iconStatus.SetActive(false);
            ButtonHandlers.Instance.HideInteractObjects();
        }
    }
}
