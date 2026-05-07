using UnityEngine;
using UnityEngine.Events;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private UnityEvent onPlayerEntered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        onPlayerEntered?.Invoke();
        CheckpointManager.Instance?.RespawnPlayer();
    }
}
