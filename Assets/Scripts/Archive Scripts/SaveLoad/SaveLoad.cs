using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Static class responsible for saving and loading game data.
/// </summary>
public static class SaveLoad
{
    /// <summary>
    /// Event invoked when saving the game.
    /// </summary>
    public static UnityAction OnSaveGame;

    /// <summary>
    /// Event invoked when loading the game.
    /// </summary>
    public static UnityAction<SaveData> OnLoadGame;

    private static string directory = "/SaveData/";
    private static string fileName = "SaveGame.sav";

    /// <summary>
    /// Saves the game data.
    /// </summary>
    /// <param name="data">The data to save.</param>
    /// <returns>True if the save was successful, false otherwise.</returns>
    public static bool Save(SaveData data)
    {
        OnSaveGame?.Invoke();

        string dir = Application.persistentDataPath + directory;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(dir + fileName, json);

        Debug.Log("Saving game");

        return true;
    }

    /// <summary>
    /// Loads the game data.
    /// </summary>
    /// <returns>The loaded game data.</returns>
    public static SaveData Load()
    {
        string fullPath = Application.persistentDataPath + directory + fileName;
        SaveData data = new SaveData();

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            data = JsonUtility.FromJson<SaveData>(json);

            OnLoadGame?.Invoke(data);
        }
        else
        {
            Debug.Log("Save file does not exist!");
        }

        return data;
    }

    /// <summary>
    /// Deletes the saved game data.
    /// </summary>
    public static void DeleteSaveData()
    {
        string fullPath = Application.persistentDataPath + directory + fileName;

        if (File.Exists(fullPath)) File.Delete(fullPath);
    }
}