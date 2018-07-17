using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller : MonoBehaviour {

    private Rigidbody2D rb2d;

    public int health;

    public int heartContainers;

    public Image[] hearts;
    public Sprite emptyHeart;
    public Sprite fullHeart;

    public FadeZ fadeScript;

    public AudioClip voltShock;
    private AudioSource source;

    public Shake Shaker;
    public float duration = 1f;




    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        source = GetComponent<AudioSource>();
        fadeScript.GetComponent<FadeZ>();
    }

    void FixedUpdate()
    {
        if (Input.GetKey("w"))
        {
            rb2d.velocity = new Vector2(0, 6);
        }

        else if (Input.GetKey("a"))
        {
            rb2d.velocity = new Vector2(-6, 0);
        }

        else if (Input.GetKey("s"))
        {
            rb2d.velocity = new Vector2(0, -6);
        }

        else if (Input.GetKey("d"))
        {
            rb2d.velocity = new Vector2(6, 0);
        }

        else
        {
            rb2d.velocity = new Vector2(0, 0);
        }
    }

    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if(i < heartContainers)
            {
                hearts[i].enabled = true;
            }

            else
            {
                hearts[i].enabled = false;
            }
        }

        if(health > heartContainers)
        {
            health = heartContainers;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //SceneManager.LoadScene("testK", LoadSceneMode.Additive);

        int buildIndex = SceneManager.GetSceneByName("testK").buildIndex;
        SceneManager.LoadScene(buildIndex);

        SceneManager.UnloadSceneAsync("Maze2");
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if(health <= 0)
        {
            health = 0;
            Debug.Log("Death Message");
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (health != 0)
        {
            TakeDamage(1);

            
            fadeScript.OnHitColor();

            source.PlayOneShot(voltShock, 1f);

            Shaker.cShake(duration);
        }
    }
}
