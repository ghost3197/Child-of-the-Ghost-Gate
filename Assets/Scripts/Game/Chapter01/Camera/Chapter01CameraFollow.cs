using UnityEngine;

public class Chapter01CameraFollow : MonoBehaviour
{
    [Header("Follow Target")]
    [SerializeField] private Transform target;

    [Header("Follow Tuning")]
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 1f, -10f);

    private float initialY;
    private float initialZ;

    private void Awake()
    {
        initialY = transform.position.y;
        initialZ = transform.position.z;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 desiredPosition = new Vector3(
            target.position.x + offset.x,
            initialY,
            initialZ
        );

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );
    }
}
