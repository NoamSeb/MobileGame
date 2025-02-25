using System.Collections.Generic;
using System;
using UnityEngine;

public class BiomeManager : MonoBehaviour
{
    [SerializeField]
    private List<BiomeStructure> Levels = new List<BiomeStructure>();

    [Serializable]
    public struct BiomeStructure
    {
        public int idBiome; 
        public GameObject biome; 
        public List<GameObject> levelPrefab;
    }

    private int currentBiomeIndex = 0; 
    private GameObject activeLevelPrefab = null; 

    void Start()
    {
        currentBiomeIndex = PlayerPrefs.GetInt("CurrentBiomeIndex", 0);
        ActivateBiome(currentBiomeIndex);
    }

    private void ActivateBiome(int biomeIndex)
    {
        for (int i = 0; i < Levels.Count; i++)
        {
            if (Levels[i].biome != null)
            {
                Levels[i].biome.SetActive(i == biomeIndex);
            }
        }

        PlayerPrefs.SetInt("CurrentBiomeIndex", biomeIndex);
        PlayerPrefs.Save();
    }

    public void OpenLevel(int levelIndex)
    {
        if (activeLevelPrefab != null) return; // empêche d'ouvrir plusieurs niveaux en même temps

        if (levelIndex >= 0 && levelIndex < Levels[currentBiomeIndex].levelPrefab.Count)
        {
            activeLevelPrefab = Levels[currentBiomeIndex].levelPrefab[levelIndex];
            activeLevelPrefab.SetActive(true);
        }
    }

    // ferme le niveau et revient au biome
    public void CloseLevel()
    {
        if (activeLevelPrefab != null)
        {
            activeLevelPrefab.SetActive(false);
            activeLevelPrefab = null;
        }
    }

    // passe au biome suivant (si possible)
    public void NextBiome()
    {
        if (currentBiomeIndex < Levels.Count - 1)
        {
            currentBiomeIndex++;
            ActivateBiome(currentBiomeIndex);
        }
    }

    // revient au biome précédent (si possible)
    public void PreviousBiome()
    {
        if (currentBiomeIndex > 0)
        {
            currentBiomeIndex--;
            ActivateBiome(currentBiomeIndex);
        }
    }
}

