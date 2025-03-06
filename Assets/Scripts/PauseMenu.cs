using System;
using System.Collections;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    //private bool IsPaused = false;
    enum Type
    {
        InGame,
        InBiome
    }
    [SerializeField] Type _type;

    [Header("Load screen information")]
    [SerializeField]
    private GameObject _loadScreen;

    [SerializeField] private TextMeshProUGUI _progressValue;

    [Header("Menu elements")]
    [SerializeField] private GameObject _pauseMenuUI;
    [SerializeField] private GameObject _pauseButton;

    [Header("Level Manager")]
    [SerializeField]
    private LevelManager _levelManager;

    [Foldout("Events")]
    [SerializeField] UnityEvent OpenSettingsMenu;
    [Foldout("Events")]
    [SerializeField] UnityEvent CloseSettingsMenu;

    [Foldout("Audio")]
    [SerializeField] private AudioClip _launchSFX;
    [Foldout("Audio")]
    private AudioSource _audioSource;

    [Foldout("Settings")]
    [SerializeField] Slider _volume;
    [Foldout("Settings")]
    [SerializeField] Toggle _isHapticEnable;

    BiomeManager _biomeManager;
    GameObject _currentBiomeEnvironment;

    private void Start()
    {
        _audioSource = Camera.main.GetComponent<AudioSource>();
        _audioSource.volume = PlayerPrefs.GetFloat("volume");
        if (_type == Type.InBiome) { _biomeManager = FindFirstObjectByType<BiomeManager>(); }
    }

    private void Update()
    {
        if (_type == Type.InGame)
        {
            if (GameManager.CurrentLevelID == 0)
            {
                _pauseButton.SetActive(false);
            }
            else
            {
                _pauseButton.SetActive(true);
            }
        }
        else
        {
            if (GameManager.CurrentLevelID != 0)
            {
                _pauseButton.SetActive(false);
            }
            else
            {
                _pauseButton.SetActive(true);
            }

            _currentBiomeEnvironment = 
                _biomeManager.Biomes.Find(x => x.idBiome == _biomeManager._currentBiomeID)
                .biome.transform.Find("Environment").gameObject;

        }
    }

    public void Pause()
    {
        if (_type == Type.InBiome) { _currentBiomeEnvironment.SetActive(false); }
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        //IsPaused = true;
    }

    public void Resume()
    {
        if (_type == Type.InBiome) { _currentBiomeEnvironment.SetActive(true); }
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        //IsPaused = false;
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
        _isHapticEnable.isOn = PlayerPrefs.GetInt("IsHapticEnabled") == 1;
    }
    public void CloseSettings()
    {
        CloseSettingsMenu.Invoke();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("Volume", _volume.value);
        PlayerPrefs.SetInt("IsHapticEnabled", _isHapticEnable.isOn ? 1 : 0);
        PlayerPrefs.Save();

        _audioSource.volume = _volume.value;
    }

    #endregion

    public static event Action OnReturnToMenuInGame;
    public void LoadMenu()
    {
        if (_type == Type.InBiome) { StartCoroutine(PlayLaunchSFXAndLoadMenuScene()); }
        else if (_type == Type.InGame) { OnReturnToMenuInGame?.Invoke(); Resume(); }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator PlayLaunchSFXAndLoadMenuScene()
    {
        if (_launchSFX)
        {
            _audioSource.PlayOneShot(_launchSFX);
            _levelManager.ChangeLevel("MainMenu");
            _loadScreen.SetActive(true);
            yield return new WaitForSecondsRealtime(_launchSFX.length);
            StartCoroutine(LoadNextLevelAsync());
        }
        else
        {
            Debug.LogWarning("Launch SFX is not assigned.");
        }
        SceneManager.LoadSceneAsync("MainMenu");
    }

    private IEnumerator LoadNextLevelAsync()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("MainMenu");

        while (!loadOperation.isDone)
        {
            _progressValue.text = (int)loadOperation.progress + "%";
            yield return null;
        }
    }
}
