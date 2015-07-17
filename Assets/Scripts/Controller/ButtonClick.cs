using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class ButtonClick : MonoBehaviour {

    public DeckManager deckManager;
    public InputField inputField;

    public void Start()
    {

    }

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
        Debug.Log(deckName + "saved " + deckManager.deck.Count + " entries");
    }

    public void Load()
    {
        deckManager = GameObject.Find("DeckManager").GetComponent<DeckManager>();
        inputField = GameObject.Find("deckName").GetComponent<InputField>();

        string deckName = inputField.text;
        //inputField.text.Remove(0,inputField.text.Length);
        //inputField.textComponent.text = "";

        deckManager.LoadDeck(deckName);
    }
    public void Delete()
    {
        deckManager = GameObject.Find("DeckManager").GetComponent<DeckManager>();
        inputField = GameObject.Find("deckName").GetComponent<InputField>();

        string deckName = inputField.text;

        deckManager.DeleteDeck(deckName);
        Debug.Log(deckName + "deleted");
    }

    public void Clear() 
    {
        //Debug.Log("old:" + deckManager.deck.Count);

        deckManager = GameObject.Find("DeckManager").GetComponent<DeckManager>();
        inputField = GameObject.Find("deckName").GetComponent<InputField>();

        deckManager.ClearDeck();
        //Debug.Log("new " + deckManager.deck.Count);
        Debug.Log("cleared");
    }

    public void Remove()
    {
        print(gameObject.name);

        deckManager = GameObject.Find("DeckManager").GetComponent<DeckManager>();
        inputField = GameObject.Find("deckName").GetComponent<InputField>();

        Text txt = gameObject.GetComponentInChildren<Text>();
        deckManager.RemoveCard(txt.text);

        Destroy(gameObject.transform.parent.gameObject);
    }
}
