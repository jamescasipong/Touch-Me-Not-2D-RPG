using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour, Interactable
{
    [Header("Quiz Status")]
    [SerializeField] private GameObject questStatus;
    [SerializeField] private string backtoChap;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    public bool enablePlayDirector;
    public PlayableDirector playerDirector;
    public string quizNameIcon;
    [Header("Portal")]
    [SerializeField] public GameObject portal;
    public Sprite characterIcon;
    private bool playerInRange;

    public static DialogueTrigger Instance;

    private void Awake()
    {
        playerInRange = false;
        questStatus.SetActive(false);

        Instance = this;

        portal.SetActive(false);
    }

    public void ShowPortal()
    {
        portal.SetActive(true);
    }

    private void Update()
    {
        if (playerInRange && !QuizDialogue.GetInstance().dialogueIsPlaying)
        {
            questStatus.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                QuizDialogue.GetInstance().EnterDialogueMode(inkJSON);

                
            }
        }
        else
        {
            questStatus.SetActive(false);
        }

        
    }

    

    public void BacktoChap()
    {
        SavingSystem.i.Load(backtoChap);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;
            ButtonHandlers.Instance.InteractObjects();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
            ButtonHandlers.Instance.HideInteractObjects();
        }
    }

    public IEnumerator Interact(Transform initiator)
    {
        QuizDialogue.GetInstance().EnterDialogueMode(inkJSON);
        QuizDialogue.instance.icon.sprite = characterIcon;
        yield return null;
    }

}
