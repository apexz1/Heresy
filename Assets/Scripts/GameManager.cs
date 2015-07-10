using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

    public Card card = new Card();

    [SyncVar]
    public int speed;
    [SyncVar]
    public bool currentTurn;
    [SyncVar]
    public bool turnId;
    public bool allowMove = true;
    int cardID;
    string cardName;

    void Awake()
    {
        //Debug.Log(card);

        cardID = card.GetID();
        cardName = card.GetName();
        currentTurn = false;

        Debug.Log(GameObject.Find("GameManager"));

        //Debug.Log(cardID);
        //Debug.Log(cardName);
    }
	void Start () 
    {
        if (isServer)
        {
            turnId = true;
        }

        if (!isServer)
        {
            turnId = false;
        }

        Debug.Log(turnId);

        //DEBUGGING ONLY; checking if the turn progression script is working - Working as intended: 07/07/14, 12:40
        /*for (int i = 0; i < 100; i++)
        {
            Debug.Log(state.GetCount() + "/" + state.GetTurn() + "/" + state.GetPhase());
            state.SetState();
        }*/
	}
	
	void Update ()
    {

    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Player connected from" + player.ipAddress + ":" + player.port);
    }
}
