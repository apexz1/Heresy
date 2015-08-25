using UnityEngine;
using UnityEngine.UI;
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
        NetworkManager.Get().HostServer();
    }

    public void UIConnect()
    {
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
                //FieldController.GetFieldController().SelectCard(FieldController.GetFieldController().cardSelected);
                FieldController.GetFieldController().confirm = false;
            }
        }
    }

    public void UIBanner()
    {
        if (GameManager.Get().players[playerId].monument == false) { return; }

        GameManager.Get().NetRPC("MonumentFx", RPCMode.Server, playerId);
        GameManager.Get().players[playerId].monument = false;
        FieldController.GetFieldController().FadeBanner();
        GameManager.Get().SendNotification(GameManager.Get().localPlayerId, "Monument's power drained");
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
            if (FieldController.GetFieldController().cardSelected != -1) { FieldController.GetFieldController().SelectCard(FieldController.GetFieldController().cardSelected); }
            GameManager.Get().NetRPC("EndTurn", RPCMode.Server, playerId);
        }
    }
    public void LoadMenu()
    {
        Application.LoadLevel("menu");
    }
}
