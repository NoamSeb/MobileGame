using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class SwitchScreen : MonoBehaviour
{
    public GameObject _prevScreen;
    public GameObject _nextScreen;

    [Header("Audio")] 
    public AudioClip _switchSound;
    public AudioSource _audioSource;
    
    public Material _material;

    public void OnChangedScreen()
    {
        StartCoroutine(Glitch());
    }
    
    IEnumerator Glitch()
    {
        _material.SetFloat("_Shake", 2f);
        _material.SetFloat("_Scale", 300f);
        _audioSource.volume -= 0.2f;
        _audioSource.PlayOneShot(_switchSound);
        _audioSource.volume += 0.2f;
        yield return new WaitForSeconds(0.25f);
        
        _prevScreen.SetActive(false);
        _nextScreen.SetActive(true);
        
        yield return new WaitForSeconds(0.25f);

        _material.SetFloat("_Shake", 0f);
        _material.SetFloat("_Scale", 0f);
    }
}
