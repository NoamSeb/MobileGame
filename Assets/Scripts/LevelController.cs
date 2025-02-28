using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public List<LevelStructure> Levels;

    [Serializable]
    public struct LevelStructure
    {
        public int idLevel;
        public GameObject level;
    }

    [SerializeField] GameObject TEMPDONTKEEP;

    private void Awake()
    {
        foreach (LevelStructure level in Levels)
        {
            if (level.idLevel == 0) { throw new ArgumentNullException($"{level.level.name} has an ID of 0"); }
            level.level.SetActive(false);
        }

        GridExit.OnLevelEnd += UnloadCurrentLevel;
    }

    public void GetActiveLevel()
    {
        foreach (LevelStructure level in Levels)
        {
            level.level.SetActive(false);
        }

        // vérifie si un niveau a déjà été sauvegardé et l'active
        LevelStructure? activeLevel = Levels.Find(l => l.idLevel == GameManager.CurrentLevelID);

        if (activeLevel.HasValue)
        {
            activeLevel.Value.level.SetActive(true);
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
        TEMPDONTKEEP.SetActive(false);
    }

    public void UnloadCurrentLevel()
    {
        int tempID = GameManager.CurrentLevelID;
        LevelStructure tempLevel = Levels.Find(x => x.idLevel == tempID);
        tempLevel.level.SetActive(false);
        GameManager.CurrentLevelID = 0;
        TEMPDONTKEEP.SetActive(true);
    }
}