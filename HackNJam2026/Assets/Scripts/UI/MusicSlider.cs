using UnityEngine;
using UnityEngine.Audio;

public class MusicSlider : MonoBehaviour
{
    public AudioMixer Mixer;
    public string MusicParam;
    public string SfxParam;

    private float DefaultVolMusic = 0.5f;
    private float DefaultVolSFX = 0.5f;


    public void SetMusicVolume(float sliderValue)
    {
        // Logarithmic conversion because human hearing is non-linear
        float volumeInDb = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;

        Mixer.SetFloat(exposedParamName, volumeInDb);

        // Log the volume handling to the Console for debugging
        Debug.Log($"Slider value: {sliderValue:F2} | Converted Volume (dB): {volumeInDb:F2}");

        // Save preference for next sessions
        PlayerPrefs.SetFloat(exposedParamName, sliderValue);
        PlayerPrefs.Save();
    }
}
