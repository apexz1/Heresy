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
        cardList.Add(new LibraryCard(900, null, "TestCard0", 0, 0, 3, 0, 0));
        cardList.Add(new LibraryCard(901, null, "TestCard1", 0, 0, 4, 4, 0));
        cardList.Add(new LibraryCard(902, null, "TestCard2", 0, 0, 1, 7, 0));
        cardList.Add(new LibraryCard(903, null, "TestCard3", 0, 0, 9, 12, 0));
        cardList.Add(new LibraryCard(904, null, "TestCard4", 0, 0, 7, 1, 0));
        cardList.Add(new LibraryCard(905, null, "TestCard5", 0, 0, 34, 0, 0));
        cardList.Add(new LibraryCard(906, null, "TestCard6", 0, 0, 8, 1, 0));
        cardList.Add(new LibraryCard(907, null, "TestCard7", 0, 0, 2, 2, 0));
        cardList.Add(new LibraryCard(908, null, "TestCard8", 0, 0, 1, 6, 0));
        cardList.Add(new LibraryCard(909, null, "TestCard9", 0, 0, 99, 99, 0));
        
        
        GetCard(900).AddFX().setAction(LibraryFX.ActionType.draw, 1);
        GetCard(901).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true, 1)
            .setAction(LibraryFX.ActionType.discard).description = "Select card to discard";

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

