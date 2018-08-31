using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public Player PlayerObj;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    /*public void StartGameObject()
    {
        SceneManager.LoadScene("MainGameScene", LoadSceneMode.Single);
    }*/

    public void CloseGameObject()
    {
        Action quitGame = () => { Application.Quit(); };

        if (Fading.Instance)
        {
            Fading.Instance.FadeSpeed = 0.8f;
            Fading.Instance.BeginFade(Fading.FadeOut, quitGame);
        }
        else
        {
            quitGame();
        }
    }

    public void CreditsGameObject()
    {
        LoadScene("CreditsScene-Final");
    }

    public void StartTutorialObject()
    {
        Action loadTutorialScene = () => LoadScene("TutorialScene-Final");
        if (PlayerObj)
        {
            PlayerObj.Infect(75);
            StartCoroutine(WaitAndExecute(1.5f, loadTutorialScene));
        }
        else
        {
            loadTutorialScene();
        }
    }

    public void LoadScene(string name)
    {
        Action loadScene = () => { SceneManager.LoadScene(name, LoadSceneMode.Single); };

        if (Fading.Instance)
        {
            Fading.Instance.FadeSpeed = 0.8f;
            Fading.Instance.BeginFade(Fading.FadeOut, loadScene);
        }
        else
        {
            loadScene();
        }
    }

    public IEnumerator WaitAndExecute(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action();
    }
}