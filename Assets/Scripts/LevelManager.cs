using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] float transitionTime = 1f;
    Coroutine _transition;

    [Button("Change scene")]
    void ChangeScene()
    {
        ChangeLevel("SampleScene");
    }
    private void ChangeLevel(string sceneName)
    {
        _transition = StartCoroutine(LoadLevel(sceneName));
    }

    IEnumerator LoadLevel(string levelName)
    {
        _animator.SetTrigger("glitch");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelName);
    }
}
