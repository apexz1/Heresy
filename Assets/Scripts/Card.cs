using UnityEngine;
using System.Collections;

[System.Serializable]
public class Card
{
    protected int _cardID = 0;
    [SerializeField]
    protected string _cardName = "";

    public Card()
    {
        this._cardID = -1;
    }

    public Card(int id, string name)
    {
        this._cardID = id;
        this._cardName = name;
    }

    public int GetID()
    {
        int id = _cardID;
        return id;
    }

    public string GetName()
    {
        string name = _cardName;
        return name;
    }
}

