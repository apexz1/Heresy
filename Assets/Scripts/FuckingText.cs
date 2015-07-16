using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FuckingText : MonoBehaviour {

    Text txt;
	// Use this for initialization
	void Start () {

        txt = gameObject.GetComponent<Text>();
        txt.text = DeckManager.listCardName;
	}
}
