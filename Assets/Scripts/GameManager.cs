using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private string playerName;
    private const int minNameLength = 1;
    private const string defaultPlayerName = "User";
    public string PlayerName
    {
        get
        {
            return playerName;
        }
        set
        {
            string input;
            Regex r = new Regex(@"\s+");
            input = r.Replace(value, @" ");

            if (input.Length < minNameLength || input == " " || input == "") playerName = defaultPlayerName;
            else playerName = value;
        }

    }
    public Records<string, int> records;
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
        records = new Records<string, int>();
        saveFilePath = Application.persistentDataPath + "/savefile.dat";
        records.records = new Dictionary<string, int>(maxRecords);
        WriteSaveFileIfNotExists(records);
        PlayerName = defaultPlayerName;
        LoadRecords();
    }

    public KeyValuePair<string, int> GetScore()
    {
        return records.records.FirstOrDefault();
    }

    [System.Serializable]
    public class Records<TK, TV>
    {
        public Dictionary<TK, TV> records;
    }

    public void SaveRecord(int points)
    {
        int score = GetScore().Value;

        if (points > 0 && points > score)
        {
            if (records.records.Count() == maxRecords)
            {
                records.records.Remove(records.records.Keys.Last());
            }
            if (records.records.Keys.Contains(PlayerName))
            {
                records.records.Remove(PlayerName);
            }
            records.records.Add(PlayerName, points);
            records.records = SortDescRecords(records.records);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(saveFilePath);
            bf.Serialize(file, records);
            file.Close();
        }
    }

    public void LoadRecords()
    {
        if (File.Exists(saveFilePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(saveFilePath, FileMode.Open);
            records = (Records<string, int>)bf.Deserialize(file);
            file.Close();
        }
    }

    private void WriteSaveFileIfNotExists(Records<string, int> records)
    {
        if (!File.Exists(saveFilePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(saveFilePath);
            bf.Serialize(file, records);
            file.Close();
        }
    }
    
    private Dictionary<string, int> SortDescRecords(Dictionary<string, int> records)
    {
        return (from entry in records orderby entry.Value descending select entry).ToDictionary(t => t.Key, t=> t.Value);
    }

}
