using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecurityCameraController : MonoBehaviour
{
    public GameObject SecurityCameraUI;

    private Camera _cctvCamera;
    private SecurityCamera _securityCamera;
    private AudioController _audioController;

    void Awake()
    {
        _audioController = GetComponent<AudioController>();
        _cctvCamera = GetComponentInChildren<Camera>();
        _securityCamera = GetComponent<SecurityCamera>();
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
            LevelManager.Instance.switchPlayerControlToFirstPerson();
            _audioController.PlayClip(1);
        }
    }

    public void RequestPlayerControl()
    {
        LevelManager.Instance.switchPlayerControl(_securityCamera);
    }
}
