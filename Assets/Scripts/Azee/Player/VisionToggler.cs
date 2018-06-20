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

    // Use this for initialization
    void Start()
    {
        fpsCamera = GetComponentInChildren<Camera>();
        xRayVision = fpsCamera.GetComponent<XRayVision>();
    }

    void Update()
    {
        UpdateBattery();
    }

    void LateUpdate()
    {
        CheckVisionToggle();
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

        if (xRayVision.enabled)
        {
            DepleteBattery();
            if (_currentBattery <= _minRequiredBattery)
            {
                DisbleXRayVision();

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
        xRayVision.enabled = !xRayVision.enabled;
    }

    void EnableXRayVision()
    {
        xRayVision.enabled = true;
    }

    void DisbleXRayVision()
    {
        xRayVision.enabled = false;
    }
}