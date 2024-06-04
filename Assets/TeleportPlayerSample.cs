using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayerSample : MonoBehaviour
{
    [SerializeField]public Transform spawnPont;

    public static TeleportPlayerSample Instance;


    private void Awake()
    {
        Instance = this;

        
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            TeleportPlayer();
        }
    }

    public void TeleportPlayer()
    {



        if (PlayerController.instance.thisPlayer != null)
        {
            CutScene.instance.StartCutsceneWithPopInAndFadeOut("Nakidnap ka at kailangan mong makatakas");
            Debug.Log("Before" + PlayerController.GetPlayer().thisPlayer.position);
            GameController.Instance.PauseGame(true);

            PlayerController.GetPlayer().thisPlayer.position = spawnPont.position;

            Debug.Log("After" + PlayerController.GetPlayer().thisPlayer.position);

            GameController.Instance.PauseGame(false);
        }
        else
        {
            Debug.Log("It doesnt exist tho");
        }


    }
    
}
