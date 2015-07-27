using UnityEngine;
using System.Collections;

public class PlayCardController : MonoBehaviour {

    public Transform target;
    bool popup = false;
	// Use this for initialization
	void Start () 
    {
        target = GameObject.Find("SceneCam").transform;
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
                        /*for (int i = 0; i < GameManager.Get().players[playerId].playHand.Count; i++)
                        {
                            if (gameObject == GameManager.Get().players[playerId].playHand[i])
                            {
                                Debug.Log("card found in hand: " + GameManager.Get().players[playerId]);
                            }
                        }*/
                        Debug.Log("Card found");
                    }
                }
            }
        }

        if (gameObject.transform.parent.gameObject.name == "Hand")
            PopUp();
    }

    void OnMouseExit()
    {
        gameObject.transform.GetChild(0).transform.position = gameObject.transform.position;
    }

    void PopUp()
    {
        //transform.GetChild(0).localRotation = Quaternion.Euler(-120,0,0);
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
    }
}
