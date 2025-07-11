using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayerData()
    {
        string filePath = Application.persistentDataPath + "/player.data";  // file path to save data to
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(filePath, FileMode.Create);

        PlayerData playerData = new PlayerData();  // creating a new PlayerData object to save
        formatter.Serialize(fileStream, playerData);  // saving the data
        Debug.Log("Save completed!");
        fileStream.Close();  // closing the filestream (IMPORTANT)
    }

    public static PlayerData LoadPlayerData()
    {
        string filePath = Application.persistentDataPath + "/player.data";  // file path to load data from
        if (File.Exists(filePath))  // condition to check if the save file exists
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open);

            PlayerData playerData = formatter.Deserialize(fileStream) as PlayerData;  // loading PlayerData as PlayerData object
            fileStream.Close();  // closing the filestream (IMPORTANT)

            return playerData;
        }
        else  // if save file not found, log a warning message
        {
            Debug.LogWarning("File not found at: " + filePath);
            return null;
        }
    }
}
