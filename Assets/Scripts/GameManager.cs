using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string PlayerName;
    public Dictionary<string, int> records;

    private const int maxRecords = 5;
    private string saveFilePath;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        saveFilePath = Application.persistentDataPath + "/savefile.json";
        records = new Dictionary<string, int>(maxRecords);
        WriteSaveFileIfNotExists(records);
        LoadRecords();
    }

    public KeyValuePair<string, int> GetScore()
    {
        return records.FirstOrDefault();
    }

    public void SaveRecord(int points)
    {
        //todo: проверить что рекорд больше нуля +
        //todo: проверить что рекорд больше чем наибольший из списка +
        //todo: если рекорд побит то удалить наименьший рекорд и добавить новый в список +
        //todo: перезаписывать одинаковые имена +
        //todo: отсортировать список рекордов по убыванию +
        Debug.Log(saveFilePath);

        int score = GetScore().Value;

        if (points > 0 && points > score)
        {
            if (records.Count() == maxRecords)
            {
                records.Remove(records.Keys.Last());
            }
            if (records.Keys.Contains(PlayerName))
            {
                records.Remove(PlayerName);
            }
            records.Add(PlayerName, points);
            records = SortDescRecords(records);
            string json = JsonUtility.ToJson(records);
            File.WriteAllText(saveFilePath, json);
            Debug.Log("record saved");
        }
    }

    public void LoadRecords()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            records = SortDescRecords(JsonUtility.FromJson<Dictionary<string, int>>(json));
        }
        
    }

    private void WriteSaveFileIfNotExists(Dictionary<string, int> records)
    {
        if (!File.Exists(saveFilePath))
        {
            string json = JsonUtility.ToJson(records);
            File.WriteAllText(saveFilePath, json);
            Debug.Log("file rewrite");
        }
        
    }
    
    private Dictionary<string, int> SortDescRecords(Dictionary<string, int> records)
    {
        return (from entry in records orderby entry.Value descending select entry).ToDictionary(t => t.Key, t=> t.Value);
    }

}
