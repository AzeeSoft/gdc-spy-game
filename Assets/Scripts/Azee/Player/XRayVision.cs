using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class XRayVision : MonoBehaviour
{
    public Color XRayVisionColor;
    Shader _xRayShader;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnValidate()
    {
        Shader.SetGlobalColor("_XRayDefaultColor", XRayVisionColor);
    }

    void OnEnable()
    {
        if (_xRayShader == null)
        {
            _xRayShader = Shader.Find("Azee/XRayShader");
        }

        GetComponent<Camera>().SetReplacementShader(_xRayShader, "");
    }

    void OnDisable()
    {
        GetComponent<Camera>().ResetReplacementShader();
    }
}
