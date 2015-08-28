using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;

public class FieldController : MonoBehaviour
{

    public int cardSelected;
    public Dictionary<int, Transform> cardGfxs = new Dictionary<int, Transform>();
    public Transform[] fields;
    public Camera cam;
    public bool isActive = false;
    public bool confirm = false;
    public bool init = false;
    public int gameVersion = -1;

    public void Awake()
    {
        fields = new Transform[2];
        fields[0] = GameObject.Find("BottomField").transform;
        fields[1] = GameObject.Find("TopField").transform;
        cardSelected = -1;
    }

    public static FieldController GetFieldController()
    {
        return GameObject.Find("PlayField").GetComponent<FieldController>();
    }
    public Transform GetGfx( int globalIndex )
    {
        Transform res = null;
        cardGfxs.TryGetValue(globalIndex, out res);
        return res;
    }

    public void Start()
    {
        Debug.LogWarning("FIELDCONTROLLER RUNNING");
        init = true;
    }
    public void Update()
    {
        if (GameManager.Get().gameOver != -1)
        {
            GameOver(GameManager.Get().gameOver);
        }
    }

    public void FixedUpdate()
    {
        //var player = GameManager.Get().players[playerId];
        var playCards = GameManager.Get().playCards;

        //RESET GAME
        if (gameVersion != GameManager.Get().gameVersion)
        {
            gameVersion = GameManager.Get().gameVersion;

            for (int i = 0; i < cardGfxs.Count; i++)
            {
                var gfx = cardGfxs[i];
                Destroy(gfx.gameObject);
            }
            cardGfxs.Clear();
        }

        if (GameManager.Get().running && init)
        {
            AssignBanner(0);
            AssignBanner(1);
            LoadMonument(0);
            LoadMonument(1);

            init = false;
        }

        if (GameManager.Get().running)
        {
            ShowPlayerHealth(0);
            ShowPlayerHealth(1);

            FadeBanner(0);
            FadeBanner(1);
        }

        // Debug.Log("FieldController " + playCards.Count + " ");
        for (int i = 0; i < playCards.Count; i++)
        {
            var card = playCards[i];
            var gfx = GetGfx(card.globalIdx);

            //Debug.Log(card.pile);
            if (card.pile == PlayCard.Pile.discard)
            {
                if (gfx == null)
                    continue;

                var controller = gfx.GetComponent<PlayCardController>();

                if (controller.pile != PlayCard.Pile.discard)
                {
                    Debug.Log(controller.pile);
                    if (controller.pile == PlayCard.Pile.field)
                    {
                        Debug.Log(card.pos.ToString());
                        GameObject.Find(controller.pos.ToString()).transform.FindChild("SacField").gameObject.SetActive(false);
                    }
                    //Debug.Log("Card detroyed?");
                    Destroy(gfx.gameObject);
                    GameObject.Find("SceneCam").transform.FindChild("ZoomCardR").gameObject.SetActive(false);
                    GameObject.Find("SceneCam").transform.FindChild("ZoomCardL").gameObject.SetActive(false);
                    controller.pile = PlayCard.Pile.discard;

                    cardGfxs.Remove(card.globalIdx);
                }
            }

            if (gfx == null) { gfx = CreateCardGFX(card); }
            var cardCtrl = gfx.GetComponent<PlayCardController>();
            cardCtrl.card = card;

            //Iteration through deck
            if (card.pile == PlayCard.Pile.deck)
            {
                if (cardCtrl.pile != PlayCard.Pile.deck)
                {
                    gfx.SetParent(fields[card.owner].Find("PlayPile"), false);
                    gfx.localPosition = new Vector3(0, 0.01f * i, 0);
                    cardCtrl.TurnCard(true);

                    if (card.owner != GameManager.Get().localPlayerId)
                    {
                        gfx.localPosition = new Vector3(0, 0.01f * (i - 30), 0);
                        cardCtrl.TurnCard(true);
                    }
                    //gfx.GetChild(0).localRotation = Quaternion.EulerAngles(Mathf.PI / 2, 0, 0);
                }

                cardCtrl.pile = PlayCard.Pile.deck;
            }

            //Iteration through hand
            if (card.pile == PlayCard.Pile.hand)
            {
                if (cardCtrl.pile != PlayCard.Pile.hand)
                {
                    gfx.SetParent(fields[card.owner].Find("Hand"), true);
                    //gfx.GetChild(0).localRotation = Quaternion.EulerAngles(Mathf.PI / 2, 0, 0);
                    cardCtrl.TurnCard(false);
                    cardCtrl.pos = -1;

                    gfx.localRotation = Quaternion.EulerAngles(0, 0, Mathf.PI / 32);

                    if (card.owner != GameManager.Get().localPlayerId)
                    {
                        gfx.GetChild(0).localRotation = Quaternion.EulerAngles(Mathf.PI, 0, 0);
                        cardCtrl.TurnCard(true);
                    }

                    ShowStats(cardCtrl, card);
                }

                if (!cardCtrl.IsMoveAnimating())
                {
                    Vector3 to = new Vector3();
                    if (card.owner == 0) { to = new Vector3(card.pos * 1.5f, 0.2f, 0); }
                    if (card.owner == 1) { to = new Vector3(card.pos * -1.5f, 0.2f, 0); }

                    if (card.owner != GameManager.Get().localPlayerId)
                    {
                        to = to * 0.5f;
                    }

                    //Debug.Log(cardCtrl.pos + " " + card.pos);

                    if (cardCtrl.pos != card.pos)
                    {
                        //Vector3 from = fields[card.owner].Find("PlayPile").localPosition;

                        //gfx.localPosition = from;

                        cardCtrl.StartMoveAnimation(to, cardCtrl.pos == -1 ? 1.0f : 0.1f);
                        cardCtrl.pos = card.pos;
                    }
                }

                cardCtrl.pile = PlayCard.Pile.hand;
                gfx.transform.FindChild("owner" + card.owner).gameObject.SetActive(false);
            }

            //Iteration through field
            if (card.pile == PlayCard.Pile.field)
            {
                if (cardCtrl.pile != PlayCard.Pile.field)
                {
                    gfx.SetParent(GameObject.Find("PlayField").transform.Find("Field"), true);
                    //gfx.localPosition = new Vector3(0, 0, 0);
                    cardCtrl.TurnCard(false);
                    cardCtrl.pos = -1;
                    gfx.localRotation = Quaternion.EulerAngles(0, 0, 0);
                    gfx.transform.FindChild("owner" + card.owner).gameObject.SetActive(true);
                }

                if (cardCtrl.pos != card.pos)
                {
                    Vector3 cardPos = GameObject.Find("PlayField").transform.Find("Field").FindChild("" + card.pos).localPosition;
                    //gfx.localPosition = cardPos;

                    cardCtrl.StartMoveAnimation(cardPos, cardCtrl.pos == -1 ? 1.0f : 0.2f);
                    cardCtrl.pos = card.pos;
                }

                //if (card.tap > 0) Debug.Log("card tapped " + cardCtrl.IsMoveAnimating() + " " + cardCtrl.IsTapAnimating() + Time.time + " " + cardCtrl.moveStart + " " + cardCtrl.moveDuration);
                //fDebug.Log(cardCtrl.IsMoveAnimating() + " " + cardCtrl.IsTapAnimating());
                if (!cardCtrl.IsMoveAnimating() && !cardCtrl.IsTapAnimating())
                {
                    //if (card.tap > 0) Debug.Log("card tapped");

                    cardCtrl.StartTapAnimation(card.tap > 0);
                    /*
                    if (GameManager.Get().playCards[card.globalIdx].tap > 0)
                    {
                        gfx.localRotation = Quaternion.EulerAngles(0, (Mathf.PI / 2), 0);
                    }
                    if (GameManager.Get().playCards[card.globalIdx].tap <= 0)
                    {
                        gfx.localRotation = Quaternion.EulerAngles(0, 0, 0);
                    }
                    /**/
                }


                if (card.saced == true)
                {
                    gfx.gameObject.SetActive(false);
                    //GameObject.Find(cardCtrl.pos.ToString()).transform.FindChild("SacField").gameObject.SetActive(true);
                }

                ShowStats(cardCtrl, card);
                //ShowSacFields();
                cardCtrl.pile = PlayCard.Pile.field;
                GameObject.Find(cardCtrl.pos.ToString()).transform.FindChild("SacField").gameObject.SetActive(card.saced);

            }

            //Debug.Log(GameObject.Find("GameUI").transform.FindChild("Main").transform.FindChild("Confirm").gameObject);
            if (GameManager.Get().running)
            {
                GameObject.Find("GameUI").transform.FindChild("Main").transform.FindChild("Confirm").gameObject.SetActive(confirm);
            }
        }
    }

