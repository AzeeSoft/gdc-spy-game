using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    private Animator _animator;

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
            _animator.SetBool("open", true);
        }      
    }

    void OnTriggerExit(Collider other)
    {
        _animator.SetBool("open", false);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
