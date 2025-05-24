using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public SaveData saveData;
    private string savePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            savePath = Application.dataPath + "/Saves/save.json";
            DontDestroyOnLoad(gameObject);
            saveData = LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game Saved to " + savePath);
    }

    public SaveData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game Loaded from " + savePath);
            return data;
        }
        else
        {
            Debug.Log("No save file found, creating new save.");
            SaveData newSave = new SaveData();
            newSave.factionRelations = new List<FactionRelationSave>
            {
                new FactionRelationSave(FactionType.Workers, 0),
                new FactionRelationSave(FactionType.Smugglers, 0),
                new FactionRelationSave(FactionType.Security, 0)
            };
            return newSave;
        }
    }
}
