using UnityEngine;

public class EnergySlider : MonoBehaviour
{
    private Animator _animator;
    private Level _parentLevel;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _parentLevel = GetComponentInParent<Level>();
        Level.OnLevelLoad += AnimateOnLevelLoad;
    }

    void AnimateOnLevelLoad(Level level)
    {
        if (level != _parentLevel)
        {
            _animator.SetTrigger("StartLoadingAnimation");
        }
    }
}
