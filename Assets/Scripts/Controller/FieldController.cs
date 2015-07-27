using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldController : MonoBehaviour {

    public int playerId;
    public int cardSelected;
    public Dictionary<int, Transform> cardGfxs=new Dictionary<int,Transform>();
    public Camera cam;

    public void Awake()
    {
        if (gameObject.name == "BottomField")
        {
            playerId = 0;
        }
        else
        {
            playerId = 1;
        }

        cardSelected = -1;
    }

    Transform GetGfx(int globalIndex)
    {
        Transform res=null;
        cardGfxs.TryGetValue(globalIndex, out res);
        return res;
    }

    public void FixedUpdate()
    {
        var player = GameManager.Get().players[playerId];

        for (int i = 0; i < player.playPile.Count; i++)
        {
            var card = player.playPile[i];
            var gfx = GetGfx(card.globalIdx);
            if (gfx != null)
            {
                continue;
            }

            var cardObject = Instantiate((GameObject)Resources.Load("Prefabs/PlayCard"));
            gfx = cardObject.transform;
            gfx.SetParent(transform.Find("PlayPile"), false);

            MeshRenderer rend = gfx.GetChild(0).gameObject.GetComponent<MeshRenderer>();
            //Debug.Log("Card Texture: " + card.GetTexture().ToString());
            rend.material.mainTexture = card.GetTexture();
            //Debug.Log(card.id);
            gfx.localPosition = new Vector3(0, 0.01f * i, 0);
            gfx.GetChild(0).localRotation = Quaternion.EulerAngles(Mathf.PI / 2, 0, 0);
            var controller = cardObject.transform.gameObject.AddComponent<PlayCardController>();
            controller.globalIdx = card.globalIdx;
            cardGfxs[card.globalIdx] = gfx;
            controller.pile = PlayCardController.Pile.deck;
        }

        for (int i = 0; i < player.playHand.Count; i++)
        {
            var card = player.playHand[i];
            var gfx = GetGfx(card.globalIdx);
            var controller = gfx.GetComponent<PlayCardController>();

            gfx.SetParent(transform.Find("Hand"), false);
            gfx.localPosition = new Vector3(i * 2.5f, 0, 0);

            if (isOwn())
                gfx.GetChild(0).localRotation = Quaternion.EulerAngles(-(Mathf.PI / 2), 0, 0);

            controller.pile = PlayCardController.Pile.hand;
        }

        for(int i = 0; i < player.field.Count; i++)
        {
            var card = player.field[i];
            var gfx = GetGfx(card.globalIdx);
            var controller = gfx.GetComponent<PlayCardController>();
            Transform fieldTransform = transform.Find("Field");

            gfx.SetParent(fieldTransform, false);

            Vector3 cardPos = fieldTransform.FindChild("" + card.pos).localPosition;
            gfx.localPosition = cardPos;
    
            if (controller.pile != PlayCardController.Pile.field)
            {
                gfx.FindChild("Selection").gameObject.SetActive(false);
                cardSelected = -1;
                gfx.GetChild(0).localRotation = Quaternion.EulerAngles(-(Mathf.PI / 2), 0, 0);
            }

            controller.pile = PlayCardController.Pile.field;
        }
    }

    public void OnSlotClicked(int slot)
    {
        Debug.Log("slot clicked: " + slot);

        var gfx = GetGfx(cardSelected);
        if (gfx == null)
        {
            return;
        }

        var controller = gfx.GetComponent<PlayCardController>();

        if (controller.pile == PlayCardController.Pile.hand)
        {
            GameManager.Get().NetRPC("PlayFromHand", RPCMode.Server, playerId, cardSelected, slot);
        }
        if (controller.pile == PlayCardController.Pile.field)
        {
            GameManager.Get().NetRPC("MoveOnField", RPCMode.Server, playerId, cardSelected, slot);
        }
    }

    public void OnHandClicked(int index)
    {
        
    }

    public void OnFieldClicked(int index)
    {
        //var gfx = GetGfx(index);
        //var controller = gfx.GetComponent<PlayCardController>();
        var player = GameManager.Get().players[playerId];

        for (int i = 0; i < player.field.Count; i++)
        {
            var card = player.field[i];
            if(card.globalIdx == index)
            {
                GameManager.Get().NetRPC("MoveOnField", RPCMode.Server, playerId, cardSelected, card.pos);
            }
        }
    }

    public void SelectCard(int index)
    {
        Transform oldTransform = GetGfx(cardSelected);
        cardSelected = index;
        Transform newTransform = GetGfx(cardSelected);

        if (oldTransform != null)
        {
            oldTransform.FindChild("Selection").gameObject.SetActive(false);
        }

        if (newTransform != null)
        {
            newTransform.FindChild("Selection").gameObject.SetActive(true);
        }
    }

    public bool isOwn()
    {
        return (playerId == GameManager.Get().localPlayerId);
    }

    public void OnGUI()
    {
        if (!isOwn())
        {
            return;
        }

        //Cheat Stuff
        if (GUI.Button (new Rect(0,0,60,25), "Swap:" + playerId))
        {
            GameManager.Get().localPlayerId = GameManager.Get().localPlayerId == 0 ? 1:0;
            //GameManager.Get().turn = !GameManager.Get().turn;
            Debug.Log(GameManager.Get().turnPlayer);
        }
        if (GUI.Button (new Rect(60,0,60,25), "Draw"))
        {
            //to server for final
            if (GameManager.Get().turnPlayer == playerId)
            {
                GameManager.Get().NetRPC("DrawCard", RPCMode.Server, playerId);
            }
        }

        //Test
        if (GUI.Button (new Rect(0,200,90,25), "End Turn"))
        {
            if (GameManager.Get().turnPlayer == playerId)
            {
                GameManager.Get().NetRPC("EndTurn", RPCMode.Server, playerId);
            }
        }

        if (GameManager.Get().notification.Length > 0)
        {
            GUI.Label(new Rect(0, Screen.height-20, 1000, 25), GameManager.Get().notification);
        }
    }
}
