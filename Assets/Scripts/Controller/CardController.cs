using UnityEngine;
using System.Collections;

public class CardController : MonoBehaviour {

    public DeckBuilder deckManager;

	// Use this for initialization
	void Start () {
        //Don't know what the fuck I'm doing here, but works. #coding101
        deckManager = GameObject.Find("DeckBuilder").GetComponent<DeckBuilder>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseOver() {
        
        if(Input.GetButtonDown("Fire1")) {            
            Debug.Log(transform.parent.gameObject.name);
            //Debug.Log(cardLibrary.cardList[3].GetName());
            string name = transform.parent.gameObject.name.Replace("(Clone)", "");
            //Debug.Log(name);
            deckManager.AddCard(name);
        }
    }
}
