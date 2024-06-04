using UnityEngine;

public class SingletonObject : MonoBehaviour
{
    private static SingletonObject instance;

    // This method is called when the object is created.
    private void Awake()
    {
        if (instance == null)
        {
            // This is the first instance; keep it.
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Another instance already exists; destroy this one.
            Destroy(gameObject);
        }
    }
}
