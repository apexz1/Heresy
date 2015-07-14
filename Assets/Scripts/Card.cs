using UnityEngine;
using System.Collections;

[System.Serializable]
public class Card
{
    protected int _cardID = 0;
    protected int _textureID = 0;
    protected string _cardName = "testname";

    public Card()
    {
        this._cardID = -1;
        this._textureID = -1;
    }

    public Card(int id, string name, int texture)
    {
        this._cardID = id;
        this._cardName = name;
        this._textureID = texture;
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

    public int GetTexture()
    {
        int textureID = _textureID;
        return textureID;
    }
}

