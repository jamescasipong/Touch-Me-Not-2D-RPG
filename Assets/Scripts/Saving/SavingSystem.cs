using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;



public class SavingSystem : MonoBehaviour
{
    public static SavingSystem i { get; private set; }
    public bool loadSavedData = true;
    public bool loadNewData = true;
    public bool loadChap1 = true;
    public bool loadChap2 = true;
    public bool loadChap3 = true;
    public bool loadChap4 = true;
    public bool loadChap5 = true;
    public bool loadChap6 = true;
    public bool loadChap7 = true;
    public bool loadChap8 = true;
    public bool loadChap9 = true;
    public bool loadChap10 = true;

    private Dictionary<string, Dictionary<string, object>> sceneStates = new Dictionary<string, Dictionary<string, object>>();

    public void DontLoadData()
    {
        loadSavedData = false;
        loadNewData = false;
        loadChap1 = false;
        loadChap2 = false;
        loadChap3 = false;
        loadChap4 = false;
        loadChap5 = false;
        loadChap6 = false;
        loadChap7 = false;
        loadChap8 = false;
        loadChap9 = false;
        loadChap10 = false;
    }

    private void Awake()
    {
        if (i == null)
        {
            i = this;
            DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed between scenes
        }
        else
        {
            Destroy(gameObject);
        }

        

        Application.targetFrameRate = 60;
    }

    public void Start()
    {
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (loadSavedData)
        {
            loadSavedData = true;
            string saveFile = "saveSlot2";
            Load(saveFile);
        }
        else if (loadNewData)
        {
            loadNewData = true;
            string saveFiles = "NewGame";
            CutScene.instance.StartCutsceneWithPopInAndFadeOut("Napunta ka sa mundo ng Noli Me Tangere..");

            if (saveFiles != null)
            {
                Load(saveFiles);
            }
            Save(saveFiles);
            //Save("Chap1");
            sceneStates.Clear();
        }

        //Chap1
        else if (loadChap1) // Remove the "else" here
        {
            loadChap1 = true; // Set LoadChap1 to false before loading the chapter
            Load("Chap1");


        }
        else if (loadChap2)
        {
            loadChap2 = true;
            Load("Chap2");
        }
        else if (loadChap3)
        {
            loadChap3 = true;
            Load("Chap3");
        }
        else if (loadChap4)
        {
            loadChap4 = true;
            Load("Chap4");
        }
        else if (loadChap5)
        {
            loadChap5 = true;
            Load("Chap5");
        }
        else if (loadChap6)
        {
            loadChap6 = true;
            Load("Chap6");
        }
        else if (loadChap7)
        {
            loadChap7 = true;
            Load("Chap7");
        }
        else if (loadChap8)
        {
            loadChap8 = true;
            Load("Chap8");
        }
        else if (loadChap9)
        {
            loadChap9 = true;
            Load("Chap9");
        }
        else if (loadChap10)
        {
            loadChap10 = true;
            Load("Chap10");
        }
    }


    public void CaptureEntityStates(List<SavableEntity> savableEntities)
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (!sceneStates.ContainsKey(currentSceneName))
        {
            sceneStates[currentSceneName] = new Dictionary<string, object>();
        }

        foreach (SavableEntity savable in savableEntities)
        {
            sceneStates[currentSceneName][savable.UniqueId] = savable.CaptureState();
        }
    }

    public void RestoreEntityStates(List<SavableEntity> savableEntities)
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (sceneStates.ContainsKey(currentSceneName))
        {
            foreach (SavableEntity savable in savableEntities)
            {
                string id = savable.UniqueId;
                if (sceneStates[currentSceneName].ContainsKey(id))
                {
                    savable.RestoreState(sceneStates[currentSceneName][id]);
                }
            }
        }
    }

    public void Save(string saveFile)
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Convert the array of SavableEntity to a List<SavableEntity>
        List<SavableEntity> savableEntities = new List<SavableEntity>(FindObjectsOfType<SavableEntity>());

        // Make sure to capture entity states before saving.
        CaptureEntityStates(savableEntities);

        if (!sceneStates.ContainsKey(currentSceneName))
        {
            sceneStates[currentSceneName] = new Dictionary<string, object>();
        }

        SaveFile($"{currentSceneName}_{saveFile}", sceneStates[currentSceneName]);
    }

    public void Load(string saveFile)
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        sceneStates[currentSceneName] = LoadFile($"{currentSceneName}_{saveFile}");
        RestoreState(sceneStates[currentSceneName]);
    }

    public void Delete(string saveFile)
    {
        string path = GetPath(saveFile);
        File.Delete(path);
    }

    private void CaptureState(Dictionary<string, object> state)
    {
        foreach (SavableEntity savable in FindObjectsOfType<SavableEntity>())
        {
            state[savable.UniqueId] = savable.CaptureState();
        }
    }

    private void RestoreState(Dictionary<string, object> state)
    {
        foreach (SavableEntity savable in FindObjectsOfType<SavableEntity>())
        {
            string id = savable.UniqueId;
            if (state.ContainsKey(id))
            {
                savable.RestoreState(state[id]);
            }
        }
    }

    void SaveFile(string saveFile, Dictionary<string, object> state)
    {
        string path = GetPath(saveFile);
        Debug.Log($"Saving to {path}");

        using (FileStream fs = File.Open(path, FileMode.Create))
        {
            // Serialize our object
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fs, state);
        }
    }

    Dictionary<string, object> LoadFile(string saveFile)
    {
        string path = GetPath(saveFile);
        if (!File.Exists(path))
            return new Dictionary<string, object>();

        using (FileStream fs = File.Open(path, FileMode.Open))
        {
            // Deserialize our object
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            return (Dictionary<string, object>)binaryFormatter.Deserialize(fs);
        }
    }

    public string GetPath(string saveFile)
    {
        return Path.Combine(Application.persistentDataPath, saveFile);
    }
}
