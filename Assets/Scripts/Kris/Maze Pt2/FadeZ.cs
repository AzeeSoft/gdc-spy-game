using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeZ : MonoBehaviour
{

    Image backgroundPanel;

    Color bColor;

    public float speed;

    private int redCounter = 0;

    private void Awake()
    {
        backgroundPanel = GetComponent<Image>();

        bColor = backgroundPanel.color;
    }

    void Start()
    {
        bColor.a = 0;
        backgroundPanel.color = bColor;
    }

    public void OnHitColor()
    {
        bColor.a = 100;

        backgroundPanel.color = bColor;

        while (bColor.a > 0)
        {
            bColor.a -= speed;
            backgroundPanel.color = bColor;
        }
    }

    private void Update()
    {
        if(Input.GetKey("g"))
        {
            bColor.a = 100;

            backgroundPanel.color = bColor;

            redCounter = 1;



        }
    }
}