using UnityEngine;
using System.Collections;

public class MonumentHover : MonoBehaviour
{

    GameObject child;
    bool side = true;
    bool init = true;
	bool hover = false;

	Texture2D tex;

    // Use this for initialization
    void Start()
    {
		//sides
		if (gameObject.transform.parent.name == "16")
		{
			side = true;
		}
		if (gameObject.transform.parent.name == "4")
		{
			side = false;
		}
    }

    // Update is called once per frame
    void Update()
    {
		if (GameManager.Get().running)
		{
			child = GameObject.Find("SceneCam").transform.FindChild("ZoomCard" + (side ? "L" : "R")).gameObject;
		}

		if(GameManager.Get().running && hover)
        {
			LoadTex();
            AssignTexture(tex);
        }
    }

    void OnMouseEnter()
    {
        //Debug.Log("adoghjdsohofijdapokdfsg");
		hover = true;
		child.SetActive(true);
    }

    void OnMouseExit()
    {
		hover = false;
        child.SetActive(false);
    }

    void AssignTexture (Texture2D tex)
    {
        MeshRenderer rend = child.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        rend.material.mainTexture = tex;
    }

	void LoadTex()
	{	
		var cult = "";
		if (side == true)
		{
			cult = GameManager.Get().players[0].cult;
		}
		if (side == false)
		{
			cult = GameManager.Get().players[1].cult;
		}


			
		string skin = "";
			
		//skins
		if (OptionsMenu.isDarkFantasy)
		{
			skin = "cards_DF";
		}
		if (OptionsMenu.isWonderland)
		{
			skin = "cards_WL";
		}
		else
		{
			skin = "cards_DF";
		}
			
		//Debug.Log("texture loading debug: " + skin);
		if (cult == "greed") { tex = (Texture2D)Resources.Load("Images/" + skin + "/preview/" + "700"); }
		if (cult == "envy") { tex = (Texture2D)Resources.Load("Images/" + skin + "/preview/" + "701"); }
		if (cult == "wrath") { tex = (Texture2D)Resources.Load("Images/" + skin + "/preview/" + "702"); }
		if (cult == "pride") { tex = (Texture2D)Resources.Load("Images/" + skin + "/preview/" + "703"); }
		if (cult == "gluttony") { tex = (Texture2D)Resources.Load("Images/" + skin + "/preview/" + "704"); }
		if (cult == "lust") { tex = (Texture2D)Resources.Load("Images/" + skin + "/preview/" + "705"); }
		if (cult == "sloth") { tex = (Texture2D)Resources.Load("Images/" + skin + "/preview/" + "706"); }

	}
}
