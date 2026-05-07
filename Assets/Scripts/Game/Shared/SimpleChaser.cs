using UnityEngine;

public class SimpleChaser : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private float catchDistance = 0.5f;
    [SerializeField] private bool chaseOnXAxisOnly;

    private Transform player;

    private void Start()
    {
        TryResolvePlayer();
    }

    private void Update()
    {
        if (!TryResolvePlayer())
        {
            return;
        }

        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = player.position;

        if (chaseOnXAxisOnly)
        {
            targetPosition.y = currentPosition.y;
        }

        float distance = Vector2.Distance(currentPosition, targetPosition);

        if (distance > detectionRange)
        {
            return;
        }

        if (distance <= catchDistance)
        {
            CheckpointManager.Instance?.RespawnPlayer();
            return;
        }

        Vector2 nextPosition = Vector2.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);
        transform.position = new Vector3(nextPosition.x, nextPosition.y, transform.position.z);
    }

    private bool TryResolvePlayer()
    {
        if (player != null)
        {
            return true;
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject == null)
        {
            return false;
        }

        player = playerObject.transform;
        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, catchDistance);
    }
}
