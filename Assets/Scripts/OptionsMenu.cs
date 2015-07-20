using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsMenu : MonoBehaviour
{

    Resolution[] resolutions;

    public Transform menuPanel;
    public GameObject buttonPrefab;

    // Use this for initialization
    void Start()
    {
        resolutions = Screen.resolutions;
        for (int i = 0; i < resolutions.Length; i++)
        {
            GameObject button = (GameObject)Instantiate(buttonPrefab);
            button.GetComponentInChildren<Text>().text = ResToString(resolutions[i]);
            int index = i;
            /*button.GetComponent<Button>().onClick.AddListener(
                () => { SetResolution(index); }
                );*/

            button.transform.parent = menuPanel;
        }
    }

    public void SetResolution(int index)
    {
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, false);
    }

    public string ResToString(Resolution res)
    {
        return res.width + " x " + res.height;
    }
}
