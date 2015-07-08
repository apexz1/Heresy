using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public Card card = new Card();
    GameState state = new GameState();

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
        //DEBUGGING ONLY; checking if the turn progression script is working - Working as intended: 07/07/14, 12:40
        for (int i = 0; i < 10; i++)
        {
            Debug.Log(state.GetCount() + "/" + state.GetTurn() + "/" + state.GetPhase());
            state.SetState();
        }
	}
	
	void Update ()
    {

    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Player connected from" + player.ipAddress + ":" + player.port);
    }

}
