using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneSwitchHandler : MonoBehaviour
{
//    public bool SkipDefaultBehaviour = false;

    [HideInInspector] public UnityEvent OnScenePaused = new UnityEvent();
    [HideInInspector] public UnityEvent OnSceneResumed = new UnityEvent();

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}