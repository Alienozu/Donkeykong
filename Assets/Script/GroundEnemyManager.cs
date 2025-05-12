using UnityEngine;
using System.Collections;

public class NewMonoBehaviourScript : MonoBehaviour
{
    //地上敵のスクリプト
    public bool isVisible;
    public GameObject DeathEffect;
    public GameObject Bear;

    private bool facedLeft = true;   // 方向変換
    private bool die = false;        // 死亡チェック
    private bool isSlope;　　　　　　// 坂道チェック

    private Animator animator;
    private SpriteRenderer sr;

    private float slopeAngle;

    //プレイヤ
    private GameObject Player;
    private Transform playerPosition;
    private Vector2 playerPos;
    private float playerPosX;


    private Transform enemyTransform;

    private Vector2 pos;
    private Vector2 rayerEdge;

    private Vector2 normal;  //法線ベクトル
    private int slopeCheck;

    private CapsuleCollider2D bearCollider;
    private Rigidbody2D rb;

    private Vector2 lastPosition;
    private Vector2 currentPosition;
    Vector2 speed;

    private double _time;

    void Start()
    {
        enemyTransform = gameObject.GetComponent<Transform>();
        animator = GetComponent<Animator>();
        animator.SetBool("isRunning", false);
        GetComponent<Animator>().enabled = false;
        
        bearCollider = gameObject.GetComponent<CapsuleCollider2D>();

        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        Player = GameObject.Find("Playwer");
        playerPosition = Player.GetComponent<Transform>();

        lastPosition = gameObject.transform.position;

        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //横方向の移動
        if (isVisible)
        {
            Vector2 now = rb.position;

            float direction = facedLeft ? -1 : 1;

            if (!isSlope)
            {
                now += new Vector2(0.02f * direction, 0);
                rb.position = now;
            }
            else if(isSlope)　
            { 
                now += new Vector2(0.02f * direction, 0.00489f * slopeCheck);
                rb.position = now;
            }
        }

        currentPosition = gameObject.transform.position;
        speed = (currentPosition - lastPosition) / Time.deltaTime;

        //死亡時
        if (die)
        {
            _time += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        //崖際に来た時に折り返す
        if(isVisible && !die)
        {
            Physics2D.SyncTransforms(); // コライダーの更新を強制
            rayerEdge = facedLeft ? bearCollider.bounds.min : new Vector2(bearCollider.bounds.max.x, bearCollider.bounds.min.y);

            LayerMask groundLayer = LayerMask.GetMask("Ground", "Slope");

            RaycastHit2D hit = Physics2D.Raycast(rayerEdge, Vector2.down, 1, groundLayer);
            Debug.DrawRay(rayerEdge, Vector2.down * 1, Color.red, 0.1f);



            if (hit.collider == null)
            {
                facedLeft = !facedLeft; //逆にする
                ChangeScale();
            }

            if (hit.collider != null && hit.collider.gameObject.layer == 7 )
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


        if (Input.GetKeyDown(KeyCode.Space))
        {
            die = true;
            DieEnemy();
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
        
        GetComponent<Animator>().enabled = true;
    }
    //カメラの外ではオブジェクトを削除する
    private void OnBecameInvisible()
    {
        isVisible = false;
    }

    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D collision)
    {    
        //プレイヤに踏まれた時
        if(collision.gameObject.tag == "Player")
        {
            GameObject pla = collision.gameObject;
            BoxCollider2D collisionCollider = pla.GetComponent<BoxCollider2D>();
            Transform collisionTransform = pla.GetComponent<Transform>();
            Rigidbody2D collRigidbody = pla.GetComponent<Rigidbody2D>();
            
           if(collRigidbody.velocity.y　< 0)
            {
                DieEnemy();
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //敵同士の接触時における反転
        if(collision.gameObject.tag == "Enemy")
        {
            Rigidbody2D collisionRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 colliVelocity = collisionRigidbody.linearVelocity;

            if (colliVelocity.x * speed.x <= 0)
            {
                facedLeft = ! facedLeft;
                ChangeScale();
            }
        }
    }

    void DieEnemy()
    {
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        rb.bodyType = RigidbodyType2D.Kinematic;

        die = true;

        StartCoroutine(DeadMotion(1f));
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
