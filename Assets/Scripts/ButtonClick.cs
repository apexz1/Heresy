using UnityEngine;
using System.Collections;

public class ButtonClick : MonoBehaviour {

    public DeckManager deckManager;

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
        deckManager.SaveDeck();
    }

    public void Delete()
    {
        deckManager = GameObject.Find("DeckManager").GetComponent<DeckManager>();
        deckManager.DeleteDeck();
    }

}
