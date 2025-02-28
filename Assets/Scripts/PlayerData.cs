using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    [Serializable]
    public struct DataElement
    {
        public int idLevel;
        public int score;
        public BiomeManager.BiomeStructure biome;

        public DataElement(int idLevel, int score, BiomeManager.BiomeStructure biome)
        {
            this.idLevel = idLevel;
            this.score = score;
            this.biome = biome;
        }
    }
    
    

    public List<DataElement> data;

    public PlayerData()
    {
        data = new List<DataElement>();
    }
}