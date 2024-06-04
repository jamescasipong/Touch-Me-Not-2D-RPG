        using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Playables;


public class PlayableTrigger : MonoBehaviour
{
    [SerializeField ]public PlayableDirector playableDirector;
    [SerializeField] string questText;
    public string saveName;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("works");
            StartCoroutine(StartCutScene());

        }

    }

    public IEnumerator StartCutScene()
    {

        var dialogManager = DialogManager.Instance;
        if (playableDirector != null)
        {


            dialogManager.controlStateofJoystick = true;
            DialogManager.Instance.joystick.SetActive(false);
            DialogManager.Instance.notificationText.enabled = false;



            playableDirector.Play();

            

            yield return new WaitForSeconds((float)playableDirector.duration);

            yield return DialogManager.Instance.ShowNotifText(questText);

            if (saveName != null)
            {
                SavingSystem.i.Save(saveName);
            }
            dialogManager.controlStateofJoystick = false;
            DialogManager.Instance.notificationText.enabled = true;
            DialogManager.Instance.joystick.SetActive(true);
            playableDirector = null;
        }


        DialogManager.Instance.joystick.SetActive(true);
        Debug.Log("sd");

        
    }

}
