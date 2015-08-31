using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    private static AudioManager _instance;
    public static float volumeControl = 1.0f;

    public static AudioSource audio;
    public static AudioClip sound_menus;
    public static AudioClip sound_main;

    public static AudioManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<AudioManager>();

                //Tell unity not to destroy this object when loading a new scene!
                DontDestroyOnLoad(_instance.gameObject);
            }       
            return _instance;
        }
    }

    void Start() { audio = GetComponent<AudioSource>(); }

    void Update() { audio.volume = volumeControl; }

    void Awake()
    {
        Debug.Log("awake()");   

        sound_menus = Resources.Load("Audio/audio_background01") as AudioClip;
        sound_main = Resources.Load("Audio/battle_music_noLoop") as AudioClip;

        if (_instance == null)
        {
            //If I am the first instance, make me the Singleton
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            //If a Singleton already exists and you find
            //another reference in scene, destroy it!
            if (this != _instance)
                Destroy(this.gameObject);
        }
    }

    public static void ChangeMainMusic() 
    {
        if (Application.loadedLevelName == "main") {   
            Debug.Log("ChangeMainMusic");
			if(audio!=null)
			{
            	audio.Stop();
            	audio.clip = sound_main;
            	audio.Play();
			} else {
				Debug.LogError("Missing Audio Src!!!");
			}
        } else { Debug.LogError("Coudln't load Main music"); }
    }
}
