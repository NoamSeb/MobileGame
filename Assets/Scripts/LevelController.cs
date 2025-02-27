using UnityEngine;
using System;
using System.Collections.Generic;

public class LevelController : MonoBehaviour
{
    public List<LevelStructure> Levels;

    [Serializable]
    public struct LevelStructure
    {
        public int idLevel;
        public GameObject level;
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
    }
}

