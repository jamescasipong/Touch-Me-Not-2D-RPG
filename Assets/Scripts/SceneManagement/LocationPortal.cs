using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocationPortal : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] DestinationIdentifier destinationPortal;
    [SerializeField] Transform spawnPoint;
    [SerializeField] string saveFile;

    PlayerController player;
    [SerializeField] private bool isSavingEnabled = false; // Add this flag

    public void OnPlayerTriggered(PlayerController player)
    {
        this.player = player;
        StartCoroutine(Teleport());
    }


    public bool TriggerRepeatedly
    {
        get { return false; } // Or return true if you want it to trigger repeatedly
    }

    Fader fader; // No need to find the Fader, it's a singleton

    public void Start()
    {
        fader = FindObjectOfType<Fader>();
        initiator = FindObjectOfType<PlayerController>();// Access the Fader singleton
    }

    PlayerController initiator;


    IEnumerator Teleport()
    {
        PlayerController playerController = initiator.GetComponent<PlayerController>();

        

        if (playerController != null && playerController.Character != null && playerController.Character.Animator != null)
        {
            playerController.Character.Animator.IsMoving = false;
        }
        if (SavingSystem.i != null)
        {
            SavingSystem.i.DontLoadData();
        }
        GameController.Instance.PauseGame(true);
        yield return fader.FadeIn(0.5f);

        var desPortal = FindObjectsOfType<LocationPortal>().First(x => x != this && x.destinationPortal == this.destinationPortal);
        player.Character.SetPositionAndSnapToTile(desPortal.SpawnPoint.position);

        /*var desPortal = FindObjectsOfType<LocationPortal>().FirstOrDefault(x => x != this && x.destinationPortal == this.destinationPortal);

        if (desPortal != null)
        {
            player.Character.SetPositionAndSnapToTile(desPortal.SpawnPoint.position);
        }
        else
        {
            Debug.LogError("No matching destination portal found!");
            // Handle the situation where there's no matching portal
        }*/
        GameController.Instance.PauseGame(false);



        yield return fader.FadeOut(0.5f);
    }



    public Transform SpawnPoint => spawnPoint;

    // Add these methods to enable/disable the Saving System trigger
    public void EnableSavingSystemTrigger()
    {
        isSavingEnabled = true;
    }

    public void DisableSavingSystemTrigger()
    {
        isSavingEnabled = false;
    }
}
