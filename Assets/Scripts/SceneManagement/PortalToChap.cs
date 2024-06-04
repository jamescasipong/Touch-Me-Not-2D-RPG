using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class PortalToChaps : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] DestinationIdentifier destinationPortal;
    [SerializeField] Transform spawnPoint;
    [SerializeField] string saveFile;

    PlayerController player;
    [SerializeField] private bool isSavingEnabled = false;
    [SerializeField] private bool disableTeleport = false;

    

    public void OnPlayerTriggered(PlayerController player)
    {
        if (!disableTeleport)
        {
            this.player = player;

            // Find the appropriate LocationPortal based on your game logic.
            var desPortal = FindObjectsOfType<PortalToChaps>().First(x => x != this && x.destinationPortal == this.destinationPortal);

            if (isSavingEnabled)
            {
                StartCoroutine(Teleport(saveFile, desPortal));
            }
            else
            {
                if (spawnPoint != null)
                {
                    player.Character.SetPositionAndSnapToTile(desPortal.SpawnPoint.position);
                }
            }
        }
    }



    public bool TriggerRepeatedly
    {
        get { return false; } // Or return true if you want it to trigger repeatedly
    }

    Fader fader;

    public void Start()
    {
        fader = FindObjectOfType<Fader>();
    }


    IEnumerator Teleport(string saveFile, PortalToChaps desPortal)
    {
        if (!disableTeleport)
        {
            GameController.Instance.PauseGame(true);
            yield return fader.FadeIn(0.5f);

            this.saveFile = saveFile;

            player.Character.SetPositionAndSnapToTile(desPortal.SpawnPoint.position);

            if (SavingSystem.i != null)
            {
                SavingSystem.i.DontLoadData();
            }

            if (saveFile != null)
            {
                Debug.Log("Before save");
                if (SavingSystem.i != null && isSavingEnabled) // Check the flag before saving
                {
                    SavingSystem.i.Save(saveFile);
                }
                Debug.Log("After save");
            }
            yield return fader.FadeOut(0.5f);
            GameController.Instance.PauseGame(false);


            yield return null;

            
        }
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
