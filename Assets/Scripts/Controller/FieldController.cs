using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldController : MonoBehaviour {

    public int playerId;
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

            var cardObject = Instantiate((GameObject)Resources.Load("Prefabs/" + card.GetName()));
            gfx = cardObject.transform;
            gfx.SetParent(transform.Find("PlayPile"), false);

            gfx.localPosition = new Vector3(0, 0.01f * i, 0);
            gfx.GetChild(0).localRotation = Quaternion.EulerAngles(Mathf.PI / 2, Mathf.PI, 0);
            cardObject.transform.gameObject.AddComponent<PlayCardController>();
            cardGfxs[card.globalIdx] = gfx;
        }

        for (int i = 0; i < player.playHand.Count; i++)
        {
            var card = player.playHand[i];
            var gfx = GetGfx(card.globalIdx);

            gfx.SetParent(transform.Find("Hand"), false);
            gfx.localPosition = new Vector3(i * 2.5f, 0, 0);
            gfx.GetChild(0).localRotation = Quaternion.EulerAngles(-(Mathf.PI / 2), Mathf.PI, 0);
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

        if (GUI.Button (new Rect(0,0,60,25), "Swap:" + playerId))
        {
            GameManager.Get().localPlayerId = GameManager.Get().localPlayerId == 0 ? 1:0;
        }
        if (GUI.Button (new Rect(60,0,60,25), "Draw"))
        {
            GameManager.Get().NetRPC("DrawCard", RPCMode.Server, playerId);
        }
        if (GUI.Button (new Rect(0,200,90,25), "End Turn"))
        {
            GameManager.Get().NetRPC("EndTurn", RPCMode.Server, playerId);
        }
    }
}
