using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecurityCameraController : MonoBehaviour
{
    public GameObject SecurityCameraUI; 

    private Camera _cctvCamera;

    void Awake()
    {
        _cctvCamera = GetComponentInChildren<Camera>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		CheckForReturnToPlayer();
	}

    void OnEnable()
    {
        SecurityCameraUI.SetActive(true);
    }

    void OnDisable()
    {
        SecurityCameraUI.SetActive(false);
    }

    void CheckForReturnToPlayer()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            GameManager.Instance.switchPlayerControlToFirstPerson();
        }
    }
}
