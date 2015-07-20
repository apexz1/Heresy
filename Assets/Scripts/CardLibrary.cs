using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class CardLibrary
{
    private static CardLibrary instance;
    public List<Card> cardList = new List<Card>();

    void Init()
    {
        cardList.Add(new Cultist(900, "TestCard0", 0, 0, 0, 0, 0, 0));
        cardList.Add(new Cultist(901, "TestCard1", 0, 0, 0, 0, 0, 0));
        cardList.Add(new Cultist(902, "TestCard2", 0, 0, 0, 0, 0, 0));

        //Debug.Log(GetCard(901).GetName());
    }

    public static CardLibrary Get()
    {
        if (instance == null)
        {
            instance = new CardLibrary();
            instance.Init();
        }

        return instance;
    }

    public Card GetCard(int id)
    {
        return cardList.Where(card => card.GetID() == id).FirstOrDefault();
    }

    public Card GetCard(string name)
    {
        return cardList.Where(card => card.GetName().Equals(name)).FirstOrDefault();
    }
}

