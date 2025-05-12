using UnityEngine;
using System.Collections;

public class NewMonoBehaviourScript : MonoBehaviour
{
    //�n��G�̃X�N���v�g
    public bool isVisible;
    public GameObject DeathEffect;
    public GameObject Bear;

    private bool facedLeft = true;   // �����ϊ�
    private bool die = false;        // ���S�`�F�b�N
    private bool isSlope;�@�@�@�@�@�@// �⓹�`�F�b�N

    private Animator animator;
    private SpriteRenderer sr;

    private float slopeAngle;

    //�v���C��
    private GameObject Player;
    private Transform playerPosition;
    private Vector2 playerPos;
    private float playerPosX;


    private Transform enemyTransform;

    private Vector2 pos;
    private Vector2 rayerEdge;

    private Vector2 normal;  //�@���x�N�g��
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
        //�������̈ړ�
        if (isVisible)
        {
            Vector2 now = rb.position;

            float direction = facedLeft ? -1 : 1;

            if (!isSlope)
            {
                now += new Vector2(0.02f * direction, 0);
                rb.position = now;
            }
            else if(isSlope)�@
            { 
                now += new Vector2(0.02f * direction, 0.00489f * slopeCheck);
                rb.position = now;
            }
        }

        currentPosition = gameObject.transform.position;
        speed = (currentPosition - lastPosition) / Time.deltaTime;

        //���S��
        if (die)
        {
            _time += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        //�R�ۂɗ������ɐ܂�Ԃ�
        if(isVisible && !die)
        {
            Physics2D.SyncTransforms(); // �R���C�_�[�̍X�V������
            rayerEdge = facedLeft ? bearCollider.bounds.min : new Vector2(bearCollider.bounds.max.x, bearCollider.bounds.min.y);

            LayerMask groundLayer = LayerMask.GetMask("Ground", "Slope");

            RaycastHit2D hit = Physics2D.Raycast(rayerEdge, Vector2.down, 1, groundLayer);
            Debug.DrawRay(rayerEdge, Vector2.down * 1, Color.red, 0.1f);



            if (hit.collider == null)
            {
                facedLeft = !facedLeft; //�t�ɂ���
                ChangeScale();
            }

            if (hit.collider != null && hit.collider.gameObject.layer == 7 )
            {
                isSlope = true;

                normal = hit.normal;
                slopeAngle = Vector2.Angle(normal, Vector2.up);
                if (normal.x < 0) //����
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
            //���]���ɃX�P�[���������ɔ��]
            Vector2 sca = enemyTransform.localScale;
            sca.x = -sca.x;
            enemyTransform.localScale = sca;
        }
    }

    //�J�������ɓ�������I�u�W�F�N�g���N��
    private void OnBecameVisible()
    {
        isVisible = true;
        
        GetComponent<Animator>().enabled = true;
    }
    //�J�����̊O�ł̓I�u�W�F�N�g���폜����
    private void OnBecameInvisible()
    {
        isVisible = false;
    }

    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D collision)
    {    
        //�v���C���ɓ��܂ꂽ��
        if(collision.gameObject.tag == "Player")
        {
            GameObject pla = collision.gameObject;
            BoxCollider2D collisionCollider = pla.GetComponent<BoxCollider2D>();
            Transform collisionTransform = pla.GetComponent<Transform>();
            Rigidbody2D collRigidbody = pla.GetComponent<Rigidbody2D>();
            
           if(collRigidbody.velocity.y�@< 0)
            {
                DieEnemy();
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�G���m�̐ڐG���ɂ����锽�]
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
     *             Debug.Log("����");
     *             Debug.LogError("null");
    */
}
