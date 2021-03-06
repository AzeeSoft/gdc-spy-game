﻿using System;
using System.Collections;
using System.Collections.Generic;
using Azee;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialLevelManager : LevelManager {

    [Serializable]
    public struct HallwayObjectsInfo
    {
        public GameObject GuardGameObject;
    }

    [Serializable]
    public struct SecondRoomObjectsInfo
    {
        public GameObject GuardGameObject;
    }

    public HallwayObjectsInfo HallwayObjects;
    public SecondRoomObjectsInfo SecondRoomObjects;

    public GameOverScreenController gameOverScreenController;

	// Use this for initialization
	new void Start ()
	{
	    base.Start();
        DeactivateHallwayObjects();
        DeactivateSecondRoomObjects();

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

    public void DeactivateSecondRoomObjects()
    {
        SecondRoomObjects.GuardGameObject.SetActive(false);
    }

    public void ActivateSecondRoomObjects()
    {
        SecondRoomObjects.GuardGameObject.SetActive(true);
    }

    public override void OnPlayerInfected(Player player)
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

                StaticTools.UpdateCursorLock(false);
            });
        }
    }

    public void ReturnToMainMenu()
    {
        Action goToStartScreen = () =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("StartScreen-Final");
        };

        if (Fading.Instance)
        {
            Fading.Instance.FadeSpeed = 0.8f;
            Fading.Instance.BeginFade(Fading.FadeOut, goToStartScreen);
        }
        else
        {
            goToStartScreen();
        }
    }
}
