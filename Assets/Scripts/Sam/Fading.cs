using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Fading : MonoBehaviour
{
    private static Fading _instance = null;

    public static Fading Instance
    {
        get { return _instance; }
    }

    public const int FadeIn = -1;
    public const int FadeOut = 1;

    public Texture2D FadeOutTexture;
    public float FadeSpeed = 0.8f;

    public bool AutoFadeIn = true;

    public bool IsFading
    {
        get { return (alpha > 0f && alpha < 1f); }
    }

    private int drawDepth = -1000;
    private float alpha = 0f;
    private int fadeDir = 0;

    private Action _callbackAction = null;

    void Awake()
    {
        SetAsInstance();
    }

    void Start()
    {
        if (AutoFadeIn)
        {
            alpha = 1; //Use this is the alpha is not set to 0 be default
            BeginFade(FadeIn);
        }
    }

    void OnGUI()
    {
        alpha += fadeDir * FadeSpeed * Time.unscaledDeltaTime;
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), FadeOutTexture);

        if (alpha <= 0f || alpha >= 1f)
        {
            if (_callbackAction != null)
            {
                _callbackAction();
                _callbackAction = null;
            }
        }
    }

    public float BeginFade(int direction, Action action = null)
    {
        fadeDir = direction;
        _callbackAction = action;

        return (FadeSpeed);
    }

    public void SetAsInstance()
    {
        _instance = this;
    }
}