using System.Collections;
using UnityEngine;


public class EnemyBase : MonoBehaviour
{
    protected Rigidbody2D enemyRigidbody;
    protected BoxCollider2D enemyCollider;

    protected bool isVisible;
    protected Vector2 rayerEdge;

    protected bool facedLeft = true;

    private void Awake()
    {
        enemyRigidbody = gameObject.GetComponent<Rigidbody2D>();
        enemyCollider = gameObject.GetComponent<BoxCollider2D>();
        isVisible = false;
    }

    private void FixedUpdate()
    {
        if (!isVisible)
        {
            Physics2D.SyncTransforms(); // コライダーの更新を強制
            LayerMask groundLayer = LayerMask.GetMask("Ground", "Slope");

            //スポーン位置の調整
            rayerEdge = enemyCollider.bounds.min;
            RaycastHit2D rayDown = Physics2D.Raycast(rayerEdge, Vector2.down, 100, groundLayer);
            RaycastHit2D rayUp = Physics2D.Raycast(rayerEdge, Vector2.up, 100, groundLayer);

            Transform sponeTransform = gameObject.transform;
            Vector2 sponePos = sponeTransform.position;

            if (rayDown.collider != null)
            {
                sponePos.y = rayDown.point.y + enemyCollider.bounds.extents.y+4f;
                sponeTransform.position = sponePos;
            }
            if (rayUp.collider != null)
            {
                sponePos.y = rayUp.point.y;
                sponeTransform.position = sponePos;

            }
            if (rayDown.collider != null && rayUp.collider != null)
            {
                Debug.LogError("複数のスポーンポイントが見つかりました。手動で設置してください");
            }
        }

    }
}
public class EnemiesLife : EnemyBase
{
    SpriteRenderer sr;
    private bool die;

    private EdgeCollider2D stampedCollider;

    void Start()
    {
        enemyRigidbody = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        stampedCollider = gameObject.GetComponent<EdgeCollider2D>();
    }

    void Update()
    {
        
    }

    public void HitByRolling()
    {
        Debug.Log("通知を取得");
        DieEnemy();
    }

    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //プレイヤに踏まれた時
        if (collision.gameObject.tag == "Player")
        {
            GameObject pla = collision.gameObject;
            Rigidbody2D collRigidbody = pla.GetComponent<Rigidbody2D>();

            if (collRigidbody.velocity.y < 0)
            {
                DieEnemy();
            }
        }
    }



    private void DieEnemy()
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
}
