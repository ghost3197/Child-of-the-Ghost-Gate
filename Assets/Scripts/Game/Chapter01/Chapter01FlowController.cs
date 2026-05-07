using UnityEngine;

public class Chapter01FlowController : MonoBehaviour
{
    public enum Chapter01State
    {
        Intro,
        TalkToFather,
        InspectCharm,
        OpenDoor,
        GoOutside,
        Completed,
    }

    [Header("State")]
    [SerializeField] private Chapter01State initialState = Chapter01State.TalkToFather;
    [SerializeField] private Chapter01State currentState = Chapter01State.Intro;

    [Header("Dialogues")]
    [SerializeField] private DialogueTrigger fatherDialogue;
    [SerializeField] private DialogueTrigger motherDialogue;

    [Header("Objects")]
    [SerializeField] private GameObject charmObject;
    [SerializeField] private GameObject doorObject;
    [SerializeField] private GameObject outsideTrigger;

    private void Start()
    {
        SetState(initialState);
    }

    private void SetState(Chapter01State nextState)
    {
        currentState = nextState;
        ApplyState();
    }

    private void ApplyState()
    {
        SetObjectActive(charmObject, currentState == Chapter01State.InspectCharm);
        SetObjectActive(doorObject, currentState == Chapter01State.OpenDoor);
        SetObjectActive(outsideTrigger, currentState == Chapter01State.GoOutside);

        SetDialogueEnabled(fatherDialogue, currentState == Chapter01State.TalkToFather);
        SetDialogueEnabled(motherDialogue, false);

        if (currentState == Chapter01State.Intro)
        {
            SetDialogueEnabled(fatherDialogue, true);
        }
    }

    private void SetObjectActive(GameObject target, bool active)
    {
        if (target != null)
            target.SetActive(active);
    }

    private void SetDialogueEnabled(DialogueTrigger dialogue, bool enabled)
    {
        if (dialogue != null)
            dialogue.SetInteractableEnabled(enabled);
    }

    public void UnlockMotherDialogue()
    {
        SetDialogueEnabled(motherDialogue, true);
    }

    public void OnFatherDialogueCompleted()
    {
        if (currentState != Chapter01State.TalkToFather) return;
        SetState(Chapter01State.InspectCharm);
    }

    public void OnCharmInspected()
    {
        if (currentState != Chapter01State.InspectCharm) return;
        SetState(Chapter01State.OpenDoor);
    }

    public void OnDoorOpened()
    {
        if (currentState != Chapter01State.OpenDoor) return;
        SetState(Chapter01State.GoOutside);
    }

    public void OnGoOutside()
    {
        if (currentState != Chapter01State.GoOutside) return;
        SetState(Chapter01State.Completed);
    }
}
