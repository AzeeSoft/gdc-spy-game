using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerControllable : MonoBehaviour
{
    private bool controlsPlayer = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool hasControl()
    {
        return controlsPlayer;
    }

    public void TakeControl()
    {
        controlsPlayer = true;
        OnControlBegin();
    }

    public void ReleaseControl()
    {
        controlsPlayer = false;
        OnControlEnd();
    }

    protected abstract void OnControlBegin();
    protected abstract void OnControlEnd();
}
