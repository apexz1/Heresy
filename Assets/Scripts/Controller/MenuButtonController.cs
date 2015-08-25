using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class MenuButtonController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("tutorial")) { LoadTutorial(); }
        if (Input.GetButtonDown("play")) { LoadGame(); }
        if (Input.GetButtonDown("deckbuilder")) { LoadDeckbuilder(); }
        if (Input.GetButtonDown("options")) { LoadOptions(); }
        if (Input.GetButtonDown("gallery")) { LoadGallery(); }
        if (Input.GetButtonDown("credits")) { LoadCredits(); }
        if (Input.GetButtonDown("back")) { LoadMenu(); }
    }

    public void LoadTutorial()
    {
        Application.LoadLevel("tutorial");
    }
    public void LoadGame()
    {
        Application.LoadLevel("main");
    }
    public void LoadDeckbuilder()
    {
        Application.LoadLevel("deckbuilder");
    }
    public void LoadMenu()
    {
        Application.LoadLevel("menu");
    }
    public void LoadOptions()
    {
        Application.LoadLevel("options");
    }
    public void LoadGallery()
    {
        Application.LoadLevel("gallery");
    }
    public void LoadCredits()
    {
        Application.LoadLevel("credits");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
