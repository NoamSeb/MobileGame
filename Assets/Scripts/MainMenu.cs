using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Audio")] 
    [SerializeField] private AudioClip _launchSFX;
    [SerializeField] private AudioClip _menuMusic;
    [SerializeField] private AudioSource _audioSource;

    [Header("Events")] 
    [SerializeField] UnityEvent OpenSettingsMenu;
    [SerializeField] UnityEvent CloseSettingsMenu;
    
    [Header("Level Manager")]
    [SerializeField] private LevelManager _levelManager;
    
    [Header("Load screen information")]
    [SerializeField] private GameObject _loadScreen;
    [SerializeField] private TextMeshProUGUI _progressValue;

    private void Start()
    {
        _audioSource.PlayOneShot(_menuMusic);
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
            _levelManager.ChangeLevel("GameScene");
            _loadScreen.SetActive(true);
            yield return new WaitForSeconds(_launchSFX.length);
            StartCoroutine(LoadNextLevelAsync());
        }
        else
        {
            Debug.LogWarning("Launch SFX is not assigned.");
        }

        SceneManager.LoadScene("GameScene");
    }

    public void OpenSettings()
    {
        OpenSettingsMenu.Invoke();
    }

    public void CloseSettings()
    {
        CloseSettingsMenu.Invoke();
    }

    private IEnumerator LoadNextLevelAsync()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("GameScene");

        while (!loadOperation.isDone)
        {
            _progressValue.text = loadOperation.progress + "%";
            yield return null;
        }
    }
    
}