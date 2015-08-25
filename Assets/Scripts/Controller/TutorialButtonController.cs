using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialButtonController : MonoBehaviour
{

    public static int index = 0;
    public TutorialController tutCtrl;

    GameObject p, n;

    public static TutorialButtonController GetTBC()
    {
        return GameObject.Find("TutorialButtonController").GetComponent<TutorialButtonController>();
    }

    public void Start()
    {
        tutCtrl = TutorialController.Get();
        p = GameObject.Find("Previous");
        n = GameObject.Find("Next");

        Debug.Log(tutCtrl.sprites.Count);
    }

    public void Update()
    {
        if (index == 0)
        {
            p.SetActive(false);
        }
        else if (index == tutCtrl.sprites.Count-1)
        {
            n.SetActive(false);
        }
        else if (index > 0 && index < tutCtrl.sprites.Count-1)
        {
            p.SetActive(true);
            n.SetActive(true);
        }
    }

    public void UITutNext()
    {
        Debug.Log(index);
        if (index < (tutCtrl.sprites.Count - 1))
        {
            index += 1;
            Image image = GameObject.Find("TutScreen").GetComponent<Image>();
            image.sprite = tutCtrl.sprites[index];
        }
    }
    public void UITutPrev()
    {
        Debug.Log(index);
        if (index > 0)
        {
            index -= 1;
            Image image = GameObject.Find("TutScreen").GetComponent<Image>();
            image.sprite = tutCtrl.sprites[index];
        }
    }
    public void LoadMenu()
    {
        Application.LoadLevel("menu");
    }
}
