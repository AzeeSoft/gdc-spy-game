using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    private Animator _animator;

    private int checkIfDoor;

	// Use this for initialization
	void Start ()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool("open", false);
	}

    private void FixedUpdate()
    {
        if (checkIfDoor == 0)
        {
            _animator.SetBool("open", false);
        }
        else if (checkIfDoor != 0)
        {
            _animator.SetBool("open", true);
        }
        else
        {
            Debug.Log("Door thing broke please check DoorScript.cs");
        }



    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Guard")
        {
            checkIfDoor = checkIfDoor + 1;
        }      
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Guard")
        {
            checkIfDoor = checkIfDoor - 1;
        }
    }
}
