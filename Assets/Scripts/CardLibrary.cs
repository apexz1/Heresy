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

        //Add(cardId, texture(always null), cardname, atk, atkRange, health, actions, costs, cult, race)
        cardList.Add(new LibraryCard(900, "TestCard0", 99, 1, 99, 2, 2, LibraryCard.Cult.wrath, LibraryCard.Race.brutal));
        cardList.Add(new LibraryCard(901, "TestCard1", 99, 9, 99, 9, 0, LibraryCard.Cult.wrath, LibraryCard.Race.brutal));
        cardList.Add(new LibraryCard(902, "TestCard2", 3, 2, 3, 1, 0, LibraryCard.Cult.wrath, LibraryCard.Race.brutal));
        cardList.Add(new LibraryCard(903, "TestCard3", 4, 1, 4, 2, 0, LibraryCard.Cult.wrath, LibraryCard.Race.brutal));
        cardList.Add(new LibraryCard(904, "TestCard4", 4, 2, 6, 1, 0, LibraryCard.Cult.wrath, LibraryCard.Race.brutal));
        cardList.Add(new LibraryCard(905, "TestCard5", 4, 1, 4, 2, 0, LibraryCard.Cult.wrath, LibraryCard.Race.brutal));
        cardList.Add(new LibraryCard(906, "TestCard6", 5, 1, 3, 2, 0, LibraryCard.Cult.wrath, LibraryCard.Race.brutal));
        cardList.Add(new LibraryCard(907, "TestCard7", 2, 2, 4, 1, 0, LibraryCard.Cult.wrath, LibraryCard.Race.brutal));
        cardList.Add(new LibraryCard(908, "TestCard8", 3, 1, 7, 2, 0, LibraryCard.Cult.wrath, LibraryCard.Race.brutal));
        cardList.Add(new LibraryCard(909, "TestCard9", 2, 2, 3, 1, 0, LibraryCard.Cult.wrath, LibraryCard.Race.brutal));

        cardList.Add(new LibraryCard(100, "BrutalFx", 0, 0, 0, 0, 0, LibraryCard.Cult.wrath, LibraryCard.Race.brutal));
        
        //setSelector(pile, selectorType, true=ownCard, true=effectOwner)

        //Card Effect 900
        //GetCard(901).AddFX().setCondition(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, false, 1).setAction(LibraryFX.ActionType.draw, 1);
        //Card Effect 901
        /*   GetCard(901).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true, 10)
            .setAction(LibraryFX.ActionType.damagePlayer).description = "Damage player";
        /**/

        //Card Effect 902
        GetCard(902).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOwn, 2)
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.discard).description = "Test";
        GetCard(902).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOwn, 4)
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.discard).description = "Test2";
        /*GetCard(902).AddFX()
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.discard, 1).description = "Select card to discard";
         * */
        /*GetCard(902).AddFX()
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, false, false)
            .setAction(LibraryFX.ActionType.discard, 1).description = "Select card to discard(opponent)";
         */

        GetCard(100).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true, 1, true)
            .setAction(LibraryFX.ActionType.discard);
        
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

