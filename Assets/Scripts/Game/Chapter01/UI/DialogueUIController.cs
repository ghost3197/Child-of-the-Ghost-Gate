using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUIController : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialogueRoot;
    [SerializeField] private Text speakerNameText;
    [SerializeField] private Text dialogueBodyText;
    [SerializeField] private GameObject nextIndicator;

    [Header("Typewriter")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Movement Lock")]
    [SerializeField] private Chapter01PlayerMove playerMovement;
    [SerializeField] private bool lockPlayerMovementWhileOpen = true;

    private Coroutine typingCoroutine;
    private string currentFullLine = string.Empty;

    public bool IsOpen => dialogueRoot != null && dialogueRoot.activeSelf;
    public bool IsTyping { get; private set; }

    private void Awake()
    {
        if (playerMovement == null)
        {
            playerMovement = FindAnyObjectByType<Chapter01PlayerMove>();
        }

        Hide();
    }

    public void ShowLine(string speakerName, string line)
    {
        if (dialogueRoot == null)
        {
            Debug.LogError("[DialogueUIController] Dialogue root is not assigned.", this);
            return;
        }

        dialogueRoot.SetActive(true);
        SetPlayerMovementLocked(true);

        if (speakerNameText != null)
        {
            speakerNameText.text = speakerName;
        }

        currentFullLine = line ?? string.Empty;
        StopTypingCoroutine();
        typingCoroutine = StartCoroutine(TypeLine(currentFullLine));
    }

    public void CompleteTyping()
    {
        if (!IsTyping)
        {
            return;
        }

        StopTypingCoroutine();

        if (dialogueBodyText != null)
        {
            dialogueBodyText.text = currentFullLine;
        }

        IsTyping = false;
        SetNextIndicatorVisible(true);
    }

    public void Hide()
    {
        StopTypingCoroutine();
        currentFullLine = string.Empty;
        IsTyping = false;

        if (dialogueBodyText != null)
        {
            dialogueBodyText.text = string.Empty;
        }

        SetNextIndicatorVisible(false);

        if (dialogueRoot != null)
        {
            dialogueRoot.SetActive(false);
        }

        SetPlayerMovementLocked(false);
    }

    private IEnumerator TypeLine(string line)
    {
        IsTyping = true;
        SetNextIndicatorVisible(false);

        if (dialogueBodyText != null)
        {
            dialogueBodyText.text = string.Empty;

            if (typingSpeed <= 0f)
            {
                dialogueBodyText.text = line;
                IsTyping = false;
                typingCoroutine = null;
                SetNextIndicatorVisible(true);
                yield break;
            }

            foreach (char character in line)
            {
                dialogueBodyText.text += character;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        else if (typingSpeed > 0f)
        {
            yield return new WaitForSeconds(typingSpeed * line.Length);
        }

        IsTyping = false;
        typingCoroutine = null;
        SetNextIndicatorVisible(true);
    }

    private void StopTypingCoroutine()
    {
        if (typingCoroutine == null)
        {
            return;
        }

        StopCoroutine(typingCoroutine);
        typingCoroutine = null;
    }

    private void SetNextIndicatorVisible(bool visible)
    {
        if (nextIndicator != null)
        {
            nextIndicator.SetActive(visible);
        }
    }

    private void SetPlayerMovementLocked(bool locked)
    {
        if (!lockPlayerMovementWhileOpen)
        {
            return;
        }

        if (playerMovement == null)
        {
            playerMovement = FindAnyObjectByType<Chapter01PlayerMove>();
        }

        if (playerMovement != null)
        {
            playerMovement.SetMovementLocked(locked);
        }
    }
}
