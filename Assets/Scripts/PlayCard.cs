using UnityEngine;
using System.Collections;

public class PlayCard
{
    public int id;
    public int health;
    public int globalIdx;
    public int pos;
    //public Transform cardGfx;

    public string GetName()
    {
        return CardLibrary.Get().GetCard(id).cardName;
    }

    public Texture2D GetTexture()
    {
        return CardLibrary.Get().GetCard(id).texture;
    }

    public PlayCard(int id = -1, int idx = 0)
    {
        this.id = id;
        this.globalIdx = idx;
        this.pos = 0;
    }
    public void FromJSON(JSONObject jsCard)
    {
        id = (int)jsCard["id"];
        globalIdx = (int)jsCard["globalIdx"];
        health = (int)jsCard["health"];
        pos = (int)jsCard["position"];

    }

    public JSONObject ToJSON()
    {
        JSONObject jsCard = JSONObject.obj;
        jsCard.AddField("id", id);
        jsCard.AddField("globalIdx", globalIdx);
        jsCard.AddField("health", health);
        jsCard.AddField("position", pos);

        return jsCard;
    }
}
