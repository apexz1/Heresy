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
            cardList.Add(new Card());
        }     
    }
}

