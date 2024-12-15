using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string saveFilePath = Path.Combine(Application.persistentDataPath, "gameData.json");

    public static void SaveGame(GameData gameData)
    {
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game data saved to: " + saveFilePath);
    }

    public static GameData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            GameData gameData = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game data loaded from: " + saveFilePath);
            return gameData;
        }
        else
        {
            Debug.LogWarning("No save file found. Returning default game data.");
            return new GameData();
        }
    }
}
