using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DBButtonController : MonoBehaviour
{
    public InputField inputField;

    void Update()
    {
        if (Input.GetButtonDown("back") && !inputField.isFocused) { LoadMenu(); }
    }

    void Start()
    {
        inputField = GameObject.Find("deckName").GetComponent<InputField>();
    }

    public void Save()
    {

        string deckName = inputField.text;

        DeckBuilder.Get().SaveDeck(deckName);
        //Debug.Log(deckName + "saved " + DeckBuilder.Get().deck.Count + " entries");
    }

    public void Load()
    {
        //InputField inputField = GameObject.Find("deckName").GetComponent<InputField>();

        string deckName = inputField.text;
        //inputField.text.Remove(0,inputField.text.Length);
        //inputField.textComponent.text = "";

        DeckBuilder.Get().LoadDeck(deckName);
    }
    public void Delete()
    {
        //InputField inputField = GameObject.Find("deckName").GetComponent<InputField>();

        string deckName = inputField.text;

        DeckBuilder.Get().DeleteDeck(deckName);
        Debug.Log(deckName + "deleted");
    }

    public void Clear()
    {
        DeckBuilder.Get();
        //InputField inputField = GameObject.Find("deckName").GetComponent<InputField>();

        DeckBuilder.Get().ClearDeck();
        Debug.Log("cleared");
    }

    public void Remove()
    {
        Debug.Log(gameObject.name);
        DeckBuilder.Get().RemoveCard(this.GetComponent<Button>());

        //Destroy(gameObject.transform.parent.gameObject);
    }

    public void LoadMenu()
    {
        Application.LoadLevel("menu");
    }
}
