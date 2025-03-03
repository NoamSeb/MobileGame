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
        
        public int idBiome;

        public DataElement(int idLevel, int score, int idBiome)
        {
            this.idLevel = idLevel;
            this.score = score;
            this.idBiome = idBiome;
        }
    }
    
    

    public List<DataElement> data;

    public PlayerData()
    {
        data = new List<DataElement>();
    }
}