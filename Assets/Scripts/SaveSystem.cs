using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(PlayerData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/PlayerInfo.lun";
        FileStream stream = new FileStream(path, FileMode.Create);
        
        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("Player saved");
    }

    public static PlayerData LoadPlayer()
    {
        PlayerData data;
        string path = Application.persistentDataPath + "/PlayerInfo.lun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            
            data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            
        }
        else
        {
            data = new PlayerData();
            SavePlayer(data);
            
        }
        Debug.Log("Player loaded");
        return data;
    }
}
