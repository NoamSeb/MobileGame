using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Level Manager")] [SerializeField]
    private LevelManager _levelManager;

    [Header("Load screen information")] [SerializeField]
    private GameObject _loadScreen;

    [SerializeField] private TextMeshProUGUI _progressValue;
    
    [Space]
    
    [Foldout("Audio")]
    [SerializeField] private AudioClip _launchSFX;
    [Foldout("Audio")]
    [SerializeField] private AudioClip _menuMusic;
    [Foldout("Audio")]
    [SerializeField] private AudioSource _audioSource;
    
    [Foldout("Events")]
    [SerializeField] UnityEvent OpenSettingsMenu;
    [Foldout("Events")]
    [SerializeField] UnityEvent CloseSettingsMenu;

    [Foldout("Settings")]
    [SerializeField] Slider _volume;
    [Foldout("Settings")]
    [SerializeField] Toggle _isHapticEnable;

    private void Awake()
    {
        _audioSource.PlayOneShot(_menuMusic);
        _audioSource.volume = PlayerPrefs.GetFloat("volume");
    }

    public void Play()
    {
        StartCoroutine(PlayLaunchSFXAndLoadScene());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator PlayLaunchSFXAndLoadScene()
    {
        if (_launchSFX != null)
        {
            _audioSource.PlayOneShot(_launchSFX);
            _levelManager.ChangeLevel("DevArthurLevelSelector");
            _loadScreen.SetActive(true);
            yield return new WaitForSeconds(_launchSFX.length);
            StartCoroutine(LoadNextLevelAsync());
        }
        else
        {
            Debug.LogWarning("Launch SFX is not assigned.");
        }
        SceneManager.LoadScene("DevNoam");
    }

    #region Settings

    public void OpenSettings()
    {
        GetSettingsValue();
        OpenSettingsMenu.Invoke();
    }

    private void GetSettingsValue()
    {
        _volume.value = PlayerPrefs.GetFloat("Volume");
        _isHapticEnable.isOn = PlayerPrefs.GetInt("IsHapticEnabled") == 1 ? true : false;
    }
    public void CloseSettings()
    {
        CloseSettingsMenu.Invoke();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("Volume", _volume.value);
        PlayerPrefs.SetString("IsHapticEnable", _isHapticEnable.isOn ? "true" : "false");
        PlayerPrefs.Save();

        _audioSource.volume = _volume.value;
    }

    #endregion


    private IEnumerator LoadNextLevelAsync()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("GameScene");

        while (!loadOperation.isDone)
        {
            _progressValue.text = (int)loadOperation.progress + "%";
            yield return null;
        }
    }
}