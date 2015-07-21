﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;


public class GameManager : MonoBehaviour
{

    Player[] players;
    NetworkView networkView;
    int localPlayerId=-1;

    void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Player connected from" + player.ipAddress + ":" + player.port);
        StartGame(0);
    }

    void Start()
    {
        //Debug.Log(isServer + " | " + isClient + " | " + isLocalPlayer);
        players = new Player[2]; 

        for (int i = 0; i < players.Length; i++)
        {
            players[i] = new Player();
            players[i].playerId = i;
        }

        networkView = GetComponent<NetworkView>();
    }

    [RPC]
    public void StartGame(int playerId)
    {
        NetworkManager networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        networkManager.enabled = false;
        localPlayerId = playerId;

        if (Network.isServer)
        {
            this.NetRPC("StartGame", RPCMode.Others, playerId + 1);
        };
        LoadDeck();
    }

    public void LoadDeck()
    {
        TextAsset textFile = (TextAsset)Resources.Load("default");
        JSONObject jsPlayer = JSONParser.parse(textFile.text);

        Debug.Log("start");

        this.NetRPC("AssignDeck", RPCMode.Server, localPlayerId, jsPlayer["Deck"].ToString());
    }



   /* public int FindPlayer(short playerId)
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
    }*/
    [RPC]
    public void AssignDeck(int playerId, string deck)
    {
        Debug.Log("AssignDeck()" + playerId +  " : " + deck);

        players[playerId].deck = DeckBuilder.DeckFromJSON(JSONParser.parse(deck));
        players[playerId].BuildPlayPile();

        SendPlayer(playerId);
    }

    public void SendPlayer(int playerIndex)
    {
        if (!Network.isServer)
        {
            Debug.LogError("Client trying to send player");
            return;
        }
        this.NetRPC("RpcReceivePlayer", RPCMode.All, playerIndex, players[playerIndex].ToJSON().ToString());
    }

    [RPC]
    public void RpcReceivePlayer(int playerIndex, string player)
    {
        Debug.Log("Receive()" + playerIndex + " | " + player);
        players[playerIndex].FromJSON(JSONParser.parse(player));
    }
}

public class Player
{
    public int playerId = -1;
    public string name;
    public List<LibraryCard> deck;
    public List<PlayCard> playPile;
    public List<PlayCard> playHand;
    public List<PlayCard> discardPile;
    public List<PlayCard> field;

    public void BuildPlayPile()
    {
        playPile.Clear();
        playHand.Clear();
        discardPile.Clear();
        field.Clear();

        for (int i = 0; i < deck.Count; i++)
        {
            playPile.Add(new PlayCard(deck[i].cardID));
        }

        playPile.Shuffle();
    }


    public void FromJSON(JSONObject jsPlayer)
    {
        playerId = (int)jsPlayer["PlayerId"];
        name = (string)jsPlayer["Name"];
        deck = DeckBuilder.DeckFromJSON(jsPlayer["Deck"]);
        playPile = PileFromJSON(jsPlayer["PlayPile"]);
        playHand = PileFromJSON(jsPlayer["PlayHand"]);
        discardPile = PileFromJSON(jsPlayer["DiscardPile"]);
        field = PileFromJSON(jsPlayer["Field"]);

    }

    public JSONObject ToJSON()
    {
        JSONObject jsPlayer = JSONObject.obj;
        jsPlayer.AddField("Name", name);
        jsPlayer.AddField("PlayerId", playerId);
        jsPlayer.AddField("Deck", DeckBuilder.DeckToJSON(deck));
        jsPlayer.AddField("PlayPile", PileToJSON(playPile));
        jsPlayer.AddField("PlayHand", PileToJSON(playHand));
        jsPlayer.AddField("DiscardPile", PileToJSON(discardPile));
        jsPlayer.AddField("Field", PileToJSON(field));

        return jsPlayer; 
    }

    public static JSONObject PileToJSON(List<PlayCard> pile)
    {
        JSONObject jsPile = JSONObject.arr;
        for (int i = 0; i < pile.Count; i++)
        {
            jsPile.Add(pile[i].ToJSON());
        }
        return jsPile;
    }

    public static List<PlayCard> PileFromJSON(JSONObject jsPile)
    {
        List<PlayCard> pile = new List<PlayCard>();

        for (int i = 0; i < jsPile.Count; i++)
        {
            PlayCard card = new PlayCard();
            card.FromJSON(jsPile[i]);

            pile.Add(card);
        }
        return pile;
    }
}
