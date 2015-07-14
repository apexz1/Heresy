using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardLibrary : MonoBehaviour
{
    public List<Card> cardList = new List<Card>();

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            cardList.Add(new Cultist());
        }

        cardList.Add(new Cultist(0, 0, "TestCard", 0, 0, 0, 0, 0, 0));
    }
}

