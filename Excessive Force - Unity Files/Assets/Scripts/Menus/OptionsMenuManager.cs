using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour
{
    [Header("Volume Options")]
    public AudioMixer gameMixer;

    public Text masterVolumeText;
    public Slider masterVolumeSlider;

    public Text musicVolumeText;
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;

    public Text sfxVolumeText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /*
    ====================================================================================================
    Sound Options
    ====================================================================================================
    */
    public void SetMasterVolume()
    {
        float newVolume = masterVolumeSlider.value / 10;

        masterVolumeText.text = "Master : " + newVolume.ToString("0.0");
        gameMixer.SetFloat("Master", ConvertToDB(masterVolumeSlider.value));
    }
    public void SetMusicVolume()
    {
        float newVolume = musicVolumeSlider.value / 10;

        musicVolumeText.text = "Master : " + newVolume.ToString("0.0");
        gameMixer.SetFloat("Music", ConvertToDB(musicVolumeSlider.value));
    }
    public void SetSfxVolume()
    {
        float newVolume = sfxVolumeSlider.value / 10;

        sfxVolumeText.text = "Master : " + newVolume.ToString("0.0");
        gameMixer.SetFloat("SFX", ConvertToDB(sfxVolumeSlider.value));
    }


    /*
    ====================================================================================================
    Utility
    ====================================================================================================
    */
    private float ConvertToDB(float sliderValue)
    {
        float db = Mathf.Log(sliderValue);
        return (db * 20);
    }

    private float ConvertToSliderValue(float db)
    {
        float sliderValue = db / 20;
        return Mathf.Exp(sliderValue);
    }
}
