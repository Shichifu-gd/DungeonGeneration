using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using System;

public class SaveSettingsSpawn : MonoBehaviour
{
    public static void SaveLocalizationTheGame(GenerationManagement generationManagement)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream stream = new FileStream(Application.dataPath + "/data/SaveSettings.dt", FileMode.Create);
        DataSettings data = new DataSettings(generationManagement);
        binaryFormatter.Serialize(stream, data);
        stream.Close();
    }

    public static string[] LoadLocalizationTheGame()
    {
        if (File.Exists(Application.dataPath + "/data/SaveSettings.dt"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(Application.dataPath + "/data/SaveSettings.dt", FileMode.Open);
            DataSettings data = binaryFormatter.Deserialize(stream) as DataSettings;
            stream.Close();
            return data.dataSettings;
        }
        else return new string[0];
    }
}

[Serializable]
public class DataSettings
{
    public string[] dataSettings;
    public DataSettings(GenerationManagement generationManagement)
    {
        dataSettings = new string[5];
        dataSettings[0] = generationManagement.RestartTime.ToString();
        dataSettings[1] = generationManagement.MaxRoom.ToString();
        dataSettings[2] = generationManagement.MinRoom.ToString();
        dataSettings[3] = generationManagement.Time.ToString();
        dataSettings[4] = generationManagement.WaitTime.ToString();
    }
}