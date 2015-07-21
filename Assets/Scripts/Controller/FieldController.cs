using UnityEngine;
using System.Collections;

public class FieldController : MonoBehaviour {

    public int playerId;

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
    public void FixedUpdate()
    {
        var player = GameManager.Get().players[playerId];

        for (int i = 0; i < player.playPile.Count; i++)
        {
            var card = player.playPile[i];
            if (card.cardGfx != null)
            {
                continue;
            }

            var cardObject = Instantiate((GameObject)Resources.Load("Prefabs/" + card.GetName()));
            card.cardGfx = cardObject.transform;
            card.cardGfx.SetParent(transform.Find("PlayPile"), false);

            card.cardGfx.localPosition = new Vector3(0,0.1f*i,0);
            card.cardGfx.GetChild(0).localRotation = Quaternion.EulerAngles(Mathf.PI/2, 0, 0);
        }
    }
}
