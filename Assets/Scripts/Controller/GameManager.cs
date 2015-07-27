﻿using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;


public class GameManager : MonoBehaviour {

    public Player[] players;
     
    NetworkView networkView;
    public int localPlayerId = -1;
    public int turnPlayer = 0;

    //unsynced vars; not in JSON
    public string notification;

    public static GameManager Get() {
        return GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnPlayerConnected( NetworkPlayer player ) {
        players[1].networkPlayer = player;
        Debug.Log("Player connected from" + player.ipAddress + ":" + player.port);
        StartGame(0, true);
    }

    void Start() {
        //Debug.Log(isServer + " | " + isClient + " | " + isLocalPlayer);
        players = new Player[2];

        for(int i = 0;i < players.Length;i++) {
            players[i] = new Player();
            players[i].playerId = i;
        }

        networkView = GetComponent<NetworkView>();

        LoadTextures("D:/ProtoTest/Images/");
    }

    [RPC]
    public void StartGame( int playerId, bool network ) {
        NetworkManager networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        networkManager.enabled = false;
        localPlayerId = playerId;
        turnPlayer = 0;

        if(Network.isServer) {
            this.NetRPC("StartGame", RPCMode.Others, playerId + 1, true);
            SendGameManager();
        }

        Debug.Log(turnPlayer);

        LoadDeck(localPlayerId);

        if(network == false) {
            LoadDeck(1);

            DrawCard(1);
            DrawCard(1);
            DrawCard(1);
            DrawCard(1);
            DrawCard(1);
            DrawCard(1);
            DrawCard(1);

            PlayFromHand(1, players[1].playHand[0].globalIdx, 0);
            PlayFromHand(1, players[1].playHand[0].globalIdx, 1);
            PlayFromHand(1, players[1].playHand[0].globalIdx, 2);

            MoveOnField(1, players[1].field[0].globalIdx, 5);
        }
    }

    public void LoadDeck( int playerIdx ) {
        TextAsset textFile = (TextAsset)Resources.Load("default");
        JSONObject jsPlayer = JSONParser.parse(textFile.text);

        Debug.Log("start");

        this.NetRPC("AssignDeck", RPCMode.Server, playerIdx, jsPlayer["Deck"].ToString());
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
    public void AssignDeck( int playerId, string deck ) {
        Debug.Log("AssignDeck()" + playerId + " : " + deck);

        players[playerId].deck = DeckBuilder.DeckFromJSON(JSONParser.parse(deck));
        players[playerId].BuildPlayPile();

        SendPlayer(playerId);
    }

    public void SendPlayer( int playerIndex ) {
        if(!Network.isServer) {
            Debug.LogError("Client trying to send player");
            return;
        }
        this.NetRPC("ReceivePlayer", RPCMode.All, playerIndex, players[playerIndex].ToJSON().ToString());
    }

    [RPC]
    public void ReceivePlayer( int playerIndex, string player ) {
        Debug.Log("Receive()" + playerIndex + " | " + player);
        players[playerIndex].FromJSON(JSONParser.parse(player));
    }

    [RPC]
    public void DrawCard( int playerIndex ) {
        var player = players[playerIndex];

        if(player.playPile.Count == 0)
            return;

        var card = player.playPile[player.playPile.Count - 1];
        player.playPile.RemoveAt(player.playPile.Count - 1);
        player.playHand.Add(card);

        SendPlayer(playerIndex);
    }
    [RPC]
    public void DiscardCard(int playerIndex, int cardIndex)
    {
        var player = players[playerIndex];
        int handIndex = player.FindCardHand(cardIndex);
        int fieldIndex = player.FindCardField(cardIndex);

        Debug.Log("DiscardCard() Log: " + playerIndex + " " + cardIndex + " " + handIndex + " " + fieldIndex);

        if (handIndex != -1)
        {
            var card = player.playHand[handIndex];
            player.playHand.RemoveAt(handIndex);
            player.discardPile.Add(card);
        }
        if (fieldIndex != -1)
        {
            var card = player.field[fieldIndex];
            player.field.RemoveAt(fieldIndex);
            player.discardPile.Add(card);
        }

        SendPlayer(playerIndex);
    }

    [RPC]
    public void PlayFromHand(int playerIndex, int cardIndex, int slotIndex)
    {
        var player = players[playerIndex];
        int handIndex = player.FindCardHand(cardIndex);

        if (slotIndex > 2)
        {
            SendNotification(playerIndex, "Cards can be placed in spawn slots only");
            return;
        }

        if (handIndex == -1)
        {
            Debug.LogError("Card not found");
            return;
        }

        var card = player.playHand[handIndex];

        for (int i = 0; i < player.field.Count; i++)
        {
            if (player.field[i].pos == slotIndex)
            {
                Debug.LogWarning("Slot already in use");
                SendNotification(playerIndex, "Slot already in use");
                return;
            }
        }

        player.playHand.RemoveAt(handIndex);
        player.field.Add(card);

        card.pos = slotIndex;

        SendPlayer(playerIndex);
    }
    [RPC]
    public void MoveOnField(int playerIndex, int cardIndex, int slotIndex)
    {
        var player = players[playerIndex];
        int fieldIndex = player.FindCardField(cardIndex);

        Debug.Log("MoveOnField() Log: " + cardIndex + " " + slotIndex);

        if (slotIndex <= 2)
        {
            SendNotification(playerIndex, "Cards can be moved to field slots only");
            return;
        }

        Debug.Log(fieldIndex);

        if (fieldIndex == -1)
        {
            Debug.LogError("Card not found");
            return;
        }

        var card = player.field[fieldIndex];

        for (int i = 0; i < player.field.Count; i++)
        {
            if (player.field[i].pos == slotIndex)
            {
                if (card.pos <= 2)
                {
                    SendNotification(playerIndex, "Cards can not be swapped from spawn slots");
                    return;
                }

                player.field[i].pos = card.pos;
            }
        }

        card.pos = slotIndex;
        SendPlayer(playerIndex);
    }

    [RPC]
    public void EndTurn(int playerIndex)
    {
        //check if incoming player's turn; comment for cheating
        if (playerIndex != turnPlayer)
            return;

        turnPlayer++;
        turnPlayer %= 2;
        Debug.Log(turnPlayer);
        SendGameManager();
    }

    public Texture2D LoadImage( string filePath ) {

        Texture2D tex = new Texture2D(2, 2);
        byte[] fileData;

        if(File.Exists(filePath)) {
            fileData = File.ReadAllBytes(filePath);            
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        //Debug.Log(tex.ToString());
        return tex;
    }

    public void LoadTextures(string filePath) {

        string[] fileArray = Directory.GetFiles(filePath);

        for(int i = 0; i < fileArray.Length;i++) {
            Texture2D tex = LoadImage(fileArray[i]);
            //Debug.Log(fileArray[i].ToString());

            string oldName = fileArray[i].ToString();
            string tmpName = oldName.Remove(0, oldName.Length - 7);
            string newName = tmpName.Remove(tmpName.Length - 4, 4);
            int texName;
            Int32.TryParse(newName, out texName);
            //Debug.Log(texName);

            for(int j = 0; j < CardLibrary.Get().cardList.Count; j++)
            {
                if(CardLibrary.Get().cardList[j].cardID == texName)
                {
                    //Debug.Log("name found");
                    CardLibrary.Get().cardList[j].texture = tex;
                }
            }
        }
    }
    public void FromJSON(JSONObject jsPlayer)
    {
        turnPlayer = (int)jsPlayer["TurnPlayer"];

    }

    public JSONObject ToJSON()
    {
        JSONObject jsPlayer = JSONObject.obj;
        jsPlayer.AddField("TurnPlayer", turnPlayer);

        return jsPlayer;
    }
    public void SendGameManager()
    {
        if (!Network.isServer)
        {
            Debug.LogError("Client trying to send game manager");
            return;
        }

        this.NetRPC("ReceiveGameManager", RPCMode.All, ToJSON().ToString());
    }

    [RPC]
    public void ReceiveGameManager(string manager)
    {
        Debug.Log("Receive()" + manager);
        FromJSON(JSONParser.parse(manager));
    }

    public void SendNotification(int playerIndex, string message)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (playerIndex != players[i].playerId  && playerIndex != -1)
            {
                continue;
            }
            //Debug.Log(">>>>>>>>>>>>>" + playerIndex + " " + i + " " + players[i].networkPlayer);
            this.NetRPC("ReceiveNotification", players[i].networkPlayer, message);
        }
    }
    [RPC]
    public void ReceiveNotification(string message)
    {
        notification = message;
    }
}

public class Player {
    public int playerId = -1;
    public string name;
    public List<LibraryCard> deck = new List<LibraryCard>();
    public List<PlayCard> playPile = new List<PlayCard>();
    public List<PlayCard> playHand = new List<PlayCard>();
    public List<PlayCard> discardPile = new List<PlayCard>();
    public List<PlayCard> field = new List<PlayCard>();
    public NetworkPlayer networkPlayer;
    static int globalIdx = 0;

