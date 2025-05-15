using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [Header("��������")]
    public Slider musicSlider;
    public Slider soundSlider;

    [Header("��������������")]
    public AudioSource musicSource;
    public AudioSource soundSource;

    void Start()
    {
        musicSlider.value = musicSource.volume;
        soundSlider.value = soundSource.volume;

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        soundSlider.onValueChanged.AddListener(SetSoundVolume);
    }

    public void SetMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    public void SetSoundVolume(float value)
    {
        soundSource.volume = value;
    }
}