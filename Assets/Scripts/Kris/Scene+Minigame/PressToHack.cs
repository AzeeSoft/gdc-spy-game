using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressToHack : MonoBehaviour
{
    public GameObject objToActivate;

    bool canActivate = false;

    public Transform progressBar;

    void OnTriggerEnter(Collider other)
    {
        canActivate = true;
    }

    private void OnTriggerExit(Collider other)
    {
        canActivate = false;
    }

    void Update()
    {
        if(canActivate == true)
        {
            if (Input.GetKeyDown("f"))
            {
                objToActivate.SetActive(true);
            }

            else if(Input.GetKeyUp("f"))
            {
                objToActivate.SetActive(false);
                progressBar.GetComponent<Progress>().currentAmount = 0;
            }
        }
        
    }

}
