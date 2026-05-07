using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Game/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [System.Serializable]
    public class DialogueLine
    {
        public string speakerName;

        [TextArea(2, 5)]
        public string text;
    }

    public DialogueLine[] lines;
}
