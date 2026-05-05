using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : InteractableBase
{
    [Header("Dialogue")]
    [SerializeField] private string speakerName = "NPC";
    [SerializeField] [TextArea(2, 4)] private string[] lines;
    [SerializeField] private DialogueUIController dialogueUI;
    [SerializeField] private bool closeDialogueOnExit = true;
    [SerializeField] private bool interactableEnabled = true;
    [SerializeField] private UnityEvent onDialogueCompleted;

    [Header("Prompt Text")]
    [SerializeField] private string firstPromptText = "E - Talk";
    [SerializeField] private string continuePromptText = "E - Next";
    [SerializeField] private string closePromptText = "E - Close";

    private int currentLineIndex;
    private bool hasCompletedOnce;

    public override string PromptText
    {
        get
        {
            if (!interactableEnabled || lines == null || lines.Length == 0)
            {
                return string.Empty;
            }

            if (currentLineIndex >= lines.Length)
            {
                return closePromptText;
            }

            return currentLineIndex == 0 ? firstPromptText : continuePromptText;
        }
    }

    private void Awake()
    {
        if (dialogueUI == null)
        {
            dialogueUI = FindFirstObjectByType<DialogueUIController>();
        }
    }

    public override bool CanInteract(InteractionSensor interactor)
    {
        return interactableEnabled && lines != null && lines.Length > 0;
    }

    public override void Interact(InteractionSensor interactor)
    {
        if (dialogueUI == null)
        {
            Debug.LogError("[DialogueTrigger] DialogueUIController was not found.", this);
            return;
        }

        if (currentLineIndex >= lines.Length)
        {
            ResetDialogue();
            return;
        }

        bool isLastLine = currentLineIndex == lines.Length - 1;

        dialogueUI.ShowLine(speakerName, lines[currentLineIndex]);
        currentLineIndex++;

        if (isLastLine && !hasCompletedOnce)
        {
            hasCompletedOnce = true;
            onDialogueCompleted?.Invoke();
        }
    }

    public override void CancelInteraction(InteractionSensor interactor)
    {
        if (!closeDialogueOnExit)
        {
            return;
        }

        ResetDialogue();
    }

    private void ResetDialogue()
    {
        currentLineIndex = 0;

        if (dialogueUI != null)
        {
            dialogueUI.Hide();
        }
    }

    public void SetInteractableEnabled(bool value)
    {
        interactableEnabled = value;
    }
}
