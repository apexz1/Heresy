using UnityEngine;
using System.Collections;

public class PlayCard : CardIdentity
{
    public int health;
    public int globalIdx;
    //public Transform cardGfx;

    public PlayCard(int id = -1,int idx = 0)
    {
        this.id = id;
        this.globalIdx = idx;
    }
    public void FromJSON(JSONObject jsCard)
    {
        base.FromJSON(jsCard);
        globalIdx = (int)jsCard["globalIdx"];
        health = (int)jsCard["health"];

    }

    public JSONObject ToJSON()
    {
        JSONObject jsCard = base.ToJSON();
        jsCard.AddField("globalIdx", globalIdx);
        jsCard.AddField("health", health);

        return jsCard;
    }
}
