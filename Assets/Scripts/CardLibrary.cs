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
        /*
        cardList.Add(new LibraryCard(910, "Ripjaw Chosen", 8, 3, 11, 1, 0, LibraryCard.Cult.wrath, LibraryCard.Race.brutal));
        cardList.Add(new LibraryCard(911, "Blightbark Chosen", 4, 3, 13, 1, 0, LibraryCard.Cult.sloth, LibraryCard.Race.protective));
        cardList.Add(new LibraryCard(912, "Skyfolk Chosen", 6, 3, 5, 1, 0, LibraryCard.Cult.pride, LibraryCard.Race.winged));
        /**/
        Debug.Log("cardlib neverfall log: " + GameManager.Neverfall_God_of_Pride);

        cardList.Add(new LibraryCard(900, "Skyfolk Marauder", 3, 1, 7, 2, 0, LibraryCard.Cult.sloth, LibraryCard.Race.winged));
        cardList.Add(new LibraryCard(901, "Hexfin Marauder", 4, 1, 8, 2, 0, LibraryCard.Cult.gluttony, LibraryCard.Race.veiled));
        cardList.Add(new LibraryCard(902, "Ripjaw Marauder", 5, 1, 3, 2, 0, LibraryCard.Cult.envy, LibraryCard.Race.brutal));
        cardList.Add(new LibraryCard(903, "Graveborn Marauder", 5, 1, 5, 2, 0, LibraryCard.Cult.wrath, LibraryCard.Race.undead));
        cardList.Add(new LibraryCard(904, "Dreadbulge Marauder", 2, 1, 6, 2, 0, LibraryCard.Cult.lust, LibraryCard.Race.tough));
        cardList.Add(new LibraryCard(905, "Blightbark Marauder", 3, 1, 7, 2, 0, LibraryCard.Cult.pride, LibraryCard.Race.protective));
        cardList.Add(new LibraryCard(906, "Pitkin Merchant", 4, 1, 6, 3, 1, LibraryCard.Cult.greed, LibraryCard.Race.stealthy));

        cardList.Add(new LibraryCard(907, "Hexfin Preacher", 2, 2, 4, 1, 0, LibraryCard.Cult.greed, LibraryCard.Race.veiled));
        cardList.Add(new LibraryCard(908, "Ripjaw Preacher", 3, 2, 5, 1, 0, LibraryCard.Cult.sloth, LibraryCard.Race.brutal));
        cardList.Add(new LibraryCard(909, "Graveborn Preacher", 3, 2, 7, 1, 0, LibraryCard.Cult.gluttony, LibraryCard.Race.tough));
        cardList.Add(new LibraryCard(910, "Dreadbulge Preacher", 2, 2, 4, 1, 0, LibraryCard.Cult.envy, LibraryCard.Race.tough));
        cardList.Add(new LibraryCard(911, "Blightbark Preacher", 3, 2, 5, 1, 0, LibraryCard.Cult.wrath, LibraryCard.Race.protective));
        cardList.Add(new LibraryCard(912, "Pitkin Preacher", 3, 2, 3, 1, 0, LibraryCard.Cult.lust, LibraryCard.Race.stealthy));
        cardList.Add(new LibraryCard(913, "Skyfolk Priest", 3, 3, 6, 1, 2, LibraryCard.Cult.pride, LibraryCard.Race.winged));

        cardList.Add(new LibraryCard(914, "Ripjaw Missionary", 5, 1, 5, 2, 0, LibraryCard.Cult.pride, LibraryCard.Race.brutal));
        cardList.Add(new LibraryCard(915, "Graveborn Missionary", 3, 1, 5, 2, 0, LibraryCard.Cult.greed, LibraryCard.Race.undead));
        cardList.Add(new LibraryCard(916, "Dreadbulge Missionary", 2, 1, 8, 2, 0, LibraryCard.Cult.sloth, LibraryCard.Race.tough));
        cardList.Add(new LibraryCard(917, "Blightbark Missionary", 3, 1, 9, 2, 0, LibraryCard.Cult.gluttony, LibraryCard.Race.protective));
        cardList.Add(new LibraryCard(918, "Pitkin Missionary", 5, 1, 3, 2, 0, LibraryCard.Cult.envy, LibraryCard.Race.stealthy));
        cardList.Add(new LibraryCard(919, "Skyfolk Missionary", 5, 1, 5, 2, 0, LibraryCard.Cult.wrath, LibraryCard.Race.winged));
        cardList.Add(new LibraryCard(920, "Hexfin Persuader", 3, 1, 7, 3, 0, LibraryCard.Cult.lust, LibraryCard.Race.veiled));

        cardList.Add(new LibraryCard(921, "Graveborn Overseer", 3, 1, 5, 2, 0, LibraryCard.Cult.wrath, LibraryCard.Race.undead));
        cardList.Add(new LibraryCard(922, "Dreadbulge Overseer", 3, 1, 7, 2, 0, LibraryCard.Cult.pride, LibraryCard.Race.tough));
        cardList.Add(new LibraryCard(923, "Blightbark Overseer", 2, 1, 6, 2, 0, LibraryCard.Cult.greed, LibraryCard.Race.protective));
        cardList.Add(new LibraryCard(924, "Pitkin Overseer", 4, 1, 6, 2, 0, LibraryCard.Cult.sloth, LibraryCard.Race.stealthy));
        cardList.Add(new LibraryCard(925, "Skyfolk Overseer", 4, 1, 8, 2, 0, LibraryCard.Cult.gluttony, LibraryCard.Race.winged));
        cardList.Add(new LibraryCard(926, "Hexfin Overseer", 4, 1, 4, 2, 0, LibraryCard.Cult.envy, LibraryCard.Race.veiled));
        cardList.Add(new LibraryCard(927, "Ripjaw Slaver", 6, 1, 6, 3, 1, LibraryCard.Cult.wrath, LibraryCard.Race.brutal));

        cardList.Add(new LibraryCard(928, "Dreadbulge Worker", 4, 1, 6, 2, 0, LibraryCard.Cult.wrath, LibraryCard.Race.tough));
        cardList.Add(new LibraryCard(929, "Blightbark Worker", 2, 1, 6, 2, 0, LibraryCard.Cult.lust, LibraryCard.Race.protective));
        cardList.Add(new LibraryCard(930, "Pitkin Worker", 5, 1, 5, 2, 0, LibraryCard.Cult.pride, LibraryCard.Race.stealthy));
        cardList.Add(new LibraryCard(931, "Skyfolk Worker", 3, 1, 5, 2, 0, LibraryCard.Cult.greed, LibraryCard.Race.winged));
        cardList.Add(new LibraryCard(932, "Hexfin Worker", 3, 1, 7, 2, 0, LibraryCard.Cult.sloth, LibraryCard.Race.veiled));
        cardList.Add(new LibraryCard(933, "Ripjaw Worker", 5, 1, 7, 2, 0, LibraryCard.Cult.gluttony, LibraryCard.Race.brutal));
        cardList.Add(new LibraryCard(934, "Graveborn Battlemason", 4, 1, 6, 3, 1, LibraryCard.Cult.envy, LibraryCard.Race.undead));

        cardList.Add(new LibraryCard(935, "Blightbark Adept", 2, 2, 4, 1, 0, LibraryCard.Cult.envy, LibraryCard.Race.protective));
        cardList.Add(new LibraryCard(936, "Pitkin Adept", 5, 2, 3, 1, 0, LibraryCard.Cult.wrath, LibraryCard.Race.stealthy));
        cardList.Add(new LibraryCard(937, "Skyfolk Adept", 2, 2, 4, 1, 0, LibraryCard.Cult.lust, LibraryCard.Race.winged));
        cardList.Add(new LibraryCard(938, "Hexfin Adept", 3, 2, 5, 1, 0, LibraryCard.Cult.pride, LibraryCard.Race.veiled));
        cardList.Add(new LibraryCard(939, "Ripjaw Adept", 3, 2, 3, 1, 0, LibraryCard.Cult.greed, LibraryCard.Race.brutal));
        cardList.Add(new LibraryCard(940, "Graveborn Adept", 2, 2, 6, 1, 0, LibraryCard.Cult.sloth, LibraryCard.Race.undead));
        cardList.Add(new LibraryCard(941, "Dreadbulge Butcher", 2, 3, 9, 1, 0, LibraryCard.Cult.gluttony, LibraryCard.Race.tough));

        cardList.Add(new LibraryCard(942, "Pitkin Doomsayer", 4, 2, 6, 1, 0, LibraryCard.Cult.gluttony, LibraryCard.Race.stealthy));
        cardList.Add(new LibraryCard(943, "Skyfolk Doomsayer", 3, 2, 3, 1, 0, LibraryCard.Cult.envy, LibraryCard.Race.winged));
        cardList.Add(new LibraryCard(944, "Hexfin Doomsayer", 4, 2, 4, 1, 0, LibraryCard.Cult.wrath, LibraryCard.Race.veiled));
        cardList.Add(new LibraryCard(945, "Ripjaw Doomsayer", 3, 2, 3, 1, 0, LibraryCard.Cult.lust, LibraryCard.Race.brutal));
        cardList.Add(new LibraryCard(946, "Graveborn Doomsayer", 3, 2, 5, 1, 0, LibraryCard.Cult.pride, LibraryCard.Race.undead));
        cardList.Add(new LibraryCard(947, "Dreadbulge Doomsayer", 1, 2, 5, 1, 0, LibraryCard.Cult.greed, LibraryCard.Race.tough));
        cardList.Add(new LibraryCard(948, "Blightbark Dashdrainer", 1, 3, 8, 1, 1, LibraryCard.Cult.sloth, LibraryCard.Race.protective));

        cardList.Add(new LibraryCard(950, "Skyfolk Colossus", 6, 1, 17, 2, 2, LibraryCard.Cult.pride, LibraryCard.Race.skyC));
        cardList.Add(new LibraryCard(951, "Hexfin Colossus", 6, 1, 17, 2, 2, LibraryCard.Cult.lust, LibraryCard.Race.hexC));
        cardList.Add(new LibraryCard(952, "Ripjaw Colossus", 7, 1, 16, 2, 2, LibraryCard.Cult.wrath, LibraryCard.Race.ripC));
        cardList.Add(new LibraryCard(953, "Graveborn Colossus", 7, 1, 16, 2, 2, LibraryCard.Cult.envy, LibraryCard.Race.graC));
        cardList.Add(new LibraryCard(954, "Dreadbulge Colossus", 5, 1, 18, 2, 2, LibraryCard.Cult.gluttony, LibraryCard.Race.dreC));
        cardList.Add(new LibraryCard(955, "Blightbark Colossus", 5, 1, 18, 2, 2, LibraryCard.Cult.sloth, LibraryCard.Race.bliC));
        cardList.Add(new LibraryCard(956, "Pitkin Colossus", 7, 1, 16, 2, 2, LibraryCard.Cult.greed, LibraryCard.Race.pitC));

        cardList.Add(new LibraryCard(957, "Skyfolk Chosen", 8, 1, 15, 3, 0, LibraryCard.Cult.pride, LibraryCard.Race.winged));
        cardList.Add(new LibraryCard(958, "Hexfin Chosen", 8, 1, 15, 3, 0, LibraryCard.Cult.lust, LibraryCard.Race.veiled));
        cardList.Add(new LibraryCard(959, "Ripjaw Chosen", 9, 1, 14, 3, 0, LibraryCard.Cult.wrath, LibraryCard.Race.brutal));
        cardList.Add(new LibraryCard(960, "Graveborn Chosen", 9, 1, 14, 3, 0, LibraryCard.Cult.envy, LibraryCard.Race.undead));
        cardList.Add(new LibraryCard(961, "Dreadbulge Chosen", 7, 1, 16, 3, 0, LibraryCard.Cult.gluttony, LibraryCard.Race.tough));
        cardList.Add(new LibraryCard(962, "Blightbark Chosen", 7, 1, 16, 3, 0, LibraryCard.Cult.sloth, LibraryCard.Race.protective));
        cardList.Add(new LibraryCard(963, "Pitkin Chosen", 9, 1, 14, 3, 0, LibraryCard.Cult.greed, LibraryCard.Race.stealthy));

        cardList.Add(new LibraryCard(964, "Archbishop Belle-Dhin", 7, 3, 20, 1, 0, LibraryCard.Cult.pride, LibraryCard.Race.winged));
        cardList.Add(new LibraryCard(965, "First Mistress Salina", 7, 3, 20, 1, 0, LibraryCard.Cult.lust, LibraryCard.Race.veiled));
        cardList.Add(new LibraryCard(966, "Ragelord Zarkhul", 8, 3, 19, 3, 0, LibraryCard.Cult.wrath, LibraryCard.Race.brutal));
        cardList.Add(new LibraryCard(967, "High Inquisitor Waljakov", 8, 3, 19, 1, 0, LibraryCard.Cult.envy, LibraryCard.Race.undead));
        cardList.Add(new LibraryCard(968, "Great Devourer Gilgamosh", 6, 3, 21, 1, 0, LibraryCard.Cult.gluttony, LibraryCard.Race.tough));
        cardList.Add(new LibraryCard(969, "Yawnbringer Keenu", 6, 3, 21, 1, 0, LibraryCard.Cult.sloth, LibraryCard.Race.protective));
        cardList.Add(new LibraryCard(970, "Master Miser Mikoin", 8, 3, 19, 1, 0, LibraryCard.Cult.greed, LibraryCard.Race.stealthy));

        cardList.Add(new LibraryCard(971, "Neverfall, God of Pride", 9, 1, 29, 3, 0, LibraryCard.Cult.pride, LibraryCard.Race.winged));
        cardList.Add(new LibraryCard(972, "Vilerose, God of Lust", 9, 1, 29, 3, 0, LibraryCard.Cult.lust, LibraryCard.Race.veiled));
        cardList.Add(new LibraryCard(973, "Flamegrim, God of Wrath", 10, 1, 28, 3, 0, LibraryCard.Cult.wrath, LibraryCard.Race.brutal));
        cardList.Add(new LibraryCard(974, "Bitterface, God of Envy", 1, 1, 1, 3, 0, LibraryCard.Cult.envy, LibraryCard.Race.undead));
        cardList.Add(new LibraryCard(975, "Rashbite, God of Gluttony", 8, 3, 30, 3, 0, LibraryCard.Cult.gluttony, LibraryCard.Race.tough));
        cardList.Add(new LibraryCard(976, "Dullmoor, God of Sloth", 8, 1, 30, 3, 0, LibraryCard.Cult.sloth, LibraryCard.Race.protective));
        cardList.Add(new LibraryCard(977, "Skinflint, God of Greed", 10, 1, 28, 3, 0, LibraryCard.Cult.greed, LibraryCard.Race.stealthy));

        cardList.Add(new LibraryCard(500, "Dummy", 10, 1, 10, 1, 0, LibraryCard.Cult.pride, LibraryCard.Race.none));
        cardList.Add(new LibraryCard(559, "Ripjaw Chosen_old", 8, 3, 11, 1, 3, LibraryCard.Cult.wrath, LibraryCard.Race.brutal));


        //Non-playCards
        cardList.Add(new LibraryCard(100, "BrutalFx", 0, 0, 0, 0, 0, LibraryCard.Cult.none, LibraryCard.Race.none));
        cardList.Add(new LibraryCard(200, "col_end", 0, 0, 0, 0, 0, LibraryCard.Cult.none, LibraryCard.Race.none));

        cardList.Add(new LibraryCard(300, "leader_buff_pride", 0, 0, 0, 0, 0, LibraryCard.Cult.pride, LibraryCard.Race.none));
        cardList.Add(new LibraryCard(301, "leader_buff_lust", 0, 0, 0, 0, 0, LibraryCard.Cult.lust, LibraryCard.Race.none));
        cardList.Add(new LibraryCard(302, "leader_buff_wrath", 0, 0, 0, 0, 0, LibraryCard.Cult.wrath, LibraryCard.Race.none));
        cardList.Add(new LibraryCard(303, "leader_buff_envy", 0, 0, 0, 0, 0, LibraryCard.Cult.envy, LibraryCard.Race.none));
        cardList.Add(new LibraryCard(304, "leader_buff_gluttony", 0, 0, 0, 0, 0, LibraryCard.Cult.gluttony, LibraryCard.Race.none));
        cardList.Add(new LibraryCard(305, "leader_buff_sloth", 0, 0, 0, 0, 0, LibraryCard.Cult.sloth, LibraryCard.Race.none));
        cardList.Add(new LibraryCard(306, "leader_buff_greed", 0, 0, 0, 0, 0, LibraryCard.Cult.greed, LibraryCard.Race.none));

        cardList.Add(new LibraryCard(364, "belle-dhin_startfx", 0, 0, 0, 0, 0, LibraryCard.Cult.none, LibraryCard.Race.none));
        cardList.Add(new LibraryCard(365, "salina_startfx", 0, 0, 0, 0, 0, LibraryCard.Cult.none, LibraryCard.Race.none));
        cardList.Add(new LibraryCard(366, "zarkhul_startfx", 0, 0, 0, 0, 0, LibraryCard.Cult.none, LibraryCard.Race.none));
        cardList.Add(new LibraryCard(367, "waljakov_startfx", 0, 0, 0, 0, 0, LibraryCard.Cult.none, LibraryCard.Race.none));
        cardList.Add(new LibraryCard(368, "gilgamosh_startfx", 0, 0, 0, 0, 0, LibraryCard.Cult.none, LibraryCard.Race.none));
        cardList.Add(new LibraryCard(369, "keenu_startfx", 0, 0, 0, 0, 0, LibraryCard.Cult.none, LibraryCard.Race.none));
        cardList.Add(new LibraryCard(370, "mikoin_startfx", 0, 0, 0, 0, 0, LibraryCard.Cult.none, LibraryCard.Race.none));


        //monuments
        cardList.Add(new LibraryCard(700, "greed", 0, 0, 0, 0, 0, LibraryCard.Cult.none, LibraryCard.Race.none));
        cardList.Add(new LibraryCard(701, "envy", 0, 0, 0, 0, 0, LibraryCard.Cult.none, LibraryCard.Race.none));
        cardList.Add(new LibraryCard(702, "wrath", 0, 0, 0, 0, 0, LibraryCard.Cult.none, LibraryCard.Race.none));
        cardList.Add(new LibraryCard(703, "pride", 0, 0, 0, 0, 0, LibraryCard.Cult.none, LibraryCard.Race.none));
        cardList.Add(new LibraryCard(704, "gluttony", 0, 0, 0, 0, 0, LibraryCard.Cult.none, LibraryCard.Race.none));
        cardList.Add(new LibraryCard(705, "lust", 0, 0, 0, 0, 0, LibraryCard.Cult.none, LibraryCard.Race.none));
        cardList.Add(new LibraryCard(706, "sloth", 0, 0, 0, 0, 0, LibraryCard.Cult.none, LibraryCard.Race.none));
        cardList.Add(new LibraryCard(707, "health_redux", 0, 0, 0, 0, 0, LibraryCard.Cult.none, LibraryCard.Race.none));

        //setSelector(pile, selectorType, true=ownCard, true=effectOwner)

        //CARD EFFECTS;
        //BASIC CULTISTS EFFECTS

        //------------------------------------------------------------------------------
        //---------------------------------------MARAUDERS
        #region Marauders
        //Sky Folk Marauder
        /*
		GetCard (900).AddFX ()
			.setSelector (PlayCard.Pile.field, LibraryFX.SelectorTap.ready, true, true)
			.setAction (LibraryFX.ActionType.tap)
			.description = "Choose entity to exhaust";
		GetCard (900).AddFX ()
			.setAction (LibraryFX.ActionType.draw, 1);
        /**/
        //Hexfin Marauder
        GetCard(901).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.damageCard, 4)
            .description = "Choose entity to damage";
        GetCard(901).AddFX()
            .setAction(LibraryFX.ActionType.draw, 1);
        //Ripjaw Marauder
        GetCard(902).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 3)
            .setAction(LibraryFX.ActionType.draw);
        #region extended FX
        GetCard(902).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 6)
            .setAction(LibraryFX.ActionType.draw);
        GetCard(902).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 9)
            .setAction(LibraryFX.ActionType.draw);
        #endregion
        //Graveborn Marauder
        GetCard(903).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 1)
            .setAction(LibraryFX.ActionType.draw);
        #region extended FX
        GetCard(903).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 2)
            .setAction(LibraryFX.ActionType.draw);
        GetCard(903).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 3)
            .setAction(LibraryFX.ActionType.draw);
        GetCard(903).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 4)
            .setAction(LibraryFX.ActionType.draw);
        GetCard(903).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 5)
            .setAction(LibraryFX.ActionType.draw);
        #endregion
        //Dreadbulge Marauder
        GetCard(904).AddFX()
            .setAction(LibraryFX.ActionType.draw, 1);
        //Blightbark Marauder
        GetCard(905).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlMoreOpp, 2)
            .setAction(LibraryFX.ActionType.draw, 2);
        #region extended FX
        GetCard(905).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlMoreOpp, 4)
            .setAction(LibraryFX.ActionType.draw, 2);
        GetCard(905).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlMoreOpp, 6)
            .setAction(LibraryFX.ActionType.draw, 2);
        #endregion
        //Pitkin Merchant
        GetCard(906).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOwn, 3)
            .setAction(LibraryFX.ActionType.draw, 2);
        #region extended FX
        GetCard(906).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOwn, 6)
            .setAction(LibraryFX.ActionType.draw, 2);
        GetCard(906).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOwn, 9)
            .setAction(LibraryFX.ActionType.draw, 2);
        #endregion
        #endregion
        //---------------------------------------PREACHERS
        #region Preachers
        //Hexfin Preacher
        GetCard(907).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOwn, 3)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAttack, 2);
        #region extended FX
        GetCard(907).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOwn, 6)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAttack, 2);
        GetCard(907).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOwn, 9)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAttack, 2);
        #endregion
        //Ripjaw Preacher
        GetCard(908).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.ready, true, true)
            .setAction(LibraryFX.ActionType.tap)
            .description = "Choose entity to exhaust";
        GetCard(908).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAttack, 2);
        //Graveborn Preacher
        GetCard(909).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.damageCard, 4)
            .description = "Choose entity to damage";
        GetCard(909).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAttack, 2);
        //Dreadbulge Preacher
        GetCard(910).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 3)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAttack, 2);
        #region extended FX
        GetCard(910).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 6)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAttack, 2);
        GetCard(910).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 9)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAttack, 2);
        #endregion
        //Blightbark Preacher
        GetCard(911).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 1)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAttack, 2);
        #region extended FX
        GetCard(911).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 2)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAttack, 2);
        GetCard(911).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 3)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAttack, 2);
        GetCard(911).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 4)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAttack, 2);
        GetCard(911).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 5)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAttack, 2);
        #endregion
        //Pitkin Preacher
        GetCard(912).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAttack, 2);
        //Skyfolk Priest
        GetCard(913).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlMoreOpp, 2)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAttack, 8);
        #region extended FX
        GetCard(913).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlMoreOpp, 4)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAttack, 8);
        GetCard(913).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlMoreOpp, 6)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAttack, 8);
        #endregion
        #endregion
        //---------------------------------------MISSIONARIES
        #region Missionaries
        //Ripjaw Missionary
        GetCard(914).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlMoreOpp, 2)
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard);
        #region extended FX
        GetCard(914).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlMoreOpp, 4)
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard);
        GetCard(914).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlMoreOpp, 6)
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard);
        #endregion
        //Graveborn Missionary
        GetCard(915).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOwn, 3)
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard);
        #region extended FX
        GetCard(915).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOwn, 6)
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard);
        GetCard(915).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOwn, 9)
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard);
        #endregion
        //Dreadbulge Missionary
        GetCard(916).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.ready, true, true)
            .setAction(LibraryFX.ActionType.tap);
        GetCard(916).AddFX()
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard);
        //Blightbark Missionary
        GetCard(917).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.damageCard, 4)
            .description = "Choose entity to damage";
        GetCard(917).AddFX()
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard);
        //Pitkin Missionary
        GetCard(918).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 3)
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard);
        #region extended FX
        GetCard(918).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 6)
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard);
        GetCard(918).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 9)
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard);
        #endregion
        //Skyfolk Missionary
        GetCard(919).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 1)
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard);
        #region extended FX
        GetCard(919).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 2)
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard);
        GetCard(919).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 3)
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard);
        GetCard(919).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 4)
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard);
        GetCard(919).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 5)
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard);
        #endregion
        //Hexfin Persuader
        GetCard(920).AddFX()
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard);
        GetCard(920).AddFX()
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard);
        #endregion
        //---------------------------------------OVERSEERS
        #region Overseers
        //Graverborn Overseer
        GetCard(921).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAction, 1);
        //Dreadbulge Overseer
        GetCard(922).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlMoreOpp, 2)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAction, 1);
        #region extended FX
        GetCard(922).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlMoreOpp, 4)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAction, 1);
        GetCard(922).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlMoreOpp, 6)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAction, 1);
        #endregion
        //Blightbark Overseer
        GetCard(923).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOwn, 3)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAction, 1);
        #region extended FX
        GetCard(923).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOwn, 6)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAction, 1);
        GetCard(923).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOwn, 9)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAction, 1);
        #endregion
        //Oiiii-(t)kin Overseer
        GetCard(924).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.ready, true, true)
            .setAction(LibraryFX.ActionType.tap);
        GetCard(924).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAction, 1);
        //Skyfolk Overseer
        GetCard(925).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.damageCard, 4)
            .description = "Choose entity to damage";
        GetCard(925).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAction, 1);
        //Hexfin Overseer
        GetCard(926).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 3)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAction, 1);
        #region extended FX
        GetCard(926).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 6)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAction, 1);
        GetCard(926).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 9)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAction, 1);
        #endregion
        //Ripjaw Slaver
        GetCard(927).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 1)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAction, 2);
        #region extended FX
        GetCard(927).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 2)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAction, 2);
        GetCard(927).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 3)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAction, 2);
        GetCard(927).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 4)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAction, 2);
        GetCard(927).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 5)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAction, 2);
        #endregion
        #endregion
        //---------------------------------------WORKERS
        #region Workers
        //Dreadbulge Worker
        GetCard(928).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 1)
            .setAction(LibraryFX.ActionType.damageSelf, -3);
        #region extended FX
        GetCard(928).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 2)
            .setAction(LibraryFX.ActionType.damageSelf, -3);
        GetCard(928).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 3)
            .setAction(LibraryFX.ActionType.damageSelf, -3);
        GetCard(928).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 4)
            .setAction(LibraryFX.ActionType.damageSelf, -3);
        GetCard(928).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 5)
            .setAction(LibraryFX.ActionType.damageSelf, -3);
        #endregion
        //Blightbark Worker
        GetCard(929).AddFX()
            .setAction(LibraryFX.ActionType.damageSelf, -3);
        //Pitkin Worker
        GetCard(930).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlMoreOpp, 2)
            .setAction(LibraryFX.ActionType.damageSelf, -3);
        #region extended FX
        GetCard(930).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlMoreOpp, 4)
            .setAction(LibraryFX.ActionType.damageSelf, -3);
        GetCard(930).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlMoreOpp, 6)
            .setAction(LibraryFX.ActionType.damageSelf, -3);
        #endregion
        //Skyfolk Worker
        GetCard(931).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOwn, 3)
            .setAction(LibraryFX.ActionType.damageSelf, -3);
        #region extended FX
        GetCard(931).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOwn, 6)
            .setAction(LibraryFX.ActionType.damageSelf, -3);
        GetCard(931).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOwn, 9)
            .setAction(LibraryFX.ActionType.damageSelf, -3);
        #endregion
        //Hexfin Worker
        GetCard(932).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.ready, true, true)
            .setAction(LibraryFX.ActionType.tap);
        GetCard(932).AddFX()
            .setAction(LibraryFX.ActionType.damageSelf, -3);
        //Ripjaw Worker
        GetCard(933).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.damageCard, 4)
            .description = "Choose entity to damage";
        GetCard(933).AddFX()
            .setAction(LibraryFX.ActionType.damageSelf, -3);
        //Graveborn Battlemason
        GetCard(934).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 3)
            .setAction(LibraryFX.ActionType.damageSelf, -6);
        #region extended FX
        GetCard(934).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 6)
            .setAction(LibraryFX.ActionType.damageSelf, -6);
        GetCard(934).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 9)
            .setAction(LibraryFX.ActionType.damageSelf, -6);
        #endregion
        #endregion
        //---------------------------------------ADEPTS
        #region Adepts
        //Blightbark Adept
        GetCard(935).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 3)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.damageCard, 2);
        #region extended FX
        GetCard(935).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 6)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.damageCard, 2);
        GetCard(935).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 9)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.damageCard, 2);
        #endregion
        //Pitkin Adept
        GetCard(936).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 1)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.damageCard, 2);
        #region extended FX
        GetCard(936).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 2)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.damageCard, 2);
        GetCard(936).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 3)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.damageCard, 2);
        GetCard(936).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 4)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.damageCard, 2);
        GetCard(936).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 5)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.damageCard, 2);
        #endregion
        //Skyfolk Adept
        GetCard(937).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.damageCard, 2);
        //Hexfin Adept
        GetCard(938).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlMoreOpp, 2)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.damageCard, 4);
        #region extended FX
        GetCard(938).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlMoreOpp, 4)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.damageCard, 4);
        GetCard(938).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlMoreOpp, 6)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.damageCard, 4);
        #endregion
        //Ripjaw Adept
        GetCard(939).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOwn, 3)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.damageCard, 2);
        #region extended FX
        GetCard(939).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOwn, 6)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.damageCard, 2);
        GetCard(939).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOwn, 9)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.damageCard, 2);
        #endregion
        //Graveborn Adept
        GetCard(940).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.ready, true, true)
            .setAction(LibraryFX.ActionType.tap);
        GetCard(940).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.damageCard, 2);
        //Dreadbulge Butcher
        GetCard(941).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.damageCard, 4)
            .description = "Choose entity to damage";
        GetCard(941).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.damageCard, 4);
        #endregion
        //---------------------------------------DOOMSAYERS
        #region Doomsayers
        //Pitkin Doomsayer
        GetCard(942).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.damageCard, 4)
            .description = "Choose entity to damage";
        GetCard(942).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.buffAttack, -3);
        //Skyfolk Doomsayer
        GetCard(943).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 3)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.buffAttack, -3);
        #region extended FX
        GetCard(943).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 6)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.buffAttack, -3);
        GetCard(943).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 9)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.buffAttack, -3);
        #endregion
        //Hexfin Doomsayer
        GetCard(944).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 1)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.buffAttack, -3);
        #region extended FX
        GetCard(944).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 2)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.buffAttack, -3);
        GetCard(944).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 3)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.buffAttack, -3);
        GetCard(944).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 4)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.buffAttack, -3);
        GetCard(944).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 5)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.buffAttack, -3);
        #endregion
        //Ripjaw Doomsayer
        GetCard(945).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.buffAttack, -3);
        //Graveborn Doomsayer
        GetCard(946).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlMoreOpp, 2)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.buffAttack, -6);
        #region extended FX
        GetCard(946).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlMoreOpp, 4)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.buffAttack, -6);
        GetCard(946).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlMoreOpp, 6)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.buffAttack, -6);
        #endregion
        //Dreadbulge Doomsayer
        GetCard(947).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOwn, 3)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.buffAttack, -3);
        #region extended FX
        GetCard(947).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOwn, 6)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.buffAttack, -3);
        GetCard(947).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOwn, 9)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.buffAttack, -3);
        #endregion
        //Blightbark Dashdrainer
        GetCard(948).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.ready, true, true)
            .setAction(LibraryFX.ActionType.tap);
        GetCard(948).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.buffAttack, -6);
        #endregion
        //------------------------------------------------------------------------------

        //BRUTAL FX
        GetCard(100).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.damageCard);
        GetCard(500).AddFX()
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard);

        //------------------------------------------------------------------------------
        GetCard(200).AddFX()
            .setAction(LibraryFX.ActionType.selfDestruct, 4);

        GetCard(300).AddFX()
            .setAction(LibraryFX.ActionType.cultBuff, 2);
        GetCard(301).AddFX()
            .setAction(LibraryFX.ActionType.cultBuff, 2);
        GetCard(302).AddFX()
            .setAction(LibraryFX.ActionType.cultBuff, 2);
        GetCard(303).AddFX()
            .setAction(LibraryFX.ActionType.cultBuff, 2);
        GetCard(304).AddFX()
            .setAction(LibraryFX.ActionType.cultBuff, 2);
        GetCard(305).AddFX()
            .setAction(LibraryFX.ActionType.cultBuff, 2);
        GetCard(306).AddFX()
            .setAction(LibraryFX.ActionType.cultBuff, 2);

        GetCard(559).AddFX()
            .setAction(LibraryFX.ActionType.damageOpp, 6);
        GetCard(961).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.discard);

        //Belle-Dhin
        GetCard(364).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAttack, 2);
        //Salina
        GetCard(365).AddFX()
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard);
        //Zarkhul
        GetCard(366).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAction, 1);
        //Pole (Waljakov)
        GetCard(367).AddFX()
            .setAction(LibraryFX.ActionType.damageSelf, -3);
        //Gilgamosh
        GetCard(368).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.damageCard, 2);
        //Keenu
        GetCard(369).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.buffAttack, -3);
        //Mikoin
        GetCard(370).AddFX()
            .setAction(LibraryFX.ActionType.draw, 1);
        //------------------------------------------------------------------------------

        //greed monument
        GetCard(700).AddFX()
            .setAction(LibraryFX.ActionType.damageSelf, 8);
        GetCard(700).AddFX()
            .setAction(LibraryFX.ActionType.draw, 1);
        //envy monument
        GetCard(701).AddFX()
            .setAction(LibraryFX.ActionType.damageSelf, 8);
        GetCard(701).AddFX()
            .setAction(LibraryFX.ActionType.damageSelf, -3);
        //wrath monument
        GetCard(702).AddFX()
            .setAction(LibraryFX.ActionType.damageSelf, 8);
        GetCard(702).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAction, 1);
        //pride monument
        GetCard(703).AddFX()
            .setAction(LibraryFX.ActionType.damageSelf, 8);
        GetCard(703).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAttack, 2);
        //gluttony monument
        GetCard(704).AddFX()
            .setAction(LibraryFX.ActionType.damageSelf, 8);
        GetCard(704).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.damageCard, 2);
        //lust monument
        GetCard(705).AddFX()
            .setAction(LibraryFX.ActionType.damageSelf, 8);
        GetCard(705).AddFX()
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard, 1);
        //sloth monument
        GetCard(706).AddFX()
            .setAction(LibraryFX.ActionType.damageSelf, 8);
        GetCard(706).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.buffAttack, -3);
        //health reduction fx
        GetCard(707).AddFX()
            .setAction(LibraryFX.ActionType.damageSelf, 0);

        //------------------------------------------------------------------------------

        //CARD EFFECT REFERENCES
        //Card Effect Skyfolk Marauder
        /*
        GetCard(900).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.ready, true, true)
            .setAction(LibraryFX.ActionType.tap)
            .description = "Choose entity to exhaust";
        GetCard(900).AddFX()
            .setAction(LibraryFX.ActionType.draw, 1);

        //Card Effect Hexfin Marauder
        GetCard(901).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.damageCard, 4)
            .description = "Choose entity to damage";
        GetCard(901).AddFX()
            .setAction(LibraryFX.ActionType.draw, 1);

        //Card Effect Graveborn Preacher
        GetCard(902).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.damageCard, 4)
            .description = "Choose entity to damage";
        GetCard(902).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAttack, 2)
            .description = "Choose entity to buff";

        //Card Effect Dreadbulge Preacher
        GetCard(903).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 3)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAttack, 2)
            .description = "Choose entity to buff";
        GetCard(903).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 6)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAttack, 2)
            .description = "Choose entity to buff";
        GetCard(903).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 9)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAttack, 2)
            .description = "Choose entity to buff";
        GetCard(903).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 12)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAttack, 2)
            .description = "Choose entity to buff";

        //Card Effect Pitkin Missionary
        GetCard(904).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 3)
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard).description = "Discard card";
        GetCard(904).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 6)
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard).description = "Discard card";
        GetCard(904).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 9)
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard).description = "Discard card";
        GetCard(904).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 12)
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard).description = "Discard card";
        GetCard(904).AddFX()
            .setCondition(LibraryFX.ConditionType.ctrlOpp, 15)
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard).description = "Discard card";

        //Card Effect Skyfolk Missionary
        GetCard(905).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 2)
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard).description = "Discard card";
        GetCard(905).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 4)
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard).description = "Discard card";
        GetCard(905).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 6)
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard).description = "Discard card";
        GetCard(905).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 8)
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard).description = "Discard card";
        GetCard(905).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 10)
            .setSelector(PlayCard.Pile.hand, LibraryFX.SelectorTap.none, true, false)
            .setAction(LibraryFX.ActionType.discard).description = "Discard card";

        //Card Effect Ripjaw Slaver        
        GetCard(906).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 2)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAction, 2);
        GetCard(906).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 4)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAction, 2);
        GetCard(906).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 6)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAction, 2);
        GetCard(906).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 8)
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, true, true)
            .setAction(LibraryFX.ActionType.buffAction, 2);

        //Card Effect Dreadbulge Worker
        GetCard(907).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 2)
            .setAction(LibraryFX.ActionType.damageSelf, -3);
        GetCard(907).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 4)
            .setAction(LibraryFX.ActionType.damageSelf, -3);
        GetCard(907).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 6)
            .setAction(LibraryFX.ActionType.damageSelf, -3);
        GetCard(907).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 8)
            .setAction(LibraryFX.ActionType.damageSelf, -3);
        GetCard(907).AddFX()
            .setCondition(LibraryFX.ConditionType.kills, 10)
            .setAction(LibraryFX.ActionType.damageSelf, -3);

        //Card Effect Blightbark Worker
        GetCard(908).AddFX()
            .setAction(LibraryFX.ActionType.damageSelf, -3);

        //Card Effect Skyfolk Adept
        GetCard(909).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.damageCard, 2)
            .description = "Choose entity to damage.";
        /*
        // Ripjaw Chosen
        GetCard(910).AddFX()
            .setAction(LibraryFX.ActionType.damageOpp, 4);

        // Blightbark Chosen
        GetCard(911).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.ready, false, true)
            .setAction(LibraryFX.ActionType.tap)
            .description = "Exhaust target ready entity from opponent";

        // Skyfolk Chosen
        GetCard(912).AddFX()
            .setSelector(PlayCard.Pile.field, LibraryFX.SelectorTap.none, false, true)
            .setAction(LibraryFX.ActionType.buffAttack)
            .description = "Target entity your opponent controls has 0 attack this turn.";
        //NON-CARD EFFECTS; NON-SPAWN EFFECTS
        /**/
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

    public LibraryCard GetCard( int id )
    {
        return cardList.Where(card => card.cardID == id).FirstOrDefault();
    }

    public LibraryCard GetCard( string name )
    {
        return cardList.Where(card => card.cardName.Equals(name)).FirstOrDefault();
    }
}

