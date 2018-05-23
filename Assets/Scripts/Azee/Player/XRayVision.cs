using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class XRayVision : MonoBehaviour
{
    public Shader XRayShader;
    public Color XRayVisionColor;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnValidate()
    {
        Shader.SetGlobalColor("_XRayVisionColor", XRayVisionColor);
    }

    void OnEnable()
    {
        if (XRayShader != null)
        {
            GetComponent<Camera>().SetReplacementShader(XRayShader, "");
        }
    }

    void OnDisable()
    {
        GetComponent<Camera>().ResetReplacementShader();
    }
}
