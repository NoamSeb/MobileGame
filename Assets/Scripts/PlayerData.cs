using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public struct DataElement
    {
        public int idLevel;
        public int score;

        public DataElement(int idLevel, int score)
        {
            this.idLevel = idLevel;
            this.score = score;
        }
    }
    
    

    public List<DataElement> data;

    public PlayerData()
    {
        data = new List<DataElement>();
    }
}