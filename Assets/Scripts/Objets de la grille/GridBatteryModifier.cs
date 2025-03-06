using System;
using UnityEngine;
using NaughtyAttributes;

public class GridBatteryModifier : GridObject
{
    [SerializeField] private int _batteryModifyAmount;
    [SerializeField] private AudioClip _batteryModifySound;
    [SerializeField] private AudioSource _audioSource;

    public static event Action<int> OnBatteryModification; 

    protected override void Effect()
    {
        OnBatteryModification?.Invoke(_batteryModifyAmount);
        _audioSource.PlayOneShot(_batteryModifySound);
        Destroy(gameObject);
    }
}
