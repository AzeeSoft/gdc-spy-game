using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDController : MonoBehaviour {

    [Serializable]
    public class UIElementsModel
    {
        [CanBeNull] public Image XRayUI;
        [CanBeNull] public Image HealthUI;
        [CanBeNull] public Image StunBarUI;
    }

    public UIElementsModel UIElements;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
