using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public struct SettingValue {
    public float musicVolume;
    public float narrationVolume;
}

public class SettingManager : MonoBehaviour
{
    public SettingValue currentSetting = new SettingValue();
    public AudioMixer audioMixer;

    public static SettingManager Instance;
    private void Awake() {
        if (Instance == null) {
            Instance = this;    
            DontDestroyOnLoad(gameObject);
            LoadSetting();
        } else {
            Destroy(gameObject);
        }
    }

    public void OnChangeMusicVolume(float val) {
        currentSetting.musicVolume = val;
        audioMixer.SetFloat("musicVolume", LinearToDecibel(val));
    }

    public void OnChangeNarrationVolume(float val) {
        currentSetting.narrationVolume = val;
        audioMixer.SetFloat("voiceVolume", LinearToDecibel(val));
    }

    public void UpdateSettings() {
        SaveSetting();
    }

    private void SaveSetting() {
        PlayerPrefs.SetFloat("musicVolume", currentSetting.musicVolume);
        PlayerPrefs.SetFloat("narrationVolume", currentSetting.narrationVolume);

        PlayerPrefs.Save();
    }

    private void LoadSetting() {
        SettingValue setting = new SettingValue();

        setting.musicVolume = PlayerPrefs.GetFloat("musicVolume", 1);
        setting.narrationVolume = PlayerPrefs.GetFloat("narrationVolume", 1);

        currentSetting = setting;
    }

    private float LinearToDecibel(float linear)
    {
        float dB;

        if (linear != 0)
            dB = 20.0f * Mathf.Log10(linear);
        else
            dB = -144.0f;

        return dB;
    }
}
