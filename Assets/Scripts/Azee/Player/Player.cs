using System.Collections;
using System.Collections.Generic;
using Azee;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : PlayerControllable
{
    private const float MaxHealth = 100f;

    public AudioSource InfectionAudioSource;

    [SerializeField] private float StunRefillRate = 0.5f;

    [SerializeField] private float StunDepleteRate = 2f;

    [SerializeField] private float _health = 100f;

    [SerializeField] private float _healthRegenerationRate = 1f;

    [SerializeField] private float _healthRegenerationWaitTime = 3f;

    [SerializeField] private float _stunBar = 100f;

    [SerializeField] [ReadOnly] private bool _depletingStunBar = false;

    public float DefaultGlitchIntensity = 0.4f;
    public float MaxGlitchness = 2f;

    [SerializeField] private float _glitchness = 0f;

    private float _currentAlertness = 0f;
    private Guard _guardWithHighestAlertness = null;

    private float _lastInfectedTime = float.MinValue;

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

    public bool IsInfected
    {
        get { return _health <= 0; }
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
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealth();
        UpdateGlitchness();

        UpdateStunBar();
        UpdateUI();
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
        _glitchness = StaticTools.Remap(_health, 0, MaxHealth, MaxGlitchness, 0);

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

    public void Infect(float infectionValue)
    {
        if (!IsInfected)
        {
            if (_health >= MaxHealth && !InfectionAudioSource.isPlaying)
            {
                InfectionAudioSource.time = 0;
                InfectionAudioSource.Play();
            }

            _health -= infectionValue;
            _lastInfectedTime = Time.time;

            if (_health <= 0)
            {
                _health = 0;
                OnInfected();
            }
        }
    }

    public void UpdateHealth()
    {
        if (!IsInfected)
        {
            if (_health < MaxHealth && (Time.time - _lastInfectedTime >= _healthRegenerationWaitTime))
            {
                _health += _healthRegenerationRate;

                if (_health > MaxHealth)
                {
                    if (InfectionAudioSource.isPlaying)
                    {
                        InfectionAudioSource.Stop();
                    }

                    _health = MaxHealth;
                }
            }

            if (_health <= (MaxHealth / 2.5f))
            {
                float slowDownFactor = StaticTools.Remap(_health, 0, MaxHealth, 0, 1);
                _firstPersonController.SlowDown(slowDownFactor);
            }
            else
            {
                _firstPersonController.ResetSpeeds();
            }

            if (_health < MaxHealth)
            {
                if (!InfectionAudioSource.isPlaying)
                {
                    InfectionAudioSource.time = 4.5f;
                    InfectionAudioSource.Play();
                }
            }

            InfectionAudioSource.volume = StaticTools.Remap(_health, 0, MaxHealth, 1, 0.3f);
        }
    }

    public void OnInfected()
    {
        _firstPersonController.enabled = false;
        LevelManager.Instance.OnPlayerInfected(this);

        StartCoroutine(FadeOutInfectionAudio());
    }

    IEnumerator FadeOutInfectionAudio()
    {
        while (InfectionAudioSource.volume > 0)
        {
            InfectionAudioSource.volume -= 0.1f;
            yield return new WaitForSeconds(0.3f);
        }
    }
}