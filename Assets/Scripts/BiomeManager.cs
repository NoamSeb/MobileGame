using System;
using System.Collections.Generic;
using UnityEngine;

public class BiomeManager : MonoBehaviour
{
    [Serializable]
    public struct BiomeStructure
    {
        public int idBiome;
        public GameObject biome;
    }

    public List<BiomeStructure> Biomes; 
    private int _currentBiomeIndex = 0; 

    void Start()
    {
        LoadSavedBiome();
    }

    void LoadSavedBiome()
    {
        if (PlayerPrefs.HasKey("CurrentBiome"))
        {
            string savedBiome = PlayerPrefs.GetString("CurrentBiome");
            for (int i = 0; i < Biomes.Count; i++)
            {
                if (Biomes[i].biome.name == savedBiome)
                {
                    _currentBiomeIndex = i;
                    break;
                }
            }
        }
        UpdateBiomeVisibility();
    }

    void UpdateBiomeVisibility()
    {
        for (int i = 0; i < Biomes.Count; i++)
        {
            Biomes[i].biome.SetActive(i == _currentBiomeIndex); // active uniquement le biome actuel
        }

        // associe le LevelController du biome actif
        LevelController activeLevelController = Biomes[_currentBiomeIndex].biome.GetComponent<LevelController>();
        if (activeLevelController != null)
        {
            activeLevelController.GetActiveLevel();
        }

        // sauvegarde du biome actuel
        PlayerPrefs.SetString("CurrentBiome", Biomes[_currentBiomeIndex].biome.name);
        PlayerPrefs.Save();
    }

    public void NextBiome()
    {
        if (_currentBiomeIndex < Biomes.Count - 1)
        {
            _currentBiomeIndex++;
            UpdateBiomeVisibility();
        }
    }

    public void PreviousBiome()
    {
        if (_currentBiomeIndex > 0)
        {
            _currentBiomeIndex--;
            UpdateBiomeVisibility();
        }
    }
}




