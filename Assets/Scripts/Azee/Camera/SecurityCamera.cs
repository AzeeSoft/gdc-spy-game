﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class SecurityCamera : PlayerControllable
{
    private SecurityCameraController _securityCameraController;
    private ActionController _actionController;
    private FreeLookCam _freeLookCam;
    private InteractiveObject _interactiveObject;
    private AudioController _audioController;

    private Camera _camera;
    private AudioListener _audioListener;

    void Awake()
    {
        _securityCameraController = GetComponent<SecurityCameraController>();
        _actionController = GetComponent<ActionController>();
        _freeLookCam = GetComponent<FreeLookCam>();
        _interactiveObject = GetComponent<InteractiveObject>();
        _audioController = GetComponent<AudioController>();

        _camera = GetComponentInChildren<Camera>();
        _audioListener = GetComponentInChildren<AudioListener>();

        ReleaseControl();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override void OnControlBegin()
    {
        _securityCameraController.enabled = true;
        _actionController.enabled = true;
        _freeLookCam.enabled = true;

        _interactiveObject.enabled = false;

        _camera.enabled = true;
        _audioListener.enabled = true;

        _audioController.PlayClip(0);
    }

    protected override void OnControlEnd()
    {
        _securityCameraController.enabled = false;
        _actionController.enabled = false;
        _freeLookCam.enabled = false;

        _interactiveObject.enabled = true;

        _camera.enabled = false;
        _audioListener.enabled = false;
    }
}
