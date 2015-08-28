using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class UIButtonController : MonoBehaviour
{

    public int playerId;
    public FieldController fCtrl;

    public void Awake()
    {
        playerId = GameManager.Get().localPlayerId;
        fCtrl = FieldController.GetFieldController();
    }

    public void UIHost()
    {

        /*if (!(CheckDeckCount()))
        {
            return;
        }
         * /**/
        NetworkManager.Get().HostServer();
    }

    public void UIConnect()
    {
        /*if (!(CheckDeckCount()))
        {
            return;
        }
         * /**/
        NetworkManager.Get().Connect();
    }
    public void UIConfirm()
    {
        Debug.Log("confirm?" + FieldController.GetFieldController().confirm);
        if (FieldController.GetFieldController().confirm == true)
        {
            if (FieldController.GetFieldController().cardSelected != -1)
            {
                GameManager.Get().NetRPC("SelectorFxDone", RPCMode.Server, playerId, FieldController.GetFieldController().cardSelected);
                //if (GameManager.Get().effectInProgess && GameManager.Get().currentFx.GetLibFx().selectorOwn == false)
                FieldController.GetFieldController().cardSelected = -1;
                FieldController.GetFieldController().confirm = false;
            }
        }
    }

    public void UIBanner()
    {
        if (GameManager.Get().players[playerId].monument == false) { return; }

        string banner = gameObject.transform.parent.name.Replace("banner_", "");

        Debug.Log(banner);
        int bannerId = (Int32.Parse(banner));

        if (bannerId != GameManager.Get().localPlayerId) { return; }

        GameManager.Get().StartMonumentFx(bannerId);
    }

    public void UISacrifice()
    {
        if (GameManager.Get().turnPlayer == playerId)
        {
            GameManager.Get().NetRPC("SacCard", RPCMode.Server, playerId, fCtrl.cardSelected);
        }
    }

    public void UIEndTurn()
    {
        if (GameManager.Get().turnPlayer == playerId)
        {
            if (GameManager.Get().effectInProgess) { return; }
            if (FieldController.GetFieldController().cardSelected != -1) { FieldController.GetFieldController().SelectCard(FieldController.GetFieldController().cardSelected); }
            GameManager.Get().NetRPC("EndTurn", RPCMode.Server, playerId);
        }
    }
    public void LoadMenu()
    {
        Application.LoadLevel("menu");
    }

    public bool CheckDeckCount()
    {
        InputField inputField = GameObject.Find("GameUI").transform.FindChild("PreGame").FindChild("DeckChoice").gameObject.GetComponent<InputField>();
        string deckChoice = inputField.text;

        return ((GameManager.Get().LoadDeck(GameManager.Get().localPlayerId, deckChoice) == 30));
    }
}
