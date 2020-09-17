using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SettingsMenu : MonoBehaviour
{
    //[SerializeField] AudioMixer masterAudioMixer;
    [SerializeField] Dropdown resolutionDropDown;
    Resolution[] resolutions;
    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropDown.ClearOptions();

        List<string> graphicsOptions = new List<string>();

        int currentResolutionSetting = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            graphicsOptions.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionSetting = i;
            }
        }
        resolutionDropDown.AddOptions(graphicsOptions);
        resolutionDropDown.value = currentResolutionSetting;
        resolutionDropDown.RefreshShownValue();
    }

    /*
    public void SetVolume(float volume)
    {
        masterAudioMixer.SetFloat("MainVolume", volume);
    }
    */
    public void SetGraphicsQuality(int graphicsQuality)
    {
        QualitySettings.SetQualityLevel(graphicsQuality);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetScreenResolution(int graphicsResolutionIndex)
    {
        Resolution resolution = resolutions[graphicsResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
