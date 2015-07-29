using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;


public class GameManager : MonoBehaviour {

    //unsynced vars; not in JSON
    NetworkView networkView;
    public int localPlayerId = -1;
    public string notification;

    //synced vars
    public Player[] players;
    public int turnPlayer = 0;
    public List<PlayCard> playCards = new List<PlayCard>();



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

        LoadTextures.LoadFromFile("D:/ProtoTest/Images/");
    }

    [RPC]
    public void StartGame( int playerId, bool network ) {
        NetworkManager networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        networkManager.enabled = false;
        localPlayerId = playerId;
        turnPlayer = 0;

        for (int i = 0; i < players.Length; i++)
        {
            players[i].playerHealth = 20;
        }

        if(Network.isServer) {
            this.NetRPC("StartGame", RPCMode.Others, playerId + 1, true);
            SendGameManager();
        }

        Debug.Log(turnPlayer);

        LoadDeck(localPlayerId);

        if(network == false) {
            LoadDeck(1);

            //Automated player opening
            DrawCard(0);
            DrawCard(0);
            DrawCard(0);
            DrawCard(0);
            DrawCard(0);
            DrawCard(0);
            DrawCard(0);
            DrawCard(0);
            DrawCard(0);
            DrawCard(0);

            PlayFromHand(0, 0, 0);
            PlayFromHand(0, 1, 1);
            PlayFromHand(0, 2, 2);

            //MoveOnField(0, players[0].field[0].globalIdx, 5);

            //Automated opponent opening
            DrawCard(1);
            DrawCard(1);
            DrawCard(1);
            DrawCard(1);
            DrawCard(1);
            DrawCard(1);
            DrawCard(1);
            DrawCard(1);
            DrawCard(1);
            DrawCard(1);

            PlayFromHand(1, 10, 0);
            PlayFromHand(1, 11, 1);
            PlayFromHand(1, 12, 2);

            //MoveOnField(1, players[1].field[0].globalIdx, 5);
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

        SendGameManager();
    }

    /*public void SendPlayer( int playerIndex ) {
        if(!Network.isServer) {
            Debug.LogError("Client trying to send player");
            return;
        }
        this.NetRPC("ReceivePlayer", RPCMode.All, playerIndex, players[playerIndex].ToJSON().ToString());
    }*/

    [RPC]
    public void DamagePlayer(int playerIndex, int amount)
    {
        var player = players[playerIndex];
        player.playerHealth -= amount;
        SendGameManager();
    }

    [RPC]
    public void ReceivePlayer( int playerIndex, string player ) {
        Debug.Log("Receive()" + playerIndex + " | " + player);
        players[playerIndex].FromJSON(JSONParser.parse(player));
    }

    [RPC]
    public void DrawCard( int playerIndex ) {
        var player = players[playerIndex];
        PlayCard drawCard = null;

        if (CountCards(playerIndex, PlayCard.Pile.hand) > 10) { return; }

        for (int i = 0; i < playCards.Count; i++)
        {
            var card = playCards[i];
            if (card.owner != playerIndex) { continue; }
            if (card.pile != PlayCard.Pile.deck) { continue; }
            drawCard = card;
        }

        if (drawCard == null) { return; }

        drawCard.pile = PlayCard.Pile.hand;
        drawCard.pos = CountCards(playerIndex, drawCard.pile)-1;

        SendGameManager();
    }

    public int CountCards (int playerIndex, PlayCard.Pile pile)
    {
        int counter = 0; 
        for (int i = 0; i < playCards.Count; i++)
        {
            var card = playCards[i];
            if (card.owner != playerIndex) continue;
            if (card.pile != pile) continue;
            counter++;
        }

        return counter;
    }
    public int CardAtSlot(int slotIndex)
    {
        for (int i = 0; i < playCards.Count; i++)
        {
            var card = playCards[i];
            if (card.pile != PlayCard.Pile.field) continue;
            if (card.pos != slotIndex) continue;

            return i;
        }

        return -1;
    }
    [RPC]
    public void DiscardCard(int playerIndex, int cardIndex)
    {
        var player = players[playerIndex];
        Debug.Log("DiscardCard() Log: " + playerIndex + " " + cardIndex);
        playCards[cardIndex].pile = PlayCard.Pile.discard;
        SendGameManager();
    }

    [RPC]
    public void PlayFromHand(int playerIndex, int cardIndex, int slotIndex)
    {
        var player = players[playerIndex];

        if ((slotIndex > 2 && playerIndex == 1) || (slotIndex < 18 && playerIndex == 0))
        {
            SendNotification(playerIndex, "Cards can be placed in spawn slots only");
            return;
        }

        if (CardAtSlot(slotIndex) != -1)
        {
            SendNotification(playerIndex, "Slot already in use");
            return;
        }

        var card = playCards[cardIndex];

        card.pile = PlayCard.Pile.field;        
        card.pos = slotIndex;

        SendGameManager();
    }
    [RPC]
    public void ActionFoF(int playerIndex, int ownCardIndex, int oppCardIndex)
    {
        var player = players[playerIndex];
        var opponent = players[(playerIndex+1)%2];

        var ownCard = playCards[ownCardIndex];
        var ownLibCard = CardLibrary.Get().GetCard(ownCard.libId);
        var oppCard = playCards[oppCardIndex];
        var oppLibCard = CardLibrary.Get().GetCard(oppCard.libId);

        if (ownCard.tap > 0)
        {
            SendNotification(playerIndex, "Card is tapped");
            return;
        }

        if (ownLibCard.attack == 0)
        {
            Debug.Log("No attack value assigned");
            return;
        }

        oppCard.health -= ownLibCard.attack;
        ownCard.health -= oppLibCard.attack;

        ownCard.tap = 1;

        if (oppCard.health <= 0)
        {
            DiscardCard(opponent.playerId, oppCard.globalIdx);
        }

        SendGameManager();
    }

    [RPC]
    public void MoveOnField(int playerIndex, int cardIndex, int slotIndex)
    {
        var player = players[playerIndex];

        Debug.Log("MoveOnField() Log: " + cardIndex + " " + slotIndex);

        if (slotIndex <= 2 && slotIndex >= 18)
        {
            SendNotification(playerIndex, "Cards can be moved to field slots only");
            return;
        }

        var card = playCards[cardIndex];

        if (card.tap > 0)
        {
            SendNotification(playerIndex, "Card is tapped");
            return;
        }

        //Uncomment for no swapping
        /*if (CardAtSlot(slotIndex) != -1)
        {
            SendNotification(playerIndex, "Slot already in use");
            return;
        }*/

        int swapIndex = CardAtSlot(slotIndex);
        if (swapIndex != -1)
        {
            var swapCard = playCards[swapIndex];

            if (swapCard.pos <= 2 && swapCard.pos >= 18)
            {
                SendNotification(playerIndex, "Cards can not be swapped from spawn slots");
                return;
            }

            swapCard.pos = card.pos;
        }

        card.pos = slotIndex;
        card.tap = 1;
        SendGameManager();
    }

    [RPC]
    public void EndTurn(int playerIndex)
    {
        //check if incoming player's turn; comment for cheating
        if (playerIndex != turnPlayer)
            return;

        var oldPlayer = players[turnPlayer];

        turnPlayer++;
        turnPlayer %= 2;

        var newPlayer = players[turnPlayer];

        for (int i = 0; i < playCards.Count; i++ )
        {
            var card = playCards[i];
            if (card.tap <= 0) { continue; }
            if (card.owner != oldPlayer.playerId) { continue; }

            card.tap--;
        }

        Debug.Log(turnPlayer);
        SendGameManager();
    }

    public void FromJSON(JSONObject jsPlayer)
    {
        turnPlayer = (int)jsPlayer["TurnPlayer"];
        playCards = Player.PileFromJSON(jsPlayer["PlayCards"]);
    }

    public JSONObject ToJSON()
    {
        JSONObject jsPlayer = JSONObject.obj;
        jsPlayer.AddField("TurnPlayer", turnPlayer);
        jsPlayer.AddField("PlayCards", Player.PileToJSON(playCards));

        return jsPlayer;
    }
    public void SendGameManager()
    {
        if (!Network.isServer)
        {
            Debug.LogError("Client trying to send game manager");
            return;
        }

        this.NetRPC("ReceiveGameManager", RPCMode.All, System.Text.UTF8Encoding.UTF8.GetBytes(ToJSON().ToString()));
        this.NetRPC("ReceivePlayer", RPCMode.All, 0, players[0].ToJSON().ToString());
        this.NetRPC("ReceivePlayer", RPCMode.All, 1, players[1].ToJSON().ToString());
        
    }

    [RPC]
    public void ReceiveGameManager(byte[] manager)
    {
        Debug.Log("Receive()" + System.Text.UTF8Encoding.UTF8.GetString(manager));
        FromJSON(JSONParser.parse(System.Text.UTF8Encoding.UTF8.GetString(manager)));
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

    public int FindCard(int cardIndex)
    {
        for (int i = 0; i < playCards.Count; i++)
        {
            if (playCards[i].globalIdx == cardIndex)
            {
                return i;
            }
        }

        return -1;
    }
}

public class Player {
    public int playerId = -1;
    public string name;
    public int playerHealth;
    public List<LibraryCard> deck = new List<LibraryCard>();
    /*public List<PlayCard> playPile = new List<PlayCard>();
    public List<PlayCard> playHand = new List<PlayCard>();
    public List<PlayCard> discardPile = new List<PlayCard>();
    public List<PlayCard> field = new List<PlayCard>();*/
    public NetworkPlayer networkPlayer;
    //static int globalIdx = 0;

    public void BuildPlayPile() {
        
        List<PlayCard> playPile = new List<PlayCard>();
        for(int i = 0;i < deck.Count;i++) {
            //globalIdx++;

            PlayCard playCard = new PlayCard(deck[i].cardID);
            playCard.InitLibrary();
            playPile.Add(playCard);
        }

        playPile.Shuffle();

        for (int i = 0; i < playPile.Count; i++)
        {
            playPile[i].pos = i;
            playPile[i].globalIdx = GameManager.Get().playCards.Count + i;
            playPile[i].pile = PlayCard.Pile.deck;
            playPile[i].owner = playerId;
        }

        GameManager.Get().playCards.AddRange(playPile);
    }


    public void FromJSON( JSONObject jsPlayer ) {
        playerId = (int)jsPlayer["PlayerId"];
        name = (string)jsPlayer["Name"];
        playerHealth = (int)jsPlayer["PlayerHealth"];
        deck = DeckBuilder.DeckFromJSON(jsPlayer["Deck"]);
        /*playPile = PileFromJSON(jsPlayer["PlayPile"]);
        playHand = PileFromJSON(jsPlayer["PlayHand"]);
        discardPile = PileFromJSON(jsPlayer["DiscardPile"]);
        field = PileFromJSON(jsPlayer["Field"]);*/
    }

    public JSONObject ToJSON() {
        JSONObject jsPlayer = JSONObject.obj;
        jsPlayer.AddField("Name", name);
        jsPlayer.AddField("PlayerId", playerId);
        jsPlayer.AddField("PlayerHealth", playerHealth);
        jsPlayer.AddField("Deck", DeckBuilder.DeckToJSON(deck));
        /*jsPlayer.AddField("PlayPile", PileToJSON(playPile));
        jsPlayer.AddField("PlayHand", PileToJSON(playHand));
        jsPlayer.AddField("DiscardPile", PileToJSON(discardPile));
        jsPlayer.AddField("Field", PileToJSON(field));*/
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
    /*public int FindCardPlayPile(int cardIndex) { return FindCard(playPile, cardIndex); }
    public int FindCardHand(int cardIndex) { return FindCard(playHand, cardIndex); }
    public int FindCardField(int cardIndex) { return FindCard(field, cardIndex); }
    public int FindCardDiscard(int cardIndex) { return FindCard(discardPile, cardIndex); }*/
}
