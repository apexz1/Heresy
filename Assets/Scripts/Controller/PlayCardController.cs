using UnityEngine;
using System;
using System.Collections;

public class PlayCardController : MonoBehaviour {

    //public Transform target;
    bool popup = false;
    public int cardIndex;
    public bool slot = false;
    public bool turned = false;
    public PlayCard.Pile pile;
    public PlayCard card;
	// Use this for initialization
	void Awake () 
    {
        cardIndex = -1;
        //target = GameObject.Find("SceneCam").transform;
        this.pile = PlayCard.Pile.invalid;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public FieldController GetFieldController()
    {
        return FieldController.GetFieldControler();
    }

    void OnMouseOver()
    {
        if (cardIndex < 0) { return; }
        GameObject parentObj = gameObject.transform.parent.gameObject;
        var fieldController = GetFieldController();
        var card = GameManager.Get().playCards[cardIndex];
        int playerId = GameManager.Get().localPlayerId;

        if (Input.GetButtonDown("Fire1"))
        {
            if (parentObj.name == "Field" || parentObj.name == "Hand")
            {
                if (card.owner == playerId)
                {
                    Debug.Log("Select clicked " + cardIndex);
                    fieldController.SelectCard(cardIndex);
                }
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (slot)
            {
                int slotNumber = Int32.Parse(gameObject.name);
                if (card.owner == playerId)
                {
                    fieldController.OnSlotClicked(slotNumber);
                }
            }

            if (parentObj.name == "Hand")
            {
                if (card.owner == playerId)
                {
                    Debug.Log("Card found");
                    fieldController.OnHandClicked(cardIndex);
                }
            }

            if (parentObj.name == "Field")
            {
                //if (fieldController.playerId == playerId)
                {
                    Debug.Log("Field clicked " + cardIndex);
                    fieldController.OnFieldCardClicked(cardIndex);
                }
            }
        }

        if (gameObject.transform.parent.gameObject.name == "Hand")
            PopUp();
    }

    void OnMouseExit()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    void PopUp()
    {
        //transform.GetChild(0).localRotation = Quaternion.Euler(-120,0,0);
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
    }

    public void TurnCard(bool turn)
    {
        if (turn)
        {
            transform.GetChild(0).localRotation = Quaternion.EulerAngles(Mathf.PI, 0, 0);
            turned = true;
        }
        else
        {
            transform.GetChild(0).localRotation = Quaternion.EulerAngles(0, 0, 0);
            turned = false;
        }
    }       
}
