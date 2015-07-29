using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldController : MonoBehaviour {

    public int cardSelected;
    public Dictionary<int, Transform> cardGfxs=new Dictionary<int,Transform>();
    public Transform[] fields;
    public Camera cam;

    public void Awake()
    {
        fields = new Transform[2];
        fields[0] = GameObject.Find("BottomField").transform;
        fields[1] = GameObject.Find("TopField").transform;
        cardSelected = -1;
    }

    public static FieldController GetFieldControler()
    {
        return GameObject.Find("PlayField").GetComponent<FieldController>();
    }

    Transform GetGfx(int globalIndex)
    {
        Transform res=null;
        cardGfxs.TryGetValue(globalIndex, out res);
        return res;
    }

    public void FixedUpdate()
    {
        //var player = GameManager.Get().players[playerId];
        var playCards = GameManager.Get().playCards;

        ShowPlayerHealth(0);
        ShowPlayerHealth(1);
        
        for (int i = 0; i < playCards.Count; i++)
        {
            var card = playCards[i];
            var gfx = GetGfx(card.globalIdx);

            if (card.pile == PlayCard.Pile.discard)
            {
                if (gfx == null)
                    continue;

                var controller = gfx.GetComponent<PlayCardController>();

                if (controller.pile != PlayCard.Pile.discard)
                {
                    Debug.Log("Card detroyed?");
                    Destroy(gfx.gameObject);
                    cardGfxs.Remove(card.globalIdx);
                }
            }

            if (gfx == null) { gfx = CreateCardGFX(card); }
            var cardCtrl = gfx.GetComponent<PlayCardController>();
            cardCtrl.card = card;
        
            //Iteration through deck
            if (card.pile == PlayCard.Pile.deck)
            {
                if(cardCtrl.pile != PlayCard.Pile.deck)
                {
                    gfx.SetParent(fields[card.owner].Find("PlayPile"), false);
                    gfx.localPosition = new Vector3(0, 0.01f * i, 0);
                    cardCtrl.TurnCard(true);
                    //gfx.GetChild(0).localRotation = Quaternion.EulerAngles(Mathf.PI / 2, 0, 0);
                }
                cardCtrl.pile = PlayCard.Pile.deck;
            }

            //Iteration through hand
            if (card.pile == PlayCard.Pile.hand)
            {
                if (cardCtrl.pile != PlayCard.Pile.hand)
                {
                    gfx.SetParent(fields[card.owner].Find("Hand"), false);
                    gfx.localPosition = new Vector3(0, 0.01f * i, 0);
                    //gfx.GetChild(0).localRotation = Quaternion.EulerAngles(Mathf.PI / 2, 0, 0);

                    if (card.owner == 0) { gfx.localPosition = new Vector3(card.pos * 1.5f, 0, 0); }
                    if (card.owner == 1) { gfx.localPosition = new Vector3(card.pos * -1.5f, 0, 0); }

                    cardCtrl.TurnCard(false);

                    if (card.owner != GameManager.Get().localPlayerId)
                    { 
                        gfx.GetChild(0).localRotation = Quaternion.EulerAngles(Mathf.PI, 0, 0);
                        gfx.localPosition = gfx.localPosition * 0.5f;
                        cardCtrl.TurnCard(true);
                    }

                    ShowStats(cardCtrl, card);
                }
                cardCtrl.pile = PlayCard.Pile.hand;
            }

            //Iteration through field
            if (card.pile == PlayCard.Pile.field)
            {
                if (cardCtrl.pile != PlayCard.Pile.field)
                {
                    gfx.SetParent(GameObject.Find("PlayField").transform.Find("Field"), false);
                    gfx.localPosition = new Vector3(0, 0, 0);
                    cardCtrl.TurnCard(false);
                    //gfx.GetChild(0).localRotation = Quaternion.EulerAngles(Mathf.PI / 2, 0, 0);
                }

                if(cardCtrl.pos!=card.pos)
                {
                    cardCtrl.pos = card.pos;

                    Vector3 cardPos = GameObject.Find("PlayField").transform.Find("Field").FindChild("" + card.pos).localPosition;
                    gfx.localPosition = cardPos;
                }

                if (card.tap > 0)
                {
                    gfx.localRotation = Quaternion.EulerAngles(0, (Mathf.PI / 2), 0);
                }
                else
                {
                    gfx.localRotation = Quaternion.EulerAngles(0, 0, 0);
                }

                ShowStats(cardCtrl, card);
                cardCtrl.pile = PlayCard.Pile.field;
            }
        }

            /*for (int i = 0; i < player.playPile.Count; i++)
            {
                var card = player.playPile[i];
                var gfx = GetGfx(card.globalIdx);
                if (gfx != null)
                {
                    continue;
                }

                gfx = CreateCardGFX(card);
                gfx.localPosition = new Vector3(0, 0.01f * i, 0);
                //gfx.GetChild(0).localRotation = Quaternion.EulerAngles(Mathf.PI / 2, 0, 0);

                var controller = gfx.GetComponent<PlayCardController>();
                controller.pile = PlayCard.Pile.deck;
            }

        for (int i = 0; i < player.playHand.Count; i++)
        {
            var card = player.playHand[i];
            var gfx = GetGfx(card.globalIdx);
            if (gfx == null) gfx = CreateCardGFX(card);
            var controller = gfx.GetComponent<PlayCardController>();

            gfx.SetParent(transform.Find("Hand"), false);

            if (playerId == 0) { gfx.localPosition = new Vector3(i * 1.5f, 0, 0); }
            if (playerId == 1) { gfx.localPosition = new Vector3(i * -1.5f, 0, 0); }
            
            ShowStats(controller, card);

            //if (isOwn())
                //gfx.GetChild(0).localRotation = Quaternion.EulerAngles(-(Mathf.PI / 2), 0, 0);

            controller.pile = PlayCard.Pile.hand;
        }

        for(int i = 0; i < player.field.Count; i++)
        {
            var card = player.field[i];
            var gfx = GetGfx(card.globalIdx);
            if (gfx == null) gfx = CreateCardGFX(card);
            var controller = gfx.GetComponent<PlayCardController>();
            Transform fieldTransform = transform.Find("Field");

            gfx.SetParent(fieldTransform, false);
            Vector3 cardPos = fieldTransform.FindChild("" + card.pos).localPosition;
            gfx.localPosition = cardPos;
            ShowStats(controller, card);
            ShowPlayerHealth(playerId);
    
            if (controller.pile != PlayCard.Pile.field)
            {
                gfx.FindChild("Selection").gameObject.SetActive(false);
                cardSelected = -1;
                //if (playerId == 0) { gfx.localRotation = Quaternion.EulerAngles(0, -(Mathf.PI / 2), 0); }
                //if (playerId == 1) { gfx.localRotation = Quaternion.EulerAngles(0, (Mathf.PI / 2), 0); }
            }

            if (card.tap > 0)
            {
                //gfx.localRotation = Quaternion.EulerAngles(0, (Mathf.PI / 2), 0);
            }
            else
            {
                //gfx.localRotation = Quaternion.EulerAngles(0, 0, 0);
            }

            controller.pile = PlayCard.Pile.field;
        }

        for (int i = 0; i < player.discardPile.Count; i++)
        {
            var card = player.discardPile[i];
            var gfx = GetGfx(card.globalIdx);

            //Debug.Log(i+": "+(gfx != null) + " " + card.globalIdx);
            if (gfx == null)
                continue;

            var controller = gfx.GetComponent<PlayCardController>();

            if (controller.pile != PlayCard.Pile.discard)
            {
                Debug.Log("Card detroyed?");
                Destroy(gfx.gameObject,1.0f);
                cardGfxs.Remove(card.globalIdx);
            }

            controller.pile = PlayCard.Pile.discard;
        }*/
    } 

    private Transform CreateCardGFX(PlayCard card)
    {
        var cardObject = Instantiate((GameObject)Resources.Load("Prefabs/PlayCard"));
        var gfx = cardObject.transform;

        MeshRenderer rend = gfx.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        //Debug.Log("Card Texture: " + card.GetTexture().ToString());
        rend.material.mainTexture = card.GetTexture();
        //Debug.Log(card.id);
        var controller = cardObject.transform.gameObject.AddComponent<PlayCardController>();
        controller.cardIndex = card.globalIdx;
        cardGfxs[card.globalIdx] = gfx;
        return gfx;
    }

    public void ShowStats(PlayCardController pcc, PlayCard playCard)
    {
        var libCard = CardLibrary.Get().GetCard(playCard.libId);

        if (libCard.health > 0)
        {
            var transHealth = pcc.transform.FindChild("Health");
            var healthText = transHealth.GetComponent<TextMesh>();
            healthText.text = "" + playCard.health;
            transHealth.gameObject.SetActive(!pcc.turned);
        }

        if (libCard.attack > 0)
        {
            var transAttack = pcc.transform.FindChild("Attack");
            var attackText = transAttack.GetComponent<TextMesh>();
            attackText.text = "" + libCard.attack;
            transAttack.gameObject.SetActive(!pcc.turned);
        }
    }

    public void ShowPlayerHealth(int playerIndex)
    {
        if (playerIndex<0||playerIndex >= GameManager.Get().players.Length) { return; }

        var player = GameManager.Get().players[playerIndex];

        var transPlayerHP = fields[playerIndex].transform.FindChild("PlayerHealth");
        var playerHpText = transPlayerHP.GetComponent<TextMesh>();
        playerHpText.text = "" + player.playerHealth;
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

        if (controller.pile == PlayCard.Pile.hand)
        {
            GameManager.Get().NetRPC("PlayFromHand", RPCMode.Server, GameManager.Get().localPlayerId, cardSelected, slot);
        }
        if (controller.pile == PlayCard.Pile.field)
        {
            GameManager.Get().NetRPC("MoveOnField", RPCMode.Server, GameManager.Get().localPlayerId, cardSelected, slot);
        }
    }

    public void OnHandClicked(int index)
    {
        
    }

    public void OnFieldCardClicked(int cardIndex)
    {
        //var gfx = GetGfx(index);
        //var controller = gfx.GetComponent<PlayCardController>();
        Debug.Log("OnFieldClicked() Log: " + cardIndex);
        //var player = GameManager.Get().players[playerId];
        var card = GameManager.Get().playCards[cardIndex];

        if (card.owner == GameManager.Get().localPlayerId)
        {
            GameManager.Get().NetRPC("MoveOnField", RPCMode.Server, GameManager.Get().localPlayerId, cardSelected, card.pos);
        }
        else
        {
            GameManager.Get().NetRPC("ActionFoF", RPCMode.Server, GameManager.Get().localPlayerId, cardSelected, cardIndex);
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

    public void OnGUI()
    {
        int playerId = GameManager.Get().localPlayerId;

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
        if (GUI.Button(new Rect(120, 0, 60, 25), "Discard"))
        {
            //to server for final
            if (GameManager.Get().turnPlayer == playerId)
            {
                GameManager.Get().NetRPC("DiscardCard", RPCMode.Server, playerId, cardSelected);
            }
        }
        if (GUI.Button(new Rect(180, 0, 60, 25), "Damage Player"))
        {
            //to server for final
            if (GameManager.Get().turnPlayer == playerId)
            {
                GameManager.Get().NetRPC("DamagePlayer", RPCMode.Server, playerId, 5);
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
