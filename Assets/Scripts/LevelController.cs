using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public List<LevelStructure> Levels;

    [SerializeField] private AudioClip _gameButton;
    [SerializeField] private AudioSource _audioSource;
    
    [Serializable]
    public struct LevelStructure
    {
        public int idLevel;
        public GameObject level;
    }

    [SerializeField] GameObject _levelSelector;

    private void Awake()
    {
        foreach (LevelStructure level in Levels)
        {
            if (level.idLevel == 0)
            {
                throw new ArgumentNullException($"{level.level.name} has an ID of 0");
            }

            level.level.SetActive(false);
        }

        ChangeColorOfFinishLevelInData();
        GridExit.OnLevelEnd += UnloadCurrentLevel;
    }

    public void GetActiveLevel()
    {
        foreach (LevelStructure level in Levels)
        {
            level.level.SetActive(false);
        }

        // v�rifie si un niveau a d�j� �t� sauvegard� et l'active
        LevelStructure? activeLevel = Levels.Find(l => l.idLevel == GameManager.CurrentLevelID);

        if (activeLevel.HasValue)
        {
            activeLevel.Value.level.SetActive(true);
            _audioSource.PlayOneShot(_gameButton);
            Debug.Log($"Niveau actif : {activeLevel.Value.idLevel}");
        }
        else if (Levels.Count > 0)
        {
            Levels[0].level.SetActive(true);
            GameManager.CurrentLevelID = Levels[0].idLevel;
        }
    }

    public void LoadLevel(int levelID)
    {
        foreach (LevelStructure level in Levels)
        {
            level.level.SetActive(level.idLevel == levelID);
        }

        GameManager.CurrentLevelID = levelID;
        _levelSelector.SetActive(false);
    }

    public void UnloadCurrentLevel()
    {
        int tempID = GameManager.CurrentLevelID;
        LevelStructure tempLevel = Levels.Find(x => x.idLevel == tempID);
        tempLevel.level.SetActive(false);
        GameManager.CurrentLevelID = 0;
        _levelSelector.SetActive(true);
    }

    /// <summary>
    /// Load the data and check which level are already there to update button color
    /// of finished levels.
    /// </summary>
    private void ChangeColorOfFinishLevelInData()
    {
        PlayerData loadedData = SaveSystem.LoadPlayer();

        var levelButtons = _levelSelector.GetComponentsInChildren<ButtonColorManager>();

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if(levelButtons[i].TryGetComponent(out Image img))
                img.color = levelButtons[i]._unfinishedColor;
        }
        
        foreach (PlayerData.DataElement levels in loadedData.data)
        {
            var selectLevelButton = levelButtons.ToList().Find(x => x.name == $"Lvl{levels.idLevel}");

            if (selectLevelButton != null)
            {
                ButtonColorManager colorManager = selectLevelButton.GetComponent<ButtonColorManager>();
                Image btnImage = selectLevelButton.GetComponent<Image>();
                if (btnImage != null && colorManager != null)
                {
                    btnImage.color = colorManager._finishedColor;
                    Debug.Log("Color Changed !", btnImage);
                }
            }
        }
    }
}