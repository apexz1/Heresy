using UnityEngine;
using System.Collections;

public class CardControllerDB : MonoBehaviour {

    public DeckBuilder deckManager;

	void Start () {
        //Don't know what the fuck I'm doing here, but works. #coding101
        deckManager = GameObject.Find("DeckBuilder").GetComponent<DeckBuilder>();
	}

    void OnMouseOver() {

        Debug.Log("card found");

        if(Input.GetButtonDown("Fire1")) {            
            //Debug.Log(cardLibrary.cardList[3].GetName());
            string name = transform.gameObject.name.Replace("(Clone)", "");
            Debug.Log(name);
            //Debug.Log(name);
            deckManager.AddCard(name);
        }
    }
}
