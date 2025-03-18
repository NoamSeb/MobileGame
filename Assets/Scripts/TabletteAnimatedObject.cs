using UnityEngine;

public class TabletteAnimatedObject : MonoBehaviour
{
    [SerializeField] PauseMenu PauseMenu;

    public void TablettePause()
    {
        PauseMenu.Pause();
        gameObject.SetActive(false);
    }

    public void TabletteDeactivate()
    {
        gameObject.SetActive(false);
    }
}
