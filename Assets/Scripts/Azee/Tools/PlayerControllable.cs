using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerControllable : MonoBehaviour
{
    public Animator CamAnimator;
    public float ExitDuration = 0.25f;

    [ReadOnly]
    public bool controlsPlayer = false;

    private IEnumerator _endControlAfterCoroutine = null;

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
        if (CamAnimator && animate)
        {
            CamAnimator.SetTrigger("exit");

            _endControlAfterCoroutine = EndControlAfter(ExitDuration, action);
            StartCoroutine(_endControlAfterCoroutine);
        }
        else
        {
            controlsPlayer = false;
            OnControlEnd();
            if (action != null)
            {
                action();
            }
        }
    }

    private IEnumerator EndControlAfter(float duration, Action action = null)
    {
//        Debug.Log("Waiting...");
        yield return new WaitForSeconds(ExitDuration);

        controlsPlayer = false;
        OnControlEnd();
        if (action != null)
        {
            action();
        }

//        Debug.Log("Transferred Control...");
        _endControlAfterCoroutine = null;
    }

    public void OnDisable()
    {

    }

    public void OnEnable()
    {
//        Debug.Log("coroutine: " + _endControlAfterCoroutine);
        if (_endControlAfterCoroutine != null)
        {
//            Debug.Log("Restarting coroutine...");
            StartCoroutine(_endControlAfterCoroutine);
        }
    }

    protected abstract void OnControlBegin();
    protected abstract void OnControlEnd();
}
