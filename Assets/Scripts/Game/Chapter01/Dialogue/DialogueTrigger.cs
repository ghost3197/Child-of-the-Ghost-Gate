using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : InteractableBase
{
    [System.Serializable]
    private class DialogueLine
    {
        public string speakerName;

        [TextArea(2, 4)]
        public string text;
    }

    [Header("Dialogue")]
    [SerializeField] private DialogueLine[] lines;
    [SerializeField] private DialogueUIController dialogueUI;
    [SerializeField] private bool closeDialogueOnExit = true;
    [SerializeField] private bool interactableEnabled = true;
    [SerializeField] private UnityEvent onDialogueCompleted;

    [Header("Next Line Input")]
    [SerializeField] private KeyCode nextLineKey = KeyCode.Space;
    [SerializeField] private KeyCode alternativeNextLineKey = KeyCode.Return;

    [Header("Prompt Text")]
    [SerializeField] private string firstPromptText = "F - Talk";
    [SerializeField] private string continuePromptText = "Space / Enter - Next";
    [SerializeField] private string closePromptText = "Space / Enter - Close";

    private int currentLineIndex;
    private bool hasCompletedOnce;
    private bool isDialogueActive;
    private InteractionSensor activeInteractor;

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
            // FindFirstObjectByType ¡æ FindAnyObjectByType
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
            ShowNextLineOrClose();
            activeInteractor?.RefreshPrompt();
        }
    }

    public override bool CanInteract(InteractionSensor interactor)
    {
        return interactableEnabled && lines != null && lines.Length > 0;
    }

    public override void Interact(InteractionSensor interactor)
    {
        if (isDialogueActive)
        {
            return;
        }

        if (dialogueUI == null)
        {
            Debug.LogError("[DialogueTrigger] DialogueUIController was not found.", this);
            return;
        }

        activeInteractor = interactor;
        StartDialogue();
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

    public void SetInteractableEnabled(bool value)
    {
        interactableEnabled = value;
    }

    private void StartDialogue()
    {
        isDialogueActive = true;
        currentLineIndex = 0;
        ShowNextLineOrClose();
    }

    private void ShowNextLineOrClose()
    {
        if (currentLineIndex >= lines.Length)
        {
            CompleteDialogue();
            ResetDialogue();
            return;
        }

        DialogueLine currentLine = lines[currentLineIndex];
        dialogueUI.ShowLine(currentLine.speakerName, currentLine.text);
        currentLineIndex++;
    }

    private void ResetDialogue()
    {
        currentLineIndex = 0;
        isDialogueActive = false;

        if (dialogueUI != null)
        {
            dialogueUI.Hide();
        }

        activeInteractor?.RefreshPrompt();
        activeInteractor = null;
    }

    private void CompleteDialogue()
    {
        if (hasCompletedOnce)
        {
            return;
        }

        hasCompletedOnce = true;
        onDialogueCompleted?.Invoke();
    }
}
