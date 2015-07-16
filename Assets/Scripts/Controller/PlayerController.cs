using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{

    [SyncVar]
    private Rigidbody rb;
    [SyncVar]
    public int speed;
    [SyncVar]
    int turnStorage;

    public bool allowMove = false;

    GameManager game = new GameManager();
    public Slider slider;
    private int inputcount;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputcount = 0;
    }

    void FixedUpdate()
    {
        //Don't know what the fuck I'm doing here, but works. #coding101
        Debug.Log(game.turnId);
        Debug.Log(game.currentTurn);
        
        if (!isLocalPlayer)
        {
            return;
        }

        if (Input.GetButtonDown("switch"))
        {
            CmdEndTurn(game.currentTurn);
        }

        allowMove = CheckMove();

        if (allowMove == true)
        {
            Move();

        }

    }
    public void Move()
    {
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        CmdMove(vert, hori);
    }

    public bool CheckMove()
    {
        return (game.turnId == game.currentTurn);
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
    public void CmdEndTurn(bool i)
    {
        i = !i;
        RpcEndTurn(i);
    }

    [ClientRpc]
    public void RpcEndTurn(bool j)
    {
        game.currentTurn = j;
    }
}