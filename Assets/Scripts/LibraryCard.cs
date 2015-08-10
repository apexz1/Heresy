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
        protective
    }

    public enum Cult
    {
        greed,
        envy,
        wrath,
        pride,
        gluttony,
        lust,
        sloth,
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
}

