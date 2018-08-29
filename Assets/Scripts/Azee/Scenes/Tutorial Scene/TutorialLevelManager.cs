using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TutorialLevelManager : LevelManager {

    [Serializable]
    public struct HallwayObjectsInfo
    {
        public GameObject GuardGameObject;
    }

    public HallwayObjectsInfo HallwayObjects;

    public GameOverScreenController gameOverScreenController;

	// Use this for initialization
	new void Start ()
	{
	    base.Start();
        DeactivateHallwayObjects();

	    TutorialManager.Instance.ShowTutorial("Initializing");
	}
	
	// Update is called once per frame
    new void Update () {
        base.Update();

	    if (Input.GetButtonDown("Submit"))
	    {
            TutorialManager.Instance.BroadcastTutorialAction("ok");
	    }
	}

    public void OnSceneResultReceived(object data)
    {
        if (data is MazeSceneController.MazeSceneData)
        {
            MazeSceneController.MazeSceneData mazeSceneData = (MazeSceneController.MazeSceneData) data;
            mazeSceneData.Terminal.OnHackAttemptResultReceived(mazeSceneData);
        }
    }

    public void DeactivateHallwayObjects()
    {
        HallwayObjects.GuardGameObject.SetActive(false);
    }

    public void ActivateHallwayObjects()
    {
        HallwayObjects.GuardGameObject.SetActive(true);
    }

    public override void OnPlayerCaughtByGuard(Player player, Guard guard)
    {
        ShowGameOverScreen();
    }

    private void ShowGameOverScreen()
    {
        Debug.Log("Showing Game Over Screen");
        if (gameOverScreenController)
        {
            gameOverScreenController.Show(() =>
            {
                Time.timeScale = 0;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            });
        }
    }
}
