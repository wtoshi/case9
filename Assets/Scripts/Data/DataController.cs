using System.IO;
using UnityEngine;

public class DataController : MonoBehaviour
{
    public static DataController Instance { get; private set; }
    public GameData GameData { get; private set; }

    private string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        saveFilePath = Path.Combine(Application.persistentDataPath, "gameData.json");
    }

    public void LoadGameData()
    {
        GameData = SaveSystem.LoadGame();

        if (GameData == null)
        {
            GameData = new GameData();
            SaveGameData(GameData);
        }
    }

    public void SaveGameData(GameData gameData)
    {
        SaveSystem.SaveGame(gameData);
    }

    [ContextMenu("Reset Game Data")]
    public void ResetData()
    {
        Debug.Log("Game data reset to default.");
        GameData = new GameData();
        SaveGameData(GameData);
    }
}
