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

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //Don't know what the fuck I'm doing here, but works. #coding101

        if (!isLocalPlayer)
        {
            return;
        }

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
