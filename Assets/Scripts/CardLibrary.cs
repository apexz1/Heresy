using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardLibrary : MonoBehaviour
{
    public List<Card> cardList = new List<Card>();

    void Start()
    {
        cardList.Add(new Cultist(0, 0, "TestCard", 0, 0, 0, 0, 0, 0));
    }
}