    public void BuildPlayPile() {
        playPile.Clear();
        playHand.Clear();
        discardPile.Clear();
        field.Clear();

        for(int i = 0;i < deck.Count;i++) {
            globalIdx++;

            PlayCard playCard = new PlayCard(deck[i].cardID, globalIdx);
            playCard.InitLibrary();
            playPile.Add(playCard);
        }

        playPile.Shuffle();
    }


    public void FromJSON( JSONObject jsPlayer ) {
        playerId = (int)jsPlayer["PlayerId"];
        name = (string)jsPlayer["Name"];
        deck = DeckBuilder.DeckFromJSON(jsPlayer["Deck"]);
        playPile = PileFromJSON(jsPlayer["PlayPile"]);
        playHand = PileFromJSON(jsPlayer["PlayHand"]);
        discardPile = PileFromJSON(jsPlayer["DiscardPile"]);
        field = PileFromJSON(jsPlayer["Field"]);

    }

    public JSONObject ToJSON() {
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

    public static JSONObject PileToJSON( List<PlayCard> pile ) {
        JSONObject jsPile = JSONObject.arr;
        for(int i = 0;i < pile.Count;i++) {
            jsPile.Add(pile[i].ToJSON());
        }
        return jsPile;
    }

    public static List<PlayCard> PileFromJSON( JSONObject jsPile ) {
        List<PlayCard> pile = new List<PlayCard>();

        for(int i = 0;i < jsPile.Count;i++) {
            PlayCard card = new PlayCard();
            card.FromJSON(jsPile[i]);

            pile.Add(card);
        }
        return pile;
    }

    public static int FindCard(List<PlayCard> pile,int cardIndex)
    {
        for (int i = 0; i < pile.Count; i++)
        {
            if (pile[i].globalIdx == cardIndex)
            {
                return i;
            }
        }

        return -1;
    }
    public int FindCardPlayPile(int cardIndex) { return FindCard(playPile, cardIndex); }
    public int FindCardHand(int cardIndex) { return FindCard(playHand, cardIndex); }
    public int FindCardField(int cardIndex) { return FindCard(field, cardIndex); }
    public int FindCardDiscard(int cardIndex) { return FindCard(discardPile, cardIndex); }
}
