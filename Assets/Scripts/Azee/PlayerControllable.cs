using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerControllable : MonoBehaviour
{
    public Animator CamAnimator;
    public float ExitDuration = 0.25f;

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

        if (CamAnimator)
        {
            CamAnimator.SetTrigger("enter");
        }
    }

    public void ReleaseControl(bool animate = false, Action action = null)
    {
        controlsPlayer = false;

        if (CamAnimator && animate)
        {
            CamAnimator.SetTrigger("exit");
            StartCoroutine(EndControlAfter(ExitDuration, action));
        }
        else
        {
            OnControlEnd();
            if (action != null)
            {
                action();
            }
        }
    }

    private IEnumerator EndControlAfter(float duration, Action action = null)
    {
        yield return new WaitForSeconds(ExitDuration);

        OnControlEnd();
        if (action != null)
        {
            action();
        }
    }

    protected abstract void OnControlBegin();
    protected abstract void OnControlEnd();
}
