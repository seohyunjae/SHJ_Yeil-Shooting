using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform[] spawnPoints;
    public GameObject[] enemyPrefabs;

    public float curEnemySpawnDelay;
    public float nextEnemySpawnDelay;
    public GameObject gameoverText;
    public Text timeText; //생존시간
    public Text scoreText; //점수텍스트
    public Text LifeText; //목숨


    private float suriveTime; //생존 시간
    private bool isGameover; //게임오버 상태
    public GameObject player;
    public bool isRespawnTime; //bool 타입 변수 선언
    private bool attackSucess; //어택성공
    

    // Start is called before the first frame update
    void Start()
    {
        //생존 시간 게임오버 초기화
        suriveTime = 0;
        isGameover = false;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }        
        else
        {
            Destroy(gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        curEnemySpawnDelay += Time.deltaTime;
        if (curEnemySpawnDelay > nextEnemySpawnDelay)
        {
            SpawnEnemy();

            nextEnemySpawnDelay = Random.Range(0.5f, 3.0f);
            curEnemySpawnDelay = 0;


        }
        if (!isGameover)
        {
            suriveTime += Time.deltaTime;
            timeText.text = "SURVIVAL TIME:" + (int)suriveTime;

            Player playerscore = player.GetComponent<Player>();


            scoreText.text = "SCORE:" + (int)playerscore.score;

            Player lifeText = player.GetComponent<Player>();

            LifeText.text = "LIFE:" + (int)lifeText.life;
        }

       
      if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("SampleScene");
            }
        
            
       
        

    }

    void SpawnEnemy()
    {

        int ranType = Random.Range(0, enemyPrefabs.Length);
        int ranPoint = Random.Range(0, spawnPoints.Length);
        GameObject goEnemy = Instantiate(enemyPrefabs[ranType], spawnPoints[ranPoint].position, Quaternion.identity);
        Enemy enemyLogic = goEnemy.GetComponent<Enemy>();
        enemyLogic.playerObject = player;
        enemyLogic.Move(ranPoint);
    }

    public void GameOver()
    {
        
    }

    public void RespawnPlayer()
    {
        Invoke("AlivePlayer", 1.0f);
        
        
    }

    void AlivePlayer()
    {
        player.transform.position = Vector3.down * 4.2f;
        player.SetActive(true);
        player.GetComponent<PolygonCollider2D>().enabled = false;
        
        Player playrLogic = player.GetComponent<Player>();
        playrLogic.isHit = false;
        Invoke(nameof(Delayplayer), 2f); 
        
        


    }


    void Delayplayer()
    {
        player.GetComponent<PolygonCollider2D>().enabled = true;
    }
   
}
