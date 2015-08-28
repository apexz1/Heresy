using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{

    Image[] images = new Image[6];

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i] = GameObject.Find("HUD").transform.FindChild("uiS_0" + (i + 1)).gameObject.GetComponent<Image>();
            //Debug.Log("image test " + images[i].name);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        SpawnBlend();
    }

    public void SpawnBlend()
    {
        var player = GameManager.Get().players[GameManager.Get().localPlayerId];
        if (player.spawns > 3) { Debug.LogWarning("cheat mode, dude"); return; }

        var g = GameManager.Get();


        Color color = new Color();
        color.a = 1f;
        color.b = 0.5f;
        color.g = 0.5f;
        color.r = 0.5f;

        Color lerped = Color.Lerp(Color.white, color, Time.time * 0.5f);


        for (int i = 0; i < images.Length; i++)
        {
            //images[i].color = lerped;
            images[i].color = color;
        }

        SpawnColor(0);
        SpawnColor(1);

        /*
        Debug.Log("player spawns " + player.spawns);

        for (int i = 0; i < player.spawns; i++)
        {
            //Debug.Log(images[i + (player.playerId * 3)].color);
            images[i + (player.playerId * 3)].color = Color.clear;
        }
        /**/
    }

    public void SpawnColor( int playerIndex )
    {
        var players = GameManager.Get().players;

        for (int i = 0; i < players[playerIndex].spawns; i++)
        {
            //Debug.Log(images[i + (player.playerId * 3)].color);
            images[i + (players[playerIndex].playerId * 3)].color = Color.white;
        }

    }
}
