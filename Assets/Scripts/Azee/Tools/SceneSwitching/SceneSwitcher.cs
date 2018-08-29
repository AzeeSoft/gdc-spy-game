using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class SceneSwitcher : MonoBehaviour
{
    [Serializable]
    public class SceneSwitcherEvent : UnityEvent<object>
    {

    }

    public static Stack<SceneSwitcher> SavedSceneSwitchers = new Stack<SceneSwitcher>();
    public static Dictionary<string, bool> SavedSceneHash = new Dictionary<string, bool>();

    public SceneSwitcherEvent OnSceneData;
    public SceneSwitcherEvent OnSceneResult;

    void Awake()
    {
        foreach (Animator animator in FindObjectsOfType<Animator>())
        {
            GameObject animGameObject = animator.gameObject;
            if (animGameObject.GetComponent<SceneSwitchAnimatorHandler>() == null)
            {
                SceneSwitchAnimatorHandler animatorHandler = animGameObject.AddComponent<SceneSwitchAnimatorHandler>();
                animatorHandler.enabled = true;
            }
        }

        foreach (AudioSource audioSource in FindObjectsOfType<AudioSource>())
        {
            GameObject audioSourceGameObject = audioSource.gameObject;
            if (audioSourceGameObject.GetComponent<SceneSwitchAudioSourceHandler>() == null)
            {
                SceneSwitchAudioSourceHandler audioSourceHandler = audioSourceGameObject.AddComponent<SceneSwitchAudioSourceHandler>();
                audioSourceHandler.enabled = true;
            }
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Tab))
        {
//            Debug.Log("Switching to SwitchSceneTest");
            SwitchScene("SwitchSceneTest", true);
        }*/
    }

    public void CallOnScenePausedHandlers()
    {
        foreach (SceneSwitchHandler sceneSwitchHandler in FindObjectsOfType<SceneSwitchHandler>())
        {
            sceneSwitchHandler.OnScenePaused.Invoke();
        }
    }

    public void CallOnSceneResumedHandlers()
    {
        foreach (SceneSwitchHandler sceneSwitchHandler in FindObjectsOfType<SceneSwitchHandler>())
        {
            sceneSwitchHandler.OnSceneResumed.Invoke();
        }
    }

    private void SaveToStack()
    {
        CallOnScenePausedHandlers();
        SetSceneObjectsState(false);
        SavedSceneSwitchers.Push(this);
    }

    private void LoadFromStack(object data)
    {
        SceneSwitcher lastSceneSwitcher = SavedSceneSwitchers.Pop();
        lastSceneSwitcher.SetSceneObjectsState(true);
        lastSceneSwitcher.CallOnSceneResumedHandlers();
        lastSceneSwitcher.OnSceneResult.Invoke(data);
    }

    private void SetSceneObjectsState(bool enable)
    {
        gameObject.SetActive(enable);
//        Time.timeScale = enable ? 1 : 0;
    }

    private void DestroySceneObjects()
    {
        Destroy(gameObject);
    }

    public void SwitchScene(string sceneName, bool saveScene, object data)
    {
        if (saveScene)
        {
            SaveToStack();
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            asyncOperation.completed += operation =>
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
                PassDataToCurrentSceneSwitcher(data);
            };
        }
        else
        {
            SceneManager.LoadScene(sceneName);
            PassDataToCurrentSceneSwitcher(data);
        }
    }

    private void PassDataToCurrentSceneSwitcher(object data)
    {
        foreach (GameObject gameObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            SceneSwitcher sceneSwitcher = gameObject.GetComponent<SceneSwitcher>();
            if (sceneSwitcher != null)
            {
                sceneSwitcher.OnSceneData.Invoke(data);
            }
        }
    }

    public void ShowLastSavedScene(object data)
    {
        if (SavedSceneSwitchers.Count > 0)
        {
            DestroySceneObjects();
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            LoadFromStack(data);
        }
        else
        {
            Debug.LogWarning("No Saved Scene Found");
        }
    }
}