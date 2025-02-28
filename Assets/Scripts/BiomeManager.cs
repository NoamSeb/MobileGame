using System;
using System.Collections.Generic;
using System.Linq;
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
    private int _currentBiomeID = 1;
    
    [Header("Glitch Filter")]
    [SerializeField] SwitchScreen _glitchFilter;

    void Start()
    {
        LoadSavedBiome();
    }

    void LoadSavedBiome()
    {
        PlayerData.DataElement LastLevel;
        PlayerData data = SaveSystem.LoadPlayer();
        if (data != null)
        {
            LastLevel = data.data.LastOrDefault();
            if (LastLevel.idLevel != null)
            {
                _currentBiomeID = LastLevel.biome.idBiome;
            }
        }
        else
        {
            _currentBiomeID = 1;
        }
        UpdateBiomeVisibility();
    }

    void UpdateBiomeVisibility()
    {
        Biomes.Find(b => b.idBiome == _currentBiomeID).biome.SetActive(true);
        // for (int i = 0; i < Biomes.Count; i++)
        // {
        //     Biomes[i].biome.SetActive(i == _currentBiomeID); // active uniquement le biome actuel
        // }

        // associe le LevelController du biome actif
        LevelController activeLevelController = Biomes[_currentBiomeID].biome.GetComponent<LevelController>();
        if (activeLevelController != null)
        {
            activeLevelController.GetActiveLevel();
        }

        // sauvegarde du biome actuel
        // PlayerPrefs.SetString("CurrentBiome", Biomes[_currentBiomeIndex].biome.name);
        // PlayerPrefs.Save();
    }

    public void NextBiome()
    {
        if (_currentBiomeID >= 1)
        {
            _currentBiomeID++;
            _glitchFilter._prevScreen = Biomes.Find(b => b.idBiome == _currentBiomeID).biome;
            _glitchFilter._nextScreen = Biomes.Find(b => b.idBiome == _currentBiomeID+1).biome;
            _glitchFilter.OnChangedScreen();
            UpdateBiomeVisibility();
        }
    }

    public void PreviousBiome()
    {
        if (_currentBiomeID > 1)
        {
            _currentBiomeID--;
            _glitchFilter._prevScreen = Biomes.Find(b => b.idBiome == _currentBiomeID).biome;
            _glitchFilter._nextScreen = Biomes.Find(b => b.idBiome == _currentBiomeID-1).biome;
            _glitchFilter.OnChangedScreen();
            UpdateBiomeVisibility();
        }
    }
}