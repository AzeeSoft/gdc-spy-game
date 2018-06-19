using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mazeMovement : MonoBehaviour {

    public Transform playerObj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("w"))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, -3);
        }

        else if(Input.GetKey("a"))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(3, 0);
        }

        else if (Input.GetKey("s"))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 3);
        }

        else if (Input.GetKey("d"))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-3, 0);
        }

        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }


    }
}
