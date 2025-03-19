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
        _audioSource.volume = PlayerPrefs.HasKey("Volume")? PlayerPrefs.GetFloat("Volume") : 0.5f;
    }
}
