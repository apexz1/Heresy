﻿using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;


//DeckManager, probably have to build the fucking inventory again and modify
public class DeckManager : MonoBehaviour {

    public List<Card> deck = new List<Card>();
   
    int cardCount;
    string fileLocation;
 
    CardLibrary cardLibary;


    //Start 
    void Start()
    {
        //Don't know what the fuck I'm doing here, but works. #coding101
        cardLibary = GameObject.Find("CardLibary").GetComponent<CardLibrary>();
        cardCount = cardLibary.cardList.Count;

        //Sets up the deck with empties to ensure decks being of known size and size being usable
        for (int i = 0; i < cardCount; i++)
        {
            deck.Add(new Card());

            //used to add Cards individually; handle through buttons via PlayerController
            if (deck[i].GetID() == -1)
            {
                //Debug.Log(deck[i]);
                deck[i] = cardLibary.cardList[i];
            }
        }

        SaveDeck();
        LoadDeck();
    }


    public void SaveDeck()
    {
        StringBuilder builder = new StringBuilder();

        fileLocation = ("D:/ProtoTest/Assets/Resources/deck.txt");
        Debug.Log(fileLocation);

        for(int i = 0; i < deck.Count; i++)
        {
            builder.Append(deck[i].GetID() + ",");
            builder.Append(deck[i].GetName() + ",");
            builder.Append(deck[i].GetTexture() + ",");
            builder.Append(";");
        }

        File.WriteAllText(fileLocation, builder.ToString());
        builder.Remove(0, builder.Length);
       
        if (File.Exists(fileLocation))
        {
            Debug.Log("File saved");
        }
    }

    public void LoadDeck()
    {
        StringBuilder builder = new StringBuilder();
        TextAsset textFile = (TextAsset)Resources.Load("deck", typeof(TextAsset));
        builder.Append(textFile.text);

        int index = 0;
        int start = 0;
        int counter = 0;
        //Uncomment to check for file content
        //Debug.Log(builder);

        for(int i = 0; i < builder.Length; i++)
        {
            index++;

            string id = "";
            string name = "";
            string textureID = "";

            if ((builder[i]).Equals(','))
            {
                //Debug.Log(counter);

                if (counter == 0)
                {
                    id = builder.ToString(start, index);
                    //Debug.Log("1");
                }

                if (counter == 1)
                {
                    name = builder.ToString(start, index);
                    //Debug.Log("2");
                }

                if (counter == 2)
                {
                    textureID = builder.ToString(start, index);
                    //Debug.Log("3");
                }

                counter++;
                start = index;
                //Debug.Log(",");
            }

            if ((builder[i]).Equals(';'))
            {
                builder.Remove(0, index);

                counter = 0;
                start = 0;
                index = 0;
                //Debug.Log("end");
                //Debug.Log(";");
            }
        }

    }
}
