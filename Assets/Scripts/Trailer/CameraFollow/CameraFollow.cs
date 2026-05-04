using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("카메라 설정")]
    public Transform player;
    public float smoothSpeed = 5f;

    [Header("X축 이동 범위")]
    public float minX = 0f;
    public float maxX = 100f;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("[CameraFollow] player가 비어 있습니다.", this);
            enabled = false;
            return;
        }
    }


    void LateUpdate()
    {
        if (player == null) return;

        float targetX = Mathf.Clamp(player.position.x, minX, maxX);

        Vector3 targetPos = new Vector3(
            targetX,
            transform.position.y,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            smoothSpeed * Time.deltaTime
        );
    }
}
