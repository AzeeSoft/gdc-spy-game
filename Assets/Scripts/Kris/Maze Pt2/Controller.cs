using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour {          

    private Rigidbody2D rb2d;       

    
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

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //SceneManager.LoadScene("testK", LoadSceneMode.Additive);

        int buildIndex = SceneManager.GetSceneByName("testK").buildIndex;
        SceneManager.LoadScene(buildIndex);

        SceneManager.UnloadSceneAsync("Maze2");
    }
}
