using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    Image image;

    public static Fader instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed on scene load
        }
        else
        {
            // If an instance already exists, destroy this duplicate
            Destroy(gameObject);
            return;
        }

        image = GetComponent<Image>();
    }

    public void FadersIn()
    {
        StartCoroutine(FadeIn(0.5f));
    }

    public void FadersOut()
    {
        StartCoroutine(FadeOut(0.5f));
    }

    public IEnumerator FadeIn(float time)
    {
        yield return image.DOFade(1f, time).WaitForCompletion();
    }

    public IEnumerator FadeOut(float time)
    {
        yield return image.DOFade(0f, time).WaitForCompletion();
    }


    public IEnumerator CutSceneIn(float time)
    {
        yield return image.DOFade(5f, time).WaitForCompletion();
    }

    public IEnumerator CutSceneOut(float time)
    {
        yield return image.DOFade(1f, time).WaitForCompletion();
    }
}
