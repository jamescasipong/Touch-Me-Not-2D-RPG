using UnityEngine;
using DG.Tweening;

public class InventoryAnimation : MonoBehaviour
{
    public float animationDuration = 0.5f;
    public Vector3 openScale = new Vector3(1f, 1f, 1f);
    public Vector3 closedScale = new Vector3(0f, 0f, 0f);
    public GameObject inventory;
    public Ease easingType = Ease.OutBack; // Change this to the desired ease type


    public static InventoryAnimation instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Set the initial scale to closedScale
       // transform.localScale = closedScale;

        // Optionally, animate the initial opening on Start
      //  CloseInventory();
    }


    private bool isOpen = false;

    public void ToggleInventory()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            OpenInventory();
        }
        else
        {
            CloseInventory();
        }
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            OpenInventory();
        }
        else if (Input.GetKey(KeyCode.O))
        {
            CloseInventory();
        }
    }
    public void OpenInventory()
    {
        inventory.SetActive(true); // Activate the inventory GameObject before the animation starts
        transform.DOScale(openScale, animationDuration)
            .SetEase(easingType);
        // Add any other opening animations here if needed
    }


    public void CloseInventory()
    {
        transform.DOScale(closedScale, animationDuration)
            .SetEase(easingType)
            .OnComplete(() =>
            {
                inventory.SetActive(false); // Deactivate the inventory GameObject after the animation completes
            });
        // Add any other closing animations here if needed
    }

}
