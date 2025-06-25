// ドンキーコング風プレイヤー制御スクリプト with 既存Runjump機能統合

using UnityEngine;
using System.Collections;

public class DonkeyPlayerController : MonoBehaviour
{
    // 移動・ジャンプ
    public float acceleration = 20f;
    public float deceleration = 25f;
    public float maxSpeed = 4f;
    public float jumpForce = 7f;
    public float gravityScale = 3f;
    public float fallMultiplier = 2.5f;

    private Rigidbody2D rb;
    private float moveInput;
    private float currentSpeed = 0f;
    private bool isGrounded = false;

    // Ground 判定用
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    // スプライト制御
    private SpriteRenderer spriteRenderer;

    // ローリング
    public float rollSpeed = 10f;
    public float rollDuration = 1f;
    private float rollTimer = 0f;
    private bool isRolling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.gravityScale = gravityScale;
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        // 加速・減速処理
        if (moveInput != 0)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, moveInput * maxSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
        }

        // 空中での慣性調整
        if (!isGrounded)
        {
            currentSpeed *= 0.99f;
        }

        transform.Translate(Vector2.right * currentSpeed * Time.deltaTime);

        // ジャンプ入力
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }

        // 落下加速
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        // ローリング処理
        if (Input.GetKeyDown(KeyCode.Z) && isGrounded && moveInput != 0)
        {
            rollTimer = rollDuration;
            isRolling = true;
            rb.linearVelocity = new Vector2(Mathf.Sign(moveInput) * rollSpeed, rb.linearVelocity.y);
        }

        if (isRolling)
        {
            rollTimer -= Time.deltaTime;
            if (rollTimer <= 0)
            {
                isRolling = false;
            }
        }

        // スプライト反転
        if (moveInput != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveInput);
            transform.localScale = scale;
        }
    }

    void FixedUpdate()
    {
        // 地面接地チェック
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemiesLife enemy = collision.gameObject.GetComponent<EnemiesLife>();
            PlayerLife life = GetComponent<PlayerLife>();

            bool isStepping = rb.linearVelocity.y <= 0 && transform.position.y > collision.transform.position.y + 0.2f;

            if (isStepping && enemy != null && !enemy.IsDead())
            {
                enemy.HitByRolling();
                isGrounded = false;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 0.7f);
                return;
            }

            if (isRolling && enemy != null && !enemy.IsDead())
            {
                enemy.HitByRolling();
                return;
            }

            if (life != null)
            {
                life.TakeDamage();
            }
        }
    }
}
