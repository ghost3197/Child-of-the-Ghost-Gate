using System.Xml.Serialization;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DialogueTrigger : InteractableBase
{
    [Header("Dialogue")]
    [System.Serializable]
    private class DialogueLine
    { 
        public string speakerName;

        [TextArea(2, 4)]
        public string text;
    }
    [SerializeField] private DialogueLine[] lines;
    [SerializeField] private DialogueUIController dialogueUI;
    [SerializeField] private bool closeDialogueOnExit = true;
    [SerializeField] private bool interactableEnabled = true;
    [SerializeField] private UnityEvent onDialogueCompleted;

    [SerializeField] private KeyCode nextLineKey = KeyCode.Space;
    [SerializeField] private KeyCode alternativeNextLineKey = KeyCode.Return;

    private bool isDialogueActive;
    private InteractionSensor activeInterractor;

    [Header("Prompt Text")]
    [SerializeField] private string firstPromptText = "E - Talk";
    [SerializeField] private string continuePromptText = "Space / Enter - Next";
    [SerializeField] private string closePromptText = "Space / Enter - Close";

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
        if (isDialogueActive)
        {
            return;
        }

        if (dialogueUI == null)
        {
            Debug.LogError("[DialogueTrigger] DialogueUIController was not found", this);
            return;
        }

        activeInterractor = interactor;
        StarDilogue();
        activeInterractor.RefreshPrompt();
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
        isDialogueActive = false;

        if (dialogueUI != null)
        {
            dialogueUI.Hide();
        }

        activeInterractor?.RefreshPrompt();
        activeInterractor = null;
    }

    public void SetInteractableEnabled(bool value)
    {
        interactableEnabled = value;
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
            activeInterractor?.RefreshPrompt();

        }


    }

    private void StarDilogue()
    {
        isDialogueActive = true;
        currentiLineIndex = 0;
        ShowNextLineOrClose();
    }

    private void ShowNextLineClose()
    {
        if(currentLineIndex >= lines.Length)
        {
            CompleteDialogue();
            ResetDialogue();
            return;
        }
        DialogueLine currentLine = lines[currentLineIndex];

        dialogueUI.ShowDialogue(currentLine.speakerName, currentLine.text);
        currentLineIndex++;
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
