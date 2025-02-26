using UnityEngine;
using GooglePlayGames;
public class GooglePlayManager : MonoBehaviour
{
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
        PlayGamesPlatform.Activate(); // Activate Google Play Games
        SignIn();
    }

    void SignIn()
    {
        Social.localUser.Authenticate(success =>
        {
            if (success)
                Debug.Log("Signed into Google Play Games");
            else
                Debug.LogError("Failed to sign in");
        });
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
            if (success)
                Debug.Log("Achievement unlocked!");
            else
                Debug.LogError("Failed to unlock achievement");
        });
    }
}
