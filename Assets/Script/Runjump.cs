using System;
using System.Data;
using System.Diagnostics;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;



public class Runjump : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 5f;
    public float rollSpeed = 10f;
    public float rollDuration = 1f;
    private float rollTimer = 0f;
    private Vector2 rollDirection;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    public bool isRolling;
    public GameObject handSlapEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }



    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * horizontal * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false; // ここで明示的に false にしておくと安全
        }

        if (Input.GetKey(KeyCode.Z) && isGrounded && horizontal != 0)
        {
            rollTimer = rollDuration;
            rollDirection = new Vector2(Mathf.Sign(horizontal), 0);
            rb.linearVelocity = rollDirection * rollSpeed;
            UnityEngine.Debug.Log("Z.Check");
            isRolling = true;
        }

        bool isCrouching = Input.GetKey(KeyCode.DownArrow);
        if (Input.GetKeyDown(KeyCode.X) && isCrouching && isGrounded)
        {
            DoHandSlap();
        }

    }

    

    void DoHandSlap()
    {
        // エフェクトの位置（キャラの下）
        Vector2 effectPos = new Vector2(transform.position.x, transform.position.y - 0.6f);

        // エフェクトを生成
        Instantiate(handSlapEffect, effectPos, Quaternion.identity);
        // アニメーション再生（必要であれば）
        // animator.SetTrigger("HandSlap");

        // 地面に向かってOverlapBoxなどで当たり判定
        Vector2 center = new Vector2(transform.position.x, transform.position.y - 0.5f);
        Vector2 size = new Vector2(5.0f, 0.2f); // スラップの範囲
        LayerMask enemyLayer = LayerMask.GetMask("Enemy");

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0, enemyLayer);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Destroy(hit.gameObject); // 敵を倒す（演出を後で追加しても可）
            }
        }

        UnityEngine.Debug.Log("ハンドスラップ発動！");
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        //ローリングをエネミーに通知する
        if (isRolling && collision.gameObject.CompareTag("Enemy"))
        {
             EnemiesLife enemy = collision.gameObject.GetComponent<EnemiesLife>();
           if (enemy != null)
            {
                enemy.HitByRolling();
            }
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Vector2 center = new Vector2(transform.position.x, transform.position.y - 0.5f);
        Vector2 size = new Vector2(1.0f, 0.2f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(center, size);
    }

   

}