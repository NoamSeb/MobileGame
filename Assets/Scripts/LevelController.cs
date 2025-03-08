using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using NaughtyAttributes;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public List<LevelStructure> Levels;

    [SerializeField] private AudioClip _gameButton;
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] public AudioClip _biomeMusic;

    [Serializable]
    public struct LevelStructure
    {
        public int idLevel;
        public GameObject level;
        public int orderAmongLevels;
        public ScreenShapedButton screenButton;
    }

    [SerializeField] GameObject _levelSelector;

    LevelController _controller;

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
        GridExit.OnLevelEnd += Victory;
        Oxygen.OnDeath += Defeat;
        WinPanel.OnLevelEnd += UnloadCurrentLevelAsWin;
        LossPanel.OnLevelEnd += UnloadCurrentLevelAsLoss;
        PauseMenu.OnReturnToMenuInGame += UnloadCurrentLevelAsLoss;
        BiomeManager.OnBiomeChange += SetLevelControllerUsedByMenus;
    }

    public static event Action<ScreenShapedButton> OnFirstLevelLoad;
    private void Start()
    {
        List<LevelStructure> levelsActivatedAtStart = new();
        int lowestLevelOrder = 100;

        foreach (LevelStructure level in Levels)
        {
            level.screenButton.Deactivate();
            if (level.orderAmongLevels < lowestLevelOrder) { lowestLevelOrder = level.orderAmongLevels; }
            if (level.screenButton.IsFinished) 
            { 
                levelsActivatedAtStart.Add(level);
            }
        }

        int highestOrderInFinishedLevels = 0;

        foreach (LevelStructure level in levelsActivatedAtStart)
        {
            OnFirstLevelLoad?.Invoke(level.screenButton);
            if (level.orderAmongLevels > highestOrderInFinishedLevels) 
            { 
                highestOrderInFinishedLevels = level.orderAmongLevels;
            }
        }

        OnFirstLevelLoad?.Invoke(Levels.Find(x => x.orderAmongLevels == lowestLevelOrder).screenButton);

        if (highestOrderInFinishedLevels != 0)
        {
            highestOrderInFinishedLevels++;
            OnFirstLevelLoad?.Invoke(Levels.Find(x => x.orderAmongLevels == highestOrderInFinishedLevels).screenButton);
        }
    }

    void SetLevelControllerUsedByMenus(LevelController controller)
    {
        _controller = controller;
    }

    public bool AreAllLevelsFinishedInThisBiome()
    {
        foreach (LevelStructure level in Levels)
        {
            if (!level.screenButton.IsFinished)
            {
                return false;
            }
        }

        BiomeManager biomeManager = FindObjectOfType<BiomeManager>();
        if (biomeManager.CurrentBiomeID == 5)
        {
            LevelManager lvlChanger = FindObjectOfType<LevelManager>();
            lvlChanger.ChangeLevel("CreditScene");
        }
        return true;
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

    public static event Action<ScreenShapedButton> OnLevelUnload;
    public void UnloadCurrentLevelAsWin()
    {
        if (_controller == this)
        {
            int tempID = GameManager.CurrentLevelID;
            LevelStructure tempLevel = Levels.Find(x => x.idLevel == tempID);

            GameManager.CurrentLevelID = 0;

            LevelStructure tempNextLevel = Levels.Find(x => x.orderAmongLevels == tempLevel.orderAmongLevels + 1);
            if (tempNextLevel.idLevel != 0 && !tempNextLevel.screenButton.IsFinished)
            {
                OnLevelUnload?.Invoke(tempNextLevel.screenButton);
            }

            _levelSelector.SetActive(true);
            tempLevel.level.SetActive(false);
        }
    }

    void UnloadCurrentLevelAsLoss()
    {
        if (_controller == this)
        {
            int tempID = GameManager.CurrentLevelID;
            LevelStructure tempLevel = Levels.Find(x => x.idLevel == tempID);

            GameManager.CurrentLevelID = 0;

            _levelSelector.SetActive(true);
            tempLevel.level.SetActive(false);
        }
    }

    /// <summary>
    /// Load the data and check which level are already there to update button color
    /// of finished levels.
    /// </summary>
    private void ChangeColorOfFinishLevelInData()
    {
        PlayerData loadedData = SaveSystem.LoadPlayer();

        var levelButtons = _levelSelector.GetComponentsInChildren<ScreenShapedButton>();

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (levelButtons[i].TryGetComponent(out Image img))
                img.color = levelButtons[i].UnfinishedColor;
        }

        foreach (PlayerData.DataElement levels in loadedData.data)
        {
            var selectLevelButton = levelButtons.ToList().Find(x => x.name == $"Lvl{levels.idLevel}");

            if (selectLevelButton != null)
            {
                ScreenShapedButton colorManager = selectLevelButton.GetComponent<ScreenShapedButton>();
                Image btnImage = selectLevelButton.GetComponent<Image>();
                if (btnImage != null && colorManager != null)
                {
                    btnImage.color = colorManager.FinishedColor;
                    Debug.Log("Color Changed !", btnImage);
                }
            }
        }
    }

    void Victory()
    {
        GameManager.Instance.VictoryCanvas.SetActive(true);
    }

    void Defeat()
    {
        GameManager.Instance.DefeatCanvas.SetActive(true);
    }
}