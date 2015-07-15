using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardLibrary : MonoBehaviour
{
    public List<Card> cardList = new List<Card>();

    void Start()
    {
        cardList.Add(new Cultist(0, "TestCard0", 0, 0, 0, 0, 0, 0));
        cardList.Add(new Cultist(1, "TestCard1", 0, 0, 0, 0, 0, 0));
        cardList.Add(new Cultist(2, "TestCard2", 0, 0, 0, 0, 0, 0));
    }
}

