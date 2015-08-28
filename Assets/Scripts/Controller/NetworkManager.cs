using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NetworkManager : MonoBehaviour {

    string userInput = "localhost";
    public static string deckChoice = "";
    int port = 35271;
	// Use this for initialization
	void Start () {
        PlayerPrefs.GetString("ip", userInput);
        Debug.Log(userInput);
        //Debug.Log(GameObject.Find("PreGame"));
        //userInput = GameObject.Find("GameUI").transform.FindChild("PreGame").transform.FindChild("ip").gameObject.GetComponent<Text>().text;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnGUI()
    {
        if (GUI.Button (new Rect(200,200,120,40), "Host Server"))
        {
            HostServer();
            //GameObject.Find("Table").SetActive(false);
        }

        userInput = GUI.TextField(new Rect(200, 250, 120, 25), userInput);
        deckChoice = GUI.TextField(new Rect(325, 250, 120, 25), deckChoice);

        if (GUI.Button (new Rect(340,200,120,40), "Connect"))
        {
            Connect();
            //GameObject.Find("Table").SetActive(false);
        }

        if (GUI.Button (new Rect(480,200,120,40), "Start Game"))
        {
            StartGame();
            //GameObject o = GameObject.FindGameObjectWithTag("Table");
            //Debug.Log(o.name);
            //o.gameObject.SetActive(true);
        }
    }
    /**/

    public void HostServer()
    {
        var initServer = Network.InitializeServer(2, port, true);
        Debug.Log(initServer);
    }

    public void Connect()
    {
        InputField inputField = GameObject.Find("GameUI").transform.FindChild("PreGame").FindChild("ip").gameObject.GetComponent<InputField>();
        userInput = inputField.text;

        PlayerPrefs.SetString("ip", userInput);
        PlayerPrefs.Save();
        var initConnection = Network.Connect(inputField.text, port);
        Debug.Log(initConnection);
    }

    public void StartGame()
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        var initServer = Network.InitializeServer(2, port, true);
        gameManager.StartGame(0, false);
    }

    public static NetworkManager Get()
    {
        return GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }
}
