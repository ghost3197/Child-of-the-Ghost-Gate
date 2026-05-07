using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    [Header("Debug")]
    [SerializeField] private bool allowManualRespawn = true;
    [SerializeField] private KeyCode manualRespawnKey = KeyCode.R;

    private Vector3 checkpointPosition;
    private GameObject player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (TryResolvePlayer())
        {
            checkpointPosition = player.transform.position;
        }
    }

    private void Update()
    {
        if (allowManualRespawn && Input.GetKeyDown(manualRespawnKey))
        {
            RespawnPlayer();
        }
    }

    public void SetCheckpoint(Vector3 position)
    {
        checkpointPosition = position;
        Debug.Log($"[CheckpointManager] Saved checkpoint at {position}.", this);
    }

    public void RespawnPlayer()
    {
        if (!TryResolvePlayer())
        {
            Debug.LogWarning("[CheckpointManager] Player with tag 'Player' was not found.", this);
            return;
        }

        Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();

        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector2.zero;
            playerRigidbody.angularVelocity = 0f;
        }

        player.transform.position = checkpointPosition;
    }

    private bool TryResolvePlayer()
    {
        if (player != null)
        {
            return true;
        }

        player = GameObject.FindGameObjectWithTag("Player");
        return player != null;
    }
}
