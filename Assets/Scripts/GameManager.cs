using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{

    public Card card = new Card();

    [SyncVar]
    public int speed;

    public bool turn = false;
    public bool turnId = false;

    int cardID;
    string cardName;

    void Awake()
    {
        //Debug.Log(card);

        cardID = card.GetID();
        cardName = card.GetName();

        //Debug.Log(cardID);
        //Debug.Log(cardName);
    }
    void Start()
    {
        if (isServer)
        {
            turnId = true;
        }

        if (!isServer)
        {
            turnId = false;
        }

        //Debug.Log(turnId);
        //Debug.Log(currentTurn);

        //DEBUGGING ONLY; checking if the turn progression script is working - Working as intended: 07/07/14, 12:40
        /*for (int i = 0; i < 100; i++)
        {
            Debug.Log(state.GetCount() + "/" + state.GetTurn() + "/" + state.GetPhase());
            state.SetState();
        }*/
    }

    void Update()
    {

    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Player connected from" + player.ipAddress + ":" + player.port);
    }

    [Command]
    public void CmdAssignTurn()
    {
        int rnd = Random.Range(0, 9);
        bool currentTurn = false;

        Debug.Log(rnd);

        if (rnd < 5)
            currentTurn = true;
        if (rnd >= 5)
            currentTurn = false;

        RpcAssignTurn(currentTurn);
    }

    [ClientRpc]
    public void RpcAssignTurn(bool i)
    {
        turn = i;
    }
}
