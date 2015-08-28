using UnityEngine;
using System.Collections;

public class MonumentHover : MonoBehaviour
{

    GameObject child;
    bool side = true;
    bool init = true;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if(GameManager.Get().running && init)
        {
            Texture2D tex = (Texture2D)Resources.Load("Images/cards_DF/preview/700");

            var cult = GameManager.Get().players[GameManager.Get().localPlayerId].cult;

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

            //sides
            if (gameObject.transform.parent.name == "16")
            {
                side = true;
            }
            if (gameObject.transform.parent.name == "4")
            {
                side = false;
            }

            Debug.Log("texture loading debug: " + skin);
            if (cult == "greed") { tex = (Texture2D)Resources.Load("Images/" + skin + "/preview/" + "700"); }
            if (cult == "envy") { tex = (Texture2D)Resources.Load("Images/" + skin + "/preview/" + "701"); }
            if (cult == "wrath") { tex = (Texture2D)Resources.Load("Images/" + skin + "/preview/" + "702"); }
            if (cult == "pride") { tex = (Texture2D)Resources.Load("Images/" + skin + "/preview/" + "703"); }
            if (cult == "gluttony") { tex = (Texture2D)Resources.Load("Images/" + skin + "/preview/" + "704"); }
            if (cult == "lust") { tex = (Texture2D)Resources.Load("Images/" + skin + "/preview/" + "705"); }
            if (cult == "sloth") { tex = (Texture2D)Resources.Load("Images/" + skin + "/preview/" + "706"); }


            //Texture2D tex = 
            Debug.Log("agmjdstdhasdhofidasjf" + GameObject.Find("SceneCam"));
            Debug.Log("agmjdstdhasdhofidasjf" + GameObject.Find("SceneCam").transform.FindChild("ZoomCard" + (side ? "L" : "R")).gameObject);
            child = GameObject.Find("SceneCam").transform.FindChild("ZoomCard" + (side ? "L" : "R")).gameObject;

            AssignTexture(tex);

            init = false;
        }
    }

    void OnMouseEnter()
    {
        Debug.Log("adoghjdsohofijdapokdfsg");
        child.SetActive(true);
    }

    void OnMouseExit()
    {
        child.SetActive(false);
    }

    void AssignTexture (Texture2D tex)
    {
        MeshRenderer rend = child.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        rend.material.mainTexture = tex;
    }

}
