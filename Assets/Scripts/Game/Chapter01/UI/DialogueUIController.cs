using UnityEngine;
using UnityEngine.UI;

public class DialogueUIController : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialogueRoot;
    [SerializeField] private Text speakerNameText;
    [SerializeField] private Text dialogueBodyText;

    [Header("Movement Lock")]
    [SerializeField] private Chapter01PlayerMove playerMovement;
    [SerializeField] private bool lockPlayerMovementWhileOpen = true;

    public bool IsOpen => dialogueRoot != null && dialogueRoot.activeSelf;

    private void Awake()
    {
        if (playerMovement == null)
        {
            playerMovement = FindFirstObjectByType<Chapter01PlayerMove>();
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

        if (dialogueBodyText != null)
        {
            dialogueBodyText.text = line;
        }
    }

    public void Hide()
    {
        if (dialogueRoot != null)
        {
            dialogueRoot.SetActive(false);
        }

        SetPlayerMovementLocked(false);
    }

    private void SetPlayerMovementLocked(bool locked)
    {
        if (!lockPlayerMovementWhileOpen)
        {
            return;
        }

        if (playerMovement == null)
        {
            playerMovement = FindFirstObjectByType<Chapter01PlayerMove>();
        }

        if (playerMovement != null)
        {
            playerMovement.SetMovementLocked(locked);
        }
    }
}
