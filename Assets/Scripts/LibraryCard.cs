using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LibraryCard
{
    public int cardID = 0;
    public Texture2D texture;
    public string cardName = "";
    public int cult = 0;
    public int costs = 0;
    public int attack = 0;
    public int health = 0;
    public int moveRange = 0;
    public int atkRange = 0;
    public int effectID = 0;

    public List<LibraryFX> fxList = new List<LibraryFX>();

     public LibraryCard() {
        this.cardID = -1;
    }

     public LibraryCard(int cardID, Texture2D texture, string cardName, int attack, int atkRange, int health, int moveRange, int costs, int cult, int effectID)
     {
         this.cardID = cardID;
         this.texture = null;
         this.cardName = cardName;

         this.cult = cult;
         this.costs = costs;
         this.attack = attack;
         this.health = health;
         this.effectID = effectID;
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

