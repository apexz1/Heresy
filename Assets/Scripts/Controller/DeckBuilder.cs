using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;

[System.Serializable]
public class DeckBuilder : MonoBehaviour
{

    public List<LibraryCard> deck = new List<LibraryCard>();
    public List<GameObject> cards = new List<GameObject>();
    public List<Button> uiCards = new List<Button>();

    int libCount;
    [SerializeField]
    const int maxDeckCount = 30;
    const int maxCardCount = 3;
    const int maxUICount = 13;
    const int cardsCount = 77;
    string deckName;
    string deckLocation;

    int[] cultArray = new int[7];

    private Rect rect = new Rect((Screen.width - 200) / 2, (Screen.height - 50) / 2, 200, 50);
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

        LoadTextures.LoadFromFile(1);
        /*for (int i = 0; i < CardLibrary.Get().cardList.Count; i++)
        {
            Debug.Log("Texture loaded: " + CardLibrary.Get().cardList[i].texture);
        }*/

        for (int i = 0; i < cultArray.Length; i++)
        {
            cultArray[i] = 0;
        }

        SpawnCard();
        Debug.Log(deck.Count);
        Debug.Log(Application.dataPath);
    }

    public void SpawnCard()
    {
        Vector3 spawnPos;
        float x = 7f;
        float y = 3.5f;
        int counter = -1;
        int nameCounter = -1;
        int cardCounter = 0;
        GameObject card;
        for (int j = 0; j < 27; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                nameCounter++;
                counter++;

                card = (GameObject)Resources.Load("Prefabs/CardDB");
                spawnPos = new Vector3(x + (i * 2.2f), y - (j * 3.05f), 0);
                GameObject cardSpawn = (GameObject)Instantiate(card, spawnPos, Quaternion.identity);
                cardSpawn.name = "card" + nameCounter.ToString();
                //Fix card position somehow...
                cardSpawn.transform.rotation = Quaternion.EulerAngles(-Mathf.PI / 2, 0, 0);
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
                cardCounter++;
                if (cardCounter == cardsCount) { return; }
            }
        }
    }

    public void AddCard( int index )
    {

        LibraryCard card;

        Debug.Log(deck.Count);
        if (deck.Count == maxDeckCount)
            return;

        int count = 0;

        for (int i = 0; i < deck.Count; i++)
        {
            if (deck[i].cardID == cardLibrary.cardList[index].cardID)
            {
                count++;
            }
        }

        if (count >= maxCardCount)
            return;

        if (index > CardLibrary.Get().cardList.Count)
        {
            Debug.LogError("Finish Cardlist, yo");
            return;
        }


        card = cardLibrary.cardList[index];
        deck.Add(card);
        AddCardUI(card.cardID);
    }

    public void AddCardUI( int id )
    {
        //Debug.Log("AddCardUI() Log: Function entered " + id);
        Vector2 spawnPos = new Vector2(0, 0);
        Button listCard = Instantiate(listPrefab, spawnPos, Quaternion.identity) as Button;

        listCard.transform.SetParent(GetListTransform().transform, false);

        var txt = listCard.GetComponentInChildren<Text>();
        var ident = listCard.GetComponent<CardIdentity>();
        ident.id = id;
        txt.text = ident.GetName();
        var list = CardLibrary.Get().cardList;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].cardID == ident.id)
            {
                Debug.Log(ident.id);
                LibraryCard.Cult libCult = CardLibrary.Get().cardList[i].cult;
                string cult = LibraryCard.CultToString(libCult);
                listCard.GetComponent<Image>().color = CultColorUI(cult);
                break;
            }
        }

        uiCards.Add(listCard);
        MoveCardsUI();
    }
    public Color CultColorUI( string cult )
    {
        //REDO COLORS WITH RGB NUMBERS
        Color c = new Color();

        switch (cult)
        {
            case "greed":
                c = Color.yellow;
                c.a = 0.7f;
                break;

            case "envy":
                c = Color.green;
                c.a = 0.7f;
                break;

            case "wrath":
                c = Color.red;
                c.a = 0.7f;
                break;

            case "pride":
                c = Color.white;
                c.a = 0.7f;
                break;

            case "gluttony":
                c.r = 0;
                c.g = 0.478f;
                c.b = 1;
                c.a = 0.7f;
                break;

            case "lust":
                c = Color.magenta;
                c.r = 1.0f;
                c.g = 0.553f;
                c.b = 1.0f;
                c.a = 0.7f;
                break;

            case "sloth":
                c.r = 0.898f;
                c.g = 0.627f;
                c.b = 0.502f;
                c.a = 0.7f;
                break;
        }
        return c;
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

    public void RemoveCard( Button button )
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

    public string GetDeckPath( string name )
    {
        return deckLocation + "/" + name + ".json";
    }

    public int CountCults()
    {
        int max = 0;
        bool same = false;

        for (int i = 0; i < cultArray.Length; i++)
        {
            cultArray[i] = 0;
        }

        for (int i = 0; i < deck.Count; i++)
        {
            if (deck[i].cult == LibraryCard.Cult.greed) { cultArray[0]++; }
            else if (deck[i].cult == LibraryCard.Cult.envy) { cultArray[1]++; }
            else if (deck[i].cult == LibraryCard.Cult.wrath) { cultArray[2]++; }
            else if (deck[i].cult == LibraryCard.Cult.pride) { cultArray[3]++; }
            else if (deck[i].cult == LibraryCard.Cult.gluttony) { cultArray[4]++; }
            else if (deck[i].cult == LibraryCard.Cult.lust) { cultArray[5]++; }
            else if (deck[i].cult == LibraryCard.Cult.sloth) { cultArray[6]++; }
        }

        for (int i = 0; i < cultArray.Length; i++)
        {
            Debug.Log("cultarray: " + cultArray[i]);
        }

        for (int i = 0; i < cultArray.Length; i++)
        {
            if (cultArray[i] >= max)
            {
                if (cultArray[i] == max)
                {
                    same = true;
                    max = cultArray[i];
                    Debug.Log("Same " + max);
                }
                else
                {
                    same = false;
                    max = cultArray[i];
                    Debug.Log("Greater " + max);
                }
            }
            else
            {
                Debug.Log("smaller" + max);
            }
        }
        /**/

        if (same == true)
        {
            return -1;
        }

        return max;
    }

    public string AssignCult()
    {
        int max = CountCults();
        Debug.Log("max " + max);
        for (int i = 0; i < cultArray.Length; i++)
        {
            Debug.Log("array " + cultArray[i] + "count " + max + (cultArray[i] == max));
            if (cultArray[i] == max)
            {
                switch (i)
                {
                    case -1:
                        return null;
                    case 0:
                        return "greed";
                    case 1:
                        return "envy";
                    case 2:
                        return "wrath";
                    case 3:
                        return "pride";
                    case 4:
                        return "gluttony";
                    case 5:
                        return "lust";
                    case 6:
                        return "sloth";
                }
            }
        }

        return null;
    }
    public void SaveDeck( string name )
    {
        //Debug.Log(deckLocation);

        string cult = AssignCult();
        Debug.Log(cult);

        if (cult == null)
        {
            Debug.LogWarning("no cult dominant");
            return;
        }

        JSONObject jsDeck = new JSONObject();
        for (int i = 0; i < deck.Count; i++)
        {
            jsDeck.Add(deck[i].cardID);
        }

        JSONObject jsSave = new JSONObject();
        //jsSave["Deck"] = jsDeck;
        //jsSave["Cult"] = AssignCult().ToString();
        jsSave.AddField("Deck", jsDeck);
        jsSave.AddField("Cult", cult);

        if (name.Equals(""))
            name = "deck";

        File.WriteAllText(GetDeckPath(name), jsSave.ToString(), Encoding.UTF8);
        Debug.Log(name + "saved " + DeckBuilder.Get().deck.Count + " entries");

        for (int i = 0; i < cultArray.Length; i++)
        {
            cultArray[i] = 0;
        }

        if (File.Exists(deckLocation + "/"))
        {
            Debug.Log("File saved");
        }
    }

    public void LoadDeck( string name )
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

    public static List<LibraryCard> DeckFromJSON( JSONObject jsDeck )
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

    public static string CultFromJSON( JSONObject jsCult )
    {
        string cult = (string)jsCult;
        return cult;
    }

    public static JSONObject DeckToJSON( List<LibraryCard> deck )
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

    public void DeleteDeck( string name )
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
    public void ErrorWindow( int windowID )
    {
        if (GUI.Button(new Rect(5, 20, rect.width - 10, 20), "Close"))
        {
            window = false;
        }
    }

    public void OnSlider( float val )
    {
        Transform listTransform = GetListTransform();

        Vector3 pos = listTransform.localPosition;
        int diff = uiCards.Count - maxUICount;

        if (diff <= 0)
            pos.y = 0;

        else
        {
            //HEIGHT VALUE CHANGED
            pos.y = (-1 + (val * ((25 - 2) * diff)));
        }

        listTransform.localPosition = pos;
    }
}
