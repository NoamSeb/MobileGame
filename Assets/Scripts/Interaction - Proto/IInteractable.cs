using System;
using UnityEngine;

public class IInteractable : MonoBehaviour 
{
    public static event Action<IInteractable> OnInteraction;

    protected virtual void Interaction()
    {
        OnInteraction?.Invoke(this);
    }
}
