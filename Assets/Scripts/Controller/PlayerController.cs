using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : MonoBehaviour
{
    GameManager gameManager;
    void Awake()
    {
        NetworkServer.SpawnObjects();
    }
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        TextAsset textFile = (TextAsset)Resources.Load("default");
        JSONObject jsPlayer = JSONParser.parse(textFile.text);

        Debug.Log("start");
        //CmdAssignDeck((short)netId.Value, jsPlayer["Deck"].ToString());
    }

    //[Command]
    public void CmdAssignDeck(short playerId, string deck)
    {
        gameManager.AssignDeck(playerId, deck);
    }

    public void Move()
    {
    }

    [Command]
    public void CmdMove(float v, float h)
    {
    }

    [ClientRpc]
    public void RpcMove(Vector3 pos)
    {
    }
}