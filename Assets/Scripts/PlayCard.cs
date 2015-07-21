using UnityEngine;
using System.Collections;

public class PlayCard : CardIdentity
{
    public int health;
    public Transform cardGfx;

    public PlayCard(int id = -1)
    {
        this.id = id;
    }
    public void FromJSON(JSONObject jsCard)
    {
        base.FromJSON(jsCard);
        health = (int)jsCard["health"];

    }

    public JSONObject ToJSON()
    {
        JSONObject jsCard = base.ToJSON();
        jsCard.AddField("health", health);

        return jsCard;
    }
}
