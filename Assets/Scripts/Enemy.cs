using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public int enemyscore;


    public GameObject cherry;
    public GameObject starryberry;
    public GameObject watermelon;

    public Sprite[] sprites;
    SpriteRenderer spriteRender;

    Rigidbody2D rd;

    public GameObject bulletPrefab;
    public float curBulletDelay = 0f;
    public float maxBulletDelay = 1f;

    public GameObject playerObject;
    
    bool isDead = false;


    void Awake()
    {
        rd = GetComponent<Rigidbody2D>();
        //rd.velocity = Vector2.down * speed; => Move 함수로 이동
        spriteRender = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
        //rd = GetComponent<Rigidbody2D>();
        ////rd.velocity = Vector2.down * speed; => Move 함수로 이동
        //spriteRender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Fire();
        ReloadBullet();
    }

    void Fire()
    {
        if (curBulletDelay > maxBulletDelay)
        {
            Power();

            curBulletDelay = 0;
        }
    }

    void Power()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rdBullet = bulletObj.GetComponent<Rigidbody2D>();
        
        Vector3 dirVec = playerObject.transform.position - transform.position;
        rdBullet.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    void ReloadBullet()
    {
        curBulletDelay += Time.deltaTime;
    }

    public void Move(int nPoint)
    {
        if (nPoint == 3 || nPoint == 4) // 오른쪽에 있는 스폰 포인트의 배열 인덳스값
        {
            transform.Rotate(Vector3.back * 90);
            rd.velocity = new Vector2(speed * (-1), -1);
        }
        else if (nPoint == 5 || nPoint == 6) // 왼쪽에 있는 스폰 포인트의 배열 인덳스값
        {
            transform.Rotate(Vector3.forward * 90);
            rd.velocity = new Vector2(speed, -1);
        }
        else
        {
            rd.velocity = Vector2.down * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();

            OnHit(bullet.power);
            Destroy(collision.gameObject);
           
        }
    }

    private void OnHit(float BulletPower)
    {
        health -= BulletPower;
        spriteRender.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);

        if (health <= 0 && isDead == false)
        {

            isDead = true;
            Destroy(gameObject);
            Player playerscore = playerObject.GetComponent<Player>();
            playerscore.score += 1;

            //random
            int random = Random.Range(0, 10);
            if (random < 3) {
                Debug.Log("Not item");
            }
            else if (random < 8)
            {
                Instantiate(cherry, transform.position, cherry.transform.rotation);
            }
            else if(random <9)
            {
                Instantiate(starryberry, transform.position, cherry.transform.rotation);
            }
            else if(random <10)
            {
                Instantiate(watermelon, transform.position, cherry.transform.rotation);
            }

        }
    }

    void ReturnSprite()
    {
        spriteRender.sprite = sprites[0];
    }
}
