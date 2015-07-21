using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

    string userInput = "localhost";
    int port = 35271;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnGUI()
    {
        if (GUI.Button (new Rect(200,200,120,40), "Host Server"))
        {
            var initServer = Network.InitializeServer(2, port, true);
            Debug.Log(initServer);
        }

        userInput = GUI.TextField(new Rect(200, 250, 100, 25), userInput);

        if (GUI.Button (new Rect(340,200,120,40), "Connect"))
        {
            var initConnection = Network.Connect(userInput, port);
            Debug.Log(initConnection);
        }

        if (GUI.Button (new Rect(480,200,120,40), "Start Game"))
        {
            GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            var initServer = Network.InitializeServer(2, port, true);
            gameManager.StartGame(0, false);
        }
    }

    /*
     * void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Network Player GUID: " + player.guid + " " + player.ToString());
        Debug.Log("Player connected from" + player.ipAddress + ":" + player.port);
    }
     */
}
