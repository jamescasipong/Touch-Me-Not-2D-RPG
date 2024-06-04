using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutSceneHouse : MonoBehaviour
{
    [SerializeField] PlayableDirector malePlayableDirector;
    [SerializeField] PlayableDirector femalePlayableDirector;
    public string sceneName;

    public bool enableNewGame = false;

    void Start()
    {
        StartCoroutine(_CutSceneHouse());
    }
    IEnumerator _CutSceneHouse()
    {
        if (MainMenu.instance.genderState == GenderState.Male)
        {
            malePlayableDirector.Play();
        }
        else if (MainMenu.instance.genderState == GenderState.Female)
        {
            femalePlayableDirector.Play();
        }

        if (MainMenu.instance.genderState == GenderState.Male)
        {
            yield return new WaitForSeconds((float)malePlayableDirector.duration);

            if (sceneName != null)
                SceneManager.LoadScene(sceneName);

            if (enableNewGame)
            {
                MainMenu.instance.NewGame();
            }
        }
        else if (MainMenu.instance.genderState == GenderState.Female)
        {
            yield return new WaitForSeconds((float)femalePlayableDirector.duration);

            if (sceneName.ToString() != null)
                SceneManager.LoadScene(sceneName);

            if (enableNewGame)
            {
                MainMenu.instance.NewGame();
            }
        }

        
    }
}

