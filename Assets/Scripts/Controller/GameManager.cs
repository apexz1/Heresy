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
    public int effectCounter = 0;
    public List<PlayCard> playCards = new List<PlayCard>();
    public List<PlayCard> sacList = new List<PlayCard>();
    public PlayFX currentFx = new PlayFX();

    public bool effectInProgess;


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

        LoadTextures.LoadFromFile(0, "D:/ProtoTest/Images/");
        LoadTextures.LoadFromFile(1, "D:/ProtoTest/Images/preview/");
    }

    [RPC]
    public void StartGame( int playerId, bool network ) {
        NetworkManager networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        networkManager.enabled = false;
        localPlayerId = playerId;
        turnPlayer = 0;
        //GameObject.Find("curtain").SetActive(false);

        LoadDeck(localPlayerId, "default");
        if (network == false) { LoadDeck(1, "default"); };

        if(Network.isServer) {
            this.NetRPC("StartGame", RPCMode.Others, playerId + 1, true);

            for (int i = 0; i < players.Length; i++)
            {
                players[i].playerHealth = 20;
                players[i].spawns = 2;
                players[i].sac = 0;
            }

            SendGameManager();
        }
        Debug.Log(turnPlayer);


        if(network == false) {
            //Automated player opening
            PlayFromHand(0, 0, 18);
            PlayFromHand(0, 1, 19);
            PlayFromHand(0, 2, 20);

            players[0].spawns = 100;
            //MoveOnField(0, players[0].field[0].globalIdx, 5);

            //Automated opponent opening
            PlayFromHand(1, 31, 0);
            PlayFromHand(1, 32, 1);
            PlayFromHand(1, 33, 2);

            players[1].spawns = 100;
            //MoveOnField(1, players[1].field[0].globalIdx, 5);
        }
    }

    public void LoadDeck( int playerIdx, string deck ) {
        TextAsset textFile = (TextAsset)Resources.Load(deck);
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

        DrawCard(playerId, 7);

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
    public void DrawCard( int playerIndex, int amount = 1 ) {
        var player = players[playerIndex];
        PlayCard drawCard = null;

        while (amount > 0)
        {
            if (CountCards(playerIndex, PlayCard.Pile.hand) > 10) { break; }

            for (int i = 0; i < playCards.Count; i++)
            {
                var card = playCards[i];
                if (card.owner != playerIndex) { continue; }
                if (card.pile != PlayCard.Pile.deck) { continue; }
                drawCard = card;
            }

            if (drawCard == null) { break; }

            drawCard.pile = PlayCard.Pile.hand;
            drawCard.pos = CountCards(playerIndex, drawCard.pile) - 1;

            amount--;
        }

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
        SortHand(playerIndex);
        SendGameManager();
    }

    [RPC]
    public void PlayFromHand(int playerIndex, int cardIndex, int slotIndex)
    {
        bool check = false;
        var player = players[playerIndex];
        var card = playCards[cardIndex];

        if (effectInProgess) { return; }

        if (((slotIndex > 2 && playerIndex == 1) || (slotIndex < 18 && playerIndex == 0)) && card.GetLibCard().costs <= 0)
        {
            SendNotification(playerIndex, "Cards can be placed in spawn slots only");
            return;
        }

        for (int i = 0; i < sacList.Count; i++)
        {
            Debug.Log(sacList[i].pos + ", " + slotIndex);

            if (CheckSacSlots(sacList[i].pos, slotIndex))
            {
                check = true;
                Debug.Log("check: " + check);
            }
        }

        if (card.GetLibCard().costs > 0 && check == false)
        {
            SendNotification(playerIndex, "Can only spawn at sacrificial grounds");
            return;
        }

        if (CardAtSlot(slotIndex) != -1)
        {
            SendNotification(playerIndex, "Slot already in use");
            return;
        }

        if (player.spawns <= 0)
        {
            SendNotification(playerIndex, "All spawns used");
            return;
        }

        if (card.GetLibCard().costs > player.sac)
        {
            SendNotification(playerIndex, "Not enough cards sacrificed");
            return;
        }

        card.pile = PlayCard.Pile.field;        
        card.pos = slotIndex;
        player.spawns -= 1;
        player.sac -= card.GetLibCard().costs;
        SortHand(playerIndex);

        if (card.GetLibCard().costs > 0)
        {
            FieldController.GetFieldController().HideSacFields();
        }
        sacList = new List<PlayCard>();

        StartCardFx(playerIndex, cardIndex);
        SendGameManager();
    }

    public bool CheckSacSlots(int sacPos, int spawnSlot)
    {
        return (sacPos == spawnSlot);
    }

    public void StartCardFx(int playerIndex, int cardIndex)
    {
        if (effectCounter >= 2) {
            Debug.Log("test");
            return;
        }

        var card = playCards[cardIndex];
        var libCard = card.GetLibCard();
        var libFx = new LibraryFX();
        Debug.Log("StartCardFX()" + libCard.fxList.Count);
        if (libCard.fxList.Count <= 0) { return; }

        Debug.Log("Start effectCounter " + effectCounter);
        currentFx = new PlayFX();
        currentFx.fxIdx = effectCounter;
        currentFx.libId = card.libId;
        currentFx.playerIdx = playerIndex;

        libFx = currentFx.GetLibFx();
        currentFx.actionCount = libFx.actionCount;
        currentFx.selectorCount = libFx.selectorCount;

        effectInProgess = true;

        if (libFx.selectorPile == PlayCard.Pile.none) { currentFx.selectorDone = true; }
        Debug.Log("currentFx" + currentFx.GetLibFx().description);
        ExeCardFx();
    }
    public void ExeCardFx()
    {
        Debug.Log("destgds tb" + currentFx.libId + "||" + currentFx.selectorDone);
        if (currentFx.libId == 0) { return; }
        if (!currentFx.selectorDone) { return; }

        var libFx = currentFx.GetLibFx();

        if (libFx.actionType == LibraryFX.ActionType.draw)
        {
            DrawCard(currentFx.playerIdx, currentFx.actionCount);
        }

        if (libFx.actionType == LibraryFX.ActionType.discard)
        {
            for(int i=0;i<currentFx.selectedCards.Count;i++)
            {
                int cardIndex=currentFx.selectedCards[i];
                var card = playCards[cardIndex];
                DiscardCard(card.owner,cardIndex);
            }
        }

        if (libFx.actionType == LibraryFX.ActionType.damagePlayer)
        {
            DamagePlayer(currentFx.playerIdx, currentFx.actionCount);
        }

        currentFx = new PlayFX();
        effectInProgess = false;
        effectCounter++;
        Debug.Log("End effectCounter " + effectCounter);
    }

    [RPC]
    public void SelectorFxDone(int playIndex, int cardIndex)
    {
        if (currentFx.libId == 0) { return; }
        if (currentFx.selectorDone) { return; }
        var libFx = currentFx.GetLibFx();

        currentFx.selectedCards.Add(cardIndex);

        if(currentFx.selectedCards.Count>=currentFx.selectorCount)
        {
            currentFx.selectorDone = true;
            ExeCardFx();
            return;
        }
    }

    private void SortHand(int playerIndex)
    {
        int counter = 0;
        for (int i = 0; i < playCards.Count; i++)
        {
            var handCard = playCards[i];
            if (handCard.pile != PlayCard.Pile.hand) { continue; }
            if (handCard.owner != playerIndex) { continue; }
            handCard.pos = counter;
            counter++;
        }
    }
    [RPC]
    public void ActionFoF(int playerIndex, int ownCardIndex, int oppCardIndex)
    {
        if (effectInProgess) { return; }

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

        int distance = CalcDistance(ownCard.pos, oppCard.pos);
        if (distance > ownCard.GetLibCard().atkRange)
        {
            SendNotification(playerIndex, "Target out of range");
            return;
        }

        oppCard.health -= ownLibCard.attack;
        ownCard.actions--;

        if (oppLibCard.atkRange >= ownLibCard.atkRange) { ownCard.health -= oppLibCard.attack; }

        if (ownCard.actions <= 0) { ownCard.tap = 1; }

        if (oppCard.health <= 0)
        {
            DiscardCard(opponent.playerId, oppCard.globalIdx);
        }
        if (ownCard.health <= 0)
        {
            DiscardCard(player.playerId, ownCard.globalIdx);
        }

        SendGameManager();
    }

    [RPC]
    public void ActionFoP(int playerIndex, int cardIndex, int attackedPlayer)
    {
        if (effectInProgess) { return; }

        Debug.Log(cardIndex);
        if (cardIndex <= 0)
        {
            SendNotification(playerIndex, "No card selected");
            return;
        }

        var opponent = players[attackedPlayer];
        var ownCard = playCards[cardIndex];
        var ownLibCard = CardLibrary.Get().GetCard(ownCard.libId);
        var controller = FieldController.GetFieldController();

        if (ownCard.tap > 0)
        {
            SendNotification(playerIndex, "Card is tapped");
            return;
        }

        if (ownLibCard.attack == 0)
        {
            SendNotification(playerIndex, "No attack value assigned");
            return;
        }
        if (playerIndex == attackedPlayer)
        {
            SendNotification(playerIndex, "Can't attack own PlayerObject");
            return;
        }

        int distance = CalcDistance(ownCard.pos, attackedPlayer == 0 ? 16 : 4);
        if (distance > ownCard.GetLibCard().atkRange)
        {
            SendNotification(playerIndex, "Target out of range");
            return;
        }

        opponent.playerHealth -= ownLibCard.attack;
        ownCard.tap++;

        if (opponent.playerHealth <= 0)
        {
            controller.GameOver();
        }

        SendGameManager();
    }

    [RPC]
    public void MoveOnField(int playerIndex, int cardIndex, int slotIndex)
    {
        if (effectInProgess) { return; }

        var player = players[playerIndex];
        var controller = FieldController.GetFieldController();

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

        int distance = CalcDistance(card.pos, slotIndex);
        Debug.Log("card actions: " + card.actions);
        if (distance > card.actions)
        {
            SendNotification(playerIndex, "Slot out of range");
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
            if (swapCard.tap > 0)
            {
                SendNotification(playerIndex, "Cannot swap if either target is tapped");
                return;
            }

            if (card.actions <= 0) {swapCard.tap = 1;}
            swapCard.pos = card.pos;
        }


        card.actions -= distance;
        card.pos = slotIndex;
        if (card.actions <= 0) 
        {
            controller.SelectCard(card.globalIdx);
            card.tap = 1;
        }
        

        SendGameManager();
    }

    [RPC]
    public void SacCard(int playerIndex, int cardIndex)
    {
        var player = players[playerIndex];
        int maxCosts = 0;

        if (player.spawns <= 0)
        {
            SendNotification(playerIndex, "Can't sacrifice card, no spawns left");
            return;
        }

        for (int i = 0; i < playCards.Count; i++)
        {
            if (playCards[i].pile == PlayCard.Pile.hand && playCards[i].owner == localPlayerId)
            {
                maxCosts += playCards[i].GetLibCard().costs;
            }
        }

        Debug.Log("Sac check: " + maxCosts + ", " + player.sac);

        if (player.sac >= maxCosts)
        {
            SendNotification(playerIndex, "Can't sacrifice more cards, no cards with costs found in hand");
            return;
        }

        if (playCards[cardIndex].tap > 0)
        {
            SendNotification(playerIndex, "Can't sacrifice tapped cards");
            return;
        }

        sacList.Add(playCards[cardIndex]);
        playCards[cardIndex].pile = PlayCard.Pile.discard;
        player.sac++;
        Debug.Log(player.sac);
    }

    [RPC]
    public void EndTurn(int playerIndex)
    {
        if (effectInProgess) { return; }

        var controller = FieldController.GetFieldController();

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
            card.actions = card.GetLibCard().moveRange;
            card.tap--;
        }

        newPlayer.spawns = 2;
        DrawCard(newPlayer.playerId, 1);
        FieldController.GetFieldController().HideSacFieldsAll();

        /*for (int i = 0; i < playCards.Count; i++ )
        {
            if (playCards[i].pile == PlayCard.Pile.field && playCards[i].owner == localPlayerId) { controller.SelectCard(playCards[i].globalIdx); }           
        }
         * */

        Debug.Log(turnPlayer);
        SendGameManager();
    }

    public void FromJSON(JSONObject jsPlayer)
    {
        turnPlayer = (int)jsPlayer["TurnPlayer"];
        playCards = Player.PileFromJSON(jsPlayer["PlayCards"]);
        sacList = Player.PileFromJSON(jsPlayer["SacrificeList"]);
        currentFx.FromJSON(jsPlayer["CurrentFx"]);
    }

    public JSONObject ToJSON()
    {
        JSONObject jsPlayer = JSONObject.obj;
        jsPlayer.AddField("TurnPlayer", turnPlayer);
        jsPlayer.AddField("PlayCards", Player.PileToJSON(playCards));
        jsPlayer.AddField("SacrificeList", Player.PileToJSON(sacList));
        jsPlayer.AddField("CurrentFx", currentFx.ToJSON());

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

    public int CalcDistance(int slotA, int slotB)
    {
        if (slotA == slotB) { return 0; }
        Vector2 a = GetSlotPos(slotA);
        Vector2 b = GetSlotPos(slotB);

        return (int)(a - b).magnitude;
    }

    public Vector2 GetSlotPos(int slot)
    {
        int x = slot / 3;
        int y = slot % 3;

        return new Vector2(x, y);
    }
}

public class Player {
    public int playerId = -1;
    public string name;
    public int playerHealth;
    public int spawns;
    public int sac;
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
        spawns = (int)jsPlayer["Spawns"];
        sac = (int)jsPlayer["Sacrifice"];
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
        jsPlayer.AddField("Spawns", spawns);
        jsPlayer.AddField("Sacrifice", sac);
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
