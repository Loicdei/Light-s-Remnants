using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class Options : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Dropdown resolutionDropDown;
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private AudioMixer audioMixer;

    private Resolution[] resolutions;
    private int currentResolutionID;

    private void Awake()
    {
        //Init
        resolutionDropDown.ClearOptions();
        resolutions = Screen.resolutions;

        List<string> _resolutionLabels = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            _resolutionLabels.Add(resolutions[i].ToString());
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) currentResolutionID = i;
        }

        resolutionDropDown.AddOptions(_resolutionLabels);

        //Init les valeurs
        resolutionDropDown.value = currentResolutionID;
        fullScreenToggle.isOn = Screen.fullScreen;
        audioMixer.GetFloat("Master", out float _volume);
        volumeSlider.value = Mathf.InverseLerp(-85f, 0f, _volume);

        //Link pour les events
        volumeSlider.onValueChanged.AddListener(UpdateVolume);
        resolutionDropDown.onValueChanged.AddListener(UpdateResolution);
        fullScreenToggle.onValueChanged.AddListener(ToggleFullscreen);
    }

    private void UpdateVolume(float _value)
    {
        print("Audio Volume : " +  _value);
        audioMixer.SetFloat("Master", Mathf.Lerp(-100, 0 , _value));
    }

    private void UpdateResolution(int _value)
    {
        currentResolutionID = _value;
        Screen.SetResolution(resolutions[currentResolutionID].height, resolutions[currentResolutionID].width, Screen.fullScreen); 
        print("Résolution ID : " + _value);
    }

    private void ToggleFullscreen(bool _value)
    {
        print("Fullscreen : " + _value);
        Screen.fullScreen = _value;
    }
}
