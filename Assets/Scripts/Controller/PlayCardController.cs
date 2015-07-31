using UnityEngine;
using System;
using System.Collections;

public class PlayCardController : MonoBehaviour {

    //public Transform target;
    bool popup = false;
    public int cardIndex;
    public bool slot = false;
    public bool turned = false;
    public int pos;

    public Vector3 moveFrom;
    public Vector3 moveTo;
    public float moveStart = 0;
    public float moveDuration = 0;
    public float tapStart = 0;
    const float tapDuration = 0.4f;
    public bool tapTap = false;
    

    public PlayCard.Pile pile;
    public PlayCard card;
	// Use this for initialization
	void Awake () 
    {
        cardIndex = -1;
        pos = -1;
        //target = GameObject.Find("SceneCam").transform;
        this.pile = PlayCard.Pile.none;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (moveStart > 0)
        {
            UpdateMoveAnimation();
        }

        if (tapStart > 0)
        {
            UpdateTapAnimation();
        }
	}

    public FieldController GetFieldController()
    {
        return FieldController.GetFieldControler();
    }

    void OnMouseOver()
    {
        //if (cardIndex < 0) { return; }
        GameObject parentObj = gameObject.transform.parent.gameObject;
        var fieldController = GetFieldController();
        var card = cardIndex >= 0 ? GameManager.Get().playCards[cardIndex] : null;
        int playerId = GameManager.Get().localPlayerId;

        if (Input.GetButtonDown("Fire1"))
        {
            //Debug.Log("f1:" + parentObj.name+" "+card.owner);
            if (parentObj.name == "Field" || parentObj.name == "Hand")
            {
                if (card!=null && card.owner == playerId)
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
                //if (card.owner == playerId)
                {
                    fieldController.OnSlotClicked(slotNumber);
                }
            }

            /*if (parentObj.name == "Hand")
            {
                if (card != null && card.owner == playerId)
                {
                    Debug.Log("Card found");
                    fieldController.OnHandClicked(cardIndex);
                }
            }*/

            if (parentObj.name == "Field")
            {
                if (card != null)
                {
                    Debug.Log("Field clicked " + cardIndex);
                    fieldController.OnFieldCardClicked(cardIndex);
                }
            }
        }

        if (pile == PlayCard.Pile.hand || pile == PlayCard.Pile.field)
        {
            PopUp();
        }      
    }

    void OnMouseExit()
    {
        if (slot) { return; }
        GetFieldController().HideCardPreview();
        transform.FindChild("hoverGlow").gameObject.SetActive(false);
    }

    void PopUp()
    {
        if (slot) { return; }
        GetFieldController().ShowCardPreview(transform.position.x > 0, card);
        transform.FindChild("hoverGlow").gameObject.SetActive(true);
    }

    public void StartMoveAnimation(Vector3 to, float duration)
    {
        moveFrom = transform.localPosition;
        moveTo = to;
        moveDuration = duration;
        moveStart = Time.time;
    }

    public void UpdateMoveAnimation()
    {
        if (Time.time > moveStart + moveDuration)
        {
            transform.localPosition = moveTo;
            moveStart = 0;
            return;
        }

        float moveProgress = (Time.time - moveStart) / moveDuration;
        transform.localPosition = Vector3.Lerp(moveFrom, moveTo, moveProgress);
    }
    public bool IsMoveAnimating()
    {
        return (Time.time <= moveStart + moveDuration);
    }

    public void StartTapAnimation(bool tap)
    {
        if (tap == tapTap) { return; }

        tapTap = tap;
        tapStart = Time.time;
    }

    public void UpdateTapAnimation()
    {
        Quaternion tapTo = tapTap ? Quaternion.EulerAngles(0, (Mathf.PI / 2), 0) : Quaternion.EulerAngles(0, 0, 0);
        Quaternion tapFrom = !tapTap ? Quaternion.EulerAngles(0, (Mathf.PI / 2), 0) : Quaternion.EulerAngles(0, 0, 0);

        if (Time.time > tapStart + tapDuration)
        {
            transform.localRotation = tapTo;
            tapStart = 0;
            return;
        }

        float tapProgress = (Time.time - tapStart) / tapDuration;
        transform.localRotation = Quaternion.Slerp(tapFrom, tapTo, tapProgress);
    }
    public bool IsTapAnimating()
    {
        return (Time.time <= tapStart + tapDuration);
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
