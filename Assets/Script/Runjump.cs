using System;
using System.Data;
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
    private bool isRolling = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

        if (Input.GetKeyDown(KeyCode.Z) && isGrounded && horizontal != 0)
        {
            rollTimer = rollDuration;
            rollDirection = new Vector2(Mathf.Sign(horizontal), 0);
            rb.linearVelocity = rollDirection * rollSpeed;
            Debug.Log("Z.Check");
        }

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

}
