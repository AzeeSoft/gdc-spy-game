using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMapD : MonoBehaviour {

    public GameObject miniMap;

    void Update()
    {
        if(miniMap.activeInHierarchy == true)
        {
            if (Input.GetKeyDown("l"))
            {
                miniMap.SetActive(false);
            }
        }

        else if(miniMap.activeInHierarchy == false)
        {
            if (Input.GetKeyDown("l"))
            {
                miniMap.SetActive(true);
            }
        }
        
    }
}
