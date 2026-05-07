using UnityEngine;
using UnityEngine.Events;

public class CheckpointTrigger : MonoBehaviour
{
    [SerializeField] private Transform checkpointOverride;
    [SerializeField] private bool triggerOnce = true;
    [SerializeField] private UnityEvent onCheckpointReached;

    private bool hasTriggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (triggerOnce && hasTriggered)
        {
            return;
        }

        Vector3 checkpointPosition = checkpointOverride != null ? checkpointOverride.position : transform.position;
        CheckpointManager.Instance?.SetCheckpoint(checkpointPosition);
        hasTriggered = true;
        onCheckpointReached?.Invoke();
    }
}
