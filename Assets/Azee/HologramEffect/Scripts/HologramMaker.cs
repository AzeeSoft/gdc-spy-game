using System.Collections;
using System.Collections.Generic;
using BasicTools.ButtonInspector;
using UnityEngine;

public class HologramMaker : MonoBehaviour
{
    public Material HologramMaterial;

    public bool RemoveColliders = false;

    [Button("Make Hologram", "MakeHologram")]
    public bool InvokeMakeHologram;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void MakeHologram()
    {
        if (HologramMaterial)
        {
            ApplyMaterialRecursively(HologramMaterial);
            if (RemoveColliders)
            {
                RemoveCollidersRecursively();
            }
        }
        else
        {
            Debug.LogError("No Hologram Material specified");
        }
    }


    private void ApplyMaterialRecursively(Material material)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>(true);

        foreach (Renderer renderer in renderers)
        {
            renderer.materials = new[]{ HologramMaterial };
        }
    }

    private void RemoveCollidersRecursively()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            DestroyImmediate(collider);
        }
    }
}
