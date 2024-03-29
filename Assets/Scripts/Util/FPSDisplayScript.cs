﻿using UnityEngine;
using System.Collections;

public class FPSDisplayScript : MonoBehaviour
{

    float deltaTime = 0.0f;
    private static bool isOn = false;

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        if (Input.GetButtonDown("showFPS")) { Debug.LogWarning("f pressed, mate"); isOn = !isOn; }
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;


        if (isOn)
        {
            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(0, 0, w, h * 2 / 100);
            style.alignment = TextAnchor.MiddleRight;
            style.fontSize = h * 3 / 100;
            style.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            GUI.Label(rect, text, style);
        }
    }
}
