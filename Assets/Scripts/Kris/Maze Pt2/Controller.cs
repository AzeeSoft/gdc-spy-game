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

    public Image damageImage;
    public float flashSpeed = 5f;
    public int numFlashes = 4;
    public float timeBetweenFlash = 0.5f;
    public Color flashColor = Color.red;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
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

    IEnumerator FlashInput(InputField input)
    {
        // save the InputField.textComponent color
        Color defaultColor = input.textComponent.color;
        for (int i = 0; i < numFlashes; i++)
        {
            // if the current color is the default color - change it to the flash color
            if (input.textComponent.color == defaultColor)
            {
                input.textComponent.color = flashColor;
            }
            else // otherwise change it back to the default color
            {
                input.textComponent.color = defaultColor;
            }
            yield return new WaitForSeconds(timeBetweenFlash);
        }
        yield return new WaitForSeconds(1);
    }   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (health != 0)
        {
            TakeDamage(1);
            StartCoroutine(FlashInput());
        }
    }
}
