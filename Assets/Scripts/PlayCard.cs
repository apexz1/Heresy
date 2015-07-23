using UnityEngine;
using System.Collections;

public class PlayCard
{
    public int id;
    public int health;
    public int globalIdx;
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
    }
    public void FromJSON(JSONObject jsCard)
    {
        id = (int)jsCard["id"];
        globalIdx = (int)jsCard["globalIdx"];
        health = (int)jsCard["health"];

    }

    public JSONObject ToJSON()
    {
        JSONObject jsCard = JSONObject.obj;
        jsCard.AddField("id", id);
        jsCard.AddField("globalIdx", globalIdx);
        jsCard.AddField("health", health);

        return jsCard;
    }
}
