using UnityEngine;

public class AudioController : MonoBehaviour
{

    [SerializeField] private AudioSource _audioSource;
    private void Reset()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _audioSource.volume = PlayerPrefs.GetFloat("Volume");
    }
}
