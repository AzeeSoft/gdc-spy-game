﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

//[ExecuteInEditMode]
public class XRayVision : MonoBehaviour
{
    public Color XRayVisionColor;
    public float FarClipPlaneDistance = 200;
    public PostProcessingProfile XRayPostProcessingProfile;

    public bool skipAnimation = false;

    public bool IsXRayVisionEnabled
    {
        get { return _xRayVisionState == XRayVisionState.XRay; }
    }

    Shader _xRayShader;

    private Camera _camera;
    private PostProcessingBehaviour _postProcessingBehaviour;
    private VolumetricLightRenderer _volumetricLightRenderer;
    private Animator _animator;
    private AudioController _audioController;
    
    private XRayVisionState _xRayVisionState = XRayVisionState.Normal;

    struct PreXRayConfig
    {
        public static float FarClipPlaneDistance = 1000;
        public static PostProcessingProfile PrevPostProcessingProfile = null;
        public static bool VolumetricLightEnabled = false;
    }

    enum XRayVisionState
    {
        Normal,
        TransitioningToXRay,
        XRay,
        TransitioningToNormal
    }

    public void Awake()
    {
        DefineVarsIfMissing();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnValidate()
    {
        ApplyDefaultXRayVisionColor();
    }

    void ApplyDefaultXRayVisionColor()
    {
        Shader.SetGlobalColor("_XRayDefaultColor", XRayVisionColor);
    }

    public void EnableXRayVision()
    {
        if (_xRayShader == null)
        {
            _xRayShader = Shader.Find("Azee/XRayShader");
            if (_xRayShader == null)
            {
                throw new Exception("Critical Error: \"Azee/XRayShader\" shader is missing. Make sure it is included in \"Always Included Shaders\" in ProjectSettings/Graphics.");
            }
        }
        ApplyDefaultXRayVisionColor();


        DefineVarsIfMissing();

        _xRayVisionState = XRayVisionState.TransitioningToXRay;

        if (_audioController)
        {
            _audioController.PlayClip(0);
        }

        if (_animator && !skipAnimation)
        {
            _animator.SetTrigger("showXRay");
        }
        else
        {
            SwitchToXRayView();
        }

        skipAnimation = false;
    }

    public void DisableXRayVision()
    {
        DefineVarsIfMissing();

        if (_audioController)
        {
            _audioController.PlayClip(1);
        }

        _xRayVisionState = XRayVisionState.TransitioningToNormal;

        if (_animator && !skipAnimation)
        {
            _animator.SetTrigger("hideXRay");
        }
        else
        {
            SwitchToNormalView();
        }

        skipAnimation = false;
    }

    private void DefineVarsIfMissing()
    {
        if (_camera == null)
        {
            _camera = GetComponent<Camera>();
        }

        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }

        if (_audioController == null)
        {
            _audioController = GetComponent<AudioController>();
        }

        if (_postProcessingBehaviour == null)
        {
            _postProcessingBehaviour = GetComponent<PostProcessingBehaviour>();
        }

        if (_volumetricLightRenderer == null)
        {
            _volumetricLightRenderer = GetComponent<VolumetricLightRenderer>();
        }
    }

    public void SwitchToXRayView()
    {
        _camera.SetReplacementShader(_xRayShader, "");

        PreXRayConfig.FarClipPlaneDistance = _camera.farClipPlane;
        _camera.farClipPlane = FarClipPlaneDistance;

        if (_postProcessingBehaviour)
        {
            PreXRayConfig.PrevPostProcessingProfile = _postProcessingBehaviour.profile;
            _postProcessingBehaviour.profile = XRayPostProcessingProfile;
        }

        if (_volumetricLightRenderer)
        {
            PreXRayConfig.VolumetricLightEnabled = _volumetricLightRenderer.enabled;
            _volumetricLightRenderer.enabled = false;
        }

        _xRayVisionState = XRayVisionState.XRay;
    }

    public void SwitchToNormalView()
    {
        _camera.ResetReplacementShader();

        _camera.farClipPlane = PreXRayConfig.FarClipPlaneDistance;
        if (_postProcessingBehaviour)
        {
            _postProcessingBehaviour.profile = PreXRayConfig.PrevPostProcessingProfile;
        }

        if (_volumetricLightRenderer)
        {
            _volumetricLightRenderer.enabled = PreXRayConfig.VolumetricLightEnabled;
        }

        _xRayVisionState = XRayVisionState.Normal;
    }

    void OnDisable()
    {

    }

    void OnEnable()
    {
        switch (_xRayVisionState)
        {
            case XRayVisionState.TransitioningToXRay:
                EnableXRayVision();
                break;
            case XRayVisionState.TransitioningToNormal:
                DisableXRayVision();
                break;
        }

    }
}
