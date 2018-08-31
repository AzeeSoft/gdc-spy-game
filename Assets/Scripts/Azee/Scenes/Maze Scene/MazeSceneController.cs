using System;
using System.Collections;
using System.Collections.Generic;
using BasicTools.ButtonInspector;
using UnityEngine;

public class MazeSceneController : MonoBehaviour {

    public class MazeSceneData
    {
        public string MazeLevelKey;
        public Terminal Terminal;
        public bool Result = false;
    }

    public List<MazeLevel> MazeLevels = new List<MazeLevel>();

    [SerializeField]
    [Button("Find maze level objects in children", "FindMazeLevelsInChildren")]
    private bool _buttonFindMazeLevelsInChildren;

    private MazeSceneData _mazeSceneData;

    // Use this for initialization
    void Start () {
        if (Fading.Instance)
        {
            Fading.Instance.FadeSpeed = 0.3f;
            Fading.Instance.BeginFade(Fading.FadeIn);
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetButtonDown("Cancel"))
	    {
            MazeSceneResult(false);
	    }
    }

    public void FindMazeLevelsInChildren()
    {
        MazeLevels.Clear();

        foreach (MazeLevel mazeLevel in GetComponentsInChildren<MazeLevel>(true))
        {
            MazeLevels.Add(mazeLevel);
        }
    }

    public void OnSceneDataReceived(object data)
    {
        MazeSceneData sceneData = data as MazeSceneData;
        if (sceneData != null)
        {
            _mazeSceneData = sceneData;

            foreach (MazeLevel mazeLevel in MazeLevels)
            {
                if (mazeLevel.Key.Equals(_mazeSceneData.MazeLevelKey))
                {
                    mazeLevel.gameObject.SetActive(true);
                    return;
                }
            }
        }
    }

    public void MazeSceneResult(bool Success)
    {
        _mazeSceneData.Result = Success;

        Action goBackToLastScene = () => { FindObjectOfType<SceneSwitcher>().ShowLastSavedScene(_mazeSceneData); };

        if (Fading.Instance)
        {
            Fading.Instance.FadeSpeed = 0.8f;
            Fading.Instance.BeginFade(Fading.FadeOut, goBackToLastScene);
        }
        else
        {
            goBackToLastScene();
        }
    }

}
