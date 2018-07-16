using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void SaveToStack()
    {
        EnableSceneObjects(false);
        SavedSceneSwitchers.Push(this);
    }

    private void EnableSceneObjects(bool enable)
    {
        gameObject.SetActive(enable);
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
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
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

            SceneSwitcher lastSceneSwitcher = SavedSceneSwitchers.Pop();
            lastSceneSwitcher.EnableSceneObjects(true);
        }
        else
        {
            Debug.LogWarning("No Saved Scene Found");
        }
    }
}