using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mazeMovement : MonoBehaviour {

    public Transform playerObj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("w"))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, -6);
        }

        else if(Input.GetKey("a"))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(6, 0);
        }

        else if (Input.GetKey("s"))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 6);
        }

        else if (Input.GetKey("d"))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-6, 0);
        }

        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }


    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene(sceneName: "testK");
    }
}
