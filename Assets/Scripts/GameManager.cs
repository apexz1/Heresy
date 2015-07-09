using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

    public Card card = new Card();

    public static int turnId;
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
	void Start () 
    {
        if (isServer)
        {
            turnId = 0;
        }
        else
        {
            turnId = 1;
        }

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
