using UnityEngine;
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
    public int selectorCount;

    //---Condition Variables---
    public bool conditionOwn;
    public bool conditionMore;
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

    public LibraryFX()
    {
        this.selectorPile = PlayCard.Pile.none;
    }

    public LibraryFX setSelector(PlayCard.Pile pile, SelectorTap tap, bool own, bool count)
    {
        return this;
    }
    public LibraryFX setCondition(PlayCard.Pile pile, SelectorTap tap, bool own, bool count)
    {
        return this;
    }
    public LibraryFX setAction(ActionType type, int count)
    {
        actionType = type;
        actionCount = count;

        return this;
    }
}
