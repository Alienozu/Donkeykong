using System.Collections;
using UnityEngine;

public class EnemiesLife : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    private bool die;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitByRolling()
    {
        Debug.Log("’Ê’m‚ðŽæ“¾");
        DieEnemy();
    }

    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ƒvƒŒƒCƒ„‚É“¥‚Ü‚ê‚½Žž
        if (collision.gameObject.tag == "Player")
        {
            GameObject pla = collision.gameObject;
            Rigidbody2D collRigidbody = pla.GetComponent<Rigidbody2D>();

            if (collRigidbody.velocity.y <= 0)
            {
                DieEnemy();
            }

        }
    }



    private void DieEnemy()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        rb.bodyType = RigidbodyType2D.Kinematic;

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
