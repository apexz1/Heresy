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
        cardList.Add(new LibraryCard(900, null, "TestCard0", 99, 1, 99, 2, 0, 0, 0));
        cardList.Add(new LibraryCard(901, null, "TestCard1", 2, 1, 6, 2, 0, 0, 0));
        cardList.Add(new LibraryCard(902, null, "TestCard2", 3, 2, 3, 1, 0, 0, 0));
        cardList.Add(new LibraryCard(903, null, "TestCard3", 4, 1, 4, 2, 0, 0, 0));
        cardList.Add(new LibraryCard(904, null, "TestCard4", 4, 2, 6, 1, 0, 0, 0));
        cardList.Add(new LibraryCard(905, null, "TestCard5", 4, 1, 4, 2, 0, 0, 0));
        cardList.Add(new LibraryCard(906, null, "TestCard6", 5, 1, 3, 2, 0, 0, 0));
        cardList.Add(new LibraryCard(907, null, "TestCard7", 2, 2, 4, 1, 0, 0, 0));
        cardList.Add(new LibraryCard(908, null, "TestCard8", 3, 1, 7, 2, 0, 0, 0));
        cardList.Add(new LibraryCard(909, null, "TestCard9", 2, 2, 3, 1, 0, 0, 0));
        
        //Card Effect 900
        GetCard(900).AddFX().setAction(LibraryFX.ActionType.draw, 1);
        /*
         * GetCard(901).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true, 1)
            .setAction(LibraryFX.ActionType.discard).description = "Select card to discard";
         */

        //Card Effect 902
        GetCard(902).AddFX()
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.discard, 1).description = "Select card to discard";
        GetCard(902).AddFX()
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, false, false)
            .setAction(LibraryFX.ActionType.discard, 1).description = "Select card to discard(opponent)";
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

