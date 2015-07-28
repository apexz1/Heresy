using UnityEngine;
using System.Collections;

public class CardControllerDB : MonoBehaviour {

    public DeckBuilder deckManager;

	void Start () {
        //Don't know what the fuck I'm doing here, but works. #coding101
        deckManager = GameObject.Find("DeckBuilder").GetComponent<DeckBuilder>();
	}

    void OnMouseOver() {

        if(Input.GetButtonDown("Fire1")) {
            int cardIndex = -1;
            string name = transform.gameObject.name.Replace("(Clone)", "");
            Debug.Log(name);

            for (int i = 0; i < CardLibrary.Get().cardList.Count; i++)
            {
                if (name == CardLibrary.Get().cardList[i].cardName)
                {
                    cardIndex = CardLibrary.Get().cardList[i].cardID;
                }
            }

            Debug.Log(cardIndex);
            if (cardIndex == -1) { return; }
            deckManager.AddCard(cardIndex);
        }
    }
}
