using UnityEngine;
using UnityEngine.UI;

public class BiomeSwitcherButton : MonoBehaviour
{
    Image _image;
    Button _button;
    LevelController _currentLevelController;
    BiomeManager _biomeManager;

    public enum Type
    {
        Previous,
        Next
    }
    public Type _type;

    void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        _biomeManager = FindFirstObjectByType<BiomeManager>();
    }

    public void SetLevelController(LevelController controller)
    {
        _currentLevelController = controller;
    }

    private void Update()
    {
        if (GameManager.CurrentLevelID != 0)
        {
            _image.enabled = false;
            _button.enabled = false;
        }
        else
        {
            _image.enabled = true;
            _button.enabled = true;
        }

        if (_type == Type.Previous)
        {
            if (_biomeManager.CurrentBiomeID == 1)
            {
                _button.interactable = false;
            }
            else
            {
                _button.interactable = true;
            }
        }
        else
        {
            if (_currentLevelController.AreAllLevelsFinishedInThisBiome())
            {
                _button.interactable = true;
            }
            else
            {
                _button.interactable = false;
            }
        }
    }
}
