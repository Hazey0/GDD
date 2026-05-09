using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    [Header("Options Panel")]
    public GameObject optionsPanel;
    public GameObject mainButtons;

    [Header("Volume")]
    public Slider volumeSlider;
    public TextMeshProUGUI volumeLabel;

    [Header("Sounds")]
    public AudioSource backgroundMusic;
    public AudioSource buttonClickSound;

    private void Start()
    {
        
        optionsPanel.SetActive(false);

       
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;
        UpdateVolumeLabel(savedVolume);

     
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    public void OpenOptions()
    {
        PlayClickSound();
        optionsPanel.SetActive(true);
        mainButtons.SetActive(false);
    }

    public void CloseOptions()
    {
        PlayClickSound();
        optionsPanel.SetActive(false);
        mainButtons.SetActive(true);
    }

    public void PlayClickSound()
    {
        if (buttonClickSound != null)
            buttonClickSound.PlayOneShot(buttonClickSound.clip);
    }

    private void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
        UpdateVolumeLabel(value);
    }

    private void UpdateVolumeLabel(float value)
    {
        int percent = Mathf.RoundToInt(value * 100);
        volumeLabel.text = "Volume  " + percent + "%";
    }
}