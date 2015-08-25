using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsMenu : MonoBehaviour
{
    public Resolution[] resolutions;
    private string curRes = "";
    //public bool showRes = false;
    [System.NonSerialized]
    public bool fullscreen = true;
    public static bool isDarkFantasy = false;
    public static bool isWonderland = true;

    [SerializeField]
    public Button resolutionsButton;
    //[SerializeField]
    //public Button showResButton;
    [SerializeField]
    public Toggle toggleDarkFant;
    [SerializeField]
    public Toggle toggleWonder;

    void Update() { if (Input.GetButtonDown("back")) { BackToMenu(); } }

    void Start()
    {
        fullscreen = true;
        resolutions = Screen.resolutions;
        Vector3 pos;
        //Button resButton = Instantiate(showResButton) as Button;
        //Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);
        /*
        resButton.transform.SetParent(GetResListTransform().transform, false);
        resButton.transform.localPosition = new Vector3(0, 0, 0);
        resButton.GetComponentInChildren<Text>().text = "Choose Resolution";
        resButton.GetComponent<Button>().onClick.AddListener(() => { ShowResButtons(); });
        */
        for (int i = 0; i < resolutions.Length; i++)
        {
            pos = new Vector3(0, -(35f * (i + 1)), 0);
            Button button = Instantiate(resolutionsButton) as Button;
            button.transform.localPosition = pos;

            button.transform.SetParent(GetResListTransform().transform, false);
            button.GetComponentInChildren<Text>().text = ResToString(resolutions[i]);

            int index = i;
            button.GetComponent<Button>().onClick.AddListener(() => { SetResolution(index); });
            button.name = "Button" + i.ToString();
            button.gameObject.SetActive(true);
        }

        toggleDarkFant.isOn = true;
        isDarkFantasy = true;
        Debug.Log("isDarkFantasy " + isDarkFantasy);
        Debug.Log("isWonderland " + isWonderland);
    }

    #region Resolution

    public void SetResolution( int index )
    {
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, fullscreen);
    }

    public string ResToString( Resolution res )
    {
        return res.width + " x " + res.height;
    }
    /*
public void ShowResButtons() {
    showRes = !showRes;
    Debug.Log(showRes);
    ShowResolutions();
}
    
public void ShowResolutions() {
    for (int i = 0; i < resolutions.Length; i++)
    {
        GameObject.Find("ResListTransform").transform.FindChild("Button" + i.ToString()).gameObject.SetActive(showRes);
    }
}
*/
    public string ShowCurrentRes()
    {
        return curRes = Screen.currentResolution.width + "x" + Screen.currentResolution.height;
    }

    public void SwitchFullscreen()
    {
        fullscreen = !fullscreen;
    }
    #endregion

    #region Skins

    public void DarkFantChosen()
    {

        isDarkFantasy = !isDarkFantasy;

        if (toggleDarkFant.isOn)
        {
            toggleWonder.isOn = false;
        }
        else if (toggleDarkFant.isOn == false)
        {
            toggleWonder.isOn = true;
        }

        Debug.Log("isDarkFantasy " + isDarkFantasy);
        Debug.Log("isWonderland " + isWonderland);
    }

    public void WonderChosen()
    {

        isWonderland = !isWonderland;

        if (toggleWonder.isOn)
        {
            toggleDarkFant.isOn = false;
        }
        else if (toggleWonder.isOn == false)
        {
            toggleDarkFant.isOn = true;
        }

        Debug.Log("isDarkFantasy " + isDarkFantasy);
        Debug.Log("isWonderland " + isWonderland);
    }

    #endregion

    public void BackToMenu() { Application.LoadLevel("menu"); }

    private static Transform GetResListTransform()
    {
        return GameObject.Find("ResListTransform").transform;
    }
}
