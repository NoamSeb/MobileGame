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

   /// <summary>
   /// The idea here is to get the CurrentLevelID from the Game Manager
   /// and set this variable with the idLevel to improve the save system
   /// </summary>
    private void GetActiveLevel()
    {
        foreach (LevelStructure level in Levels)
        {
            GameObject levelObject = GameObject.Find($"Level_{level.idLevel}");
        
            if (levelObject != null && levelObject.activeInHierarchy)
            {
                Debug.Log($"Active level found: ID {level.idLevel}");
                GameManager.CurrentLevelID = level.idLevel;
            }
        }
    }
   
   
}
