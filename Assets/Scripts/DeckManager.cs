﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;

[System.Serializable]
//DeckManager, probably have to build the fucking inventory again and modify
public class DeckManager : MonoBehaviour {

    public List<Card> deck = new List<Card>();
   
    int libCount;
    int deckCount;
    [SerializeField]
    int maxDeckCount;
    string deckName;
    string deckLocation;
    bool isNewDeck = false;
 
    CardLibrary cardLibrary;

    [NonSerialized]
    public GameObject card;
    public Button listPrefab;
    public static string listCardName;
    

    //Start 
    void Start()
    {
        //Don't know what the fuck I'm doing here, but works. #coding101
        cardLibrary = GameObject.Find("CardLibary").GetComponent<CardLibrary>();
        libCount = cardLibrary.cardList.Count;
        deckLocation = (Application.dataPath + "/Resources/");

        //Debugging, sets up deck with one copy of each card in the CardLibrary
        /*for (int i = 0; i < cardCount; i++)
        {
            deck.Add(new Card());

            //used to add Cards individually; handle through buttons via PlayerController
            //if (deck[i].GetID() == -1)
            //{
                deck[i] = cardLibrary.cardList[i];
                Debug.Log(deck[i].GetName());
            //}            
        }*/

        SpawnCard();
        Debug.Log(deck.Count);

        //SaveDeck();
        //LoadDeck();
    }

    public void AddCard(string name) {

        Debug.Log(isNewDeck);
        if(isNewDeck) 
            deckCount = 0;        

        Cultist card;

        if(deckCount == maxDeckCount)
            return;

        for(int i = 0;i < libCount;i++) {
            if(cardLibrary.cardList[i].GetName().Equals(name)) {
                isNewDeck = false;
                listCardName = cardLibrary.cardList[i].GetName();
                card = (Cultist)cardLibrary.cardList[i];
                deck.Add(card);
                deckCount++;
                ListCard(deckCount);

                Debug.Log(deck[deck.Count - 1].GetName());
                Debug.Log(card.GetID());
            }
        }          
    }

    public void RemoveCard(string name)
    {
        for (int i = 0; i < libCount; i++)
        {
            if (cardLibrary.cardList[i].GetName().Equals(name))
            {
                Debug.Log("Entry found");
                deck.Remove(cardLibrary.cardList[i]);
                Debug.Log("Removed?");
            }
        }
    }

    public void ListCard(int deckCount)
    {
        Vector2 spawnPos = new Vector2(0, 0);
        Button listCard = Instantiate(listPrefab, spawnPos, Quaternion.identity) as Button;

        listCard.transform.SetParent(GameObject.Find("List").transform, false);
        listCard.transform.localPosition = new Vector3(0, (219 - (23* (deckCount-1))), 0);
    }

    public void SpawnCard() {
        Vector3 spawnPos;
        float x = 6f;
        float y = 3.5f;
        int counter = -1;

        GameObject card;

        for(int i = 0; i < 5;i++){
            for(int j = 0;j < 17;j++) {

                counter++;

                card = (GameObject)Resources.Load("Prefabs/" + cardLibrary.cardList[counter].GetName());
                spawnPos = new Vector3(x-(i * 1.9f),y-(j* 2.5f), 0);
                GameObject cardSpawn = (GameObject)Instantiate(card, spawnPos, Quaternion.identity);

                if (counter >= (cardLibrary.cardList.Count - 1))
                {
                    counter = -1;
                }
            }
        }
    }

    public void SaveDeck(string name)
    {
        StringBuilder builder = new StringBuilder();

        Debug.Log(deckLocation);

        for(int i = 0; i < deck.Count; i++)
        {
            builder.Append(deck[i].GetID() + ",");
        }

        File.WriteAllText(deckLocation + name + ".txt", builder.ToString());
        builder.Remove(0, builder.Length);
       
        if (File.Exists(deckLocation))
        {
            Debug.Log("File saved");
        }

        isNewDeck = true;
    }

    //Not working anymore, changed SaveDeck() method
    public void LoadDeck(string name)
    {
        Cultist card;
        StringBuilder builder = new StringBuilder();
        TextAsset textFile = (TextAsset)Resources.Load(name, typeof(TextAsset));
        Debug.Log(textFile);
        builder.Append(textFile.text);

        //Uncomment to check for file content
        Debug.Log(builder);

        for (int i = 0; i < builder.Length; i++)
        {
            int id;
            string addName;

            if ((builder[i]).Equals(','))
            {
                addName = builder.ToString(i - 3, 3);
                id = Int32.Parse(builder.ToString((i - 3), 3));
                 for(int j = 0; j < libCount ; j++) 
                 {
                    if(cardLibrary.cardList[j].GetID() == id)
                    {
                        card = (Cultist)cardLibrary.cardList[j];
                        deck.Add(card);
                        deckCount++;

                        listCardName = cardLibrary.cardList[j].GetName();
                        ListCard(deckCount);
                    }
                 }
                Debug.Log(id);
            }
        }
    }

    public void DeleteDeck(string name)
    {
        File.Delete(deckLocation + name + ".txt");
    }
}
