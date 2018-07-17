using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Progress : MonoBehaviour {

    public Transform LoadingBar;

    public Transform TextForHacking;

    public Transform TextLoading;

    public Transform SavePos; 



    [SerializeField] public float currentAmount;
    [SerializeField] private float speed;


    void Update () {
		if(currentAmount < 100)
        {
            currentAmount += speed * Time.deltaTime;

            TextForHacking.GetComponent<Text>().text = ((int)currentAmount).ToString() + "%";
            TextLoading.gameObject.SetActive(true);
        }
        else
        {
            TextLoading.gameObject.SetActive(false);
            TextForHacking.GetComponent<Text>().text = "Done!";
            
          //  Time.timeScale = 0.0f;
            SceneManager.LoadScene("Maze2", LoadSceneMode.Additive);

           int buildIndex = SceneManager.GetSceneByName("Maze2").buildIndex;
           SceneManager.LoadScene(buildIndex, LoadSceneMode.Additive);
            
        }

        LoadingBar.GetComponent<Image>().fillAmount = currentAmount / 100;
	}
}
