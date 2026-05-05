using UnityEngine;

public class Chapter01FlowController : MonoBehaviour
{
    [Header("Dialogue Order")]
    [SerializeField] private DialogueTrigger fatherDialogue;
    [SerializeField] private DialogueTrigger motherDialogue;

    private void Awake()
    {
        if (fatherDialogue != null)
        {
            fatherDialogue.SetInteractableEnabled(true);
        }

        if (motherDialogue != null)
        {
            motherDialogue.SetInteractableEnabled(false);
        }
    }

    public void UnlockMotherDialogue()
    {
        if (motherDialogue == null)
        {
            Debug.LogWarning("[Chapter01FlowController] Mother dialogue is not assigned.", this);
            return;
        }

        motherDialogue.SetInteractableEnabled(true);
    }
}
