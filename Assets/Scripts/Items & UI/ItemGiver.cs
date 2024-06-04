using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemGiver : MonoBehaviour, ISavable
{
    [SerializeField] ItemBase item;
    [SerializeField] int count = 1;
    [SerializeField] Dialog dialog;
    



    bool used = false;


    public IEnumerator GiveItem(PlayerController player)
    {
        
        Debug.Log("worked");

        player.Character.Animator.IsMoving = false;


        yield return DialogManager.Instance.ShowDialog(dialog);

        player.GetComponent<Inventory>().AddItem(item, count);

        used = true;

        string dialogText = $"Player received {item.name}";
        if (count > 1)
        {
            dialogText = $"Player received {count} {item.name}s";
        }

        yield return DialogManager.Instance.ShowDialogText(dialogText);

    }

    public bool CanBeGiven()
    {
        return item != null && count > 0 && !used;
    }

    public object CaptureState()
    {
        return used;
    }

    public void RestoreState(object state)
    {
        used = (bool)state;
    }
}
