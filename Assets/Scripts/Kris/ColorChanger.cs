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
        TurnColor(Color.red);
    }

    public void turnDefault()
    {
        TurnColor(Color.yellow);
    }

    public void TurnColor(Color color)
    {
        Renderer renderer = GetComponent<Renderer>();
        Material mat = renderer.material;
        mat.SetColor("_EmissionColor", color);

        SpotlightColor(color);
    }

    public void SpotlightColor(Color color)
    {
        Light light = GetComponentInChildren<Light>();
        light.color = color;
    }

    public Color GetColor()
    {
        Renderer renderer = GetComponent<Renderer>();
        Material mat = renderer.material;
        return mat.GetColor("_EmissionColor");
    }
}
