using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : PlayerControllable
{

    [SerializeField]
    private int _health = 100;

    public GameObject PlayerHUD;

    private FirstPersonController _firstPersonController;
    private ActionController _actionController;
    private VisionToggler _visionToggler;
    private Lean _leanScript;
    private PlayerHUDController _playerHudController;

    private Camera _camera;
    private PostProcessingBehaviour _postProcessingBehaviour;
    private AudioListener _audioListener;


    void Awake()
    {
        _firstPersonController = GetComponent<FirstPersonController>();
        _actionController = GetComponent<ActionController>();
        _visionToggler = GetComponent<VisionToggler>();
        _leanScript = GetComponentInChildren<Lean>();
        _camera = GetComponentInChildren<Camera>();
        _postProcessingBehaviour = GetComponentInChildren<PostProcessingBehaviour>();
        _audioListener = GetComponentInChildren<AudioListener>();
        _playerHudController = GetComponent<PlayerHUDController>();
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		UpdateUI();
	}

    void UpdateUI()
    {
        if (_playerHudController.UIElements.HealthUI != null)
            _playerHudController.UIElements.HealthUI.fillAmount = _health;
    }

    protected override void OnControlBegin()
    {
        _firstPersonController.canRotate = true;
        _firstPersonController.enabled = true;
        _actionController.enabled = true;
        _visionToggler.enabled = true;
        _leanScript.enabled = true;
        _camera.enabled = true;
        _postProcessingBehaviour.enabled = true;
        _audioListener.enabled = true;
        PlayerHUD.SetActive(true);
    }

    protected override void OnControlEnd()
    {
        _firstPersonController.canRotate = false;
        _firstPersonController.enabled = false;
        _actionController.enabled = false;
        _visionToggler.enabled = false;
        _leanScript.enabled = false;
        _camera.enabled = false;
        _postProcessingBehaviour.enabled = false;
        _audioListener.enabled = false;
        PlayerHUD.SetActive(false);
    }

    new void OnEnable()
    {
        base.OnEnable();
    }

    new void OnDisable()
    {
        base.OnDisable();
    }
}
