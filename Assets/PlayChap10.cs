using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class PlayChap10 : MonoBehaviour
{
    public PlayableDirector chap10;

    bool playchap10 = false;

    private void Start()
    {
        playchap10 = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!playchap10)
            {
                StartCoroutine(CHap10());
            }
        }
    }

    IEnumerator CHap10()
    {
        UIsManager.instance.DisableUIS();
        chap10.Play();

        yield return new WaitForSeconds((float)chap10.duration);

        SceneManager.LoadScene("MainMenu");
        Destroy(PlayerController.instance.thisPlayerObject);

    }
}
