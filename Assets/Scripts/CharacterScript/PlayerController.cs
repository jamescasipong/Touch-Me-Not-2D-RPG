using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UI;
using System.Linq;
using SimpleInputNamespace;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour, ISavable
{

    public Vector2 input;
    //private Animator animator;

    private Character character;
    public GameObject playerLight;
    public Button upButton;
    public Button downButton;
    public Button leftButton;
    public Button rightButton;

    [SerializeField] public Transform thisPlayer;

    public FixedJoystick joystick;
    public static PlayerController instance;

    public GameObject thisPlayerObject;
    public string Name { get; set; }

    [SerializeField] AudioSource playerAudioSource;
    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void Awake()
    {
        character = GetComponent<Character>();

        instance = this;
    }

    public void PlayerLightDisable()
    {
        if (playerLight != null) // Check if the object is not null before performing operations on it
        {
            playerLight.SetActive(false);
        }

        else
        {
            Debug.LogWarning("Player light GameObject is null or has been destroyed.");
        }
    }

    public void PlayerLightEnable()
    {
        playerLight.SetActive(true);
    }

    

    

    public static PlayerController GetPlayer()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    //players movement
    public void HandleUpdate()
    {
        if (!character.IsMoving)
        {
            // Check if a joystick is connected and has input
            if (joystick != null && joystick.gameObject.activeInHierarchy && joystick.Direction != Vector2.zero)
            {
                input = joystick.Direction;
            }
            else
            {
                float movY = SimpleInput.GetAxisRaw("Vertical");
                float movX = SimpleInput.GetAxisRaw("Horizontal");
                input.x = movX;
                input.y = movY;
            }

            //input.Normalize();

            if (input != Vector2.zero)
            {
                StartCoroutine(character.Move(input, OnMoveOver));  
            }
        }


        


        character.HandleUpdate();

        if (Input.GetKeyDown(KeyCode.E))
            StartCoroutine(Interact());
    }


    public void InteractBtn()
    {
        playerAudioSource.Play();
        StartCoroutine(Interact());
    }

    IPlayerTriggerable currentlyInTrigger;
    private void OnMoveOver()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position - new Vector3(0, character.yOffset), 0.2f, GameLayers.i.TriggerableLayers);

        IPlayerTriggerable triggerable = null; // Initialize triggerable to null

        foreach (var collider in colliders)
        {
            triggerable = collider.GetComponent<IPlayerTriggerable>();
            if (triggerable != null)
            {
                if (triggerable == currentlyInTrigger && !triggerable.TriggerRepeatedly)
                    break;

                triggerable.OnPlayerTriggered(this);
                currentlyInTrigger = triggerable;
                break; // You may or may not want to break here, depending on your logic
            }
        }

        // Move this condition outside of the loop
        if (colliders.Count() == 0 || triggerable != currentlyInTrigger)
            currentlyInTrigger = null;
    }



    //this is when the player interacts with an NPC
    IEnumerator Interact()
    {
        var facingDir = new Vector3(character.Animator.MoveX, character.Animator.MoveY);
        var interactPos = transform.position + facingDir;

        //Debug.DrawLine(transform.position, interactPos, Color.black, 0.5f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.i.InteractableLayer);
        if (collider != null)
        {
            yield return collider.GetComponent<Interactable>()?.Interact(transform);

        }
    }

    public object CaptureState()
    {
        return new SerializableVector3(transform.position);
    }

    public void RestoreState(object state)
    {
        var position = (SerializableVector3)state;
        transform.position = position.ToVector3();
    }


    public Character Character => character;

}

[Serializable]
public class SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializableVector3(Vector3 vector3)
    {
        x = vector3.x;
        y = vector3.y;
        z = vector3.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}
