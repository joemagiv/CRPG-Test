using UnityEngine;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Database system for object data that can load from CSV or use default strings
/// </summary>
public class ObjectDatabase : MonoBehaviour
{
    public static ObjectDatabase Instance { get; private set; }
    
    [System.Serializable]
    public class ObjectData
    {
        public string id;
        public string name;
        public string description;
        // Add more fields as needed
    }
    
    public List<ObjectData> objectDatabase = new List<ObjectData>();
    public string csvFilePath = "object_data"; // Will look for Resources/object_data.csv
    public bool useCSV = true; // Set to false to use only the default database
    
    void Awake()
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
        
        LoadDatabase();
    }
    
    /// <summary>
    /// Load the database from CSV or use default data
    /// </summary>
    public void LoadDatabase()
    {
        if (useCSV)
        {
            LoadFromCSV();
        }
        
        // If no data loaded or CSV failed, add some default entries
        if (objectDatabase.Count == 0)
        {
            AddDefaultData();
        }
    }
    
    /// <summary>
    /// Load object data from CSV file
    /// </summary>
    private void LoadFromCSV()
    {
        TextAsset csvFile = Resources.Load<TextAsset>(csvFilePath);
        
        if (csvFile != null)
        {
            string[] lines = csvFile.text.Split('\n');
            
            // Skip header row if it exists
            int startIndex = 0;
            if (lines.Length > 0 && lines[0].Contains("id"))
            {
                startIndex = 1;
            }
            
            for (int i = startIndex; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;
                    
                string[] values = lines[i].Split(',');
                
                if (values.Length >= 3)
                {
                    ObjectData data = new ObjectData
                    {
                        id = values[0].Trim(),
                        name = values[1].Trim(),
                        description = values[2].Trim()
                    };
                    
                    objectDatabase.Add(data);
                }
            }
            
            Debug.Log("Loaded " + objectDatabase.Count + " object entries from CSV");
        }
        else
        {
            Debug.LogWarning("CSV file not found: " + csvFilePath + ". Using default data.");
        }
    }
    
    /// <summary>
    /// Add default data entries
    /// </summary>
    private void AddDefaultData()
    {
        objectDatabase.Add(new ObjectData { id = "default1", name = "Mystery Object", description = "An object of unknown origin and purpose." });
        objectDatabase.Add(new ObjectData { id = "default2", name = "Ancient Artifact", description = "This artifact appears to be very old and valuable." });
        objectDatabase.Add(new ObjectData { id = "default3", name = "Strange Device", description = "A device with unfamiliar controls and mysterious functions." });
        
        Debug.Log("Added default object data entries");
    }
    
    /// <summary>
    /// Get object data by ID
    /// </summary>
    public ObjectData GetObjectDataById(string id)
    {
        if (string.IsNullOrEmpty(id))
            return null;
            
        foreach (ObjectData data in objectDatabase)
        {
            if (data.id == id)
            {
                return data;
            }
        }
        
        return null;
    }
    
    /// <summary>
    /// Get object data by name
    /// </summary>
    public ObjectData GetObjectDataByName(string name)
    {
        foreach (ObjectData data in objectDatabase)
        {
            if (data.name == name)
            {
                return data;
            }
        }
        
        return null;
    }
    
    /// <summary>
    /// Add or update object data
    /// </summary>
    public void AddOrUpdateObjectData(ObjectData data)
    {
        // Check if data with this ID already exists
        ObjectData existingData = GetObjectDataById(data.id);
        
        if (existingData != null)
        {
            // Update existing data
            existingData.name = data.name;
            existingData.description = data.description;
        }
        else
        {
            // Add new data
            objectDatabase.Add(data);
        }
    }
}