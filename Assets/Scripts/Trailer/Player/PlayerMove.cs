using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 4f;

    [Header("원근감 설정 - 길의 실제 Y좌표로 맞출 것")]
    public float minY = -3.19f;
    public float maxY = -2.36f;
    public float closeScale = 1.2f;
    public float farScale = 0.5f;

    [Header("소팅 설정")]
    public float sortingOffset = 10f;
    public int sortingBase = 100;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("[PlayerMove] SpriteRenderer가 필요합니다.", this);
            enabled = false;
            return;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    void Update()
    {
        Move();
        ApplyPerspective();
        FlipSprite();
        ApplySortingOrder();
    }

    void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(moveX, moveY).normalized;

        Vector3 pos = transform.position;
        pos.x += direction.x * moveSpeed * Time.deltaTime;
        pos.y += direction.y * moveSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;

        //Debug.Log("현재 Y: " + transform.position.y + " / maxY: " + maxY);
    }

    void ApplyPerspective()
    {
        float t = Mathf.InverseLerp(minY, maxY, transform.position.y);
        float scale = Mathf.Lerp(closeScale, farScale, t);
        transform.localScale = new Vector3(scale, scale, 1f);
    }

    void FlipSprite()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        if (moveX > 0) sr.flipX = false;
        else if (moveX < 0) sr.flipX = true;
    }

    void ApplySortingOrder()
    {
        sr.sortingOrder = sortingBase + (int)(-transform.position.y * sortingOffset);
    }
}
