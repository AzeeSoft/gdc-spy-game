using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{

    // Use this for initialization
    public void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        Material mat = renderer.material;

        float emission = Mathf.PingPong(Time.time, 1.0f);
        Color baseColor = Color.red;

        Color finalColor = baseColor; //* Mathf.LinearToGammaSpace(emission);

        mat.SetColor("_EmissionColor", finalColor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
