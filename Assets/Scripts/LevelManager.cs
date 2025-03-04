using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] float transitionTime = 1f;
    Coroutine _transition;
    
    public void ChangeLevel(string sceneName)
    {
        _transition = StartCoroutine(LoadLevel(sceneName));
    }

    IEnumerator LoadLevel(string levelName)
    {
        _animator.SetTrigger("start");
        yield return new WaitForSeconds(transitionTime);
        Debug.Log(levelName);
        SceneManager.LoadSceneAsync(levelName);
    }
}
