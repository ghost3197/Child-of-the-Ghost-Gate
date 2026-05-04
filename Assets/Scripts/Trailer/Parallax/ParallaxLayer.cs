using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Header("패럴렉스 설정")]
    public float parallaxSpeed = 0.5f;
    // 0 = 안움직임 (아주 멀리)
    // 1 = 카메라랑 같이 움직임 (아주 가까이)

    private Transform cam;
    private float startPosX;
    private float startCamX;

    void Start()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("[ParallaxLayer] MainCamera 태그가 붙은 카메라를 찾을 수 없습니다.", this);
            enabled = false;
            return;
        }

        cam = mainCamera.transform;
        startPosX = transform.position.x;
        startCamX = cam.position.x;
    }


    void LateUpdate()
    {
        float camDisplacement = cam.position.x - startCamX;
        float newX = startPosX + camDisplacement * parallaxSpeed;

        transform.position = new Vector3(
            newX,
            transform.position.y,
            transform.position.z
        );
    }
}