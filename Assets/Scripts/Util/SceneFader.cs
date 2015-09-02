using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFader : MonoBehaviour
{

    private float fadeSpeed = 2.0f;
    private bool sceneStarting = true;
    public Image image;

    void Start()
    {
        //GameObject.Find("Fader").gameObject.SetActive(true);
    }

    void Update()
    {
        if (sceneStarting)
            StartScene();
    }

    public void FadeToClear()
    {
        // Lerp the colour of the texture between itself and transparent.
        image.color = Color.Lerp(image.color, Color.clear, fadeSpeed * Time.deltaTime);
    }

    public void FadeToBlack()
    {
        // Lerp the colour of the texture between itself and black.
        image.color = Color.Lerp(image.color, Color.black, fadeSpeed * Time.deltaTime);
    }

    void StartScene()
    {

        FadeToClear();

        // If the texture is almost clear...
        if (image.color.a <= 0.01f)
        {
            image.color = Color.clear;
            image.enabled = false;

            sceneStarting = false;
        }
    }
}
