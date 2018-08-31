using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Terminal : MonoBehaviour
{
    [Serializable]
    public class Link
    {
        public Transform StartTransform;
        public Transform EndTransform;
    }

    public string MazeLevelKey;

    public Link LinkData;


    public UnityEvent OnHacked;
    public UnityEvent OnFailed;

    private SceneSwitcher _sceneSwitcher;
    private InteractiveObject _interactiveObject;

    private LineRenderer _linkLineRenderer;

    void Awake()
    {
        _sceneSwitcher = FindObjectOfType<SceneSwitcher>();
        _interactiveObject = GetComponent<InteractiveObject>();
        _linkLineRenderer = GetComponentInChildren<LineRenderer>();
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!LinkData.StartTransform)
        {
            LinkData.StartTransform = transform;
        }

        _linkLineRenderer.SetPosition(0, LinkData.StartTransform.position);
        _linkLineRenderer.SetPosition(1,
            (_interactiveObject.enabled && LinkData.EndTransform)
                ? LinkData.EndTransform.position
                : LinkData.StartTransform.position);
    }

    public void AttemptHacking()
    {
        MazeSceneController.MazeSceneData mazeSceneData = new MazeSceneController.MazeSceneData()
        {
            MazeLevelKey = MazeLevelKey,
            Terminal = this
        };

        Action loadMazeScene = () => { _sceneSwitcher.SwitchScene("MazeScene-Final", true, mazeSceneData); };

        if (Fading.Instance)
        {
            Fading.Instance.FadeSpeed = 0.7f;
            Fading.Instance.BeginFade(Fading.FadeOut, loadMazeScene);
        }
        else
        {
            loadMazeScene();
        }
    }

    public void OnHackAttemptResultReceived(MazeSceneController.MazeSceneData mazeSceneData)
    {
        if (Fading.Instance)
        {
            Fading.Instance.FadeSpeed = 0.8f;
            Fading.Instance.BeginFade(Fading.FadeIn);
        }

        if (mazeSceneData.Result)
        {
            _interactiveObject.enabled = false;
            OnHacked.Invoke();
            Debug.Log("Hacked");
        }
        else
        {
            Debug.Log("Couldn't hack");
            OnFailed.Invoke();
        }
    }
}