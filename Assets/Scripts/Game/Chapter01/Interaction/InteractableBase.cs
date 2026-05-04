using UnityEngine;

public abstract class InteractableBase : MonoBehaviour
{
    [Header("Prompt")]
    [SerializeField] private string promptText = "E - Interact";

    public virtual string PromptText => promptText;

    public virtual bool CanInteract(InteractionSensor interactor)
    {
        return true;
    }

    public abstract void Interact(InteractionSensor interactor);

    public virtual void CancelInteraction(InteractionSensor interactor)
    {
    }
}
