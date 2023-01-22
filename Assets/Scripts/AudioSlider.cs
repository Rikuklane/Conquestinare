using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

// author https://www.youtube.com/watch?v=BX8IyTmkiMY
[RequireComponent(typeof(Slider))]
public class AudioSlider : MonoBehaviour
{
    Slider slider
    {
        get { return GetComponent<Slider>(); }
    }

    public AudioMixer mixer;
    public string volumeName;
    public TextMeshProUGUI volumeLabel;

    private void Start()
    {
        float volumeSlider = PlayerPrefs.GetFloat("volumeSlider", 1f);
        slider.value = volumeSlider;
    }

    public void UpdateValueOnChange()
    {
        if (mixer != null)
        {
            mixer.SetFloat(volumeName, Mathf.Log(slider.value) * 20f);
            AudioController.Instance.volumeSliderValue = slider.value;
        }
        if (volumeLabel != null) volumeLabel.text = Mathf.Round(slider.value * 100.0f).ToString() + "%";
    }
}
