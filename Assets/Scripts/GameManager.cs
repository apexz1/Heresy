using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;


public class GameManager : NetworkBehaviour
{

    Player[] players; 

    [SyncVar]
    public int speed;

    [SyncVar]
    public bool currentTurn = false;

    public bool turnId = false;

    int cardID;
    string cardName;

    void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Player connected from" + player.ipAddress + ":" + player.port);
    }

    void Start()
    {
        players = new Player[2]; 

        for (int i = 0; i < players.Length; i++)
        {
            players[i] = new Player();
        }
    }

    public int FindPlayer(short playerId)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].playerId == playerId)
                return i;
        }
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].playerId == -1)
            {
                players[i].playerId = (short)i;
                return i;
            }
        }

        return -1;
    }
    public void AssignDeck(short playerId, string deck)
    {
        Debug.Log("" + playerId + " : " + FindPlayer(playerId) +  " : " + deck);
    }
}

public class Player
{
    public short playerId = -1;
    public List<Card> deck;
    public string name;

    public void FromJSON(JSONObject jsPlayer)
    {
        name = (string)jsPlayer["Name"];
        deck = DeckBuilder.ParseDeck(jsPlayer["Deck"]);
    }

    public JSONObject ToJSON()
    {
        JSONObject jsPlayer = new JSONObject();
        jsPlayer.AddField("Name", name);

        JSONObject jsDeck = new JSONObject();
        for (int i = 0; i < deck.Count; i++)
        {
            jsDeck.Add(deck[i].GetID());
        } 

        jsPlayer.AddField("Deck", jsDeck);

        return jsPlayer; 
    }
}
