using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutScenePlay : MonoBehaviour
{
    public PlayableDirector introCutScene;
    void Start()
    {
        StartCoroutine(PlayCutSceneOnAwake());
    }

    IEnumerator PlayCutSceneOnAwake()
    {
        introCutScene.Play();

        yield return new WaitForSeconds((float)introCutScene.duration);

        MainMenu.instance.NewGame();
    }
    
}
