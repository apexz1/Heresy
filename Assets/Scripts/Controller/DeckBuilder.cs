﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;

[System.Serializable]
public class DeckBuilder : MonoBehaviour {

    public List<LibraryCard> deck = new List<LibraryCard>();
    public List<GameObject> cards = new List<GameObject>();
    public List<Button> uiCards = new List<Button>();
   
    int libCount;
    [SerializeField]
    const int maxDeckCount = 30; 
    const int maxUICount = 13;
    string deckName;
    string deckLocation;

    private Rect rect = new Rect((Screen.width - 200)/2, (Screen.height - 50)/2, 200, 50);
    bool window;

    public CardLibrary cardLibrary
    {
        get { return CardLibrary.Get(); }
    }

    public static DeckBuilder Get()
    {
        return GameObject.Find("DeckBuilder").GetComponent<DeckBuilder>();
    }

    public Button listPrefab;
    GameManager gameManager;

    //Don't know what the fuck I'm doing anymore, but works. #coding101
    void Start()
    {
        libCount = cardLibrary.cardList.Count;
        deckLocation = SaveGameLocation.getSaveGameDirectory() + "/Heresy";
        gameManager = GameManager.Get();
        Directory.CreateDirectory(deckLocation);

        LoadTextures.LoadFromFile(1, "D:/ProtoTest/Images/preview/");
        for (int i = 0; i < CardLibrary.Get().cardList.Count; i++)
        {
            Debug.Log("Texture loaded: " + CardLibrary.Get().cardList[i].texture);
        }

        SpawnCard();
        Debug.Log(deck.Count);
        Debug.Log(Application.dataPath);
    }

