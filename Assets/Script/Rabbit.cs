using UnityEngine;

public class Rabbit : EnemyBase
{
    private Transform tr;


    private bool isGround;
    private int jumpCount;
    private float coolTime;

    //プレイヤ
    private GameObject Player;
    private Transform playerPosition;
    private Vector2 playerPos;
   



    /*ウサギの設定
     * 方向転換や地面の認識は行わない。
     * プレイヤーがいる方向に対して向かうようにする予定
     */

    private void Awake()
    {
        tr = gameObject.GetComponent<Transform>();
        enemyRigidbody = gameObject.GetComponent<Rigidbody2D>();
        enemyCollider = gameObject.GetComponent<BoxCollider2D>();

        isVisible = false;
    }
    void Start()
    {
        Player = GameObject.Find("Player");
        playerPosition = Player.GetComponent<Transform>();
        playerPos = playerPosition.position;

        isGround = true;
        jumpCount = 0;
    }


    void Update()
    {                 
        if (isVisible)
        {
            Vector2 sca = tr.localScale;

            int direction = facedLeft ? -1 : 1;
            //基本的な挙動
            if (isGround)
            {

                coolTime -= Time.deltaTime;

                if (coolTime <= 0 && jumpCount == 1)
                {
                    Vector2 jump = new Vector2(direction * 5f, 5f);
                    enemyRigidbody.AddForce(jump, ForceMode2D.Impulse);

                    jumpCount--;
                }
            }
            

            //プレイヤーに合わせ方向転換
            if (playerPosition.position.x > tr.position.x)
            {
                sca.x = Mathf.Abs(sca.x);
                tr.localScale = sca;
                facedLeft = false;
            }
            else if (playerPosition.position.x < tr.position.x)
            {
                sca.x = -Mathf.Abs(sca.x);
                tr.localScale = sca;
                facedLeft = true;
            }
        }
    }

    private void OnBecameVisible()
    {
        isVisible = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            jumpCount++;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;
            coolTime = 1.5f;
        }

    }
}
