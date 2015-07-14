using UnityEngine;
using System.Collections;

public class CardController : MonoBehaviour {

    public GameObject abcObject;

    public CardLibrary cardLibrary;
    public DeckManager deckManager;

	// Use this for initialization
	void Start () {
        //Don't know what the fuck I'm doing here, but works. #coding101
        cardLibrary = GameObject.Find("CardLibary").GetComponent<CardLibrary>();
        deckManager = GameObject.Find("DeckManager").GetComponent<DeckManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseOver() {
        
        if(Input.GetButtonDown("Fire1")) {            
            //Debug.Log(this.gameObject.name);
            //Debug.Log(cardLibrary.cardList[3].GetName());
            string name = this.gameObject.name.Replace("(Clone)","");
            Debug.Log(name);
            deckManager.AddCard(name);
        }
    }
}
