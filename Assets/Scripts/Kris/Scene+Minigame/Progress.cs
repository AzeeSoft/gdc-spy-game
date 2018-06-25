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
            SceneManager.LoadScene(sceneName: "Minigame");
        }

        LoadingBar.GetComponent<Image>().fillAmount = currentAmount / 100;
	}
}
