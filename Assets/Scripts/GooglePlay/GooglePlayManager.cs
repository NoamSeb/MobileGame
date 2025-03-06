using System;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GooglePlayManager : MonoBehaviour
{
    /*[SerializeField] private GameObject _failToConnectScreen;
    public static GooglePlayManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status) {
        if (status == SignInStatus.Success) {
            
            _failToConnectScreen?.SetActive(false);
            
            PlayGamesPlatform.Instance.LoadAchievements(achievements =>
            {
                if (achievements.Length > 0)
                {
                    Debug.Log($"Loaded {achievements.Length} achievements");
                }
                else
                {
                    Debug.Log("No achievements found");
                }
            });
        } else {
            Debug.LogWarning($"Google Play Games Authentication Failed: {status}");
            
            _failToConnectScreen?.SetActive(true);
        }
    }

    public void ManualConnect()
    {
        PlayGamesPlatform.Instance.ManuallyAuthenticate(success =>
        {

            if (success == SignInStatus.Success)
            {
                _failToConnectScreen?.SetActive(false);
            }
            else
            {
                _failToConnectScreen?.SetActive(true);
            }
        });
        _failToConnectScreen?.SetActive(false);
    }

    /// <summary>
    /// Use the Google Play Games user to claim the achievement thanks to ID
    /// 
    /// Tooltip : Use the GPGSlds at the root project to get the achivementID
    /// </summary>
    /// <param name="achievementID"></param>
    public void UnlockAchievement(string achievementID)
    {
        Social.ReportProgress(achievementID, 100.0f, success =>
        {
            Social.ShowAchievementsUI();
        });
    }*/
}
