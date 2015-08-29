using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsMenu : MonoBehaviour
{
    public Resolution[] resolutions;
    private string curRes = "";
    [System.NonSerialized]
    public bool fullscreen = true;

    public static bool isDarkFantasy = false;
    public static bool isWonderland = false;
    public int skin = 0;

    [SerializeField]
    public Button resolutionsButton;
    [SerializeField]
    public Toggle toggleDarkFant;
    [SerializeField]
    public Toggle toggleWonder;
    public Slider slider;
    public Text volumeText;

    void Update()
    {
        if (Input.GetButtonDown("back") || Input.GetKeyDown(KeyCode.Escape)) { BackToMenu(); }
        volumeText.text = "" + (AudioManager.volumeControl * 100) + "%";


        isWonderland = toggleWonder.isOn;
        isDarkFantasy = toggleDarkFant.isOn;

        if (isDarkFantasy) { skin = 0; }
        if (isWonderland) { skin = 1; }


        PlayerPrefs.SetInt("skin", skin);
        PlayerPrefs.SetFloat("volume", AudioManager.volumeControl);
        PlayerPrefs.Save();
    }

    void Awake() { }

    void Start()
    {
        fullscreen = true;
        resolutions = Screen.resolutions;
        Vector3 pos;

        for (int i = 0; i < resolutions.Length; i++)
        {
            pos = new Vector3(0, -(50f * (i + 1)), 0);
            Button button = Instantiate(resolutionsButton) as Button;
            button.transform.localPosition = pos;

            button.transform.SetParent(GetResListTransform().transform, false);
            button.GetComponentInChildren<Text>().text = ResToString(resolutions[i]);

            int index = i;
            button.GetComponent<Button>().onClick.AddListener(() => { SetResolution(index); });
            button.name = "Button" + i.ToString();
            button.gameObject.SetActive(true);
        }

        slider.value = (AudioManager.volumeControl * 100);
        Debug.Log(slider.value);

        toggleWonder.isOn = isWonderland;
        toggleDarkFant.isOn = isDarkFantasy;

        Debug.Log("" + isDarkFantasy + " " + isWonderland);
    }

    public void ChangeVolume()
    {
        AudioManager.volumeControl = (slider.value / 100);
        Debug.Log(AudioManager.volumeControl + ", " + slider.value);
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
        Debug.Log("DarkFantChosen()");
        //if (toggleChecked) { return; }

        //isDarkFantasy = !isDarkFantasy;

        if (toggleDarkFant.isOn)
        {
            isDarkFantasy = true;
            toggleWonder.isOn = false;
        }
        else if (toggleDarkFant.isOn == false)
        {
            isDarkFantasy = false;
            toggleWonder.isOn = true;
        }

        Debug.Log("" + isDarkFantasy + " " + isWonderland);
    }

    public void WonderChosen()
    {
        Debug.Log("WonderChosen()");
        //if (toggleChecked) { return; }

        //isWonderland = !isWonderland;

        if (toggleWonder.isOn)
        {
            isWonderland = true;
            toggleDarkFant.isOn = false;
        }
        else if (toggleWonder.isOn == false)
        {
            isWonderland = false;
            toggleDarkFant.isOn = true;
        }

        Debug.Log("" + isDarkFantasy + " " + isWonderland);
    }

    #endregion

    public void BackToMenu() { Application.LoadLevel("menu"); }

    private static Transform GetResListTransform()
    {
        return GameObject.Find("ResListTransform").transform;
    }
}
