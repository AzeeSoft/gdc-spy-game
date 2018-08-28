using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionToggler : MonoBehaviour
{
    public bool InfiniteBattery = false;
    public float MaxBattery = 100f;
    public float DepletionTime = 3f;
    public float RechargeTime = 6f;
    public float CoolDownTime = 2f;

    [SerializeField] [ReadOnly] private float _currentBattery = 100f;

    private float _minRequiredBattery = 0f;

    private float DepletionPerSecond
    {
        get { return MaxBattery / DepletionTime; }
    }

    private float RechargePerSecond
    {
        get { return MaxBattery / RechargeTime; }
    }

    private Camera fpsCamera;
    private XRayVision xRayVision;
    private PlayerHUDController playerHudController;

    // Use this for initialization
    void Start()
    {
        fpsCamera = GetComponentInChildren<Camera>();
        xRayVision = fpsCamera.GetComponent<XRayVision>();
        playerHudController = GetComponent<PlayerHUDController>();
    }

    void Update()
    {
        UpdateBattery();
        UpdateUI();
    }

    void LateUpdate()
    {
        CheckVisionToggle();
    }

    void UpdateUI()
    {
        if (playerHudController.UIElements.XRayUI != null)
            playerHudController.UIElements.XRayUI.fillAmount = _currentBattery / 100f;
    }

    void UpdateBattery()
    {
        if (InfiniteBattery)
        {
            if (_currentBattery < MaxBattery)
            {
                _currentBattery = MaxBattery;
            }

            return;
        }

        if (xRayVision.IsXRayVisionEnabled)
        {
            DepleteBattery();
            if (_currentBattery <= _minRequiredBattery)
            {
                DisableXRayVision();

                // If force disabled xray vision, set current battery to negative (Works like a cooldown value)
                _currentBattery = -RechargePerSecond * CoolDownTime;
            }
        }
        else
        {
            RechargeBattery();
        }
    }

    void DepleteBattery()
    {
        if (_currentBattery > 0)
        {
            _currentBattery -= DepletionPerSecond * Time.deltaTime;

            if (_currentBattery < 0)
            {
                _currentBattery = 0;
            }
        }
    }

    void RechargeBattery()
    {
        if (_currentBattery < MaxBattery)
        {
            _currentBattery += RechargePerSecond * Time.deltaTime;

            if (_currentBattery > MaxBattery)
            {
                _currentBattery = MaxBattery;
            }
        }
    }

    void CheckVisionToggle()
    {
        if (Input.GetButtonDown("Toggle Vision") && xRayVision && _currentBattery >= _minRequiredBattery)
        {
            ToggleVision();
        }
    }

    void ToggleVision()
    {
        if (xRayVision.IsXRayVisionEnabled)
        {
            DisableXRayVision();
        }
        else
        {
            EnableXRayVision();
        }
    }

    public void EnableXRayVision(bool immediate = false)
    {
        xRayVision.skipAnimation = immediate;
        xRayVision.EnableXRayVision();
    }

    public void DisableXRayVision(bool immediate = false)
    {
        xRayVision.skipAnimation = immediate;
        xRayVision.DisableXRayVision();
    }
}