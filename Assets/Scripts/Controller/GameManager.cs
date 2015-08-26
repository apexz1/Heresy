using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text;


public class GameManager : MonoBehaviour
{
    //unsynced vars; not in JSON
    NetworkView networkView;
    public int localPlayerId = -1;
    public string notification;
    public float notifTime;

    //synced vars
    public Player[] players;
    public int turnPlayer = 0;
    public int effectCounter = 0;
    public List<PlayCard> playCards = new List<PlayCard>();
    //public List<PlayCard> sacList = new List<PlayCard>();
    public PlayFX currentFx = new PlayFX();
    public string deckChoice;
    string deckLocation;

    public bool effectInProgess;
    public int gameOver = -1;
    public bool running = false;
    public int gameVersion = 0;
    public int brutalOD = 0;
    public bool setUp = false;
    //public bool monument = true;

    public static int Neverfall_God_of_Pride = 0;

    public static GameManager Get()
    {
        return GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnPlayerConnected( NetworkPlayer player )
    {
        players[1].networkPlayer = player;
        Debug.Log("Player connected from" + player.ipAddress + ":" + player.port);
        StartGame(0, true);
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

        gameOver = -1;
        setUp = true;

        networkView = GetComponent<NetworkView>();
        Debug.Log(Application.dataPath);
        LoadTextures.LoadFromFile(0);
        LoadTextures.LoadFromFile(1);

        deckLocation = SaveGameLocation.getSaveGameDirectory() + "/Heresy";
        deckChoice = "default";
        Debug.Log(OptionsMenu.isDarkFantasy);
    }

    void Update()
    {
        if (running)
        {
            if (players[0].playerHealth == 0)
            {
                gameOver = 0;
            }
            if (players[1].playerHealth == 0)
            {
                gameOver = 1;
            }
        }
    }

    [RPC]
    public void StartGame( int playerId, bool network )
    {
        string deckLocation = SaveGameLocation.getSaveGameDirectory() + "/Heresy";
        InputField inputField = GameObject.Find("GameUI").transform.FindChild("PreGame").FindChild("DeckChoice").gameObject.GetComponent<InputField>();
        deckChoice = inputField.text;

        if (!File.Exists(deckLocation + "/" + deckChoice + ".json"))
        {
            if (Resources.Load("default") != null)
            {
                deckChoice = "default";
            }

            else
            {
                Debug.Log("Deck not found;");
                return;
            }
        }

        NetworkManager networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        networkManager.enabled = false;
        localPlayerId = playerId;
        turnPlayer = 0;

        Debug.Log("Deck to load: " + deckChoice);
        LoadDeck(localPlayerId, deckChoice);

        //FieldController.GetFieldController().LoadMonument(0);
        //FieldController.GetFieldController().LoadMonument(1);

        if (network == false) { LoadDeck(1, "default"); };

        if (Network.isServer)
        {
            this.NetRPC("StartGame", RPCMode.Others, playerId + 1, true);

            for (int i = 0; i < players.Length; i++)
            {
                players[i].playerHealth = 20;
                players[i].sac = 0;
                players[i].kills = 4;
                players[i].monument = true;
            }

            players[0].spawns = 1;
            players[1].spawns = 2;

            SendGameManager();
        }
        Debug.Log(turnPlayer);


        if (network == false)
        {
            players[0].spawns = 3;
            players[1].spawns = 3;
        }

        running = true;

        GameObject.Find("GameUI").transform.FindChild("PreGame").gameObject.SetActive(false);
        GameObject.Find("GameUI").transform.FindChild("Main").gameObject.SetActive(true);
        GameObject.Find("SceneCam").transform.FindChild("loading").gameObject.SetActive(true);

        var o = GameObject.Find("SceneCam").transform.FindChild("curtain").gameObject;
        GameObject.Destroy(o, 1.5f);
        o = GameObject.Find("SceneCam").transform.FindChild("loading").gameObject;
        GameObject.Destroy(o, 1.5f);
    }

    public void LoadDeck( int playerIdx, string deck )
    {
        JSONObject jsPlayer = new JSONObject();

        if (File.Exists(deckLocation + "/" + deckChoice + ".json"))
        {
            string file = File.ReadAllText(deckLocation + "/" + deckChoice + ".json");
            //TextAsset textFile = (TextAsset)Resources.Load(deck);
            jsPlayer = JSONParser.parse(file);
        }
        else if (File.Exists(deckLocation + "/default.json"))
        {
            string file = File.ReadAllText(deckLocation + "/default.json");
            //TextAsset textFile = (TextAsset)Resources.Load(deck);
            jsPlayer = JSONParser.parse(file);
        }
        else
        {
            TextAsset textFile = (TextAsset)Resources.Load(deck);
            jsPlayer = JSONParser.parse(textFile.text);
        }

        Debug.Log("start");
        this.NetRPC("AssignDeck", RPCMode.Server, playerIdx, jsPlayer["Deck"].ToString());
        //this.NetRPC("AssignCult", RPCMode.Server, playerIdx, jsPlayer["Cult"].ToString());
        AssignCult(0, jsPlayer["Cult"].ToString());
        AssignCult(1, jsPlayer["Cult"].ToString());
        //Debug.Log(players[0].cult + " " + players[1].cult);
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
    public void AssignDeck( int playerId, string deck )
    {
        Debug.Log("AssignDeck()" + playerId + " : " + deck);

        players[playerId].deck = DeckBuilder.DeckFromJSON(JSONParser.parse(deck));
        players[playerId].BuildPlayPile();

        DrawCard(playerId, 8 - playerId);

        SendGameManager();
    }

    public void AssignCult( int playerId, string cult )
    {
        Debug.Log("AssignCult()" + playerId + " : " + cult);

        players[playerId].cult = DeckBuilder.CultFromJSON(JSONParser.parse(cult));
        Debug.Log("cults: " + players[0].cult + " " + players[1].cult);
    }

    [RPC]
    public void DamagePlayer( int playerIndex, int amount )
    {
        var player = players[playerIndex];

        Debug.Log("amount " + amount);
        player.playerHealth -= amount;
        Debug.Log("playerHealth: " + player.playerHealth);

        //Bitterface, God of Envy PlayerHealth/Attack-Sync FX
        for (int i = 0; i < playCards.Count; i++)
        {
            if (playCards[i].libId == 974)
            {
                playCards[i].attack = players[playCards[i].owner].playerHealth;
            }
        }

        SendGameManager();
    }

    [RPC]
    public void ReceivePlayer( int playerIndex, string player )
    {
        Debug.Log("Receive()" + playerIndex + " | " + player);
        players[playerIndex].FromJSON(JSONParser.parse(player));
    }

    [RPC]
    public void DrawCard( int playerIndex, int amount = 1 )
    {
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
            //drawCard.pos = CountCards(playerIndex, drawCard.pile) - 1;

            amount--;
        }

        SortHand(playerIndex);
        SendGameManager();
    }

    [RPC]
    public void TapCard( int playerIndex, int cardIndex )
    {
        playCards[cardIndex].tap++;
        SendGameManager();
    }

    public int CountCards( int playerIndex, PlayCard.Pile pile )
    {
        int counter = 0;
        for (int i = 0; i < playCards.Count; i++)
        {
            var card = playCards[i];
            if (card.owner != playerIndex)
                continue;
            if (card.pile != pile)
                continue;
            counter++;
        }

        return counter;
    }
    public int CardAtSlot( int slotIndex )
    {
        for (int i = 0; i < playCards.Count; i++)
        {
            var card = playCards[i];
            if (card.pile != PlayCard.Pile.field)
                continue;
            if (card.pos != slotIndex)
                continue;

            return i;
        }

        return -1;
    }
    [RPC]
    public void DiscardCard( int playerIndex, int cardIndex )
    {
        var player = players[playerIndex];
        Debug.Log("DiscardCard() Log: " + playerIndex + " " + cardIndex);

        /*
        if (FieldController.GetFieldController().cardSelected == cardIndex)
        {
            FieldController.GetFieldController().cardSelected = -1;
        }
        /**/
        playCards[cardIndex].pile = PlayCard.Pile.discard;
        playCards[cardIndex].pos = -1;

        if (playCards[cardIndex].GetLibCard().cardID == 971)
        {
            Neverfall_God_of_Pride = 0;
        }

        SortHand(playerIndex);
        SendGameManager();
    }
    public void DiscardCardAll( int playerIndex )
    {
        var player = players[playerIndex];

        for (int i = 0; i < playCards.Count; i++)
        {
            if (playCards[i].pile == PlayCard.Pile.hand)
            {
                DiscardCard(playerIndex, playCards[i].globalIdx);
            }
        }

        SortHand(playerIndex);
        SendGameManager();
    }

    [RPC]
    public void PlayFromHand( int playerIndex, int cardIndex, int slotIndex )
    {
        bool check = false;
        var player = players[playerIndex];
        var card = playCards[cardIndex];
        int cardAtSlot = CardAtSlot(slotIndex);

        if (effectInProgess) { return; }

        if (((slotIndex > 2 && playerIndex == 1) || (slotIndex < 18 && playerIndex == 0)) && card.GetLibCard().costs <= 0)
        {
            SendNotification(playerIndex, "Cards can be placed in spawn slots only");
            return;
        }

        /*for (int i = 0; i < sacList.Count; i++)
        {
            Debug.Log(sacList[i].pos + ", " + slotIndex);

            if (CheckSacSlots(sacList[i].pos, slotIndex))
            {
                check = true;
                Debug.Log("check: " + check);
            }
        }*/

        if (cardAtSlot != -1)
        {
            if (playCards[cardAtSlot].saced)
            {
                check = true;
            }
            else
            {
                SendNotification(playerIndex, "Slot already in use");
                return;
            }
        }

        if (card.GetLibCard().costs > 0 && check == false)
        {
            SendNotification(playerIndex, "Can only spawn at sacrificial grounds");
            return;
        }
        /*
        if (card.GetLibCard().costs == 0 && player.sac > 0)
        {
            SendNotification(playerIndex, "Please complete sacrificial rite before summoning additional basic cultists");
            return;
        }
        /**/

        if (player.spawns <= 0)
        {
            SendNotification(playerIndex, "All spawns used");
            return;
        }

        bool skinflint = false;

        for (int i = 0; i < playCards.Count; i++)
        {
            if (playCards[i].pile == PlayCard.Pile.field && playCards[i].libId == 977)
            {
                skinflint = true;
            }
        }

        if (skinflint)
        {
            players[playerIndex].sac++;
        }

        if (card.GetLibCard().costs > player.sac)
        {
            SendNotification(playerIndex, "Not enough cards sacrificed");
            return;
        }

        card.pile = PlayCard.Pile.field;
        card.pos = slotIndex;
        FieldController.GetFieldController().SelectCard(card.globalIdx);

        if (card.GetLibCard().costs > 0)
        {
            card.tap++;
        }

        player.spawns -= 1;
        player.sac = 0;
        SortHand(playerIndex);

        if (card.GetLibCard().costs > 0)
        {
            for (int i = 0; i < playCards.Count; i++)
            {
                if (playCards[i].saced)
                {
                    playCards[i].pile = PlayCard.Pile.discard;
                }
            }
        }

        for (int i = 0; i < playCards.Count; i++)
        {
            if (playCards[i].pile == PlayCard.Pile.field)
            {
                playCards[i].saced = false;
            }
        }

        //effectCounter = 0;
        //StartCardFxCon(playerIndex, cardIndex);


        StartCardFx(playerIndex, card.libId);
        #region Chosen Entry FX
        //Skyfolk Chosen
        if (card.libId == 957)
        {
            for (int i = 0; i < playCards.Count; i++)
            {
                if (playCards[i].pile == PlayCard.Pile.field && playCards[i].owner != card.owner)
                {
                    playCards[i].attack = 0;
                }
            }
        }
        //Hexfin Chosen
        if (card.libId == 958)
        {
            for (int i = 0; i < playCards.Count; i++)
            {
                if (playCards[i].pile == PlayCard.Pile.field && playCards[i].owner == card.owner)
                {
                    playCards[i].health = playCards[i].GetLibCard().health;
                }
            }
        }
        //Ripjaw Chosen
        if (card.libId == 959)
        {
            DamagePlayer((playerIndex + 1) % 2, 6);
        }
        //Graveborn Chosen
        if (card.libId == 960)
        {
            for (int i = 0; i < playCards.Count; i++)
            {
                if ((playCards[i].pile == PlayCard.Pile.field && playCards[i].owner != card.owner) && playCards[i].GetLibCard().costs <= 0)
                {
                    if (CountCards((card.owner + 1) % 2, PlayCard.Pile.hand) < 10)
                    {
                        playCards[i].pile = PlayCard.Pile.hand;
                    }
                    else
                    {
                        DiscardCard((card.owner + 1) % 2, playCards[i].globalIdx);
                    }
                }
            }
        }
        //Dreadbulge Chosen in CardLibary
        //Blightbark Chosen
        if (card.libId == 962)
        {
            for (int i = 0; i < playCards.Count; i++)
            {
                if (playCards[i].pile == PlayCard.Pile.field && playCards[i].owner != card.owner)
                {
                    playCards[i].tap = 2;
                }
            }
        }
        //Pitkin Chosen
        if (card.libId == 963)
        {
            players[card.owner].spawns++;
        }
        #endregion
        #region God Entry FX
        //Neverfall, God of Pride
        if (card.libId == 971)
        {
            Neverfall_God_of_Pride = 1;
        }
        //Bitterface, God of Envy
        if (card.libId == 974)
        {
            card.attack = players[card.owner].playerHealth;
        }
        #endregion
        SendGameManager();
    }

    public bool CheckSacSlots( int sacPos, int spawnSlot )
    {
        return (sacPos == spawnSlot);
    }

    /*public void StartCardFxCon(int playerIndex, int cardIndex)
    {
        var card = playCards[cardIndex];
        var libCard = card.GetLibCard();
        var libFx = new LibraryFX();

        if (libFx.conditionCount <= 0) { return; }
        if (libFx.conditionMore)
        {

        }
        if (!libFx.conditionMore)
        {
            int value = 0;

            for (int i = 0; i < playCards.Count; i++)
            {
                if (playCards[i].pile == libFx.selectorPile)
                {
                    value++;
                }
            }
        }

    }*/

    public void StartCardFx( int playerIndex, int libCardIndex, int fxIndex = 0, int adjacentPos = -1, PlayCard card = null )
    {
        Debug.Log("StartCardFx entered");
        /*if (effectCounter >= 2) {
            Debug.Log("test");
            return;
        }*/

        //var card = playCards[cardIndex];
        var libCard = CardLibrary.Get().GetCard(libCardIndex);
        //var libFx = new LibraryFX();

        //if (libCard == null) { return; }

        Debug.Log("StartCardFX()" + libCard.fxList.Count + " " + fxIndex);
        if (libCard.fxList.Count <= 0) { return; }

        Debug.Log("Start effectCounter " + effectCounter);
        currentFx = new PlayFX();
        //currentFx.cardId = libCardIndex;
        currentFx.fxIdx = effectCounter;
        currentFx.libId = libCardIndex;
        currentFx.fxIdx = fxIndex;
        currentFx.playerIdx = playerIndex;
        if (card != null)
        {
            currentFx.globalIdx = card.globalIdx;
        }

        var libFx = currentFx.GetLibFx();
        currentFx.actionCount = libFx.actionCount;
        currentFx.selectorCount = libFx.selectorCount;
        if (libFx.adjacentPos) { currentFx.adjacentPos = adjacentPos; }

        effectInProgess = true;

        //Condition
        if (libFx.conditionType != LibraryFX.ConditionType.none)
        {
            Debug.Log(currentFx.GetLibFx().conditionType);
            //GREED
            if (libFx.conditionType == LibraryFX.ConditionType.ctrlOwn)
            {
                //Debug.Log("value check storage: " + (CountCards(playerIndex, PlayCard.Pile.field) + 1) + " / " + libFx.conditionCount + " * " + currentFx.actionCount);
                Debug.Log(CountCards(playerIndex, PlayCard.Pile.field));
                Debug.Log(CountCards(playerIndex, PlayCard.Pile.field) + 1);
                int storage = ((CountCards(playerIndex, PlayCard.Pile.field) + 1) / libFx.conditionCount);
                if (storage > 1) { storage = 1; }
                storage = storage * currentFx.actionCount;
                //Debug.Log("storage: " + storage);

                currentFx.actionCount = currentFx.selectorCount = storage;
            }
            //ENVY
            if (libFx.conditionType == LibraryFX.ConditionType.ctrlOpp)
            {
                Debug.Log("value check storage: " + (CountCards((playerIndex + 1) % 2, PlayCard.Pile.field) + 1) + " / " + libFx.conditionCount + " * " + currentFx.actionCount);
                int storage = ((CountCards((playerIndex + 1) % 2, PlayCard.Pile.field) + 1) / libFx.conditionCount);
                Debug.Log("storage " + storage);
                if (storage > 1) { storage = 1; }
                storage = storage * currentFx.actionCount;
                Debug.Log("storage: " + storage);

                currentFx.actionCount = currentFx.selectorCount = storage;
            }

            //PRIDE
            if (libFx.conditionType == LibraryFX.ConditionType.ctrlMoreOwn)
            {
                Debug.Log("value check storage: " + ((CountCards(playerIndex, PlayCard.Pile.field) + 1) + " / " + (CountCards(playerIndex + 1 % 2, PlayCard.Pile.field) + 1)));
                
                //int storage = (((CountCards(playerIndex, PlayCard.Pile.field) - libFx.conditionCount) + 1) / (CountCards(playerIndex + 1 % 2, PlayCard.Pile.field) + 1));
                int storage = (CountCards(playerIndex, PlayCard.Pile.field) + 1) - (CountCards(playerIndex + 1 % 2, PlayCard.Pile.field) + 1);
                Debug.Log("storage: " + storage + "conCount: " + libFx.conditionCount);
                if (storage >= libFx.conditionCount) { storage = 1; Debug.Log(storage); }
                else if (storage <  libFx.conditionCount) { storage = 0; Debug.Log(storage); }
                storage = storage * currentFx.actionCount;
                Debug.Log("storage: " + storage);

                currentFx.actionCount = storage;
                if (storage > 0)
                {
                    currentFx.selectorCount = storage;
                }
                if (storage < 0)
                {
                    currentFx.selectorCount = -storage;
                }
            }
            /*
             * //DELETE FOR BUILD, NOT IN USE
            if (libFx.conditionType == LibraryFX.ConditionType.ctrlMoreOpp)
            {
                currentFx.actionCount = currentFx.selectorCount = (CountCards(playerIndex, PlayCard.Pile.field) / libFx.conditionCount);
            }
            /**/

            //WRATH
            if (libFx.conditionType == LibraryFX.ConditionType.kills)
            {
                //Debug.Log("calc ints: " + (players[playerIndex].kills + " " + libFx.conditionCount + " " + libFx.actionCount));
                int storage = currentFx.selectorCount = (players[playerIndex].kills / libFx.conditionCount);
                //prevents exponential actioncounts
                if (storage > 1) { storage = 1; }
                storage = storage * currentFx.actionCount;
                //Debug.Log("storage: " + storage);

                currentFx.actionCount = storage;
            }

            if (currentFx.selectorCount > 1) { currentFx.selectorCount = 1; }
            Debug.Log("sdgdssh " + currentFx.selectorCount);
        }
        //Selector 
        if (libFx.selectorPile == PlayCard.Pile.none || currentFx.selectorCount <= 0) { currentFx.selectorDone = true; }

        if (!CheckSelectorAll(playerIndex)) { currentFx.selectorDone = true; Debug.Log("Skip Selector"); }
        Debug.Log("currentFx" + currentFx.GetLibFx().description);
        ExeCardFx();
    }
    public void ExeCardFx()
    {
        Debug.Log("destgds tb" + currentFx.libId + "||" + currentFx.selectorDone);
        if (currentFx.libId == 0)
        {
            SendGameManager();
            return;
        }

        bool monumentfx = false;

        if (currentFx.libId >= 701 && currentFx.libId <= 706)
        {
            monumentfx = true;
        }

        if (!currentFx.selectorDone)
        {
            SendGameManager();
            return;
        }

        var libFx = currentFx.GetLibFx();

        if (libFx.actionType == LibraryFX.ActionType.none)
        {
            Debug.Log("No effect assigned");
        }

        if (libFx.actionType == LibraryFX.ActionType.draw)
        {
            if (monumentfx == true)
            {
                currentFx.actionCount += Neverfall_God_of_Pride;
            }

            Debug.Log("adgldjsoidjoisjoi draw");
            DrawCard(currentFx.playerIdx, currentFx.actionCount);
        }

        if (libFx.actionType == LibraryFX.ActionType.discard)
        {
            for (int i = 0; i < currentFx.selectedCards.Count; i++)
            {
                int cardIndex = currentFx.selectedCards[i];
                var card = playCards[cardIndex];

                if (card.pile == PlayCard.Pile.field && currentFx.playerIdx == ((card.owner + 1) % 2))
                {
                    players[((card.owner + 1) % 2)].kills++;
                }

                DiscardCard(card.owner, cardIndex);
            }

            if (monumentfx == true)
            {
                monumentfx = false;
                StartCardFx(currentFx.playerIdx, 500);
            }
        }

        if (libFx.actionType == LibraryFX.ActionType.tap)
        {
            for (int i = 0; i < currentFx.selectedCards.Count; i++)
            {
                int cardIndex = currentFx.selectedCards[i];
                var card = playCards[cardIndex];
                card.actions = 0;
                card.tap++;
            }
        }

        if (libFx.actionType == LibraryFX.ActionType.ready)
        {
            for (int i = 0; i < currentFx.selectedCards.Count; i++)
            {
                int cardIndex = currentFx.selectedCards[i];
                var card = playCards[cardIndex];
                card.tap = 0;
            }
        }

        if (libFx.actionType == LibraryFX.ActionType.buffAction)
        {
            if (monumentfx == true)
            {
                currentFx.actionCount += Neverfall_God_of_Pride;
            }

            for (int i = 0; i < currentFx.selectedCards.Count; i++)
            {
                int cardIndex = currentFx.selectedCards[i];
                var card = playCards[cardIndex];
                card.actions += currentFx.actionCount;
                if (card.actions > 0) { card.tap = 0; }
            }
        }

        if (libFx.actionType == LibraryFX.ActionType.buffAttack)
        {
            if (monumentfx == true)
            {
                currentFx.actionCount += Neverfall_God_of_Pride;
            }

            Debug.Log(currentFx.actionCount);
            Debug.Log(10 + currentFx.actionCount);

            for (int i = 0; i < currentFx.selectedCards.Count; i++)
            {
                int cardIndex = currentFx.selectedCards[i];
                var card = playCards[cardIndex];

                Debug.Log(card.attack);
                card.attack += currentFx.actionCount;

                if (currentFx.libId == 957)
                {
                    card.attack = 0;
                }

                if (card.attack < 0)
                {
                    card.attack = 0;
                }
            }
        }

        if (libFx.actionType == LibraryFX.ActionType.cultBuff)
        {
            for (int i = 0; i < playCards.Count; i++)
            {
                Debug.Log(currentFx.libId);
                Debug.Log(currentFx.globalIdx);
                Debug.Log("Selfbuff stuff: " + playCards[i].GetLibCard().cult + " " + playCards[currentFx.globalIdx].GetLibCard().cult);
                if (playCards[i].pile == PlayCard.Pile.field && playCards[i].GetLibCard().cult == CardLibrary.Get().GetCard(currentFx.libId).cult)
                {
                    playCards[i].attack += 2;
                }
            }
        }


        if (libFx.actionType == LibraryFX.ActionType.damageCard)
        {
            if (monumentfx == true)
            {
                currentFx.actionCount += Neverfall_God_of_Pride;
            }

            if (currentFx.libId == 100)
            {
                currentFx.actionCount = brutalOD;
                brutalOD = 0;
            }

            for (int i = 0; i < currentFx.selectedCards.Count; i++)
            {
                int cardIndex = currentFx.selectedCards[i];
                var card = playCards[cardIndex];

                card.health -= currentFx.actionCount;

                if (card.health <= 0)
                {
                    DiscardCard(card.owner, cardIndex);

                    if (card.owner != currentFx.playerIdx)
                    {
                        players[currentFx.playerIdx].kills++;
                    }
                }

            }
            //DamagePlayer(currentFx.playerIdx, currentFx.actionCount);
        }

        if (libFx.actionType == LibraryFX.ActionType.selfDestruct)
        {
            var card = playCards[currentFx.globalIdx];

            card.health -= currentFx.actionCount;

            if (card.health <= 0)
            {
                DiscardCard(card.owner, card.globalIdx);
            }
        }

        if (libFx.actionType == LibraryFX.ActionType.damageSelf)
        {

            if (currentFx.libId == 707)
            {
                Debug.Log("monument test: " + CountCultMember() + " " + currentFx.actionCount);
                currentFx.actionCount = (CountCultMember() * -2);

                if (currentFx.actionCount < -8)
                {
                    currentFx.actionCount = -8;
                }

                Debug.Log("monument test actioncount: " + currentFx.actionCount);
            }
            else if (monumentfx == true)
            {
                currentFx.actionCount += Neverfall_God_of_Pride;
                DamagePlayer(currentFx.playerIdx, -Neverfall_God_of_Pride);
            }

            DamagePlayer(currentFx.playerIdx, currentFx.actionCount);
        }

        if (libFx.actionType == LibraryFX.ActionType.damageOpp)
        {
            DamagePlayer((currentFx.playerIdx + 1) % 2, currentFx.actionCount);
        }

        Debug.Log("nextfx bugged: " + currentFx.fxIdx + " " + CardLibrary.Get().GetCard(currentFx.libId).fxList.Count);
        if (currentFx.NextFx())
        {
            Debug.Log("true");
            Debug.Log("next fx started??");
            StartCardFx(currentFx.playerIdx, currentFx.libId, currentFx.fxIdx);
            return;
        }
        currentFx = new PlayFX();
        effectInProgess = false;
        effectCounter++;
        Debug.Log("End effectCounter " + effectCounter);

        SendGameManager();
    }

    public void StartMonumentFx(int playerIndex)
    {
        this.NetRPC("MonumentFx", RPCMode.Server, playerIndex);
        players[playerIndex].monument = false;

        SendNotification(localPlayerId, "Monument's power drained");

        SendGameManager();
    }

    [RPC]
    public void MonumentFx( int playerIndex )
    {
        if (players[playerIndex].cult == "greed") { StartCardFx(playerIndex, 707); StartCardFx(playerIndex, 700); }
        if (players[playerIndex].cult == "envy") { StartCardFx(playerIndex, 707); StartCardFx(playerIndex, 701); }
        if (players[playerIndex].cult == "wrath") { StartCardFx(playerIndex, 707); StartCardFx(playerIndex, 702); }
        if (players[playerIndex].cult == "pride") { StartCardFx(playerIndex, 707); StartCardFx(playerIndex, 703); }
        if (players[playerIndex].cult == "gluttony") { StartCardFx(playerIndex, 707); StartCardFx(playerIndex, 704); }
        if (players[playerIndex].cult == "lust") { StartCardFx(playerIndex, 707); StartCardFx(playerIndex, 705); }
        if (players[playerIndex].cult == "sloth") { StartCardFx(playerIndex, 707); StartCardFx(playerIndex, 706); }
    }

    public int CountCultMember()
    {
        int counter = 0;

        for (int i = 0; i < playCards.Count; i++)
        {
            if ((playCards[i].pile == PlayCard.Pile.field) && (LibraryCard.CultToString(playCards[i].GetLibCard().cult) == players[GameManager.Get().localPlayerId].cult))
            {
                counter++;
            }
        }

        return counter;
    }

    [RPC]
    public void SelectorFxDone( int playerIndex, int cardIndex )
    {
        if (currentFx.libId == 0) { return; }
        if (currentFx.selectorDone) { return; }
        var libFx = currentFx.GetLibFx();

        if (!CheckSelectorForCard(playerIndex, cardIndex, true)) { return; }

        currentFx.selectedCards.Add(cardIndex);

        if (currentFx.selectedCards.Count >= currentFx.selectorCount)
        {
            currentFx.selectorDone = true;
            ExeCardFx();
            return;
        }
    }

    private bool CheckSelectorForCard( int playerIndex, int cardIndex, bool notif )
    {
        var card = playCards[cardIndex];
        var libFx = currentFx.GetLibFx();
        bool target = false;

        if (libFx == null)
            return false;

        /*if (currentFx.adjacentPos >= 0)
        {
            if (CalcDistance(card.pos, currentFx.adjacentPos) > 1)
            {
                if (notif)
                    SendNotification(playerIndex, "Can select adjacent card only " + " " + card.pos + " " + currentFx.adjacentPos);
                return false;
            }
        }
         * /**/

        if (CountCards((playerIndex + 1) % 2, PlayCard.Pile.field) <= 0 && currentFx.GetLibFx().selectorOwn == false)
        {
            if (notif)
                SendNotification(playerIndex, "No eligable target");
            //currentFx = new PlayFX();
            return false;
        }

        if (CountCards(playerIndex, PlayCard.Pile.field) <= 0 && currentFx.GetLibFx().selectorOwn == true)
        {
            if (notif)
                SendNotification(playerIndex, "No eligable target");
            //currentFx = new PlayFX();
            return false;
        }


        if (card.pile != libFx.selectorPile)
        {
            if (notif)
                SendNotification(playerIndex, ".pile wrong");
            return false;
        }

        //VEILED RACE ABILITY
        if (((card.GetLibCard().race == LibraryCard.Race.veiled || card.GetLibCard().race == LibraryCard.Race.hexC || card.GetLibCard().race == LibraryCard.Race.pitC) && card.pile == PlayCard.Pile.field && card.owner == players[(playerIndex + 1) % 2].playerId) && card.libId != 901)
        {
            if (notif)
                SendNotification(playerIndex, "target veiled");
            return false;
        }

        if (currentFx.libId == 961 && card.GetLibCard().costs > 0)
        {
            if (notif)
                SendNotification(playerIndex, "can target basic cultist only");
            return false;
        }

        for (int i = 0; i < playCards.Count; i++)
        {
            if (playCards[i].pile == PlayCard.Pile.field && (playCards[i].GetLibCard().race != LibraryCard.Race.veiled || card.GetLibCard().race != LibraryCard.Race.hexC || card.GetLibCard().race != LibraryCard.Race.pitC || playCards[i].libId == 901))
            {
                target = true;
            }
        }

        if (!target)
        {
            return false;
        }

        /*
        for (int i = 0; i < playCards.Count; i++)
        {
            if ((playCards[i].owner == (playerIndex+1)%2) && (playCards[i].pile == PlayCard.Pile.field))
            {
                if (playCards[i].GetLibCard().race != LibraryCard.Race.veiled)
                {
                    target = true;
                    break;
                }
            }
        }
        /**/

        return true;
    }

    public bool CheckSelectorAll( int playerIndex )
    {
        for (int i = 0; i < playCards.Count; i++)
        {
            if (CheckSelectorForCard(playerIndex, playCards[i].globalIdx, false)) { return true; }
        }

        return false;
    }

    private void SortHand( int playerIndex )
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

    private void ResetStats( int cardIndex )
    {
        var card = playCards[cardIndex];

        card.attack = card.GetLibCard().attack;
        card.health = card.GetLibCard().health;
        card.actions = card.GetLibCard().moveRange;
        card.tap = 0;
    }

    [RPC]
    public void ActionFoF( int playerIndex, int ownCardIndex, int oppCardIndex )
    {
        if (effectInProgess) { return; }

        int damage = 0;
        var player = players[playerIndex];
        var opponent = players[(playerIndex + 1) % 2];

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

        /*
        for (int i = 0; i < playCards.Count; i++)
        {
            if (playCards[i].pile == PlayCard.Pile.field && playCards[i].owner == opponent.playerId)
            {
                if (playCards[i].GetLibCard().race == LibraryCard.Race.protective)
                {
                    if (CalcDistance(ownCard.pos, playCards[i].pos) <= ownCard.GetLibCard().atkRange)
                    {
                        if (!(oppLibCard.race == LibraryCard.Race.protective))
                        {
                            SendNotification(playerIndex, "Must attack protective units if possible");
                            return;
                        }
                    }
                }
            }
        }
        /**/

        if (!SelectorProtective(playerIndex, oppCardIndex, ownCardIndex)) { return; }

        //basic damage dealt to cards
        damage += oppCard.tap > 0 ? (ownCard.attack + 1) : ownCard.attack;

        //WINGED ABILITY DAMAGE MODIFIER
        damage += ((oppLibCard.race == LibraryCard.Race.winged || oppLibCard.race == LibraryCard.Race.skyC || oppLibCard.race == LibraryCard.Race.bliC) && ownLibCard.atkRange == 1) ? (-1) : 0;
        //TOUGH ABILITY DAMAGE MODIFIER
        damage += ((oppLibCard.race == LibraryCard.Race.tough || oppLibCard.race == LibraryCard.Race.skyC || oppLibCard.race == LibraryCard.Race.dreC) && ownLibCard.atkRange > 1) ? (-1) : 0;


        //oppCard.health -= oppCard.tap > 0 ? (ownCard.attack+1) : ownCard.attack;
        if (oppCard.libId != 974)
        {
            oppCard.health -= damage;
        }
        else if (oppCard.libId == 974)
        {
            DamagePlayer(players[oppCard.owner].playerId, damage);
            //players[oppCard.owner].playerHealth -= damage;
        }

        ownCard.actions--;

        //EXPIREMENTAL; COULD BUG RETALIATE DAMAGE
        damage = oppCard.attack;
        //WINGED ABILITY DAMAGE MODIFIER
        damage += ((ownLibCard.race == LibraryCard.Race.winged || ownLibCard.race == LibraryCard.Race.skyC || ownLibCard.race == LibraryCard.Race.bliC) && oppLibCard.atkRange == 1) ? (-1) : 0;
        //TOUGH ABILITY DAMAGE MODIFIER
        damage += ((ownLibCard.race == LibraryCard.Race.tough || ownLibCard.race == LibraryCard.Race.skyC || ownLibCard.race == LibraryCard.Race.dreC) && oppLibCard.atkRange > 1) ? (-1) : 0;

        if ((oppLibCard.atkRange >= ownLibCard.atkRange) || (distance == 1))
        {
            //STEALTHY RACE ABILITY; NOT TESTED YET
            /*if (ownCard.GetLibCard().race == LibraryCard.Race.stealthy && !(oppCard.GetLibCard().race == LibraryCard.Race.stealthy))
            {
                if (oppCard.health <= 0)
                {
                    Debug.Log("Stealthy activated, no retaliate");
                }*/
            //else
            {
                if (ownCard.libId != 974)
                {
                    ownCard.health -= damage;
                }
                else if (ownCard.libId == 974)
                {
                    DamagePlayer(players[ownCard.owner].playerId, damage);
                }
            }
            //}
        }

        if (ownCard.actions <= 0) { ownCard.tap++; }

        if (oppCard.health <= 0)
        {
            if (ownCard.GetLibCard().race == LibraryCard.Race.brutal || ownCard.GetLibCard().race == LibraryCard.Race.ripC || ownCard.GetLibCard().race == LibraryCard.Race.graC)
            {
                StartCardFx(playerIndex, 100, 0, oppCard.pos);
                brutalOD = ownCard.attack - oppCard.health;
            }

            if (oppCard.GetLibCard().race == LibraryCard.Race.undead || oppCard.GetLibCard().race == LibraryCard.Race.hexC || oppCard.GetLibCard().race == LibraryCard.Race.graC)
            {
                ResetStats(oppCard.globalIdx);
                oppCard.pile = PlayCard.Pile.hand;
                oppCard.pos = 0;
                SortHand(opponent.playerId);
            }
            else
            {
                DiscardCard(opponent.playerId, oppCard.globalIdx);
            }

            player.kills++;
        }

        if (ownCard.health <= 0)
        {
            if (ownCard.GetLibCard().race == LibraryCard.Race.undead || ownCard.GetLibCard().race == LibraryCard.Race.hexC || ownCard.GetLibCard().race == LibraryCard.Race.graC)
            {
                ResetStats(ownCard.globalIdx);
                ownCard.pile = PlayCard.Pile.hand;
                ownCard.pos = 0;
                SortHand(playerIndex);
            }
            else
            {
                DiscardCard(player.playerId, ownCard.globalIdx);
            }
            //player.kills++;
        }

        SendGameManager();
    }

    public bool SelectorProtective( int playerIndex, int oppCardIndex, int ownCardIndex = 0 )
    {
        var player = players[playerIndex];
        var opponent = players[(playerIndex + 1) % 2];

        var ownCard = playCards[ownCardIndex];
        var ownLibCard = CardLibrary.Get().GetCard(ownCard.libId);
        var oppCard = playCards[oppCardIndex];
        var oppLibCard = CardLibrary.Get().GetCard(oppCard.libId);

        if (ownCardIndex != 0)
        {
            for (int i = 0; i < playCards.Count; i++)
            {
                if (playCards[i].pile == PlayCard.Pile.field && playCards[i].owner == opponent.playerId)
                {
                    if (playCards[i].GetLibCard().race == LibraryCard.Race.protective || playCards[i].GetLibCard().race == LibraryCard.Race.ripC)
                    {
                        if (CalcDistance(ownCard.pos, playCards[i].pos) <= ownCard.GetLibCard().atkRange)
                        {
                            if (!(oppLibCard.race == LibraryCard.Race.protective))
                            {
                                SendNotification(playerIndex, "Must attack protective units if possible");
                                return false;
                            }
                        }
                    }
                }
            }
        }

        if (ownCardIndex == 0)
        {
            for (int i = 0; i < playCards.Count; i++)
            {
                if (playCards[i].pile == PlayCard.Pile.field && playCards[i].owner == opponent.playerId)
                {
                    if (playCards[i].GetLibCard().race == LibraryCard.Race.protective)
                    {
                        if (!(oppLibCard.race == LibraryCard.Race.protective))
                        {
                            SendNotification(playerIndex, "Must target protective units if possible");
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }


    [RPC]
    public void ActionFoP( int playerIndex, int cardIndex, int attackedPlayer )
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

        DamagePlayer(opponent.playerId, ownCard.attack);
        //opponent.playerHealth -= ownCard.attack;
        ownCard.actions--;

        if (ownCard.actions <= 0) { ownCard.tap++; }

        SendGameManager();
    }

    [RPC]
    public void MoveOnField( int playerIndex, int cardIndex, int slotIndex )
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

            if (swapCard.actions < distance)
            {
                SendNotification(playerIndex, "Card does not have enough actions left");
                return;
            }
            if (swapCard.tap > 0)
            {
                SendNotification(playerIndex, "Cannot swap if either target is tapped");
                return;
            }

            swapCard.actions -= distance;
            if (swapCard.actions <= 0) { swapCard.tap++; }
            swapCard.pos = card.pos;
        }


        card.actions -= distance;
        card.pos = slotIndex;
        if (card.actions <= 0)
        {
            controller.SelectCard(card.globalIdx);
            card.tap++;
        }


        SendGameManager();
    }

    [RPC]
    public void SacCard( int playerIndex, int cardIndex )
    {
        var player = players[playerIndex];
        int maxCosts = 0;

        if (player.spawns <= 0)
        {
            SendNotification(playerIndex, "Can't sacrifice card, no spawns left");
            return;
        }

        if (playCards[cardIndex].owner != player.playerId)
        {
            SendNotification(playerIndex, "Can sacrifice own cards only");
            return;
        }

        Debug.Log(playCards[cardIndex].pile + ", " + playCards[cardIndex].owner + ", " + localPlayerId);

        if (playCards[cardIndex].pile != PlayCard.Pile.field)
        {
            SendNotification(playerIndex, "Can sacrifice cards in play only");
            return;
        }

        for (int i = 0; i < playCards.Count; i++)
        {
            if (playCards[i].pile == PlayCard.Pile.hand && playCards[i].owner == player.playerId)
            {
                maxCosts += playCards[i].GetLibCard().costs;
            }
        }

        Debug.Log("Sac check: " + maxCosts + ", " + player.sac);

        /*if (player.sac >= maxCosts)
        {
            SendNotification(playerIndex, "Can't sacrifice more cards, no cards with sufficient cost found in hand");
            return;
        }*/

        if (playCards[cardIndex].tap > 0)
        {
            SendNotification(playerIndex, "Can't sacrifice tapped cards");
            return;
        }

        //sacList.Add(playCards[cardIndex]);
        //playCards[cardIndex].pile = PlayCard.Pile.discard;
        playCards[cardIndex].saced = true;
        player.sac++;
        Debug.Log(player.sac);

        SendGameManager();
    }

    [RPC]
    public void BuffCard( int playerIndex, int cardIndex, int stat, int amount )
    {
        var player = players[playerIndex];
        var card = playCards[cardIndex];

        if (cardIndex == null) { return; }

        switch (stat)
        {
            case 0:
                card.health += amount;
                break;

            case 1:
                card.attack += amount;
                break;

            case 2:
                card.actions += amount;
                if ((card.actions > 0) && (card.tap > 0))
                {
                    card.tap--;
                }
                break;

            default:
                Debug.LogError("Error accessing specified card stat; please code the game correctly, retard");
                break;
        }

        Debug.Log(card.attack);
        SendGameManager();
    }
    [RPC]
    public void BuffCardMulti( int playerIndex, int cardIndex, int stat, int amount, int targets = 0 )
    {

        Debug.Log(targets);
        switch (targets)
        {
            case 0:
                BuffCard(playerIndex, cardIndex, stat, amount);
                break;

            case 1:
                for (int i = 0; i < playCards.Count; i++)
                {
                    if (playCards[i].owner == localPlayerId && playCards[i].pile == PlayCard.Pile.field)
                    {
                        BuffCard(playerIndex, playCards[i].globalIdx, stat, amount);
                    }
                }
                break;

            case 2:
                for (int i = 0; i < playCards.Count; i++)
                {
                    if (playCards[i].pile == PlayCard.Pile.field)
                    {
                        BuffCard(playerIndex, playCards[i].globalIdx, stat, amount);
                    }
                }
                break;

            default:
                Debug.LogError("Error accessing specified card stat");
                break;
        }

        SendGameManager();
    }

    [RPC]
    public void EndTurn( int playerIndex )
    {
        if (effectInProgess) { return; }
        if (players[playerIndex].sac > 0) { SendNotification(playerIndex, "Sacrificial rite in progess, can't end turn"); return; }

        var controller = FieldController.GetFieldController();

        //check if incoming player's turn; comment for cheating
        if (playerIndex != turnPlayer)
            return;

        var oldPlayer = players[turnPlayer];

        turnPlayer++;
        turnPlayer %= 2;

        var newPlayer = players[turnPlayer];


        //EFFECT TRIGGERS

        for (int i = 0; i < playCards.Count; i++)
        {
            if (playCards[i].pile == PlayCard.Pile.field)
            {
                #region Colossi EndTurn FX
                //Skyfolk Colossus
                if (playCards[i].libId == 950) { if (CountCards(playerIndex, PlayCard.Pile.field) < (CountCards((playerIndex + 1) % 2, PlayCard.Pile.field))) { StartCardFx(playerIndex, 200, 0, -1, playCards[i]); } }

                //Hexfin Colossus
                if (playCards[i].libId == 951)
                {
                    int own = 0;
                    int opp = 0;

                    for (int j = 0; j < playCards.Count; j++)
                    {
                        if ((playCards[j].owner == playerIndex && playCards[j].pile == PlayCard.Pile.field) && (playCards[j].health == playCards[j].GetLibCard().health))
                        {
                            own++;
                        }
                        if ((playCards[j].owner == (playerIndex + 1) % 2 && playCards[j].pile == PlayCard.Pile.field) && (playCards[j].health == playCards[j].GetLibCard().health))
                        {
                            opp++;
                        }
                    }

                    //Debug.Log("hexfin col debug: " + opp + " " + own);

                    if (opp > own)
                    {
                        StartCardFx(playerIndex, 200, 0, -1, playCards[i]);
                    }
                }

                //Ripjaw Colossus
                if (playCards[i].libId == 952)
                {
                    int own = 0;
                    int opp = 0;

                    for (int j = 0; j < playCards.Count; j++)
                    {
                        if ((playCards[j].owner == playerIndex && playCards[j].pile == PlayCard.Pile.field) && (playCards[j].GetLibCard().costs > 0))
                        {
                            own++;
                        }
                        if ((playCards[j].owner == (playerIndex + 1) % 2 && playCards[j].pile == PlayCard.Pile.field) && (playCards[j].GetLibCard().costs > 0))
                        {
                            opp++;
                        }
                    }

                    //Debug.Log("ripjaw col debug: " + opp + " " + own);

                    if (opp > own)
                    {
                        StartCardFx(playerIndex, 200, 0, -1, playCards[i]);
                    }
                }

                //Graveborn Colossus
                if (playCards[i].libId == 953)
                {
                    if (players[playerIndex].playerHealth < players[(playerIndex + 1) % 2].playerHealth) { StartCardFx(playerIndex, 200, 0, -1, playCards[i]); }
                }

                //Dreadbulge Colossus
                if (playCards[i].libId == 954)
                {
                    int own = 0;
                    int opp = 0;

                    for (int j = 0; j < playCards.Count; j++)
                    {
                        if ((playCards[j].owner == playerIndex && playCards[j].pile == PlayCard.Pile.field) && (playCards[j].health < playCards[j].GetLibCard().health))
                        {
                            own++;
                        }
                        if ((playCards[j].owner == (playerIndex + 1) % 2 && playCards[j].pile == PlayCard.Pile.field) && (playCards[j].health < playCards[j].GetLibCard().health))
                        {
                            opp++;
                        }
                    }

                    //Debug.Log("dread col debug: " + opp + " " + own);

                    if (opp > own)
                    {
                        StartCardFx(playerIndex, 200, 0, -1, playCards[i]);
                    }
                }

                //Blightbark Colossus
                if (playCards[i].libId == 955)
                {
                    int own = 0;
                    int opp = 0;

                    for (int j = 0; j < playCards.Count; j++)
                    {
                        if ((playCards[j].owner == playerIndex && playCards[j].pile == PlayCard.Pile.field) && (playCards[j].tap > 0))
                        {
                            own++;
                        }
                        if ((playCards[j].owner == (playerIndex + 1) % 2 && playCards[j].pile == PlayCard.Pile.field) && (playCards[j].tap > 0))
                        {
                            opp++;
                        }
                    }

                    if (opp > own)
                    {
                        StartCardFx(playerIndex, 200, 0, -1, playCards[i]);
                    }
                }

                //Pitkin Colossus
                if (playCards[i].libId == 956)
                {
                    if (CountCards(playerIndex, PlayCard.Pile.hand) < (CountCards((playerIndex + 1) % 2, PlayCard.Pile.hand))) { StartCardFx(playerIndex, 200, 0, -1, playCards[i]); }
                }
                #endregion
                #region GODS ENDTURNFX
                //Vilerose, God of Lust
                if (playCards[i].libId == 972)
                {
                    for (int j = 0; j < playCards.Count; j++)
                    {
                        if (playCards[j].pile == PlayCard.Pile.field && playCards[j].owner == playCards[i].owner)
                        {
                            playCards[j].health += 2;
                            if (playCards[j].health > playCards[j].GetLibCard().health) { playCards[j].health = playCards[j].GetLibCard().health; }
                        }
                    }

                    DamagePlayer(playCards[i].owner, -2);
                }
                //Rashbite, Gluttony
                if (playCards[i].libId == 975)
                {
                    Debug.Log("Rashbite found: ");
                    for (int j = 0; j < playCards.Count; j++)
                    {
                        if (playCards[j].pile == PlayCard.Pile.field && playCards[j].owner != playCards[i].owner)
                        {
                            Debug.Log("Rashbite check: " + (playCards[j].tap > 0) + "|" + (playCards[j].health < playCards[j].GetLibCard().health));
                            if (playCards[j].tap > 0 && playCards[j].health < playCards[j].GetLibCard().health)
                            {
                                DiscardCard(playCards[j].owner, playCards[j].globalIdx);
                            }
                        }
                    }
                }
                #endregion
            }
        }

        //--------------------------------------------------------


        for (int i = 0; i < playCards.Count; i++)
        {
            var card = playCards[i];
            card.actions = card.GetLibCard().moveRange;
            if (playCards[i].libId != 974) { card.attack = card.GetLibCard().attack; }
            if (card.tap <= 0) { continue; }
            if (card.owner != newPlayer.playerId) { continue; }
            card.tap--;
        }

        //Dullmoor, God of Sloth
        for (int i = 0; i < playCards.Count; i++)
        {
            if (playCards[i].libId == 976)
            {
                Debug.Log("Dullmoor found: ");
                for (int j = 0; j < playCards.Count; j++)
                {
                    if (playCards[j].pile == PlayCard.Pile.field && playCards[j].owner != playCards[i].owner)
                    {
                        playCards[j].actions = 1;
                    }
                }
            }
        }

        for (int i = 0; i < playCards.Count; i++)
        {
            var card = playCards[i];
            Debug.Log("WALL OF TEXT: " + card.saced);
            if (card.saced == true)
            {
                card.saced = false;
                //DiscardCard(card.owner, card.globalIdx);
            }
        }

        newPlayer.spawns = 2;
        newPlayer.monument = true;

        for (int i = 0; i < playCards.Count; i++)
        {
            if (playCards[i].pile == PlayCard.Pile.field)
            {
                #region Leader StartTurn FX
                //Skyfolk Leader - Archbishop Belle-Dhin
                if (playCards[i].libId == 964)
                {
                    StartCardFx(playerIndex, 300);
                    Debug.Log("belledhin fx: " + playCards[i].owner + " " + newPlayer.playerId);
                    if (playCards[i].owner == newPlayer.playerId)
                    {
                        StartCardFx(newPlayer.playerId, 364);
                    }
                }
                //Hexfin Leader - First Mistress Salina
                if (playCards[i].libId == 965)
                {
                    StartCardFx(playerIndex, 301);
                    Debug.Log("salina fx: " + playCards[i].owner + " " + newPlayer.playerId);
                    if (playCards[i].owner == newPlayer.playerId)
                    {
                        Debug.Log("effect trigger check ");
                        StartCardFx(newPlayer.playerId, 365);
                    }
                }
                //Ripjaw Leader - Ragelord Zarkhul
                if (playCards[i].libId == 966)
                {
                    StartCardFx(playerIndex, 302);
                    Debug.Log("zarkhul fx: " + playCards[i].owner + " " + newPlayer.playerId);
                    if (playCards[i].owner == newPlayer.playerId)
                    {
                        StartCardFx(newPlayer.playerId, 366);
                    }
                }
                //Graveborn Leader - High Inquisitor Waljakov
                if (playCards[i].libId == 967)
                {
                    StartCardFx(playerIndex, 303);
                    Debug.Log("pole fx: " + playCards[i].owner + " " + newPlayer.playerId);
                    if (playCards[i].owner == newPlayer.playerId)
                    {
                        StartCardFx(newPlayer.playerId, 367);
                    }
                }
                //Dreadbulge Leader - Great Devourer Gilgamosh
                if (playCards[i].libId == 968)
                {
                    StartCardFx(playerIndex, 304);
                    Debug.Log("gilga fx: " + playCards[i].owner + " " + newPlayer.playerId);
                    if (playCards[i].owner == newPlayer.playerId)
                    {
                        StartCardFx(newPlayer.playerId, 368);
                    }
                }
                //Blightbark Leader - Yawnbringer Keenu
                if (playCards[i].libId == 969)
                {
                    StartCardFx(playerIndex, 305);
                    Debug.Log("keenu fx: " + playCards[i].owner + " " + newPlayer.playerId);
                    if (playCards[i].owner == newPlayer.playerId)
                    {
                        StartCardFx(newPlayer.playerId, 369);
                    }
                }
                //Pitkin Leader - Master Miser Mikoin
                if (playCards[i].libId == 970)
                {
                    StartCardFx(playerIndex, 306);
                    Debug.Log("mikoin fx: " + playCards[i].owner + " " + newPlayer.playerId);
                    if (playCards[i].owner == newPlayer.playerId)
                    {
                        StartCardFx(newPlayer.playerId, 370);
                    }
                }
                #endregion
                #region GODS START FX
                //Neverfall, God of Pride
                if (playCards[i].libId == 971)
                {
                    Debug.Log("neverfall, god of pride debug log: ");
                    if (playCards[i].owner == newPlayer.playerId)

                        if (CountCards(newPlayer.playerId, PlayCard.Pile.field) > CountCards(oldPlayer.playerId, PlayCard.Pile.field))
                        {
                            gameOver = newPlayer.playerId;
                        }
                }
                //Flamegrim, God of Wrath
                if (playCards[i].libId == 973)
                {
                    for (int j = 0; j < playCards.Count; j++)
                    {
                        Debug.Log("flamegrim debug: " + playCards[j].owner + " " + playCards[i].owner);
                        if (playCards[j].pile == PlayCard.Pile.field && playCards[j].owner != playCards[i].owner)
                        {
                            playCards[j].health -= 2;
                            Debug.Log("health reduced? " + playCards[j].health);
                        }
                    }
                }
                //Dullmoor, God of Sloth
                if (playCards[i].libId == 976)
                {
                    for (int j = 0; j < playCards.Count; j++)
                    {
                        if (playCards[j].pile == PlayCard.Pile.field && playCards[j].owner != playCards[i].owner)
                        {
                            playCards[j].actions = 0;
                            //playCards[j].tap++;
                        }
                    }
                }           
                //Skinflint, God of Greed
                if (playCards[i].libId == 977)
                {
                    for (int j = 0; j < playCards.Count; j++)
                    {
                        Debug.Log("skinflint fx check: " + players[playCards[i].owner].spawns);
                        if(players[playCards[i].owner].spawns < 3)
                        {
                            if (playCards[i].owner == newPlayer.playerId)
                            {
                                newPlayer.spawns += 1;
                                Debug.Log("skinflint fx check 2: " + players[playCards[i].owner].spawns);
                            }
                        }
                    }
                }
                #endregion
            }
        }

        /*for (int i = 0; i < playCards.Count; i++)
        {
            if (playCards[i].globalIdx == controller.cardSelected)
            {
                controller.SelectCard(playCards[i].globalIdx);
            }
        }
        /**/

        for (int i = 0; i < playCards.Count; i++)
        {
            if (playCards[i].pile == PlayCard.Pile.field && playCards[i].pos == 10)
            {
                if (newPlayer.playerId == playCards[i].owner)
                {
                    DrawCard(newPlayer.playerId, 1);
                }
            }
        }

        Debug.Log("Kills " + oldPlayer.kills);
        //remove
        oldPlayer.kills = 4;
        newPlayer.kills = 4;
        DrawCard(newPlayer.playerId, 1);

        /* if (oldPlayer.sac > 0)
         {
             FieldController.GetFieldController().HideSacFieldsAll();
         }*/


        /*for (int i = 0; i < playCards.Count; i++ )
        {
            if (playCards[i].pile == PlayCard.Pile.field && playCards[i].owner == localPlayerId) { controller.SelectCard(playCards[i].globalIdx); }           
        }
         * */

        Debug.Log(turnPlayer);
        SendGameManager();
    }

    //SWITCH FILE PATH
    [RPC]
    public void SaveGame()
    {
        File.WriteAllText("D:/ProtoTest/Assets/game.json", ToJSON().ToString(), Encoding.UTF8);
    }

    [RPC]
    public void LoadGame()
    {
        string textFile = File.ReadAllText("D:/ProtoTest/Assets/game.json", Encoding.UTF8);
        JSONObject jsGameState = JSONParser.parse(textFile);
        int tmp = gameVersion;
        FromJSON(jsGameState);
        gameVersion = tmp + 1;
        SendGameManager();
    }

    public void FromJSON( JSONObject jsPlayer )
    {
        turnPlayer = (int)jsPlayer["TurnPlayer"];
        playCards = Player.PileFromJSON(jsPlayer["PlayCards"]);
        //sacList = Player.PileFromJSON(jsPlayer["SacrificeList"]);
        currentFx.FromJSON(jsPlayer["CurrentFx"]);
        effectInProgess = (bool)jsPlayer["effectInProgess"];
        gameVersion = (int)jsPlayer["gameVersion"];
        players[0].FromJSON(jsPlayer["Player0"]);
        players[1].FromJSON(jsPlayer["Player1"]);
    }

    public JSONObject ToJSON()
    {
        JSONObject jsPlayer = JSONObject.obj;
        jsPlayer.AddField("TurnPlayer", turnPlayer);
        jsPlayer.AddField("PlayCards", Player.PileToJSON(playCards));
        //jsPlayer.AddField("SacrificeList", Player.PileToJSON(sacList));
        jsPlayer.AddField("CurrentFx", currentFx.ToJSON());
        jsPlayer.AddField("effectInProgess", effectInProgess);
        jsPlayer.AddField("gameVersion", gameVersion);
        jsPlayer.AddField("Player0", players[0].ToJSON());
        jsPlayer.AddField("Player1", players[1].ToJSON());
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
        //this.NetRPC("ReceivePlayer", RPCMode.All, 0, players[0].ToJSON().ToString());
        //this.NetRPC("ReceivePlayer", RPCMode.All, 1, players[1].ToJSON().ToString());
    }

    [RPC]
    public void ReceiveGameManager( byte[] manager )
    {
        Debug.Log("Receive()" + System.Text.UTF8Encoding.UTF8.GetString(manager));
        FromJSON(JSONParser.parse(System.Text.UTF8Encoding.UTF8.GetString(manager)));
    }

    public void SendNotification( int playerIndex, string message )
    {
        Debug.Log("SendNotification " + playerIndex + " " + message);
        for (int i = 0; i < players.Length; i++)
        {
            if (playerIndex != players[i].playerId && playerIndex != -1)
            {
                continue;
            }
            //Debug.Log(">>>>>>>>>>>>>" + playerIndex + " " + i + " " + players[i].networkPlayer);
            this.NetRPC("ReceiveNotification", players[i].networkPlayer, message);
        }
    }
    [RPC]
    public void ReceiveNotification( string message )
    {
        notifTime = Time.time;
        notification = message;
    }

    public int CalcDistance( int slotA, int slotB )
    {
        if (slotA == slotB) { return 0; }
        Vector2 a = GetSlotPos(slotA);
        Vector2 b = GetSlotPos(slotB);

        return (int)(a - b).magnitude;
    }

    public Vector2 GetSlotPos( int slot )
    {
        int x = slot / 3;
        int y = slot % 3;

        return new Vector2(x, y);
    }
}

public class Player
{
    public int playerId = -1;
    public string name;
    public int playerHealth;
    public int spawns;
    public int sac;
    public int kills;
    public bool win;
    public bool monument;
    public string cult;
    public List<LibraryCard> deck = new List<LibraryCard>();
    /*public List<PlayCard> playPile = new List<PlayCard>();
    public List<PlayCard> playHand = new List<PlayCard>();
    public List<PlayCard> discardPile = new List<PlayCard>();
    public List<PlayCard> field = new List<PlayCard>();*/
    public NetworkPlayer networkPlayer;
    //static int globalIdx = 0;

    public void BuildPlayPile()
    {

        List<PlayCard> playPile = new List<PlayCard>();
        for (int i = 0; i < deck.Count; i++)
        {
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


    public void FromJSON( JSONObject jsPlayer )
    {
        playerId = (int)jsPlayer["PlayerId"];
        name = (string)jsPlayer["Name"];
        playerHealth = (int)jsPlayer["PlayerHealth"];
        spawns = (int)jsPlayer["Spawns"];
        sac = (int)jsPlayer["Sacrifice"];
        kills = (int)jsPlayer["kills"];
        win = (bool)jsPlayer["win"];
        monument = (bool)jsPlayer["monument"];
        cult = (string)jsPlayer["cult"];
        deck = DeckBuilder.DeckFromJSON(jsPlayer["Deck"]);

        /*playPile = PileFromJSON(jsPlayer["PlayPile"]);
        playHand = PileFromJSON(jsPlayer["PlayHand"]);
        discardPile = PileFromJSON(jsPlayer["DiscardPile"]);
        field = PileFromJSON(jsPlayer["Field"]);*/
    }

    public JSONObject ToJSON()
    {
        JSONObject jsPlayer = JSONObject.obj;
        jsPlayer.AddField("Name", name);
        jsPlayer.AddField("PlayerId", playerId);
        jsPlayer.AddField("PlayerHealth", playerHealth);
        jsPlayer.AddField("Spawns", spawns);
        jsPlayer.AddField("Sacrifice", sac);
        jsPlayer.AddField("kills", kills);
        jsPlayer.AddField("win", win);
        jsPlayer.AddField("monument", monument);
        jsPlayer.AddField("cult", cult);
        jsPlayer.AddField("Deck", DeckBuilder.DeckToJSON(deck));

        /*jsPlayer.AddField("PlayPile", PileToJSON(playPile));
        jsPlayer.AddField("PlayHand", PileToJSON(playHand));
        jsPlayer.AddField("DiscardPile", PileToJSON(discardPile));
        jsPlayer.AddField("Field", PileToJSON(field));*/
        return jsPlayer;
    }

    public static JSONObject PileToJSON( List<PlayCard> pile )
    {
        JSONObject jsPile = JSONObject.arr;
        for (int i = 0; i < pile.Count; i++)
        {
            jsPile.Add(pile[i].ToJSON());
        }
        return jsPile;
    }

    public static List<PlayCard> PileFromJSON( JSONObject jsPile )
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
    /*public int FindCardPlayPile(int cardIndex) { return FindCard(playPile, cardIndex); }
    public int FindCardHand(int cardIndex) { return FindCard(playHand, cardIndex); }
    public int FindCardField(int cardIndex) { return FindCard(field, cardIndex); }
    public int FindCardDiscard(int cardIndex) { return FindCard(discardPile, cardIndex); }*/
}
