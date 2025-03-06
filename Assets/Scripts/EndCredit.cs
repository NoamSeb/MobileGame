using UnityEngine;
using UnityEngine.Events;
public class EndCredit : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;
    
    public void GoBackToMainMenu()
    {
        _levelManager.ChangeLevel("MainMenu");
    }
}
