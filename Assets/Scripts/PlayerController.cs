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

    public bool allowMove = true;
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


        if (!isLocalPlayer)
        {
            return;
        }

        if (game.turnId == game.turn)
        {
            allowMove = true;

            if (Input.GetButtonDown("switch"))
            {
                endTurn(game.turn);
            }
        }

        if (allowMove == true)
        {
            Move();
        }

        Debug.Log(allowMove);

    }
    public void Move()
    {
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(hori, 0.0f, vert) * speed;

        if (move.magnitude > 0.0001f)
        {
            transform.position += move * speed * Time.deltaTime;
        }
    }

    public void endTurn(int turn)
    {
            allowMove = false;

            //turn 0 = server, turn 1 = client
            if (turn == 1)
                game.turn = 0;

            if (turn == 0)
                game.turn = 1;
    }
}


