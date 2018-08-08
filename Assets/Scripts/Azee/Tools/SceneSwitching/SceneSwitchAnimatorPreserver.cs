using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitchAnimatorPreserver : SceneSwitchHandler
{
    private Animator _animator = null;

    private AnimatorStateInfo[] pendingLayerInfo;

    void Awake()
    {
        InitMissingVars();

        OnScenePaused.AddListener(OnScenePausedCallback);
        OnSceneResumed.AddListener(OnSceneResumedCallback);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void PrintAnimatorStateInfo(int layer, AnimatorStateInfo animatorStateInfo)
    {
        Debug.Log("Layer: " + layer);
        Debug.Log("FullPathHash: " + animatorStateInfo.fullPathHash);
        Debug.Log("NormalizedTime: " + animatorStateInfo.normalizedTime);
        Debug.Log("Length: " + animatorStateInfo.length);
        Debug.Log("String: " + animatorStateInfo.ToString());
    }

    void OnScenePausedCallback()
    {
        InitMissingVars();

//        Debug.Log("Saving Animator State Info");

        pendingLayerInfo = new AnimatorStateInfo[_animator.layerCount];
        for (int i = 0; i < _animator.layerCount; i++)
        {
            pendingLayerInfo[i] = _animator.GetCurrentAnimatorStateInfo(i);
//            PrintAnimatorStateInfo(i, pendingLayerInfo[i]);
        }

//        Debug.Log("Saved Animator State Info");
    }

    void OnSceneResumedCallback()
    {
        InitMissingVars();

//        Debug.Log("Loading Animator State Info: ");

        for (int i = 0; i < pendingLayerInfo.Length; i++)
        {
//            PrintAnimatorStateInfo(i, pendingLayerInfo[i]);
            _animator.Play(pendingLayerInfo[i].fullPathHash, i, pendingLayerInfo[i].normalizedTime);
        }

//        Debug.Log("Loaded Animator State Info: ");
    }

    void InitMissingVars()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
    }
}
