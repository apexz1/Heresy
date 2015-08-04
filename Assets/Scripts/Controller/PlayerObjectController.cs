using UnityEngine;
using System.Collections;

public class PlayerObjectController : MonoBehaviour {


    GameManager gameManager;
    public int playerId;

    public FieldController GetFieldController()
    {
        return FieldController.GetFieldController();
    }

	// Use this for initialization
	void Start () {
        gameManager = GameManager.Get();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseOver()
    {
        //Debug.Log(FieldController.GetFieldController());
        //Debug.Log(playerId);
        if (Input.GetButtonDown("Fire1"))
        {
            //ShowTempleSkillMenu
        }
        if (Input.GetButtonDown("Fire2"))
        {
            GetFieldController().OnPlayerClicked(playerId);
        }
    }
}
