using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public Color DefaultColor = Color.yellow;

    private Renderer[] _renderers;

    void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();
    }

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
        TurnColor(DefaultColor);
    }

    public void TurnColor(Color color)
    {
        foreach (Renderer renderer in _renderers)
        {
            Material mat = renderer.material;
            mat.SetColor("_EmissionColor", color);
        }

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
