using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class CardLibrary
{
    private static CardLibrary instance;
    public List<LibraryCard> cardList = new List<LibraryCard>();

    void Init()
    {
        cardList.Add(new LibraryCard(900, "TestCard0", 0, 0, 0, 0, 0));
        cardList.Add(new LibraryCard(901, "TestCard1", 0, 0, 0, 0, 0));
        cardList.Add(new LibraryCard(902, "TestCard2", 0, 0, 0, 0, 0));

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

    public LibraryCard GetCard(int id)
    {
        return cardList.Where(card => card.cardID == id).FirstOrDefault();
    }

    public LibraryCard GetCard(string name)
    {
        return cardList.Where(card => card.cardName.Equals(name)).FirstOrDefault();
    }
}

