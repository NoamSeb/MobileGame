using System;
using System.Collections;
using UnityEngine;

public class WinPanel : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Deactivate());
    }

    public static event Action OnLevelEnd;
    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(1f);
        OnLevelEnd?.Invoke();
        gameObject.SetActive(false);    
    }
}
