using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumUponTrigger : MonoBehaviour
{
    public int arrayNum; // Renamed variable for clarity

    public void Start()
    {
        ButtonHandlers.Instance.arrowPointers[arrayNum].SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player entered trigger");

            ButtonHandlers.Instance.arrowPointers[arrayNum].SetActive(true);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player entered trigger");

            ButtonHandlers.Instance.arrowPointers[arrayNum].SetActive(false);
        }

    }

}