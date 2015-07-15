using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class ButtonClick : MonoBehaviour {

    public DeckManager deckManager;
    public InputField inputField;

    public void LoadGame() {
        Debug.Log("game loaded");
        Application.LoadLevel("first");
    }
    public void LoadDeckbuilder() {
        Debug.Log("Deckbuilder loaded");
        Application.LoadLevel("deckbuilder");
    }
    public void LoadMenu() {
        Debug.Log("menu loaded");
        Application.LoadLevel("menu");
    }
    public void LoadOptions() {
        Debug.Log("options loaded");
        Application.LoadLevel("options");
    }
    public void LoadCredits() {
        Debug.Log("credits loaded");
        Application.LoadLevel("credits");
    }
    public void QuitGame() {
        Debug.Log("game quit");
        Application.Quit();
    }


    public void Save()
    {
        deckManager = GameObject.Find("DeckManager").GetComponent<DeckManager>();
        inputField = GameObject.Find("deckName").GetComponent<InputField>();
        string deckName = inputField.text; 
       
        deckManager.SaveDeck(deckName);
        Debug.Log(deckName + " saved. Inhalt: " + deckManager.deck.Count);
    }

    public void Delete()
    {
        deckManager = GameObject.Find("DeckManager").GetComponent<DeckManager>();
        inputField = GameObject.Find("deckName").GetComponent<InputField>();
        string deckName = inputField.text;

        deckManager.DeleteDeck(deckName);
        Debug.Log(deckName + "deleted");
    }

    public void Clear() {
        deckManager = GameObject.Find("DeckManager").GetComponent<DeckManager>();
        Debug.Log("old:" + deckManager.deck.Count);
        deckManager.deck.Clear();
        Debug.Log("new " + deckManager.deck.Count);
        Debug.Log("cleared");
    }
}
