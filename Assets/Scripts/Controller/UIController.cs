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

        AssignCult(0);
        AssignCult(1);
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

    public void AssignCult(int playerId)
    {
        var player = GameManager.Get().players[playerId];
        //Debug.Log("cult: " + player.cult);
        var image = GameObject.Find("banner_" + playerId).GetComponent<Image>();

        var array = Resources.LoadAll("Images/UI/main/banner", typeof(Texture2D));
        var sprites = new List<Sprite>();
        var spritesRef = new List<String>();
        var current = new Sprite();
        string currentRef = "";

        //Debug.Log("files loaded " + array.Length);

        var imgArray = new Texture2D[array.Length];

        for (int i = 0; i < array.Length; i++)
        {
            imgArray[i] = array[i] as Texture2D;
        }

        for (int i = 0; i < imgArray.Length; i++)
        {
            //Debug.Log("tex name: " + imgArray[i].name);
            current = Sprite.Create(imgArray[i], new Rect(0, 0, imgArray[i].width, imgArray[i].height), new Vector2(0.5f, 0.5f));
            currentRef = imgArray[i].name;
            //Debug.Log(current);
            sprites.Add(current);
            spritesRef.Add(currentRef);
        }

        //image.sprite = sprites[0];

        for (int i = 0; i < sprites.Count; i++)
        {
            Debug.Log("name: " + spritesRef[i] + " " + "b_" + player.cult);
            if (spritesRef[i].Equals("b_" + player.cult))
            {
                image.sprite = sprites[i];
            }
        }
    }
}
