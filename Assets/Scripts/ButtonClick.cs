﻿using UnityEngine;
using System.Collections;

public class ButtonClick : MonoBehaviour {

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

}
