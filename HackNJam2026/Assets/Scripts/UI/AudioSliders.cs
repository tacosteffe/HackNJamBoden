using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSliders : MonoBehaviour
{
    public AudioMixer Mixer;
    public string MusicParam;
    public string SfxParam;

    public Slider SliderMusic;
    public Slider SliderSFX;


    private void Awake()
    {
        SliderMusic.onValueChanged.AddListener(SetMusicVolume);
        SliderSFX.onValueChanged.AddListener(SetSFXVolume);

        float volumeInDb = Mathf.Log10(Mathf.Clamp(0.5f, 0.0001f, 1f)) * 20f;
        Mixer.SetFloat(SfxParam, volumeInDb);
        Mixer.SetFloat(SfxParam, volumeInDb);
    }

    public void SetMusicVolume(float sliderValue)
    {
        // Logarithmic conversion because human hearing is non-linear
        float volumeInDb = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
        Mixer.SetFloat(MusicParam, volumeInDb);
        Debug.Log("CALLED MUSIC" + volumeInDb);
    }

    public void SetSFXVolume(float sliderValue)
    {
        // Logarithmic conversion because human hearing is non-linear
        float volumeInDb = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
        Mixer.SetFloat(SfxParam, volumeInDb);
        Debug.Log("CALLED SFX" + volumeInDb);

    }
}
