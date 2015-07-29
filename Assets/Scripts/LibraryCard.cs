using UnityEngine;
using System.Collections;

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
    public int effectID = 0;

     public LibraryCard() {
        this.cardID = -1;
    }

     public LibraryCard(int cardID, Texture2D texture, string cardName, int cult, int costs,
         int attack, int health, int effectID)
     {

         this.cardID = cardID;
         this.texture = null;
         this.cardName = cardName;

         this.cult = cult;
         this.costs = costs;
         this.attack = attack;
         this.health = health;
         this.effectID = effectID;
         this.moveRange = 1;
     }

    public LibraryCard(int id, string name)
    {
        this.cardID = id;
        this.cardName = name;
    }
}

