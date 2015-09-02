using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

	private bool loading = true;
	
	public Texture loadingTexture;
	
	void Awake () {
		DontDestroyOnLoad(gameObject);
	}

	void Start() {
		loadingTexture = (Texture2D)Resources.Load("Images/UI/inGame/fake_loadingscreen");
	}
	
	void Update () {

		if(Application.isLoadingLevel)
		{
			loading = true;
		}
		else
		{
			loading = false;
		}
	}
	
	void OnGUI () {
		if(loading)
			GUI.DrawTexture (new Rect(0,0,Screen.width,Screen.height), loadingTexture, ScaleMode.StretchToFill);
	}
}
