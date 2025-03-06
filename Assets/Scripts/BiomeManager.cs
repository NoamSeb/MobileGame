using System;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Feedbacks;
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
    internal int _currentBiomeID = 1;
    
    [Header("Glitch Filter")]
    [SerializeField] SwitchScreen _glitchFilter;
    [SerializeField] private AudioSource _audioSource;

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
            if (LastLevel.idLevel != 0)
            {
                print("GET BIOME LAST LEVEL");
                _currentBiomeID = LastLevel.idBiome;
            }
        }
        else
        {
            _currentBiomeID = 1;
        }
        
        UpdateBiomeVisibility();
    }

    public static event Action<LevelController> OnBiomeChange;
    void UpdateBiomeVisibility()
    {
        BiomeStructure currentBiome = Biomes.Find(b => b.idBiome == _currentBiomeID);

        print( "_currentBiomeID =" + _currentBiomeID);
        if (currentBiome.biome != null)
        {
            currentBiome.biome.SetActive(true);
            _audioSource.clip = null;
            _audioSource.clip = currentBiome.biome.GetComponentInChildren<LevelController>()._biomeMusic;
        }
        else
        {
            Debug.LogError($"Aucun biome trouvé avec l'ID {_currentBiomeID} ou le champ 'biome' est null !");
        }

        // associe le LevelController du biome actif
        Debug.Log(_currentBiomeID);
        LevelController activeLevelController = Biomes.Find(x => x.idBiome == _currentBiomeID).biome.GetComponentInChildren<LevelController>();
        if (activeLevelController != null)
        {
            //activeLevelController.GetActiveLevel();
            OnBiomeChange?.Invoke(activeLevelController);
        }
        
    }

    public void NextBiome()
    {
        if (1 <= _currentBiomeID  && _currentBiomeID < 5)
        {
            int nextBiomeID = _currentBiomeID + 1;
            _glitchFilter._prevScreen = Biomes.Find(b => b.idBiome == _currentBiomeID).biome;
            _glitchFilter._nextScreen = Biomes.Find(b => b.idBiome == nextBiomeID).biome;
            _glitchFilter.OnChangedScreen();
            
            _currentBiomeID++;
            UpdateBiomeVisibility();
        }
    }

    public void PreviousBiome()
    {
        if (_currentBiomeID > 1)
        {
            int previousBiomeID = _currentBiomeID - 1;
            _glitchFilter._prevScreen = Biomes.Find(b => b.idBiome == _currentBiomeID).biome;
            _glitchFilter._nextScreen = Biomes.Find(b => b.idBiome == previousBiomeID).biome;
            _glitchFilter.OnChangedScreen();
            _currentBiomeID--;
            UpdateBiomeVisibility();
        }
    }
}