    public void SpawnCard()
    {
        Vector3 spawnPos;
        float x = 6f;
        float y = 3.5f;
        int counter = -1;
        int nameCounter = -1;

        GameObject card;

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 22; j++)
            {
                nameCounter++;
                counter++;

                card = (GameObject)Resources.Load("Prefabs/CardDB");
                spawnPos = new Vector3(x - (i * 2.4f), y - (j * 3.5f), 0);
                GameObject cardSpawn = (GameObject)Instantiate(card, spawnPos, Quaternion.identity);
                cardSpawn.name = "card" + nameCounter.ToString();
                //Fix card position somehow...
                cardSpawn.transform.rotation = Quaternion.EulerAngles(-Mathf.PI/2, 0, 0);
                //cardSpawn.transform.localScale = new Vector3(1, 1, 1);
                cardSpawn.transform.gameObject.AddComponent<CardControllerDB>();
                cards.Add(cardSpawn);

                var rend = cardSpawn.transform.FindChild("GFX").GetComponent<MeshRenderer>();

                for (int k = 0; k < cards.Count; k++)
                {
                    rend.material.mainTexture = CardLibrary.Get().cardList[k].texture_p;
                }

                if (counter >= (cardLibrary.cardList.Count - 1))
                {
                    counter = -1;
                }
            }
        }
    }

    public void AddCard(int index) {

        LibraryCard card;

        Debug.Log(deck.Count);
        if(deck.Count == maxDeckCount)
            return;
        if (index > CardLibrary.Get().cardList.Count)
        {
            Debug.LogError("Finish Cardlist, yo");
            return;
        }


        Debug.Log("AddCard() Log: if statement passed, proceeding to add card");
        card = cardLibrary.cardList[index];
        Debug.Log(card);
        deck.Add(card);
        AddCardUI(card.cardID);

                //Debug.Log(deck[deck.Count - 1].GetName());
                //Debug.Log(card.GetID());
    }

    public void AddCardUI(int id)
    {
        Debug.Log("AddCardUI() Log: Function entered " + id);
        Vector2 spawnPos = new Vector2(0, 0);
        Button listCard = Instantiate(listPrefab, spawnPos, Quaternion.identity) as Button;

        listCard.transform.SetParent(GetListTransform().transform, false);

        var txt = listCard.GetComponentInChildren<Text>();
        var ident = listCard.GetComponent<CardIdentity>();
        ident.id = id;
        txt.text = ident.GetName();

        uiCards.Add(listCard);
        MoveCardsUI();
    }

    private static Transform GetListTransform()
    {
        return GameObject.Find("ListTransform").transform;
    }

    public void MoveCardsUI()
    {
        for (int i = 0; i < uiCards.Count; i++)
        {
            var listCard = uiCards[i];
            var locPos = listCard.transform.localPosition;
            var h = listCard.GetComponent<RectTransform>().rect.height;
            locPos.y = -(i * (h - 2)) - (h / 2);

            listCard.transform.localPosition = locPos;
        }
    }

    public void RemoveCard(Button button)
    {
        int cardIndex = -1;

        for (int i = 0; i < uiCards.Count; i++)
        {
            if (uiCards[i] == button)
            {
                cardIndex = i;
                break;
            }
        }

        //Debug.Log(cardIndex);

        deck.RemoveAt(cardIndex);
        uiCards.RemoveAt(cardIndex);

        Destroy(button.gameObject);
        MoveCardsUI();

    }

    public void ClearDeck()
    {
        deck.Clear();
        uiCards.Clear();

        var objects = GameObject.FindGameObjectsWithTag("Destroy");
        foreach (GameObject o in objects)
        {
            Destroy(o.gameObject);
        }
    }

    public string GetDeckPath(string name)
    {
        return deckLocation + "/" + name + ".json";
    }
    public void SaveDeck(string name)
    {

        //Debug.Log(deckLocation);

        JSONObject jsDeck = new JSONObject();
        for (int i = 0; i < deck.Count; i++)
        {
            jsDeck.Add(deck[i].cardID);
        }

        JSONObject jsSave = new JSONObject();
        jsSave["Deck"] = jsDeck;

        if (name.Equals(""))
            name = "deck";

        File.WriteAllText(GetDeckPath(name), jsSave.ToString(), Encoding.UTF8);
       
        if (File.Exists(deckLocation + "/"))
        {
            Debug.Log("File saved");
        }
    }

    public void LoadDeck(string name)
    {
        //Debug.Log(deckLocation + "/" + name + ".json");
        if (!File.Exists(deckLocation + "/" + name + ".json"))
        {
            window = true;
            return;
        }

        string textFile = File.ReadAllText(GetDeckPath(name), Encoding.UTF8);
        JSONObject jsSave = JSONParser.parse(textFile);

        //Debug.Log(name);
        //Debug.Log(jsSave);

        JSONObject jsDeck = jsSave["Deck"];

        deck = DeckFromJSON(jsDeck);

        CreateAllCardsUI();
    }
    
    public static List<LibraryCard> DeckFromJSON(JSONObject jsDeck)
    {
        List<LibraryCard> deck = new List<LibraryCard>();

        for (int i = 0; i < jsDeck.Count; i++)
        {
            int id = (int)jsDeck[i];
            LibraryCard card = CardLibrary.Get().GetCard(id);

            deck.Add(card);
        }
        return deck;
    }

    public static JSONObject DeckToJSON(List<LibraryCard> deck)
    {
        JSONObject jsDeck = JSONObject.arr;
        for (int i = 0; i < deck.Count; i++)
        {
            jsDeck.Add(deck[i].cardID);
        }
        return jsDeck;
    }

    public void CreateAllCardsUI()
    {
        for (int i = 0; i < deck.Count; i++)
            AddCardUI(deck[i].cardID);
    }

    public void DeleteDeck(string name)
    {
        File.Delete(deckLocation + name + ".txt");
    }

    public void OnGUI()
    {
        if (window)
        {
            rect = GUI.Window(0, rect, ErrorWindow, "Error: File not found");
        }
    }
    public void ErrorWindow(int windowID)
    {
        if (GUI.Button(new Rect(5,20, rect.width -10, 20), "Close"))
        {
            window = false;
        }
    }
    
    public void OnSlider(float val)
    {
        Transform listTransform = GetListTransform();

        Vector3 pos = listTransform.localPosition;
        int diff = uiCards.Count - maxUICount;

        if (diff <= 0)
            pos.y = 0;

        else
        {
            //HEIGHT VALUE CHANGED
            pos.y = (-1 + (val * ((25-2)*diff)));
        }

        listTransform.localPosition = pos;
    }
}
