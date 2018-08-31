using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Server : MonoBehaviour
{
    [Serializable]
    public class InfectedStateInfo
    {
        public Color ServerEmissionColor;
        public Color TubesEmissionColor;
    }

    public MeshRenderer ServerMeshRenderer;
    public InfectedStateInfo InfectedStateData;

    private Color _originalServerEmissionColor;
    private Color _originalTubesEmissionColor;

    void Awake()
    {
        if (ServerMeshRenderer == null)
        {
            ServerMeshRenderer = GetComponentInChildren<MeshRenderer>();
        }
    }

	// Use this for initialization
	void Start () {
	    Material serverMaterial = GetServerMaterial();
	    Material tubesMaterial = GetTubesMaterial();

	    _originalServerEmissionColor = serverMaterial.GetColor("_EmissionColor");
	    _originalTubesEmissionColor = tubesMaterial.GetColor("_EmissionColor");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Infect()
    {
        Material serverMaterial = GetServerMaterial();
        Material tubesMaterial = GetTubesMaterial();

        serverMaterial.SetColor("_EmissionColor", InfectedStateData.ServerEmissionColor);
        tubesMaterial.SetColor("_EmissionColor", InfectedStateData.TubesEmissionColor);
    }

    Material GetServerMaterial()
    {
        return ServerMeshRenderer.materials[0];
    }

    Material GetTubesMaterial()
    {
        return ServerMeshRenderer.materials[1];
    }
}
