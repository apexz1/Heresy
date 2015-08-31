﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayFX {

    //public int cardId;
    public int libId;
    public int fxIdx;
    public int globalIdx;
    public int playerIdx;
    public int actionCount;
    public int selectorCount;
    public int conditionCount;
    public bool selectorDone;
    public int adjacentPos = -1;
    public List<int> selectedCards=new List<int>();

    public LibraryFX GetLibFx()
    {
        return CardLibrary.Get().GetCard(libId).fxList[fxIdx];
    }

    public bool NextFx()
    {
        if (fxIdx >= CardLibrary.Get().GetCard(libId).fxList.Count-1)
        {
            return false;
        }
        fxIdx++;
        return true;
    }

    public void FromJSON(JSONObject jsCard)
    {
        //cardId = (int)jsCard["cardId"];
        libId = (int)jsCard["id"];
		fxIdx = (int)jsCard["fxIdx"];
        globalIdx = (int)jsCard["globalIdx"];
        adjacentPos = (int)jsCard["adjacentPos"];
        playerIdx = (int)jsCard["playerIdx"];
        actionCount = (int)jsCard["actionCount"];
        selectorDone = (bool)jsCard["selectorDone"];
        adjacentPos = (int)jsCard["adjacentPos"];
    }

    public JSONObject ToJSON()
    {
        JSONObject jsCard = JSONObject.obj;
        //jsCard.AddField("cardId", cardId);
        jsCard.AddField("id", libId);
        jsCard.AddField("fxIdx", fxIdx);
        jsCard.AddField("globalIdx", globalIdx);
        jsCard.AddField("playerIdx", playerIdx);
        jsCard.AddField("actionCount", actionCount);
        jsCard.AddField("selectorDone", selectorDone);
        jsCard.AddField("adjacentPos", adjacentPos);
        return jsCard;
    }
}
