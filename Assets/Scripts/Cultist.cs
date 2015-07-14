using UnityEngine;
using System.Collections;

[System.Serializable]
public class Cultist : Card {

    private int _cult = 0;
    private int _costs = 0;
    private int _attack = 0;
    private int _health = 0;
    private int _retaliate = 0;    
    private int _effectID = 0;   


    public Cultist() {
        this._cardID = -1;
    }

    public Cultist(int cardID, int textureID, string cardName, int cult, int costs,
        int attack, int health, int retaliate, int effectID){

        this._cardID = cardID;
        this._textureID = textureID;
        this._cardName = cardName;

        this._cult = cult;
        this._costs = costs;
        this._attack = attack;
        this._health = health;
        this._retaliate = retaliate;
        this._effectID = effectID;
    }

    // Karteneffekte

    public void EffDeath() { }
    public void EffEntry() { }
    public void EffEnd() { }
    public void EffAttack() { }
    public void EffRetaliate() { }
    public void EffKill() { }
    public void EffPermanent() { }
    public void EffStart() { }

    // Kulteffekte

    public void EffAlert() { }
    public void EffRanged() { }
    public void EffWinged() { }
    public void EffShielded() { }
    public void EffUndead() { }
    public void EffBrutal() { }
    public void EffTough() { }

    // GETTER & SETTER
    public int Cult {
        get { return _cult; }
        set { _cult = value; }
    }
    public int Costs {
        get { return _costs; }
        set { _costs = value; }
    }
    public int Attack {
        get { return _attack; }
        set { _attack = value; }
    }
    public int Health {
        get { return _health; }
        set { _health = value; }
    }
    public int Retaliate {
        get { return _retaliate; }
        set { _retaliate = value; }
    }
    public int EffectID {
        get { return _effectID; }
        set { _effectID = value; }
    }
}
