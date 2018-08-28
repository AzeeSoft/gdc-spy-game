using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    private Animator _animator;

    private int _objectsInProximity = 0;

	// Use this for initialization
	void Start ()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool("open", false);
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Guard")
        {
            _objectsInProximity++;
        }      
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Guard")
        {
            _objectsInProximity--;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        _animator.SetBool("open", _objectsInProximity > 0);
    }
}
