using UnityEngine;
using UnityEngine.Events;

public class DoorObject : InteractableBase
{
    [Header("Door")]
    [SerializeField] private Transform destination;
    [SerializeField] private string promptText = "E - Open Door";

    [Header("Unlock Conditions")]
    [SerializeField] private InspectableObject[] requiredInspections;
    [SerializeField] private DialogueTrigger[] requiredDialogues;

    [Header("Locked Dialogue")]
    [SerializeField] private DialogueData lockedDialogue;
    [SerializeField] private DialogueUIController dialogueUI;
    [SerializeField] private bool closeDialogueOnExit = true;
    [SerializeField] private KeyCode nextLineKey = KeyCode.Space;
    [SerializeField] private KeyCode alternativeNextLineKey = KeyCode.Return;
    [SerializeField] private string lockedContinuePromptText = "Space / Enter - Next";
    [SerializeField] private string lockedClosePromptText = "Space / Enter - Close";

    [Header("Events")]
    [SerializeField] private UnityEvent onDoorOpened;

    private InteractionSensor activeInteractor;
    private DialogueData.DialogueLine[] lockedLines;
    private int currentLineIndex;
    private bool isLockedDialogueActive;

    public override string PromptText
    {
        get
        {
            if (!isLockedDialogueActive)
            {
                return promptText;
            }

            if (lockedLines != null && currentLineIndex >= lockedLines.Length && !IsTypingLine())
            {
                return lockedClosePromptText;
            }

            return lockedContinuePromptText;
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
        if (!isLockedDialogueActive)
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

            ShowLockedLineOrClose();
            activeInteractor?.RefreshPrompt();
        }
    }

    public override void Interact(InteractionSensor interactor)
    {
        if (isLockedDialogueActive)
        {
            return;
        }

        if (!CheckConditions())
        {
            StartLockedDialogue(interactor);
            return;
        }

        MovePlayerToDestination();
        onDoorOpened?.Invoke();
        interactor?.RefreshPrompt();
    }

    public override void CancelInteraction(InteractionSensor interactor)
    {
        if (!closeDialogueOnExit)
        {
            return;
        }

        ResetLockedDialogue();
    }

    private bool CheckConditions()
    {
        if (requiredInspections != null)
        {
            foreach (InspectableObject inspectable in requiredInspections)
            {
                if (inspectable != null && !inspectable.IsInspected)
                {
                    return false;
                }
            }
        }

        if (requiredDialogues != null)
        {
            foreach (DialogueTrigger dialogue in requiredDialogues)
            {
                if (dialogue != null && !dialogue.HasTriggered)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void StartLockedDialogue(InteractionSensor interactor)
    {
        lockedLines = lockedDialogue != null ? lockedDialogue.lines : null;

        if (lockedLines == null || lockedLines.Length == 0)
        {
            return;
        }

        if (dialogueUI == null)
        {
            dialogueUI = FindAnyObjectByType<DialogueUIController>();
        }

        if (dialogueUI == null)
        {
            Debug.LogError("[DoorObject] DialogueUIController was not found.", this);
            return;
        }

        activeInteractor = interactor;
        currentLineIndex = 0;
        isLockedDialogueActive = true;
        ShowLockedLineOrClose();
        activeInteractor?.RefreshPrompt();
    }

    private void ShowLockedLineOrClose()
    {
        if (lockedLines == null || currentLineIndex >= lockedLines.Length)
        {
            ResetLockedDialogue();
            return;
        }

        DialogueData.DialogueLine currentLine = lockedLines[currentLineIndex];
        dialogueUI.ShowLine(currentLine.speakerName, currentLine.text);
        currentLineIndex++;
    }

    private void ResetLockedDialogue()
    {
        currentLineIndex = 0;
        isLockedDialogueActive = false;
        lockedLines = null;

        if (dialogueUI != null)
        {
            dialogueUI.Hide();
        }

        activeInteractor?.RefreshPrompt();
        activeInteractor = null;
    }

    private void MovePlayerToDestination()
    {
        if (destination == null)
        {
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            player.transform.position = destination.position;
        }
    }

    private bool IsTypingLine()
    {
        return dialogueUI != null && dialogueUI.IsTyping;
    }
}
