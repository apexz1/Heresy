using UnityEngine;
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

        players[playerId].deck = DeckBuilder.ParseDeck(JSONParser.parse(deck));
        SendPlayer(playerId);
    }

    public void SendPlayer(int playerIndex)
    {
        RpcReceivePlayer(playerIndex, players[playerIndex].ToJSON().ToString());
    }

    //[ClientRpc]
    public void RpcReceivePlayer(int playerIndex, string player)
    {
        Debug.Log("Receive()" + playerIndex + " | " + player);
        players[playerIndex].FromJSON(JSONParser.parse(player));
    }
}

public class Player
{
    public int playerId = -1;
    public List<Card> deck;
    public string name;

    public void FromJSON(JSONObject jsPlayer)
    {
        playerId = (int)jsPlayer["PlayerId"];
        name = (string)jsPlayer["Name"];
        deck = DeckBuilder.ParseDeck(jsPlayer["Deck"]);
    }

    public JSONObject ToJSON()
    {
        JSONObject jsPlayer = new JSONObject();
        jsPlayer.AddField("Name", name);
        jsPlayer.AddField("PlayerId", playerId);

        JSONObject jsDeck = new JSONObject();
        for (int i = 0; i < deck.Count; i++)
        {
            jsDeck.Add(deck[i].GetID());
        } 

        jsPlayer.AddField("Deck", jsDeck);

        return jsPlayer; 
    }
}
