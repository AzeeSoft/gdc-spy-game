using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPress : MonoBehaviour {

    private Animator _animator;

    public GameObject OpenPanel = null;


	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            OpenPanel.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _animator.SetBool("open", false);
            OpenPanel.SetActive(false);
        }
    }

    private bool IsOpenPanelActive
    {
        get
        {
            return OpenPanel.activeInHierarchy;
        }
    }
    // Update is called once per frame
    void Update ()
    {
		if(IsOpenPanelActive)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                OpenPanel.SetActive(false);
                _animator.SetBool("open", true);
            }
        }
	}
}
