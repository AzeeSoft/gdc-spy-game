using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static Stack<SceneSwitcher> SavedSceneSwitchers = new Stack<SceneSwitcher>();
    public static Dictionary<string, bool> SavedSceneHash = new Dictionary<string, bool>();

    void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Switching to SwitchSceneTest");
            SwitchScene("SwitchSceneTest", true);
        }
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

    private void LoadFromStack()
    {
        SceneSwitcher lastSceneSwitcher = SavedSceneSwitchers.Pop();
        lastSceneSwitcher.SetSceneObjectsState(true);
        CallOnSceneResumedHandlers();
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

    public void SwitchScene(string sceneName, bool saveScene)
    {
        if (saveScene)
        {
            SaveToStack();
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            asyncOperation.completed += operation =>
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            };
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void ShowLastSavedScene()
    {
        if (SavedSceneSwitchers.Count > 0)
        {
            DestroySceneObjects();
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            LoadFromStack();
        }
        else
        {
            Debug.LogWarning("No Saved Scene Found");
        }
    }
}