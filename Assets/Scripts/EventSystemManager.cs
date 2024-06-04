using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemManager : MonoBehaviour
{
    private static EventSystemManager instance;

    private void Awake()
    {
        // Check if an instance of the EventSystemManager already exists
        if (instance == null)
        {
            // If not, set this instance as the singleton
            instance = this;

            // Make sure this GameObject persists across scene changes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy this GameObject
            Destroy(gameObject);
        }
    }

    // Function to destroy the Event System when needed
    public static void DestroyEventSystem()
    {
        if (instance != null)
        {
            // Destroy the Event System GameObject
            Destroy(instance.gameObject);
        }
    }
}
