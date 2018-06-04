using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{

    // Use this for initialization
    public void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void turnRed()
    {
        Renderer renderer = GetComponent<Renderer>();
        Material mat = renderer.material;

        float emission = Mathf.PingPong(Time.time, 1.0f);
        Color baseColor = Color.red;

        Color finalColor = baseColor; //* Mathf.LinearToGammaSpace(emission);

        mat.SetColor("_EmissionColor", finalColor);
    }

    public void turnDefault()
    {
        Renderer renderer = GetComponent<Renderer>();
        Material mat = renderer.material;

        float emission = Mathf.PingPong(Time.time, 1.0f);
        Color baseColor = Color.green;

        Color finalColor = baseColor; //* Mathf.LinearToGammaSpace(emission);

        mat.SetColor("_EmissionColor", finalColor);
    }

    public void spotlightRed()
    {
        Light renderer = GetComponentInChildren<Light>();

        Color baseColor = Color.red;

        Color finalColor = baseColor;

        renderer.color = finalColor; 
    }

    public void spotlightDefault()
    {
        Light renderer = GetComponentInChildren<Light>();

        Color baseColor = Color.yellow;

        Color finalColor = baseColor;

        renderer.color = finalColor;
    }
}
