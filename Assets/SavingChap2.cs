using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class SavingChap2 : MonoBehaviour, ISavable
{
    public bool save = false;

    public object CaptureState()
    {
        return save;
    }

    public void RestoreState(object state)
    {
        save = (bool)state;

        if (save)
        {
            GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!save)
            {
                SavingSystem.i.Save("Chap2");
                GetComponent<Renderer>().enabled = false;

                save = true;
            }
        }
    }
}
