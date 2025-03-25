using System;
using UnityEngine;
using UnityEngine.Events;
public class EndCredit : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] Animator _animator;

    private void Start()
    {
        Time.timeScale = 1f;
        _animator.SetTrigger("StartAnim");
    }

    public void GoBackToMainMenu()
    {
        _levelManager.ChangeLevel("MainMenu");
        GooglePlayManager.UnlockAchievement((GPGSlds.achievement_spaceships_commander));
    }
}
