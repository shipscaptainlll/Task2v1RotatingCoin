using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeChanger : MonoBehaviour
{
    [SerializeField] AudioMixerGroup masterMixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    float musicVolume;
    float sfxVolume;

    // Start is called before the first frame update
    void Awake()
    {
        UploadPlayerPrefs();
    }

    public void UpdateMusicVolume(Slider slider)
    {
        musicVolume = slider.value;
        masterMixer.audioMixer.SetFloat("MusicVol", Mathf.Log10(musicVolume) * 20);

        PlayerPrefs.SetFloat("musicVolume", musicVolume);
    }

    public void UpdateSfxVolume(Slider slider)
    {
        sfxVolume = slider.value;
        masterMixer.audioMixer.SetFloat("SfxVol", Mathf.Log10(sfxVolume) * 20);

        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
    }

    public void UploadPlayerPrefs()
    {
        musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        musicSlider.value = musicVolume;

        sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 0.5f);
        sfxSlider.value = sfxVolume;
    }
}
