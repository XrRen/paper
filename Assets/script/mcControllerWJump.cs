using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("移动参数")]
    public float maxSpeed = 5f;
    public float decelRate = 10f;

    [Header("死区校准")]
    [Range(0f, 1f)] public float calibration = 0f;
    [Range(0f, 1f)] public float maxDeadzone = 0.6f;

    [Header("跳跃参数")]
    public float jumpForce = 12f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    Rigidbody2D rb;
    float currentVelocity = 0f;
    bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        // —— 1. 读输入并应用死区 —— 
        float input = 0f;
        if (Input.GetKey(KeyCode.A)) input -= 1f;
        if (Input.GetKey(KeyCode.D)) input += 1f;
        float deadzone = calibration * maxDeadzone;
        if (Mathf.Abs(input) < deadzone) input = 0f;

        // —— 2. 计算并平滑水平速度 —— 
        float targetVel = input * maxSpeed;
        currentVelocity = Mathf.MoveTowards(
            currentVelocity,
            targetVel,
            decelRate * Time.fixedDeltaTime
        );

        // —— 3. 把速度给刚体 —— 
        rb.linearVelocity = new Vector2(currentVelocity, rb.linearVelocity.y);
    }

    void Update()
    {
        // …前面死区、水平移动代码…

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }


    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(
                groundCheck.position,
                groundCheckRadius
            );
        }
    }
}
