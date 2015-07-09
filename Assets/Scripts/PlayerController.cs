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

    public bool allowMove = true;
    private int inputcount;

    public GameState state = new GameState();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputcount = 0;
    }

    void FixedUpdate()
    {
        //Don't know what the fuck I'm doing here, but works. #coding101

        Debug.Log(turnStorage);
        Debug.Log(GameManager.turnId);

        if (!isLocalPlayer)
        {
            return;
        }

        if (Input.GetButtonDown("switch"))
        {
            inputcount++;

            for (int i = 0; i < 4; i++)
            {
                //Debug.Log(state.GetCount() + "/" + state.GetTurn() + "/" + state.GetPhase());
                //Player 1 = return 0 | Player 2 = return 1;

                turnStorage = state.SetState();

                if (turnStorage != GameManager.turnId)
                {
                    allowMove = false;
                    return;
                    //Application.LoadLevel("menu");
                }
                else
                {
                    allowMove = true;
                }
            }
        }

        Debug.Log(allowMove);
        if (allowMove == true)
        {
            Move();
        }
    }
    public void Move()
    {

        Debug.Log(turnStorage);
        Debug.Log(GameManager.turnId);

        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(hori, 0.0f, vert) * speed;

        if (move.magnitude > 0.0001f)
        {
            transform.position += move * speed * Time.deltaTime;
        }
    }
}


