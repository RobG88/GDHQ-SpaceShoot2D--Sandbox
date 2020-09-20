using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoSingleton<AudioManager>
{
    public AudioMixer _MasterMixer;

    [SerializeField] Slider _SFX_Volume_Slider;
    [SerializeField] Slider _Music_Volume_Slider;

    [SerializeField] string _SFX_Exposed_parameter;
    [SerializeField] string _BGM_Exposed_parameter;

    [SerializeField] float _playerPrefBGMusic_Volume;
    [SerializeField] float _playerPrefSFX_Volume;

    public override void Init()
    {
        base.Init();
    }

    private void Start()
    {
        // The getting & setting AudioMixer needs to happen in Start(), can not be Awake
        _playerPrefSFX_Volume = PlayerPrefs.GetFloat(_SFX_Exposed_parameter);
        _playerPrefBGMusic_Volume = PlayerPrefs.GetFloat(_BGM_Exposed_parameter);

        _SFX_Volume_Slider.value = PlayerPrefs.GetFloat(_SFX_Exposed_parameter);
        _Music_Volume_Slider.value = PlayerPrefs.GetFloat(_BGM_Exposed_parameter);

        SetFXVol(_playerPrefSFX_Volume);
        SetBGMusicVol(_playerPrefBGMusic_Volume);
    }

    public void SetMasterVolume(Slider volume)
    {
        _MasterMixer.SetFloat("MasterVolume", volume.value);
    }
    /*
    public void SetBGMVolume(Slider volume)
    {
        _MasterMixer.SetFloat("BackgroundMusicVolume", volume.value);
        PlayerPrefs.SetFloat(_BGM_Exposed_parameter, volume.value);
    }

    public void SetSFXVolume(Slider volume)
    {
        _MasterMixer.SetFloat("SFXVolume", volume.value);
        PlayerPrefs.SetFloat(_SFX_Exposed_parameter, volume.value);
    }
    */
    public void SetFXVol(float sfxLevel)
    {
        _MasterMixer.SetFloat(_SFX_Exposed_parameter, sfxLevel);
        PlayerPrefs.SetFloat(_SFX_Exposed_parameter, sfxLevel);
        //AudioManager.Instance._MasterMixer.SetFloat(_SFX_Exposed_parameter, Mathf.Log10(sfxLevel) * 20);
    }

    public void SetBGMusicVol(float BGMusicLevel)
    {
        PlayerPrefs.SetFloat(_BGM_Exposed_parameter, BGMusicLevel);
        AudioManager.Instance._MasterMixer.SetFloat("BackgroundMusicVolume", Mathf.Log10(BGMusicLevel) * 20);
    }
}
/*
[System.Serializable]
public class AudioSetting
{
    public Slider slider;
    public GameObject redX;
    public string exposedParam;

    public void Initialize()
    {
        slider.value = PlayerPrefs.GetFloat(exposedParam);
    }

    public void SetExposedParam(float value) // 1
    {
        redX.SetActive(value <= slider.minValue); // 2
        AudioManager.Instance._MasterMixer.SetFloat(exposedParam, value); // 3
        PlayerPrefs.SetFloat(exposedParam, value); // 4
    }
}
*/