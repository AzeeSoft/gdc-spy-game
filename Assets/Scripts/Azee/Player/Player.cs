using System.Collections;
using System.Collections.Generic;
using Azee;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : PlayerControllable
{
    [SerializeField]
    private float StunRefillRate = 0.5f;

    [SerializeField]
    private float StunDepleteRate = 2f;

    [SerializeField]
    private float _health = 100f;

    [SerializeField]
    private float _stunBar = 100f;

    [SerializeField] [ReadOnly]
    private bool _depletingStunBar = false;

    public float DefaultGlitchIntensity = 0.4f;
    public float MaxGlitchness = 2f;

    [SerializeField]
    private float _glitchness = 0f;

    private float _currentAlertness = 0f;
    private Guard _guardWithHighestAlertness = null;

    public GameObject PlayerHUD;

    private FirstPersonController _firstPersonController;
    private ActionController _actionController;
    private VisionToggler _visionToggler;
    private Lean _leanScript;
    private PlayerHUDController _playerHudController;

    private Camera _camera;
    private PostProcessingBehaviour _postProcessingBehaviour;
    private AudioListener _audioListener;

    private GlitchEffect _glitchEffect;

    private bool _isInfected = false;

    public bool IsInfected
    {
        get { return _isInfected; }
    }

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
        _glitchEffect = GetComponentInChildren<GlitchEffect>();
    }

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update () {
        UpdateStunBar();
	    UpdateUI();

        UpdateGlitchness();
    }

    void UpdateUI()
    {
        if (_playerHudController.UIElements.HealthUI != null)
            _playerHudController.UIElements.HealthUI.fillAmount = _health / 100.0f;

        if (_playerHudController.UIElements.StunBarUI != null)
            _playerHudController.UIElements.StunBarUI.fillAmount = _stunBar / 100.0f;
    }

    void UpdateGlitchness()
    {
        _glitchEffect.intensity = _glitchness + DefaultGlitchIntensity;
        _glitchEffect.colorIntensity = _glitchness;
        _glitchEffect.flipIntensity = _glitchness + ((Time.timeScale > 0) ? DefaultGlitchIntensity : 0f);
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

    public void ResetStunBar()
    {
        _depletingStunBar = true;
    }

    public bool CanStun()
    {
        return (!_depletingStunBar) && (_stunBar >= 100);
    }

    private void UpdateStunBar()
    {
        if (_depletingStunBar)
        {
            _stunBar -= StunDepleteRate;
            if (_stunBar < 0)
            {
                _depletingStunBar = false;
            }
        }
        else
        {
            if (_stunBar < 100)
            {
                Debug.Log("Refilling Stun Bar");
                _stunBar += StunRefillRate;
                if (_stunBar > 100)
                {
                    _stunBar = 100;
                }
            }
        }
    }

    /// <summary>
    /// Called when the player is in guard's sight
    /// </summary>
    /// <param name="guard"></param>
    /// <param name="alertness">A value between 0 and 1</param>
    public void OnBeingInGuardSight(Guard guard, float alertness)
    {
        if (alertness > _currentAlertness || guard == _guardWithHighestAlertness)
        {
            _currentAlertness = alertness;
            _guardWithHighestAlertness = guard;

            _glitchness = StaticTools.Remap(alertness, 0f, 1f, 0f, MaxGlitchness);

            if (alertness > 0.6f)
            {
                _firstPersonController.SlowDown(1 - alertness);
            }
            else
            {
                _firstPersonController.ResetSpeeds();
            }
        }
    }

    public void OnSeenByGuard(Guard guard)
    {
        _currentAlertness = 1f;
        _guardWithHighestAlertness = guard;

        _glitchness = MaxGlitchness;

        _isInfected = true;

        _firstPersonController.enabled = false;

        LevelManager.Instance.OnPlayerCaughtByGuard(this, guard);
    }
}
