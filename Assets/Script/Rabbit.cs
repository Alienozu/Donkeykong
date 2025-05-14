using UnityEngine;

public class Rabbit : MonoBehaviour
{
    private bool isVisible = false;

    private bool facedLeft = true;   // •ûŒü•ÏŠ·

    private Transform trRabbit;
    private Rigidbody2D rbRabbit;
    private BoxCollider2D colliRabbit;

    private Vector2 rayerEdge;


    //ƒvƒŒƒCƒ„
    private GameObject Player;
    private Transform playerPosition;
    private Vector2 playerPos;
    private float playerPosX;

    void Start()
    {
        trRabbit = gameObject.GetComponent<Transform>();
        rbRabbit = gameObject.GetComponent<Rigidbody2D>();
        colliRabbit = gameObject.GetComponent<BoxCollider2D>();


        Player = GameObject.Find("Playwer");
        playerPosition = Player.GetComponent<Transform>();
    }


    void Update()
    {
        if (isVisible)
        {

        }
    }

    private void OnBecameVisible()
    {
        if (isVisible)
        {

        }
        isVisible = true;
    }

}
