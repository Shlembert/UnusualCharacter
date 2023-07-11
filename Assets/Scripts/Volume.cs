using FMOD.Studio;
using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    public static Volume instance;

    [SerializeField] private Slider music;
    [SerializeField] private Slider sound;
    [SerializeField] private Slider sence;
    [SerializeField] private GameObject musicAudioSource;
    [SerializeField] private GameObject soundAudioSource;

    private VCA musicVca;
    private VCA soundVca;
    private const string musicVolumeKey = "MusicVolume";
    private const string soundVolumeKey = "SoundVolume";
    private const string senceValueKey = "SenceValue";
    private const float senceMinValue = 1.0f;
    private const float senceMaxValue = 4.0f;

    private void Awake()
    {
        instance = this;
        LoadSettings();
        soundVca = FMODUnity.RuntimeManager.GetVCA("vca:/Sounds");
        musicVca = FMODUnity.RuntimeManager.GetVCA("vca:/Music");
    }

    private void Start()
    {
        music.onValueChanged.AddListener(OnMusicVolumeChanged);
        sound.onValueChanged.AddListener(OnSoundVolumeChanged);
        sence.onValueChanged.AddListener(OnSenceValueChanged);

        sence.minValue = senceMinValue;
        sence.maxValue = senceMaxValue;
    }

    private void OnMusicVolumeChanged(float value)
    {
        musicVca.setVolume(value);
        SaveSettings();
    }

    private void OnSoundVolumeChanged(float value)
    {
        soundVca.setVolume(value);
        SaveSettings();
    }

    private void OnSenceValueChanged(float value)
    {
        sence.value = value;
        SaveSettings();
    }

    private void SaveSettings()
    {
        float musicVolume;
        musicVca.getVolume(out musicVolume);

        float soundVolume;
        soundVca.getVolume(out soundVolume);

        PlayerPrefs.SetFloat(musicVolumeKey, musicVolume);
        PlayerPrefs.SetFloat(soundVolumeKey, soundVolume);
        PlayerPrefs.SetFloat(senceValueKey, sence.value);

        PlayerPrefs.Save();

    }

    public void Send()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        float musicVolume = PlayerPrefs.GetFloat(musicVolumeKey, 0.5f);
        float soundVolume = PlayerPrefs.GetFloat(soundVolumeKey, 0.5f);
        float senceValue = PlayerPrefs.GetFloat(senceValueKey, 2.0f);

        music.value = musicVolume;
        sound.value = soundVolume;
        sence.value = senceValue;

        musicVca.setVolume(musicVolume);
        soundVca.setVolume(soundVolume);
        
        PlayerMovement.instance.sence = senceValue;
    }
}
