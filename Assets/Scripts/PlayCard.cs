using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayCard
{
    public int libId;
    public int health;
    public int attack;
	public int actions ;//{ get; private set;}
    public int globalIdx;
    public int pos;
    public int tap;
    public int owner;
    public bool saced = false;
    public Pile pile;
    public enum Pile
    {
        none=-1,
        deck=0,
        hand,
        field,
        discard,
    }
    //public Transform cardGfx;

    public string GetName()
    {
        return CardLibrary.Get().GetCard(libId).cardName;
    }

    public Texture2D GetTexture()
    {
        return CardLibrary.Get().GetCard(libId).texture;
    }
    public Texture2D GetTexturePreview()
    {
        return CardLibrary.Get().GetCard(libId).texture_p;
    }

    public LibraryCard GetLibCard()
    {
        return CardLibrary.Get().GetCard(libId);
    }

    public PlayCard(int id = -1, int idx = 0)
    {
        this.libId = id;
        this.globalIdx = idx;
        this.pos = 0;
        this.pile = Pile.none;
    }

    public void InitLibrary()
    {
        this.health = CardLibrary.Get().GetCard(libId).health;
        this.attack = CardLibrary.Get().GetCard(libId).attack;
        this.actions = CardLibrary.Get().GetCard(libId).moveRange;
    }
    public void FromJSON(JSONObject jsCard)
    {
        libId = (int)jsCard["id"];
        globalIdx = (int)jsCard["globalIdx"];
        health = (int)jsCard["health"];
        attack = (int)jsCard["attack"];
        actions = (int)jsCard["actions"];
        saced = (bool)jsCard["saced"];
        pos = (int)jsCard["position"];
        tap = (int)jsCard["tapped"];
        owner = (int)jsCard["owner"];
        pile = (Pile)(int)jsCard["pile"];
    }

    public JSONObject ToJSON()
    {
        JSONObject jsCard = JSONObject.obj;
        jsCard.AddField("id", libId);
        jsCard.AddField("globalIdx", globalIdx);
        jsCard.AddField("health", health);
        jsCard.AddField("attack", attack);
        jsCard.AddField("actions", actions);
        jsCard.AddField("saced", saced);
        jsCard.AddField("position", pos);
        jsCard.AddField("tapped", tap);
        jsCard.AddField("owner", owner);
        jsCard.AddField("pile", (int)pile);

        return jsCard;
    }
}
