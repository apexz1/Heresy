﻿using UnityEngine;
using System.Collections;

public class LibraryFX {

    //---Selector Variables---
    public enum SelectorTap
    {
        none,
        tapped,
        ready
    }

    public PlayCard.Pile selectorPile;
    public SelectorTap selectorTap;
    public bool selectorOwn;
    public bool selectorWho;
    public int selectorCount;

    //---Condition Variables---
    public enum ConditionType
    {
        none,
        ctrlOwn, //Control X amount of cards
        ctrlOpp,
        ctrlMoreOwn, //Control X amount of cards more
        ctrlMoreOpp,
    }

    public ConditionType conditionType;
    public int conditionCount;

    //---Action Variables---
    public enum ActionType
    {
        none,
        draw,
        discard,
        damageCard,
        damagePlayer,
        tap,
        ready,
    }

    public ActionType actionType;
    public int actionCount;


    public string description;

    public LibraryFX()
    {
        this.selectorPile = PlayCard.Pile.none;
    }

    public LibraryFX setSelector(PlayCard.Pile pile, SelectorTap tap, bool own, bool who, int count = 0)
    {
        selectorPile = pile;
        selectorTap = tap;
        selectorOwn = own;
        selectorWho = who;
        selectorCount = count;
        return this;
    }
    public LibraryFX setCondition(ConditionType type, int count)
    {
        conditionType = type;
        conditionCount = count;
        return this;
    }
    public LibraryFX setAction(ActionType type, int count = 0)
    {
        actionType = type;
        actionCount = count;

        return this;
    }
}
