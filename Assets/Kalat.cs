using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kalat : MonoBehaviour, Interactable
{
    bool spriteRenderer = true;
    public void Start()
    {
        spriteRenderer = true;
    }
    public IEnumerator Interact(Transform initiator)
    {


        spriteRenderer = GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;

        yield return null;
    }
}
