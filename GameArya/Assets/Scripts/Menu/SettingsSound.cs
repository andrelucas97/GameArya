using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsSound : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider sliderMusic;
    public Slider sliderSFX;
    void Start()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        sliderMusic.value = musicVolume;
        sliderSFX.value = sfxVolume;

        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);

        sliderMusic.onValueChanged.AddListener(SetMusicVolume);
        sliderSFX.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetSFXVolume(float sfxVolume)
    {
        float volumeInDb = Mathf.Log10(Mathf.Clamp(sfxVolume, 0.0001f, 1f)) * 20;
        audioMixer.SetFloat("SFXVolume", volumeInDb);

        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float musicVolume)
    {
        float volumeInDb = Mathf.Log10(Mathf.Clamp(musicVolume, 0.0001f, 1f)) * 20;
        audioMixer.SetFloat("MusicVolume", volumeInDb);

        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();
    }
}