    private Transform CreateCardGFX( PlayCard card )
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

    public void ShowStats( PlayCardController pcc, PlayCard playCard )
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
            attackText.text = "" + playCard.attack;
            transAttack.gameObject.SetActive(!pcc.turned);
        }
        if (libCard.moveRange > 1)
        {
            var transActions = pcc.transform.FindChild("Action");
            var actionsText = transActions.GetComponent<TextMesh>();
            actionsText.text = "" + playCard.actions;
            transActions.gameObject.SetActive(!pcc.turned);
        }
        if (libCard.atkRange > 1)
        {
            var transRange = pcc.transform.FindChild("Action");
            var rangeText = transRange.GetComponent<TextMesh>();
            rangeText.text = "" + libCard.atkRange;
            transRange.gameObject.SetActive(!pcc.turned);
        }
    }

    public void ShowPlayerHealth( int playerIndex )
    {
        if (playerIndex < 0 || playerIndex >= GameManager.Get().players.Length) { return; }

        var player = GameManager.Get().players[playerIndex];

        var transPlayerHP = fields[playerIndex].transform.FindChild("PlayerHealth");
        transPlayerHP = GameObject.Find("GameUI").transform.FindChild("Main").transform.FindChild("health_" + playerIndex);
        var playerHpText = transPlayerHP.GetComponent<Text>();
        playerHpText.text = "" + player.playerHealth;
    }

    public void ShowCardPreview( bool side, PlayCard card )
    {
        if (card.owner != GameManager.Get().localPlayerId && card.pile == PlayCard.Pile.hand) { return; }

        GameObject child = Camera.main.transform.FindChild("ZoomCard" + (side ? "L" : "R")).gameObject;

        MeshRenderer rend = child.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        rend.material.mainTexture = card.GetTexturePreview();

        child.SetActive(true);
    }

    public void HideCardPreview()
    {
        Camera.main.transform.FindChild("ZoomCardL").gameObject.SetActive(false);
        Camera.main.transform.FindChild("ZoomCardR").gameObject.SetActive(false);
    }

    public void AssignBanner( int playerId )
    {
        Debug.Log("banner_" + playerId);
        Debug.Log(GameObject.Find("banner_" + playerId).GetComponent<Image>());
        Debug.Log(GameManager.Get().players[playerId].cult);

        var player = GameManager.Get().players[playerId];
        //Debug.Log("cult: " + player.cult);
        var image = GameObject.Find("banner_" + playerId).GetComponent<Image>();

        var array = Resources.LoadAll("Images/UI/main/banner", typeof(Texture2D));
        var sprites = new List<Sprite>();
        var spritesRef = new List<String>();
        var current = new Sprite();
        string currentRef = "";

        //Debug.Log("files loaded " + array.Length);

        var imgArray = new Texture2D[array.Length];

        for (int i = 0; i < array.Length; i++)
        {
            imgArray[i] = array[i] as Texture2D;
        }

        for (int i = 0; i < imgArray.Length; i++)
        {
            //Debug.Log("tex name: " + imgArray[i].name);
            current = Sprite.Create(imgArray[i], new Rect(0, 0, imgArray[i].width, imgArray[i].height), new Vector2(0.5f, 0.5f));
            currentRef = imgArray[i].name;
            //Debug.Log(current);
            sprites.Add(current);
            spritesRef.Add(currentRef);
        }

        //image.sprite = sprites[0];

        for (int i = 0; i < sprites.Count; i++)
        {
            //Debug.Log("name: " + spritesRef[i] + " " + "b_" + player.cult);
            if (spritesRef[i].Equals("b_" + player.cult))
            {
                image.sprite = sprites[i];
            }
        }
    }
    public void FadeBanner( int playerIndex )
    {
        Color c = new Color();

        if (!GameManager.Get().running || !GameManager.Get().setUp) { return; }

        for (int i = 0; i < GameManager.Get().players.Length; i++)
        {
            if (GameManager.Get().players[i].monument == false)
            {
                c = Color.grey;
            }
            else
            {
                c = Color.white;
            }

            var image = GameObject.Find("banner_" + GameManager.Get().players[i].playerId).GetComponent<Image>();
            image.color = c;
        }
    }
    public void LoadMonument( int playerIndex )
    {
        var players = GameManager.Get().players;
        string skin = "";

        if (OptionsMenu.isDarkFantasy)
        {
            skin = "cards_DF";
        }
        else if (OptionsMenu.isWonderland)
        {
            skin = "cards_WL";
        }
        else
        {
            skin = "cards_DF";
        }

        //Debug.Log(playerIndex);
        //Debug.Log("monument load path check: " + "Images/" + skin + "/monuments/m_" + players[playerIndex].cult);
        Texture2D tex = (Texture2D)Resources.Load("Images/" + skin + "/monuments/m_" + players[playerIndex].cult);
        //tex = (Texture2D)Resources.Load("Images/cards_DF/monuments/m_wrath");
        MeshRenderer rend = new MeshRenderer();

        if (playerIndex == 0)
        {
            rend = GameObject.Find("16").transform.FindChild("GFX").GetComponent<MeshRenderer>();
        }
        if (playerIndex == 1)
        {
            //Debug.Log(GameObject.Find("16").transform.FindChild("GFX").GetComponent<MeshRenderer>());
            rend = GameObject.Find("4").transform.FindChild("GFX").GetComponent<MeshRenderer>();
        }

        //Debug.Log(rend.name);
        rend.material.mainTexture = tex;

        //var rend = cardSpawn.transform.FindChild("GFX").GetComponent<MeshRenderer>();
    }

    /*public void ShowSacFields()
    {
        var gameManager = GameManager.Get();
        var sacList = gameManager.sacList;

        for (int i = 0; i < sacList.Count; i++)
        {
            int slotIndex = sacList[i].pos;
            GameObject.Find(slotIndex.ToString()).transform.FindChild("SacField").gameObject.SetActive(true);
        }
    }
    public void HideSacFields()
    {
        Debug.Log("Sacrificial Fields hidden?");
        var gameManager = GameManager.Get();
        var sacList = gameManager.sacList;

        for (int i = 0; i < sacList.Count; i++)
        {
            int slotIndex = sacList[i].pos;
            Debug.Log(GameObject.Find(slotIndex.ToString()).transform.FindChild("SacField"));
            GameObject.Find(slotIndex.ToString()).transform.FindChild("SacField").gameObject.SetActive(false);
        }
    }
    public void HideSacFieldsAll()
    {
        for (int i = 0; i < 21; i++)
        {
            GameObject.Find(i.ToString()).transform.FindChild("SacField").gameObject.SetActive(false);
        }
    }*/

    public void OnSlotClicked( int slot )
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

    public void OnHandClicked( int index )
    {

    }

    public void OnFieldCardClicked( int cardIndex )
    {
        //var gfx = GetGfx(index);
        //var controller = gfx.GetComponent<PlayCardController>();
        Debug.Log("OnFieldClicked() Log: " + cardIndex);
        //var player = GameManager.Get().players[playerId];
        var card = GameManager.Get().playCards[cardIndex];

        Debug.Log(cardSelected);
        if (cardSelected == -1) { return; }

        if (card.owner == GameManager.Get().localPlayerId)
        {
            GameManager.Get().NetRPC("MoveOnField", RPCMode.Server, GameManager.Get().localPlayerId, cardSelected, card.pos);
        }
        else
        {
            GameManager.Get().NetRPC("ActionFoF", RPCMode.Server, GameManager.Get().localPlayerId, cardSelected, cardIndex);
        }
    }

    public void OnPlayerClicked( int attackedPlayer )
    {
        GameManager.Get().NetRPC("ActionFoP", RPCMode.Server, GameManager.Get().localPlayerId, cardSelected, attackedPlayer);
    }

    public void SelectCard( int index )
    {
        Transform oldTransform = GetGfx(cardSelected);
        bool sameCard = false;
        Debug.Log("index " + index);
        Debug.Log("selected " + cardSelected);

        if (cardSelected == index)
        {
            sameCard = true;
            Debug.Log("same card");
        }

        cardSelected = index;

        Transform newTransform = GetGfx(cardSelected);

        var card = GameManager.Get().playCards[index];
        var g = GameManager.Get();

        //if (g.effectInProgess && ((card.GetLibCard().race == LibraryCard.Race.veiled && (/*card.GetLibCard().cardID != 901 &&*/ g.currentFx.libId != 901)) && card.pile == PlayCard.Pile.field)) { cardSelected = -1; return; }
        //if (g.effectInProgess && (g.currentFx.GetLibFx().selectorPile == PlayCard.Pile.hand && (g.CountCards(g.currentFx.GetLibFx().selectorWho == true ? 0 : 1, PlayCard.Pile.hand) > 0))) {return;}

        if (oldTransform != null)
        {
            oldTransform.FindChild("Selection").gameObject.SetActive(false);
        }

        if (newTransform != null)
        {
            if (sameCard)
            {
                isActive = isActive == false ? true : false;
                newTransform.FindChild("Selection").gameObject.SetActive(isActive);
                if (!isActive)
                {
                    cardSelected = -1;
                }
            }
            else
            {
                newTransform.FindChild("Selection").gameObject.SetActive(true);
                isActive = true;
            }
        }
    }


    public void StartSelectorFx()
    {

        confirm = false;

        int playerId = GameManager.Get().localPlayerId;
        var currentFx = GameManager.Get().currentFx;

        var gfx = GetGfx(cardSelected);
        if (currentFx.libId <= 0) { return; }
        if (currentFx.selectorDone) { return; }

        var libFx = currentFx.GetLibFx();
        bool ownFx = currentFx.playerIdx == GameManager.Get().localPlayerId;

        #region description
        //description
        string desc = "";
        string indicatr = "";
        string who = "";

        if (currentFx.GetLibFx().selectorWho == true)
        {
            indicatr = "Player 1, ";
        }
        if (currentFx.GetLibFx().selectorWho == false)
        {
            indicatr = "Player 2, ";
        }

        if (currentFx.GetLibFx().actionType != LibraryFX.ActionType.discard)
        {
            if (currentFx.GetLibFx().selectorOwn == true)
            {
                who = "choose own ";
            }
            if (currentFx.GetLibFx().selectorOwn == true)
            {
                who = "choose enemy ";
            }
        }

        #endregion

        desc = (indicatr + who + libFx.description);
        GUI.Label(new Rect(((Screen.width / 2) - 3 * desc.Length), (Screen.height - Screen.height / 4.4f), 1000, 25), desc);

        if (cardSelected == -1) { return; }

        var card = GameManager.Get().playCards[cardSelected];

        //Debug.Log("confirm?" + confirm);
        //Debug.Log((libFx.selectorPile == PlayCard.Pile.none) + " " + (!GameManager.Get().effectInProgess) + " " + (libFx.selectorWho != ownFx));
        if (libFx.selectorPile == PlayCard.Pile.none) { return; }

        if (card.pile != PlayCard.Pile.hand && card.pile != PlayCard.Pile.field) { return; }

        if (!GameManager.Get().effectInProgess) { return; }
        //All Hail SmartGit Log
        if (libFx.selectorWho != ownFx) { return; }

        //if (!GameManager.Get().SelectorProtective( GameManager.Get().localPlayerId, currentFx.libId, cardSelected)) { return; }

        if (libFx.selectorTap != LibraryFX.SelectorTap.none)
        {
            Debug.Log("selectorTap detected");
            if (libFx.selectorTap == LibraryFX.SelectorTap.ready)
            {
                if (!(GameManager.Get().playCards[cardSelected].tap <= 0))
                {
                    Debug.Log("Card is not ready");
                    return;
                }
            }

            if (libFx.selectorTap == LibraryFX.SelectorTap.tapped)
            {
                if (!(GameManager.Get().playCards[cardSelected].tap > 0))
                {
                    Debug.Log("Card is ready");
                    return;
                }
            }
        }

        //up to 20 for all hard code checks, maybe seperate

        //if (currentFx.libId == 901 && cardSelected == currentFx.libId) { Debug.LogWarning("Cant target self"); return; }

        //DOESNT WORK; WORK IN PROGRESS
        /*
        for (int i = 0; i < currentFx.selectedCards.Count; i++)
        {
            Debug.Log("adgdjsorktgapztkubhjuptfrdj " + currentFx.libId + " " + currentFx.selectedCards[i]);
            if (currentFx.libId == currentFx.selectedCards[i])
            {
                Debug.Log("Can't target itself");
                return;
            }
        }
        /**/

        confirm = true;

        /*
        if (GUI.Button(new Rect(0, 25, 60, 25), "Confirm"))
        {
            if (cardSelected != -1)
            {
                GameManager.Get().NetRPC("SelectorFxDone", RPCMode.Server, playerId, cardSelected);
            }
        }
        /**/
    }


    public void GameOver( int player )
    {
        //Debug.Log("player " + ((player+1)%2+1) + " won");
        GameObject.Find("SceneCam").transform.FindChild("gameOver").gameObject.SetActive(true);
        GameObject.Find("GameUI").transform.FindChild("Main").gameObject.SetActive(false);
        GameObject.Find("GameUI").transform.FindChild("PreGame").gameObject.SetActive(false);
    }

    public void OnGUI()
    {
        StartSelectorFx();

        int playerId = GameManager.Get().localPlayerId;

        //Cheat Stuff
        /*
        if (GUI.Button(new Rect(0, 0, 60, 25), "Swap:" + playerId))
        {
            GameManager.Get().localPlayerId = GameManager.Get().localPlayerId == 0 ? 1 : 0;
            //GameManager.Get().turn = !GameManager.Get().turn;
            Debug.Log(GameManager.Get().turnPlayer);
        }
        if (GUI.Button(new Rect(60, 0, 60, 25), "Draw"))
        {
            //to server for final
            if (GameManager.Get().turnPlayer == playerId)
            {
                GameManager.Get().NetRPC("DrawCard", RPCMode.Server, playerId, 1);
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
        if (GUI.Button(new Rect(120, 25, 60, 25), "All"))
        {
            //to server for final
            if (GameManager.Get().turnPlayer == playerId)
            {
                GameManager.Get().NetRPC("DiscardCardAll", RPCMode.Server, playerId);
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
        if (GUI.Button(new Rect(240, 0, 60, 25), "Sacrifice"))
        {
            //to server for final
            if (GameManager.Get().turnPlayer == playerId)
            {
                GameManager.Get().NetRPC("SacCard", RPCMode.Server, playerId, cardSelected);
            }
        }
        if (GUI.Button(new Rect(360, 0, 60, 25), "Tap"))
        {
            //to server for final
            if (GameManager.Get().turnPlayer == playerId)
            {
                GameManager.Get().NetRPC("TapCard", RPCMode.Server, playerId, cardSelected);
            }
        }
        if (GUI.Button(new Rect(420, 0, 60, 25), "Save"))
        {
            //to server for final
            if (GameManager.Get().turnPlayer == playerId)
            {
                GameManager.Get().NetRPC("SaveGame", RPCMode.Server);
            }
        }
        if (GUI.Button(new Rect(420, 25, 60, 25), "Load"))
        {
            //to server for final
            if (GameManager.Get().turnPlayer == playerId)
            {
                GameManager.Get().NetRPC("LoadGame", RPCMode.Server);
            }
        }
        if (GUI.Button(new Rect(300, 0, 60, 25), "Buff Card"))
        {
            //to server for final
            if (GameManager.Get().turnPlayer == playerId)
            {
                GameManager.Get().NetRPC("BuffCard", RPCMode.Server, playerId, cardSelected, 2, 1);
            }
        }
        if (GUI.Button(new Rect(300, 25, 60, 25), "Buff All Cards own"))
        {
            //to server for final
            if (GameManager.Get().turnPlayer == playerId)
            {
                GameManager.Get().NetRPC("BuffCardMulti", RPCMode.Server, playerId, cardSelected, 1, 1, 1);
            }
        }
        if (GUI.Button(new Rect(300, 50, 60, 25), "Buff All Cards"))
        {
            //to server for final
            if (GameManager.Get().turnPlayer == playerId)
            {
                GameManager.Get().NetRPC("BuffCardMulti", RPCMode.Server, playerId, cardSelected, 1, 1, 2);
            }
        }


        //Test
        if (GUI.Button(new Rect(0, 200, 90, 25), "End Turn"))
        {
            if (GameManager.Get().turnPlayer == playerId)
            {
                if (cardSelected != -1) { SelectCard(cardSelected); }
                GameManager.Get().NetRPC("EndTurn", RPCMode.Server, playerId);
            }
        }
        /**/


        if (GameManager.Get().notification.Length > 0 && Time.time - GameManager.Get().notifTime < 3.5f)
        {
            GUI.Label(new Rect(0, Screen.height - 20, 1000, 25), GameManager.Get().notification);
        }
    }
}
