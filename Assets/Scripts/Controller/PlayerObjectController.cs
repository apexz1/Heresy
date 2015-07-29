using UnityEngine;
using System.Collections;

public class PlayerObjectController : MonoBehaviour {


    GameManager gameManager;
    public int playerId;

    public FieldController GetFieldController()
    {
        return FieldController.GetFieldControler();
    }

	// Use this for initialization
	void Start () {
        gameManager = GameManager.Get();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseOver()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            GetFieldController().OnPlayerClicked(playerId);
            Debug.Log("Right mouse button yo");
        }
    }
}
