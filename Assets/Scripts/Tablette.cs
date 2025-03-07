using UnityEngine;

public class Tablette : MonoBehaviour
{
    [SerializeField] PauseMenu PauseMenu;
    Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void TablettePause()
    {
        if (name == "Tablette_InGame")
        {
            float number = Camera.main.orthographicSize / 5f;
            transform.localScale = new(number, number, number);
        }

        PauseMenu.Pause();
        gameObject.SetActive(false);
    }
}
