using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class MenuButtonController : MonoBehaviour
{

    public void LoadGame() {
        Debug.Log("game loaded");
        Application.LoadLevel("main");
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

    public void Save() {
        InputField inputField = GameObject.Find("deckName").GetComponent<InputField>();

        string deckName = inputField.text;

        DeckBuilder.Get().SaveDeck(deckName);
        Debug.Log(deckName + "saved " + DeckBuilder.Get().deck.Count + " entries");
    }

    public void Load() {
        InputField inputField = GameObject.Find("deckName").GetComponent<InputField>();

        string deckName = inputField.text;
        //inputField.text.Remove(0,inputField.text.Length);
        //inputField.textComponent.text = "";

        DeckBuilder.Get().LoadDeck(deckName);
    }
    public void Delete() {
        InputField inputField = GameObject.Find("deckName").GetComponent<InputField>();

        string deckName = inputField.text;

        DeckBuilder.Get().DeleteDeck(deckName);
        Debug.Log(deckName + "deleted");
    }

    public void Clear() {
        //Debug.Log("old:" + deckManager.deck.Count);

        DeckBuilder.Get();
        InputField inputField = GameObject.Find("deckName").GetComponent<InputField>();

        DeckBuilder.Get().ClearDeck();
        //Debug.Log("new " + deckManager.deck.Count);
        Debug.Log("cleared");
    }

    public void Remove() {
        Debug.Log(gameObject.name);
        DeckBuilder.Get().RemoveCard(this.GetComponent<Button>());

        //Destroy(gameObject.transform.parent.gameObject);
    }
}
