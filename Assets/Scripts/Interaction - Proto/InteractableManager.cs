using Unity.VisualScripting;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    public static InteractableManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
            Instance = this;
        }


    }
}
