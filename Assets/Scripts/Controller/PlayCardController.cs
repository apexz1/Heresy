using UnityEngine;
using System.Collections;

public class PlayCardController : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        //GameManager.Get().AssignTexture();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseOver()
    {
        int playerId = GameManager.Get().localPlayerId;
        GameObject parentObj;

        if (Input.GetButtonDown("Fire1"))
        {
            parentObj = gameObject.transform.parent.gameObject;

            if (parentObj.name == "Hand")
            {
                {
                    if(GameManager.Get().turn)
                    {
                        Debug.Log("card found in hand: " + transform.gameObject);
                    }
                }
            }
        }
    }
}
