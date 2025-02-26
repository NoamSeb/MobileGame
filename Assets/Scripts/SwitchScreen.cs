using System.Collections;
using UnityEngine;

public class SwitchScreen : MonoBehaviour
{
    public GameObject _prevScreen;
    public GameObject _nextScreen;
    
    public Material _material;

    public void OnChangedScreen()
    {
        StartCoroutine(Glitch());
    }
    
    IEnumerator Glitch()
    {
        _material.SetFloat("_Shake", 2f);
        _material.SetFloat("_Scale", 300f);
        
        yield return new WaitForSeconds(0.25f);
        
        _prevScreen.SetActive(false);
        _nextScreen.SetActive(true);
        
        yield return new WaitForSeconds(0.25f);

        _material.SetFloat("_Shake", 0f);
        _material.SetFloat("_Scale", 0f);
    }
}
