using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class CanvasManager : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    [Button("PLAY OUT GLITCH")]
    private void PlayGlitch()
    {
        _animator.SetTrigger("Glitch");
    }

    [Button("PLAY IN GLITCH")]
    private void PlayInGlitch()
    {
        _animator.SetTrigger("InGlitch");
    }
}