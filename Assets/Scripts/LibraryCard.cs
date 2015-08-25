using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LibraryCard
{
    public int cardID = 0;
    public Texture2D texture;
    public Texture2D texture_p;
    public string cardName = "";
    public Cult cult;
    public Race race;
    public int costs = 0;
    public int attack = 0;
    public int health = 0;
    public int moveRange = 0;
    public int atkRange = 0;

    public enum Race
    {
        stealthy, 
        undead,
        brutal,
        winged,
        tough,
        veiled,
        protective,
        skyC, //winged/tough
        hexC, //veiled/undead
        ripC, //brutal/protective
        graC, //undead/brutal
        dreC, //tough/stealthy
        bliC, //protective/winged
        pitC, //stealthy/veiled
        none,
    }

    public enum Cult
    {
        greed = 0,
        envy = 1,
        wrath = 2,
        pride = 3,
        gluttony = 4,
        lust = 5,
        sloth = 6,
        none,
    }

    public List<LibraryFX> fxList = new List<LibraryFX>();

     public LibraryCard() {
        this.cardID = -1;
    }

     public LibraryCard( int cardID, string cardName, int attack, int atkRange, int health, int moveRange, int costs, Cult cult, Race race)
     {
         this.cardID = cardID;
         this.texture = null;
         this.cardName = cardName;

         this.cult = cult;
         this.race = race;
         this.costs = costs;
         this.attack = attack;
         this.health = health;
         this.moveRange = moveRange;
         this.atkRange = atkRange;
     }

    public LibraryCard(int id, string name)
    {
        this.cardID = id;
        this.cardName = name;
    }

    public LibraryFX AddFX()
    {
        var fx = new LibraryFX();
        fxList.Add(fx);
        return fx;
    }

    public static string CultToString(LibraryCard.Cult cEnum)
    {
        string cult = null;

        switch ((int)cEnum)
        {
            case 0:
                cult = "greed";
                break;
            case 1:
                cult = "envy";
                break;
            case 2:
                cult = "wrath";
                break;
            case 3:
                cult = "pride";
                break;
            case 4:
                cult = "gluttony";
                break;
            case 5:
                cult = "lust";
                break;
            case 6:
                cult = "sloth";
                break;
        }

        return cult;
    }
}

