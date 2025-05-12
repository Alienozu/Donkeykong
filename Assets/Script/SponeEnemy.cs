using UnityEngine;

public class SponeEnemy : MonoBehaviour
{
    public GameObject Player;
    public GameObject Bear;
    public GameObject Rabbit;

    Transform playerTr;
    private float playerPos;

    private static int bearIndex = 0;
    private static float[] bearSponePoints = { 20, 30, 40 };

    private static int rabbitIndex = 0;
    private static float[] rabbitSponePoints = { 40 };



    private GameObject[]  enemyList;
    private int whoSpone;

    void Start()
    {
        playerTr = Player.GetComponent<Transform>();
        playerPos = playerTr.position.x;

        enemyList = new GameObject[] { Bear, Rabbit };
    }

    // Update is called once per frame
    void Update()
    {
        //クマのスポーン
        if (bearIndex < bearSponePoints.Length && playerTr.position.x > bearSponePoints[bearIndex])
        {
            whoSpone = 0;
            EnemySpone(bearSponePoints[bearIndex]);
            bearIndex++;
        }

        //ウサギ
        if (rabbitIndex < rabbitSponePoints.Length && playerTr.position.x > rabbitSponePoints[rabbitIndex])
        {
            whoSpone = 1;
            EnemySpone(rabbitSponePoints[rabbitIndex]);
            rabbitIndex++; 
        }

    }

    void EnemySpone(float bornPos)
    {
        bornPos += 11.5f;
        Instantiate(enemyList[whoSpone], new Vector2(bornPos, -1.0f), Quaternion.identity);
    }
}
