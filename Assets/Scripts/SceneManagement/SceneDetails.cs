using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDetails : MonoBehaviour
{
    [SerializeField] List<SceneDetails> connectedScenes;
    public bool IsLoaded { get; private set; }

    List<SavableEntity> savableEntities;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //Debug.Log($"Entered {gameObject.name}");

            LoadScene();

            GameController.Instance.SetCurrentScene(this);
            //Load all connected Scenes
            foreach (var scene in connectedScenes)
            {
                scene.LoadScene();
            }

            var prevScene = GameController.Instance.PrevScene;
            if (prevScene != null)
            {
                var previouslyLoadedScenes = GameController.Instance.PrevScene.connectedScenes;
                foreach (var scene in previouslyLoadedScenes)
                {
                    if (!connectedScenes.Contains(scene) && scene != this)
                        scene.UnloadScene();
                }

                if (!connectedScenes.Contains(prevScene))
                {
                    prevScene.UnloadScene();
                    //Debug.Log("this one works too");
                }
            }

        }
    }

    public void LoadScene()
    {
        if (!IsLoaded)
        {
            var sceneName = gameObject.name;
            //Debug.Log($"Loading {sceneName}");
            var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            IsLoaded = true;

            operation.completed += (AsyncOperation op) =>
            {
                if (SavingSystem.i != null) 
                {
                savableEntities = GetSavableEntitiesInScene();
                SavingSystem.i.RestoreEntityStates(savableEntities);
                //Debug.Log($"{sceneName} loaded.");
                }
            };
        }
    }

    public void UnloadScene()
    {
        if (IsLoaded)
        {
            var sceneName = gameObject.name;
            //Debug.Log($"Unloading {sceneName}");
            if (SavingSystem.i != null)
            {
                SavingSystem.i.CaptureEntityStates(savableEntities);
            }
            SceneManager.UnloadSceneAsync(sceneName);
            IsLoaded = false;
            //Debug.Log($"{sceneName} unloaded.");
        }
    }



    List<SavableEntity> GetSavableEntitiesInScene()
    {
        var currScene = SceneManager.GetSceneByName(gameObject.name);
        var savableEntities = FindObjectsOfType<SavableEntity>().Where(x => x.gameObject.scene == currScene).ToList();

        return savableEntities;
    }
}
