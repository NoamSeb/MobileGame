using MoreMountains.Feedbacks;
using UnityEngine;

public class EnergySliderFeedbacks : MonoBehaviour
{
    [SerializeField] private MMF_Player _loadingFeedbacks;

    private void Start()
    {
        PlayFeedbacks(_loadingFeedbacks);
    }

    void PlayFeedbacks(MMF_Player player)
    {
        player.PlayFeedbacks();
    }
}
