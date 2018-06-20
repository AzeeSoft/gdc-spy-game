using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionToggler : MonoBehaviour
{
    public bool InfiniteBattery = false;
    public float MaxBattery = 100f;
    public float depletionTime = 3f;
    public float rechargeTime = 6f;

    [SerializeField] [ReadOnly] private float _currentBattery = 100f;
    [SerializeField] [ReadOnly] private float _minRequiredBattery = 10f;

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
            if (_currentBattery <= 0)
            {
                DisbleXRayVision();
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
            _currentBattery -= (MaxBattery / depletionTime) * Time.deltaTime;

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
            _currentBattery += (MaxBattery / rechargeTime) * Time.deltaTime;

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