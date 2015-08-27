using UnityEngine;
using System.Collections;

public class LibraryFX
{

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
    public bool adjacentPos;

    //---Condition Variables---
    public enum ConditionType
    {
        none,
        ctrlOwn, //Control X amount of cards
        ctrlOpp,
        ctrlMoreOwn, //Control X amount of cards more
        ctrlMoreOpp,
        kills,
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
        damageSelf,
        damageOpp,
        selfDestruct,
        buffAction,
        buffAttack,
        cultBuff,
        tap,
        ready,
    }

    public ActionType actionType;
    public int actionCount;
    public bool actionSelf;


    public string description;

    public LibraryFX() {
        this.selectorPile = PlayCard.Pile.none;
    }

    public LibraryFX setSelector( PlayCard.Pile pile, SelectorTap tap, bool own, bool who, int count = 1, bool pos = false ) {
        selectorPile = pile;
        selectorTap = tap;
        selectorOwn = own;
        selectorWho = who;
        selectorCount = count;
        adjacentPos = pos;

        return this;
    }
    public LibraryFX setCondition( ConditionType type, int count ) {
        conditionType = type;
        conditionCount = count;
        return this;
    }
    public LibraryFX setAction( ActionType type, int count = 0, bool self = true ) {
        actionType = type;
        actionCount = count;
        actionSelf = self;
        return this;
    }
}
