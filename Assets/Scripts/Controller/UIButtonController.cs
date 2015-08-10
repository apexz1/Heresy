using UnityEngine;
using System.Collections;

public class UIButtonController : MonoBehaviour {

    public int playerId;
    public FieldController fCtrl;

    public void Awake()
    {
        playerId = GameManager.Get().localPlayerId;
        fCtrl = FieldController.GetFieldController();
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
            GameManager.Get().NetRPC("EndTurn", RPCMode.Server, playerId);
        }
    }
}
