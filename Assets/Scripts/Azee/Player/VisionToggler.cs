using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionToggler : MonoBehaviour {

    private Camera fpsCamera;
    private XRayVision xRayVision;

    // Use this for initialization
    void Start () {
        fpsCamera = GetComponentInChildren<Camera>();
        xRayVision = fpsCamera.GetComponent<XRayVision>();
    }

    void LateUpdate () {
        CheckVisionToggle();
    }

    void CheckVisionToggle()
    {
        if (Input.GetButtonDown("Toggle Vision") && xRayVision)
        {
            xRayVision.enabled = !xRayVision.enabled;
        }
    }
}
