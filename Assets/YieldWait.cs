using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class YieldWait : MonoBehaviour, ISavable
{
    public PlayableDirector player;
    public PlayableDirector nextCutScene;

    public PlayableDirector chapter8;

    public PlayableDirector quizScene;
    public GameObject quizzer;

    bool startPlaying, startPlaying2;

    public object CaptureState()
    {
        // Saving the state of boolean variables
        Dictionary<string, object> state = new Dictionary<string, object>();
        state["startPlaying"] = startPlaying;
        state["startPlaying2"] = startPlaying2;
        return state;
    }

    public void RestoreState(object state)
    {
        // Restoring the state of boolean variables
        if (state is Dictionary<string, object> dict)
        {
            if (dict.ContainsKey("startPlaying"))
                startPlaying = (bool)dict["startPlaying"];

            if (dict.ContainsKey("startPlaying2"))
                startPlaying2 = (bool)dict["startPlaying2"];
        }
    }

    private void Start()
    {
        startPlaying = true;
        startPlaying2 = true;
    }
    void Update()
    {

        if (startPlaying)
        {
            if (player != null && player.state == PlayState.Playing)
            {
                Debug.Log("Player Time: " + player.time + ", Player Duration: " + player.duration);
                if (player.time + 0.1f >= player.duration) // Adding a small threshold for comparison
                {
                    Debug.Log("Triggering next cut scene!");
                    nextCutScene.Play();
                    startPlaying = false;
                }
            }
        }

        if (startPlaying2)
        {
            if (chapter8 != null && chapter8.state == PlayState.Playing)
            {
                if (chapter8.time + 0.1f >= chapter8.duration)
                {
                    quizzer.SetActive(true);
                    StartCoroutine(DialogManager.Instance.ShowNotifText("pumunta sa gamemaster para simulan ang quest"));
                    StartCoroutine(StartCutScene());
                    chapter8 = null;
                    startPlaying2 = false;
                }
            }
        }
    }

    IEnumerator StartCutScene()
    {
        
        yield return new WaitForSeconds(1f);
        UIsManager.instance.DisableUIS();

        quizScene.Play();

        yield return new WaitForSeconds((float)quizScene.duration);
        UIsManager.instance.EnableUIS();
    }

}
