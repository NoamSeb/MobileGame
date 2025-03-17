using UnityEngine;
using System;

public class LossPanel : MonoBehaviour
{
    public static event Action OnLevelEnd;
    public void EndLevel()
    {
        OnLevelEnd?.Invoke();
        gameObject.SetActive(false);
    }
}
