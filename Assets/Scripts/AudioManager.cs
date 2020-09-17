using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField] AudioMixer _MasterMixer;

    public override void Init()
    {
        base.Init();
    }
    public void SetMasterVolume(Slider volume)
    {
        _MasterMixer.SetFloat("MasterVolume", volume.value);
    }

    public void SetBGMVolume(Slider volume)
    {
        _MasterMixer.SetFloat("BackgroundMusicVolume", volume.value);
    }

    public void SetSFXVolume(Slider volume)
    {
        _MasterMixer.SetFloat("SFXVolume", volume.value);
    }

}