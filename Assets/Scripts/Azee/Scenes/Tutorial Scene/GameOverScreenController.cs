using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameOverScreenController : MonoBehaviour
{
    private Animator _animator;

    private Action callback;

    void Awake()
    {
    }

    // Use this for initialization
    void Start ()
    {
        FindMissingVars();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void FindMissingVars()
    {
        _animator = GetComponent<Animator>();
    }

    public void Show(Action action)
    {
        callback = action;
        gameObject.SetActive(true);

        FindMissingVars();

        _animator.SetTrigger("show");
    }

    public void OnGameOverScreenShown()
    {
        if (callback != null)
        {
            callback();
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        // TODO: Load Main Menu Scene
    }
}
