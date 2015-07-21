using UnityEngine;
using System.Collections;

public class CardIdentity : MonoBehaviour
{
    public int id;

    public string GetName(int id)
    {
        return CardLibrary.Get().GetCard(id).cardName;
    }

    public void FromJSON(JSONObject jsCard)
    {
        id = (int)jsCard["id"];
    }

    public JSONObject ToJSON()
    {
        JSONObject jsCard = JSONObject.obj;
        jsCard.AddField("id", id);

        return jsCard;
    }
}
