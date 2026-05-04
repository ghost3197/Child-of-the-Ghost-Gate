using UnityEngine;
using UnityEngine.UI;

public class InteractionSensor : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Prompt UI")]
    [SerializeField] private GameObject promptRoot;
    [SerializeField] private Text promptText;

    private InteractableBase currentInteractable;

    private void Awake()
    {
        HidePrompt();
    }

    private void Update()
    {
        if (currentInteractable == null)
        {
            return;
        }

        if (Input.GetKeyDown(interactKey) && currentInteractable.CanInteract(this))
        {
            currentInteractable.Interact(this);
            RefreshPrompt();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TrySetCurrentInteractable(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (currentInteractable == null)
        {
            TrySetCurrentInteractable(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        InteractableBase interactable = other.GetComponent<InteractableBase>();

        if (interactable == null)
        {
            interactable = other.GetComponentInParent<InteractableBase>();
        }

        if (interactable == null || interactable != currentInteractable)
        {
            return;
        }

        currentInteractable.CancelInteraction(this);
        currentInteractable = null;
        HidePrompt();
    }

    public void RefreshPrompt()
    {
        if (currentInteractable == null)
        {
            HidePrompt();
            return;
        }

        if (promptRoot != null)
        {
            promptRoot.SetActive(true);
        }

        if (promptText != null)
        {
            promptText.text = currentInteractable.PromptText;
        }
    }

    private void TrySetCurrentInteractable(Collider2D other)
    {
        InteractableBase interactable = other.GetComponent<InteractableBase>();

        if (interactable == null)
        {
            interactable = other.GetComponentInParent<InteractableBase>();
        }

        if (interactable == null || !interactable.CanInteract(this))
        {
            return;
        }

        currentInteractable = interactable;
        RefreshPrompt();
    }

    private void HidePrompt()
    {
        if (promptRoot != null)
        {
            promptRoot.SetActive(false);
        }
    }
}
