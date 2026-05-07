using UnityEngine;
using UnityEngine.Events;

public class InspectableObject : InteractableBase
{
    [Header("Dialogue")]
    [SerializeField] private DialogueData inspectDialogue;
    [SerializeField] private DialogueData.DialogueLine[] inlineLines;
    [SerializeField] private DialogueUIController dialogueUI;
    [SerializeField] private bool closeDialogueOnExit = true;

    [Header("Inspection")]
    [SerializeField] private bool inspectOnce = true;
    [SerializeField] private string firstPromptText = "E - Inspect";
    [SerializeField] private string continuePromptText = "Space / Enter - Next";
    [SerializeField] private string closePromptText = "Space / Enter - Close";
    [SerializeField] private KeyCode nextLineKey = KeyCode.Space;
    [SerializeField] private KeyCode alternativeNextLineKey = KeyCode.Return;

    [Header("Events")]
    [SerializeField] private UnityEvent onInspected;

    private int currentLineIndex;
    private bool isDialogueActive;
    private InteractionSensor activeInteractor;
    private DialogueData.DialogueLine[] activeLines;

    public bool IsInspected { get; private set; }

    public override string PromptText
    {
        get
        {
            if (GetLineCount() == 0)
            {
                return string.Empty;
            }

            if (!isDialogueActive)
            {
                return firstPromptText;
            }

            if (currentLineIndex >= activeLines.Length && !IsTypingLine())
            {
                return closePromptText;
            }

            return continuePromptText;
        }
    }

    private void Awake()
    {
        if (dialogueUI == null)
        {
            dialogueUI = FindAnyObjectByType<DialogueUIController>();
        }
    }

    private void Update()
    {
        if (!isDialogueActive)
        {
            return;
        }

        if (Input.GetKeyDown(nextLineKey) ||
            Input.GetKeyDown(alternativeNextLineKey) ||
            Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (IsTypingLine())
            {
                dialogueUI.CompleteTyping();
                activeInteractor?.RefreshPrompt();
                return;
            }

            ShowNextLineOrClose();
            activeInteractor?.RefreshPrompt();
        }
    }

    public override bool CanInteract(InteractionSensor interactor)
    {
        return (!inspectOnce || !IsInspected) && GetLineCount() > 0;
    }

    public override void Interact(InteractionSensor interactor)
    {
        if (isDialogueActive || (inspectOnce && IsInspected))
        {
            return;
        }

        if (dialogueUI == null)
        {
            dialogueUI = FindAnyObjectByType<DialogueUIController>();
        }

        if (dialogueUI == null)
        {
            Debug.LogError("[InspectableObject] DialogueUIController was not found.", this);
            return;
        }

        activeLines = ResolveLines();

        if (activeLines == null || activeLines.Length == 0)
        {
            Debug.LogWarning("[InspectableObject] No dialogue lines were assigned.", this);
            return;
        }

        activeInteractor = interactor;
        isDialogueActive = true;
        currentLineIndex = 0;
        ShowNextLineOrClose();
        activeInteractor?.RefreshPrompt();
    }

    public override void CancelInteraction(InteractionSensor interactor)
    {
        if (!closeDialogueOnExit)
        {
            return;
        }

        ResetDialogue();
    }

    private void ShowNextLineOrClose()
    {
        if (activeLines == null || currentLineIndex >= activeLines.Length)
        {
            CompleteInspection();
            ResetDialogue();
            return;
        }

        DialogueData.DialogueLine currentLine = activeLines[currentLineIndex];
        dialogueUI.ShowLine(currentLine.speakerName, currentLine.text);
        currentLineIndex++;
    }

    private void CompleteInspection()
    {
        if (IsInspected)
        {
            return;
        }

        IsInspected = true;
        onInspected?.Invoke();
    }

    private void ResetDialogue()
    {
        currentLineIndex = 0;
        isDialogueActive = false;
        activeLines = null;

        if (dialogueUI != null)
        {
            dialogueUI.Hide();
        }

        activeInteractor?.RefreshPrompt();
        activeInteractor = null;
    }

    private int GetLineCount()
    {
        if (inspectDialogue != null && inspectDialogue.lines != null && inspectDialogue.lines.Length > 0)
        {
            return inspectDialogue.lines.Length;
        }

        return inlineLines != null ? inlineLines.Length : 0;
    }

    private DialogueData.DialogueLine[] ResolveLines()
    {
        if (inspectDialogue != null && inspectDialogue.lines != null && inspectDialogue.lines.Length > 0)
        {
            return inspectDialogue.lines;
        }

        return inlineLines;
    }

    private bool IsTypingLine()
    {
        return dialogueUI != null && dialogueUI.IsTyping;
    }
}
