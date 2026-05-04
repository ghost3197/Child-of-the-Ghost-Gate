using UnityEngine;

public class DialogueTrigger : InteractableBase
{
    [Header("Dialogue")]
    [SerializeField] private string speakerName = "NPC";
    [SerializeField] [TextArea(2, 4)] private string[] lines;
    [SerializeField] private DialogueUIController dialogueUI;
    [SerializeField] private bool closeDialogueOnExit = true;

    [Header("Prompt Text")]
    [SerializeField] private string firstPromptText = "E - Talk";
    [SerializeField] private string continuePromptText = "E - Next";

    private int currentLineIndex;

    public override string PromptText
    {
        get
        {
            if (lines == null || lines.Length == 0)
            {
                return string.Empty;
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
        return lines != null && lines.Length > 0;
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

        dialogueUI.ShowLine(speakerName, lines[currentLineIndex]);
        currentLineIndex++;
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
}
