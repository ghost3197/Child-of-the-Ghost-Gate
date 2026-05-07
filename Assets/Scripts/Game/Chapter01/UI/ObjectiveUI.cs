using UnityEngine;
using UnityEngine.UI;

public class ObjectiveUI : MonoBehaviour
{
    public static ObjectiveUI Instance { get; private set; }

    [SerializeField] private Text objectiveText;
    [SerializeField] private string prefix = "\uD604\uC7AC \uBAA9\uD45C: ";
    [SerializeField] private bool hideWhenObjectiveIsEmpty;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetObjective(string objective)
    {
        bool hasObjective = !string.IsNullOrWhiteSpace(objective);

        if (hideWhenObjectiveIsEmpty)
        {
            gameObject.SetActive(hasObjective);
        }

        if (objectiveText != null)
        {
            objectiveText.text = hasObjective ? prefix + objective : string.Empty;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
