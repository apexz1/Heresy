using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class TutorialController : MonoBehaviour
{

    public byte[] screens;
    public Sprite current = new Sprite();
    public List<Sprite> sprites = new List<Sprite>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetButtonDown("Fire1")) { TutorialButtonController.GetTBC().UITutNext(); }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetButtonDown("Fire2")) { TutorialButtonController.GetTBC().UITutPrev(); }
        if (Input.GetButtonDown("back") || Input.GetKeyDown(KeyCode.Escape)) { TutorialButtonController.GetTBC().LoadMenu(); }
    }

    public void Start()
    {
        var array = Resources.LoadAll("Images/tut", typeof(Texture2D));
        Image image = GameObject.Find("TutScreen").GetComponent<Image>();

        Debug.Log("files loaded " + array.Length);

        var imgArray = new Texture2D[array.Length];

        for (int i = 0; i < array.Length; i++)
        {
            imgArray[i] = array[i] as Texture2D;
        }

        for (int i = 0; i < imgArray.Length; i++)
        {
            current = Sprite.Create(imgArray[i], new Rect(0, 0, imgArray[i].width, imgArray[i].height), new Vector2(0.5f, 0.5f));
            Debug.Log(current);
            sprites.Add(current);
        }

        image.sprite = sprites[0];
    }

    public static TutorialController Get()
    {
        return GameObject.Find("TutorialController").GetComponent<TutorialController>();
    }
}
