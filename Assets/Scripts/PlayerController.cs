using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    [SyncVar]
    private Rigidbody rb;
    [SyncVar]
    public int speed;
    [SyncVar]
    int turnStorage;

    GameManager game = new GameManager();
    private int inputcount;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputcount = 0;
    }

    void FixedUpdate()
    {
        //Don't know what the fuck I'm doing here, but works. #coding101

        if (!isLocalPlayer)
        {
            return;
        }

        if (game.allowMove == true)
        {
            Move();

            if (Input.GetButtonDown("switch"))
            {
                CmdEndTurn(game.currentTurn);
            }

        }
    }
    public void Move()
    {
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        CmdMove(vert, hori);
    }

    [Command]
    public void CmdMove(float v, float h)
    {
        Vector3 move = new Vector3(h, 0.0f, v) * speed;

        if (move.magnitude > 0.0001f)
        {
            RpcMove(transform.position + move * speed * Time.deltaTime);
        }
    }
    [ClientRpc]
    public void RpcMove(Vector3 pos)
    {
        transform.position = pos;
    }

    [Command]
    public void CmdCheckTurn(bool turnID, bool turn)
    {
        bool check;

        if (game.turnId == game.currentTurn)
        {
            check = true;
        }
        else
        {
            check = false;
        }

        RpcCheckTurn(check);
    }
    [ClientRpc]
    public void RpcCheckTurn(bool check)
    {
        game.allowMove = check;
    }

    [Command]
    public void CmdEndTurn(bool turnid)
    {
        Debug.Log(turnid);
        turnid = !turnid;
        Debug.Log(turnid);
        RpcEndTurn(turnid);
    }
    [ClientRpc]
    public void RpcEndTurn(bool turnid)
    {
        game.currentTurn = turnid;
        Debug.Log(game.currentTurn + "//" + turnid);
        game.allowMove = turnid;
    }
}


