using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour, ISavable
{
    [SerializeField] List<Sprite> walkDownSprites;
    [SerializeField] List<Sprite> walkUpSprites;
    [SerializeField] List<Sprite> walkRightSprites;
    [SerializeField] List<Sprite> walkLeftSprites;


    [SerializeField] List<Sprite> walkDownFemaleSprites;
    [SerializeField] List<Sprite> walkUpFemaleSprites;
    [SerializeField] List<Sprite> walkRightFemaleSprites;
    [SerializeField] List<Sprite> walkLeftFemaleSprites;

    [SerializeField] List<Sprite> idleDownSprites;
    [SerializeField] List<Sprite> idleUpSprites;
    [SerializeField] List<Sprite> idleRightSprites;
    [SerializeField] List<Sprite> idleLeftSprites;

    SpriteAnimator idleDownAnim;
    SpriteAnimator idleUpAnim;
    SpriteAnimator idleRightAnim;
    SpriteAnimator idleLeftAnim;
    bool issmoving;
    public GenderState genderState;
    // Parameters
    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }

    
    public enum InitialDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    [SerializeField]
    private InitialDirection initialDirection = InitialDirection.Down;

    // States
    SpriteAnimator walkDownAnim;
    SpriteAnimator walkUpAnim;
    SpriteAnimator walkRightAnim;
    SpriteAnimator walkLeftAnim;
    SpriteAnimator currentAnim;
    bool wasPreviouslyMoving;

    // References
    SpriteRenderer spriteRenderer;
    public PlayerController playerController;


    private void Start()
    {

        if (playerController != null)
        {
            if (MainMenu.instance.genderState == GenderState.Male)
            {
                Debug.Log("Instantiate Male");
                spriteRenderer = GetComponent<SpriteRenderer>();
                walkDownAnim = new SpriteAnimator(walkDownSprites, spriteRenderer);
                walkUpAnim = new SpriteAnimator(walkUpSprites, spriteRenderer);
                walkRightAnim = new SpriteAnimator(walkRightSprites, spriteRenderer);
                walkLeftAnim = new SpriteAnimator(walkLeftSprites, spriteRenderer);

                switch (initialDirection)
                {
                    case InitialDirection.Up:
                        currentAnim = walkUpAnim;
                        break;
                    case InitialDirection.Down:
                        currentAnim = walkDownAnim;
                        break;
                    case InitialDirection.Left:
                        currentAnim = walkLeftAnim;
                        break;
                    case InitialDirection.Right:
                        currentAnim = walkRightAnim;
                        break;
                    default:
                        currentAnim = walkLeftAnim; // Default to Left
                        break;
                }


            }
            else if (MainMenu.instance.genderState == GenderState.Female && playerController != null)

            {
                Debug.Log("Instantiate FMale");
                spriteRenderer = GetComponent<SpriteRenderer>();
                walkDownAnim = new SpriteAnimator(walkDownFemaleSprites, spriteRenderer);
                walkUpAnim = new SpriteAnimator(walkUpFemaleSprites, spriteRenderer);
                walkRightAnim = new SpriteAnimator(walkRightFemaleSprites, spriteRenderer);
                walkLeftAnim = new SpriteAnimator(walkLeftFemaleSprites, spriteRenderer);

                switch (initialDirection)
                {
                    case InitialDirection.Up:
                        currentAnim = walkUpAnim;
                        break;
                    case InitialDirection.Down:
                        currentAnim = walkDownAnim;
                        break;
                    case InitialDirection.Left:
                        currentAnim = walkLeftAnim;
                        break;
                    case InitialDirection.Right:
                        currentAnim = walkRightAnim;
                        break;
                    default:
                        currentAnim = walkLeftAnim; // Default to Left
                        break;
                }

                /*idleDownAnim = new SpriteAnimator(idleDownSprites, spriteRenderer, 0.25f);
                idleUpAnim = new SpriteAnimator(idleUpSprites, spriteRenderer, 0.25f);
                idleRightAnim = new SpriteAnimator(idleRightSprites, spriteRenderer, 0.25f);
                idleLeftAnim = new SpriteAnimator(idleLeftSprites, spriteRenderer, 0.25f);

                switch (initialDirection)
                {
                    case InitialDirection.Up:
                        currentAnim = idleUpAnim;
                        break;
                    case InitialDirection.Down:
                        currentAnim = idleDownAnim;
                        break;
                    case InitialDirection.Left:
                        currentAnim = idleLeftAnim;
                        break;
                    case InitialDirection.Right:
                        currentAnim = idleRightAnim;
                        break;
                    default:
                        currentAnim = idleRightAnim; // Default to Left
                        break;
                }*/
            }
        }
        else
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            walkDownAnim = new SpriteAnimator(walkDownSprites, spriteRenderer);
            walkUpAnim = new SpriteAnimator(walkUpSprites, spriteRenderer);
            walkRightAnim = new SpriteAnimator(walkRightSprites, spriteRenderer);
            walkLeftAnim = new SpriteAnimator(walkLeftSprites, spriteRenderer);

            switch (initialDirection)
            {
                case InitialDirection.Up:
                    currentAnim = walkUpAnim;
                    break;
                case InitialDirection.Down:
                    currentAnim = walkDownAnim;
                    break;
                case InitialDirection.Left:
                    currentAnim = walkLeftAnim;
                    break;
                case InitialDirection.Right:
                    currentAnim = walkRightAnim;
                    break;
                default:
                    currentAnim = walkLeftAnim; // Default to Left
                    break;
            }
        }

        
    }


    private void Update()
    {

        if (playerController != null)
        {
            if (MainMenu.instance.genderState == GenderState.Male)
            {
                var prevAnim = currentAnim;



                // Determine the animation based on input
                if (Mathf.Abs(MoveX) > Mathf.Abs(MoveY))
                {
                    if (MoveX > 0)
                        currentAnim = walkRightAnim;
                    else if (MoveX < 0)
                        currentAnim = walkLeftAnim;
                }
                else
                {
                    if (MoveY > 0)
                        currentAnim = walkUpAnim;
                    else if (MoveY < 0)
                        currentAnim = walkDownAnim;
                }

                if (currentAnim != prevAnim || IsMoving != wasPreviouslyMoving)
                    currentAnim.Start();

                if (IsMoving)
                    currentAnim.HandleUpdate();
                else
                    spriteRenderer.sprite = currentAnim.Frames[0];

                wasPreviouslyMoving = IsMoving;
            }

            else if (MainMenu.instance.genderState == GenderState.Female && playerController != null)
            {
                //AnimationNIdle();

                var prevAnim = currentAnim;



                // Determine the animation based on input
                if (Mathf.Abs(MoveX) > Mathf.Abs(MoveY))
                {
                    if (MoveX > 0)
                        currentAnim = walkRightAnim;
                    else if (MoveX < 0)
                        currentAnim = walkLeftAnim;
                }
                else
                {
                    if (MoveY > 0)
                        currentAnim = walkUpAnim;
                    else if (MoveY < 0)
                        currentAnim = walkDownAnim;
                }

                if (currentAnim != prevAnim || IsMoving != wasPreviouslyMoving)
                    currentAnim.Start();

                if (IsMoving)
                    currentAnim.HandleUpdate();
                else
                    spriteRenderer.sprite = currentAnim.Frames[0];

                wasPreviouslyMoving = IsMoving;

            }
        }
        else
        {
            var prevAnim = currentAnim;

            // Determine the animation based on input
            if (Mathf.Abs(MoveX) > Mathf.Abs(MoveY))
            {
                if (MoveX > 0)
                    currentAnim = walkRightAnim;
                else if (MoveX < 0)
                    currentAnim = walkLeftAnim;
            }
            else
            {
                if (MoveY > 0)
                    currentAnim = walkUpAnim;
                else if (MoveY < 0)
                    currentAnim = walkDownAnim;
            }

            if (currentAnim != prevAnim || IsMoving != wasPreviouslyMoving)
                currentAnim.Start();

            if (IsMoving)
                currentAnim.HandleUpdate();
            else
                spriteRenderer.sprite = currentAnim.Frames[0];

            wasPreviouslyMoving = IsMoving;
        }
    }

    void AnimationNIdle()
    {
        var prevAnim = currentAnim;

        // Determine the animation based on input
        if (Mathf.Abs(MoveX) > Mathf.Abs(MoveY))
        {
            if (MoveX > 0)
            {
                currentAnim = walkRightAnim;
                if (!IsMoving)
                {
                    currentAnim = idleRightAnim;
                }
            }
            else if (MoveX < 0)
            {
                currentAnim = walkLeftAnim;
                if (!IsMoving)
                {
                    currentAnim = idleLeftAnim;
                }
            }
        }
        else
        {
            if (MoveY > 0)
            {
                currentAnim = walkUpAnim;
                if (!IsMoving)
                {
                    currentAnim = idleUpAnim;
                }
            }
            else if (MoveY < 0)
            {
                currentAnim = walkDownAnim;
                if (!IsMoving)
                {
                    currentAnim = idleDownAnim;
                }
            }
        }

        StartCoroutine(ISMOving());
    }

    IEnumerator ISMOving()
    {

        if (IsMoving)
        {

            currentAnim.HandleUpdate();
            issmoving = false;
        }
        else
        {
            if (!issmoving)
            {
                spriteRenderer.sprite = currentAnim.Frames[0];
                issmoving = true;
            }
            currentAnim.HandleUpdate();
        }
        yield return null;
    }

    public object CaptureState()
    {
        CharacterAnimatorState state = new CharacterAnimatorState
        {
            MoveX = MoveX,
            MoveY = MoveY,
            IsMoving = IsMoving,
            CurrentAnimName = currentAnim != null ? currentAnim.ToString() : "DefaultAnimationName",
            GenderState = genderState // Save the genderState
        };
        return state;
    }

    public void RestoreState(object state)
    {
        if (state is CharacterAnimatorState charState)
        {
            MoveX = charState.MoveX;
            MoveY = charState.MoveY;
            IsMoving = charState.IsMoving;
            genderState = charState.GenderState; // Restore the genderState

            // Restore the animation based on the animation name stored in the state
            switch (charState.CurrentAnimName)
            {
                case "walkDownAnim":
                    currentAnim = walkDownAnim;
                    break;
                case "walkUpAnim":
                    currentAnim = walkUpAnim;
                    break;
                case "walkRightAnim":
                    currentAnim = walkRightAnim;
                    break;
                case "walkLeftAnim":
                    currentAnim = walkLeftAnim;
                    break;
                default:
                    // Default animation if needed
                    break;
            }
        }
    }

}

// Define a serializable data structure to capture the state
[System.Serializable]
public class CharacterAnimatorState
{
    public float MoveX;
    public float MoveY;
    public bool IsMoving;
    public string CurrentAnimName;
    public GenderState GenderState; // Include the genderState variable
}

public enum GenderState
{
    NonBinary,
    Male,
    Female
}