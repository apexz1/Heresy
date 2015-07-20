using UnityEngine;
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
    public List<Button> uiCards = new List<Button>();
   
    int libCount;
    int deckCount;
    [SerializeField]
    int maxDeckCount;
    string deckName;
    string deckLocation;
    bool isNewDeck = false;

    private Rect rect = new Rect((Screen.width - 200)/2, (Screen.height - 50)/2, 200, 50);
    bool window;

    public CardLibrary cardLibrary
    {
        get { return CardLibrary.Get(); }
    }

    [NonSerialized]
    public GameObject card;
    public Button listPrefab;
    public static string listCardName;
    

    //Start 
    void Start()
    {
        //Don't know what the fuck I'm doing here, but works. #coding101
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

    public void OnGUI()
    {
        if (window)
        {
            rect = GUI.Window(0, rect, ErrorWindow, "Error: File not found");
        }
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

    public void RemoveCard(Button button)
    {
       /* for (int i = 0; i < libCount; i++)
        {
            if (cardLibrary.cardList[i].GetName().Equals(name))
            {
                int countStorage;
                var card = cardLibrary.cardList[i];

                Debug.Log("Entry found");
                deck.Remove(card);
                countStorage = deckCount -1;
                deckCount = 0;
                Debug.Log(deckCount);

                //var objects = GameObject.FindGameObjectsWithTag("Destroy");
                //foreach (GameObject o in objects)
                //{
                //    Destroy(card.gameObject);
                //}

                for (int j = 0; j < countStorage; j++)
                {
                    Debug.Log("-");
                    deckCount++;    
                    ListCard(deckCount);

                }
                //AssetDatabase.Refresh();

                deckCount = countStorage;
                Debug.Log("Removed?");
            }
        }*/


    }

    public void ClearDeck()
    {
        deck.Clear();

        var objects = GameObject.FindGameObjectsWithTag("Destroy");
        foreach (GameObject o in objects)
        {
            Destroy(o.gameObject);
        }

        deckCount = 0;
    }

    public void ListCard(int deckCount)
    {
        Vector2 spawnPos = new Vector2(0, 0);
        Button listCard = Instantiate(listPrefab, spawnPos, Quaternion.identity) as Button;

        listCard.transform.SetParent(GameObject.FindWithTag("ListTransform").transform, false);
        var locPos = listCard.transform.localPosition;
        var h = listCard.GetComponent<RectTransform>().rect.height;
        locPos.y = -(deckCount * (h-2)) + (h/2);

        listCard.transform.localPosition = locPos;

        var txt = listCard.GetComponentInChildren<Text>();
        //txt.text = DeckManager.listCardName;
        var ident=listCard.GetComponent<CardIdentity>();
        ident.id = cardLibrary.GetCard(DeckManager.listCardName).GetID();
        txt.text = ident.GetName();

        uiCards.Add(listCard);
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

        if (name.Equals(""))
            name = "deck";

        File.WriteAllText(deckLocation + name + ".txt", builder.ToString());
        builder.Remove(0, builder.Length);
       
        if (File.Exists(deckLocation))
        {
            Debug.Log("File saved");
        }

        isNewDeck = true;
    }

    //Not quite working, better though
    public void LoadDeck(string name)
    {
        Debug.Log(deckLocation + name + ".txt");
        if (!File.Exists(deckLocation + name + ".txt"))
        {
            window = true;
            return;
        }

        Cultist card;
        StringBuilder builder = new StringBuilder();


        Debug.Log(name);
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
                for (int j = 0; j < libCount; j++)
                {
                    if (cardLibrary.cardList[j].GetID() == id)
                    {
                        deckCount++;

                        card = (Cultist)cardLibrary.cardList[j];
                        listCardName = cardLibrary.cardList[j].GetName();
                        ListCard(deckCount);
                        deck.Add(card);
                    }
                }
                Debug.Log(id);
            }
        }
        Debug.Log(deck.Count + " entries loaded");
    }

    public void DeleteDeck(string name)
    {
        File.Delete(deckLocation + name + ".txt");
    }

    void ErrorWindow(int windowID)
    {
        if (GUI.Button(new Rect(5,20, rect.width -10, 20), "Close"))
        {
            window = false;
        }
    }
}
