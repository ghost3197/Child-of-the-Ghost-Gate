using UnityEngine;
using UnityEngine.UI;

public class DialogueUIController : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialogueRoot;
    [SerializeField] private Text speakerNameText;
    [SerializeField] private Text dialogueBodyText;

    public bool IsOpen => dialogueRoot != null && dialogueRoot.activeSelf;

    private void Awake()
    {
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
    }
}
