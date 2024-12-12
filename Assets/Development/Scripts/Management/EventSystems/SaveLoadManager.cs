using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class LevelDataSerializable
{
    public int levelCount;
    public int enemyCount;
    public int hostileCount;
    public int levelTime; 
    public bool isResume;
    public bool isFinish;
}

public class SaveLoadManager : MonoBehaviour
{
    public List<LevelData> levelDataList = new List<LevelData>(); 
    private string saveFilePath;
    
    public static SaveLoadManager Instance { get; private set; }

    public GameStarter gameStarter;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        saveFilePath = Path.Combine(Application.persistentDataPath, "levelData.json");

        LoadGame();
    }

    public void SaveGame()
    {
        List<LevelDataSerializable> serializedData = new List<LevelDataSerializable>();

        foreach (var levelData in levelDataList)
        {
            LevelDataSerializable data = new LevelDataSerializable
            {
                levelCount = levelData.levelCount,
                enemyCount = levelData.enemyCount,
                hostileCount = levelData.hostileCount,
                levelTime = levelData.levelTime,
                isResume = levelData.isResume,
                isFinish = levelData.isFinish
            };
            serializedData.Add(data);
        }

        string json = JsonUtility.ToJson(new SerializableWrapper<LevelDataSerializable>(serializedData), true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game Saved to " + saveFilePath);
    }

    public void LoadGame()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("Save file not found. First Game");
            for (int i = 0; i < levelDataList.Count; i++)
            {
                levelDataList[i].levelCount = i + 1; 
                levelDataList[i].enemyCount = i + 1; 
                levelDataList[i].levelTime = 150 - i * 2 ;
                levelDataList[i].isResume = false ; 
                levelDataList[i].isFinish = false ; 
            }
            gameStarter.InitGame(true);
            return;
        }

        string json = File.ReadAllText(saveFilePath);
        var serializedData = JsonUtility.FromJson<SerializableWrapper<LevelDataSerializable>>(json);

        for (int i = 0; i < levelDataList.Count; i++)
        {
            if (i < serializedData.items.Count)
            {
                var data = serializedData.items[i];
                levelDataList[i].levelCount = data.levelCount;
                levelDataList[i].enemyCount = data.enemyCount;
                levelDataList[i].hostileCount = data.hostileCount;
                levelDataList[i].levelTime = data.levelTime;
                levelDataList[i].isResume = data.isResume;
                levelDataList[i].isFinish = data.isFinish;
            }
        }
        gameStarter.InitGame(false);

        Debug.Log("Game Loaded from " + saveFilePath);
    }
}

[System.Serializable]
public class SerializableWrapper<T>
{
    public List<T> items;

    public SerializableWrapper(List<T> items)
    {
        this.items = items;
    }
}
