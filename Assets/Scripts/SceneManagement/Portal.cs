using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Text = TMPro.TMP_Text;

public class Portal : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] int sceneToLoad = -1;
    [SerializeField] DestinationIdentifier destinationPortal;
    [SerializeField] Transform spawnPoint;


    PlayerController player;
    public void OnPlayerTriggered(PlayerController player)
    {
        this.player =player;
        StartCoroutine(SwitchScene());
    }

    public bool TriggerRepeatedly => false;

    IEnumerator SwitchScene()
    {

        DontDestroyOnLoad(gameObject);

        GameController.Instance.PauseGame(true);

        yield return SceneManager.LoadSceneAsync(sceneToLoad);

        var desPortal = FindObjectsOfType<Portal>().First(x => x != this && x.destinationPortal == this.destinationPortal);
        player.Character.SetPositionAndSnapToTile(desPortal.SpawnPoint.position);

        GameController.Instance.PauseGame(false);

        Destroy(gameObject);

    }


    public Transform SpawnPoint => spawnPoint;
}

public enum DestinationIdentifier
{
    A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z, S1Convent
}
