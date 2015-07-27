using UnityEngine;
using System;
using System.Collections;

public class PlayCardController : MonoBehaviour {

    public Transform target;
    bool popup = false;
    public int globalIdx;
    public bool slot = false;
    public enum Pile
    {
        deck,
        hand,
        field,
        discard
    }
    public Pile pile;
	// Use this for initialization
	void Start () 
    {
        target = GameObject.Find("SceneCam").transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public FieldController GetFieldController()
    {
        return GetComponentInParent<FieldController>();
    }

    void OnMouseOver()
    {
        var fieldController = GetFieldController();
        int playerId = GameManager.Get().localPlayerId;
        GameObject parentObj = gameObject.transform.parent.gameObject;

        if (Input.GetButtonDown("Fire1"))
        {
            if (parentObj.name == "Field" || parentObj.name == "Hand")
            {
                if (fieldController.playerId == playerId)
                {
                    if (globalIdx == 0)
                        return;

                    Debug.Log("Select clicked " + globalIdx);
                    fieldController.SelectCard(globalIdx);
                }
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (slot)
            {
                int slotNumber = Int32.Parse(gameObject.name);
                if (fieldController.playerId == playerId)
                {
                    fieldController.OnSlotClicked(slotNumber);
                }
            }

            if (parentObj.name == "Hand")
            {
                if (fieldController.playerId == playerId)
                {
                    Debug.Log("Card found");
                    fieldController.OnHandClicked(globalIdx);
                }
            }

            if (parentObj.name == "Field")
            {
                if (fieldController.playerId == playerId)
                {
                    if (globalIdx == 0)
                        return;

                    Debug.Log("Field clicked " + globalIdx);
                    fieldController.OnFieldClicked(globalIdx);
                }
            }
        }

        if (gameObject.transform.parent.gameObject.name == "Hand")
            PopUp();
    }

    void PopUp()
    {
        //transform.GetChild(0).localRotation = Quaternion.Euler(-120,0,0);
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
    }
}
