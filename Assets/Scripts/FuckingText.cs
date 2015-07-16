using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FuckingText : MonoBehaviour {

    Text txt;
	// Use this for initialization
	void Start () {

        //Still no idea. Fuck yeah.
        txt = gameObject.GetComponent<Text>();
        txt.text = DeckManager.listCardName;
	}
}
