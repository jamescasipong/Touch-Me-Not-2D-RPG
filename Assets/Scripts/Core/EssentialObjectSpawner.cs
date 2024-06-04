using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject essentialObjectsPrefab;
    [SerializeField] GameObject loader;
    [SerializeField] Vector3 vector3;

    private static bool created = false;

    private void Awake()
    {
        if (!created)
        {
            // Ensure that this object persists across scenes
            DontDestroyOnLoad(this.gameObject);

            // Check if an instance of essentialObjectsPrefab already exists
            var existingObjects = FindObjectsOfType<EssentialObjects>();
            if (existingObjects.Length == 0)
            {
                var spawnPos = vector3;

                var grid = FindObjectOfType<Grid>();
                if (grid != null)
                    spawnPos = grid.transform.position;
                Instantiate(essentialObjectsPrefab, spawnPos, Quaternion.identity);
            }

            Destroy(loader);
            created = true;
        }
        else
        {
            // If another instance already exists, destroy this one
            Destroy(this.gameObject);
        }
    }
}
