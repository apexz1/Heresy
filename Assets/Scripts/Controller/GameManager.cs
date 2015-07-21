using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;


public class GameManager : MonoBehaviour
{

    Player[] players;
    NetworkView networkView;

    void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Player connected from" + player.ipAddress + ":" + player.port);
        StartGame();
    }

    void Start()
    {
        //Debug.Log(isServer + " | " + isClient + " | " + isLocalPlayer);
        players = new Player[2]; 

        for (int i = 0; i < players.Length; i++)
        {
            players[i] = new Player();
        }

        networkView = GetComponent<NetworkView>();
    }

    [RPC]
    public void StartGame()
    {
        NetworkManager networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        networkManager.enabled = false;

        networkView.RPC("StartGame", RPCMode.Others);
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
        int playerIndex = FindPlayer(playerId);
        Debug.Log("AssignDeck()" + playerId + " : " + playerIndex +  " : " + deck);

        players[playerIndex].deck = DeckBuilder.ParseDeck(JSONParser.parse(deck));
        SendPlayer(playerIndex);
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
    public short playerId = -1;
    public List<Card> deck;
    public string name;

    public void FromJSON(JSONObject jsPlayer)
    {
        playerId = (short)jsPlayer["PlayerId"];
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
