﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayFX {

    public int libId;
    public int fxIdx;
    public int playerIdx;
    public int actionCount;
    public int selectorCount;
    public bool selectorDone;
    public List<int> selectedCards=new List<int>();

    public LibraryFX GetLibFx()
    {
        return CardLibrary.Get().GetCard(libId).fxList[fxIdx];
    }

    public void FromJSON(JSONObject jsCard)
    {
        libId = (int)jsCard["id"];
        fxIdx = (int)jsCard["fxIdx"];
        playerIdx = (int)jsCard["playerIdx"];
        actionCount = (int)jsCard["actionCount"];
        selectorDone = (bool)jsCard["selectorDone"];
    }

    public JSONObject ToJSON()
    {
        JSONObject jsCard = JSONObject.obj;
        jsCard.AddField("id", libId);
        jsCard.AddField("fxIdx", fxIdx);
        jsCard.AddField("playerIdx", playerIdx);
        jsCard.AddField("actionCount", actionCount);
        jsCard.AddField("selectorDone", selectorDone);
        return jsCard;
    }
}
