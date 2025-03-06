using System;
using UnityEngine;

public class GridRepairTool : GridObject
{
    [SerializeField] private AudioClip _repairSound;
    [SerializeField] private AudioSource _audioSource;
        
    public static event Action OnRepairPickup;
    protected override void Effect()
    {
        OnRepairPickup?.Invoke();
        _audioSource.PlayOneShot(_repairSound);
        Destroy(gameObject);
    }
}
