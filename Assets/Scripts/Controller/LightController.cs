using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //Debug.Log("rotation: " + transform.eulerAngles.x);
        if (GameManager.Get().running == true)
        {
            if (transform.eulerAngles.x > 270 && transform.eulerAngles.x < 330)
            {
                Debug.Log("faster rotation?");
                transform.Rotate(80 * Time.deltaTime, 0, 0);
            }
            else
            {
                transform.Rotate(10 * Time.deltaTime, 0, 0);
            }
        }  
	}
}
