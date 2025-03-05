using MoreMountains.Feedbacks;
using System;
using System.Collections;
using UnityEngine;

public class EnergySliderFeedbacks : MonoBehaviour
{
    [SerializeField] private MMF_Player _loadingFeedbacks;

    private void Awake()
    {
        _loadingFeedbacks.Initialization();
    }

    private void Start()
    {
        PlayFeedbacks(_loadingFeedbacks);
    }

    void PlayFeedbacks(MMF_Player player)
    {
        player.PlayFeedbacks();
        StartCoroutine(WaitForFeedbacksEnd(player));
    }

    IEnumerator WaitForFeedbacksEnd(MMF_Player player)
    {
        yield return new WaitForSeconds(player.TotalDuration);
        SendFeedbacksFinished();
    }

    public static event Action OnSliderFeedbackFinished;
    void SendFeedbacksFinished()
    {
        OnSliderFeedbackFinished?.Invoke();
    }
}
