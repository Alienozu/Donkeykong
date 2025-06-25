using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StageSystem : MonoBehaviour
{

    public GameObject player;
    private Transform playerTransform;

    private float timer;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI clearText;

    private RectTransform textTransform;

    void Start()
    {
        playerTransform = player.GetComponent<Transform>();

        timer = 300;

        textTransform = clearText.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {           
            
        }
        timerText.text = timer.ToString("F0");

        if(playerTransform.position.x > 30)
        {       
            //右から左へクリアテキストを移動させる
            Vector2 clearTextPosition = textTransform.position;

            clearText.text = "Congratulations!!";
            clearTextPosition.x -= 1f;
            textTransform.position = clearTextPosition;
            
        }
    }
}
