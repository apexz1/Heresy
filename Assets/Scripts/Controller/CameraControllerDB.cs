using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraControllerDB : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    Slider slider;

    void Start()
    {
        slider = GameObject.Find("ViewSlider").GetComponent<Slider>();
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            slider.value += 0.03f;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            slider.value -= 0.03f;
        }
    }

    public void ScrollCamera()
    {
        Vector3 pos = cam.transform.position;
        pos.y = -(-2.1f + (slider.value * 74f));
        cam.transform.position = new Vector3(pos.x, pos.y, pos.z);
    }
}
