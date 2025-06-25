using UnityEngine;
using System.Collections;

public class GroundEnemyManager : EnemyBase
{
    //地上敵のスクリプト
    private Transform enemyTransform;


    private bool die = false;        // 死亡チェック
    private bool isSlope;　　　　　　// 坂道チェック

    private SpriteRenderer sr;

    private float slopeAngle;

    private Vector2 normal;  //法線ベクトル
    private int slopeCheck;

    Vector2 speed;


    //プレイヤ
    private GameObject Player;

    private void Awake()
    {
        enemyRigidbody = gameObject.GetComponent<Rigidbody2D>();
        enemyRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        enemyCollider = gameObject.GetComponent<BoxCollider2D>();
        enemyCollider.enabled = false;
    }

    void Start()
    {
        enemyTransform = gameObject.GetComponent<Transform>();
       


        Player = GameObject.Find("Player");


        sr = gameObject.GetComponent<SpriteRenderer>();
    }



    void Update()
    {
        //横方向の移動
        if (isVisible && !die)
        {
            Vector2 now = enemyRigidbody.position;

            float direction = facedLeft ? -1 : 1;

            if (!isSlope)
            {
                now += new Vector2(0.02f * direction, 0);
                enemyRigidbody.position = now;
            }
            else if (isSlope)
            {
                now += new Vector2(0.02f * direction, 0.00489f * slopeCheck);
                enemyRigidbody.position = now;
            }
        }
    }

    private void FixedUpdate()
    {
        Physics2D.SyncTransforms(); // コライダーの更新を強制
        LayerMask groundLayer = LayerMask.GetMask("Ground", "Slope");



        //崖際に来た時に折り返す
        if (isVisible && !die )
        {
            rayerEdge = facedLeft ? enemyCollider.bounds.min : new Vector2(enemyCollider.bounds.max.x, enemyCollider.bounds.min.y);


            RaycastHit2D hit = Physics2D.Raycast(rayerEdge, Vector2.down, 1, groundLayer);
            Debug.DrawRay(rayerEdge, Vector2.down * 1, Color.red, 0.1f);
            if (hit.collider == null)
            {
                facedLeft = !facedLeft; //逆にする
                ChangeScale();
            }

            if (hit.collider != null && hit.collider.gameObject.layer == 7)
            {
                isSlope = true;

                normal = hit.normal;
                slopeAngle = Vector2.Angle(normal, Vector2.up);
                if (normal.x < 0) //上り坂
                {
                    slopeCheck = -3;
                }
                else if (normal.x > 0)
                {
                    if (speed.x > 0)
                    {
                        slopeCheck = -3;
                    }
                }
            }

            if (hit.collider != null && hit.collider.gameObject.layer == 6)
            {
                isSlope = false;
                slopeCheck = 1;
            }
        }
    }

    void ChangeScale()
    {
        if (!die)
        {
            //反転時にスケールも同時に反転
            Vector2 sca = enemyTransform.localScale;
            sca.x = -sca.x;
            enemyTransform.localScale = sca;
        }
    }

    //カメラ内に入ったらオブジェクトを起動
    private void OnBecameVisible()
    {
        isVisible = true;
        enemyCollider.enabled = true;
    }
    //カメラの外ではオブジェクトを削除する
    private void OnBecameInvisible()
    {
        isVisible = false;
    }


    [System.Obsolete]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //敵同士の接触時における反転
        if (collision.gameObject.tag == "Enemy")
        {
            Rigidbody2D collisionRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 colliVelocity = collisionRigidbody.linearVelocity;

            if (colliVelocity.x * speed.x <= 0)
            {
                facedLeft = !facedLeft;
                ChangeScale();
            }
        }
    }


    void DieEnemy()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        enemyRigidbody.bodyType = RigidbodyType2D.Kinematic;

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 70 / 255f);
        die = true;

        StartCoroutine(DeadMotion(0.3f));
    }

    private IEnumerator DeadMotion(float deleyTime)
    {
        yield return new WaitForSeconds(deleyTime);
        Destroy(gameObject);
    }

    /*
     *             Debug.Log("成功");
     *             Debug.LogError("null");
    */
}
