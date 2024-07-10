using UnityEngine;
using System.IO;
using Dummiesman; // Include Dummiesman namespace

public class HeavyObjImporter : MonoBehaviour
{
    public string objDirectoryPath; // Path to the directory containing OBJ files
    public string jsonDirectoryPath; // Path to the directory containing JSON files

    private GameObject objModel;

    void Start()
    {
        // Example paths (adjust as needed or provide dynamically)
        objDirectoryPath = @"D:\TP\models\nepal_jodhpuriVilla\model";
        jsonDirectoryPath = @"D:\TP\models\nepal_jodhpuriVilla";

        // Call method to find and load OBJ and JSON files
        LoadAndMoveOBJ(objDirectoryPath, jsonDirectoryPath);
    }

    void LoadAndMoveOBJ(string objPath, string jsonPath)
    {
        if (string.IsNullOrEmpty(objPath) || string.IsNullOrEmpty(jsonPath))
        {
            Debug.LogError("Invalid directory path.");
            return;
        }

        // Check if the OBJ directory exists
        if (!Directory.Exists(objPath))
        {
            Debug.LogError($"OBJ directory not found: {objPath}");
            return;
        }

        // Check if the JSON directory exists
        if (!Directory.Exists(jsonPath))
        {
            Debug.LogError($"JSON directory not found: {jsonPath}");
            return;
        }

        // Search for .obj files in the OBJ directory
        string[] objFiles = Directory.GetFiles(objPath, "*.obj");

        if (objFiles.Length == 0)
        {
            Debug.LogError($"No OBJ files found in directory: {objPath}");
            return;
        }

        // Search for .json files in the JSON directory
        string[] jsonFiles = Directory.GetFiles(jsonPath, "*.json");

        if (jsonFiles.Length == 0)
        {
            Debug.LogError($"No JSON files found in directory: {jsonPath}");
            return;
        }

        // Load the first found OBJ file
        string objFilePath = objFiles[0];
        Debug.Log("Loading OBJ file: " + objFilePath);

        // Load the first found JSON file
        string jsonFilePath = jsonFiles[0];
        Debug.Log("Loading JSON file: " + jsonFilePath);

        // Load OBJ model
        objModel = new OBJLoader().Load(objFilePath);
        if (objModel != null)
        {
            // Load coordinates from JSON file
            Vector3 coordinates = LoadCoordinatesFromJSON(jsonFilePath);

            // Move the loaded OBJ model to the coordinates
            objModel.transform.position = coordinates;
            objModel.name = "ImportedOBJ"; // Optionally rename the instantiated object
            Debug.Log("OBJ model loaded and moved to coordinates: " + coordinates);
        }
        else
        {
            Debug.LogError("Failed to load OBJ model from path: " + objFilePath);
        }
    }

    Vector3 LoadCoordinatesFromJSON(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning("JSON file not found. Using default position (0,0,0).");
            return Vector3.zero; // Default position if JSON is not found
        }

        try
        {
            string jsonContent = File.ReadAllText(path);
            RootObject rootData = JsonUtility.FromJson<RootObject>(jsonContent);
            Vector3 dollHousePosition = new Vector3(
                rootData.hotspots.dollHouse.position.x,
                rootData.hotspots.dollHouse.position.y,
                rootData.hotspots.dollHouse.position.z
            );
            return dollHousePosition;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error reading or parsing JSON file: " + ex.Message);
            return Vector3.zero; // Default position in case of error
        }
    }

    [System.Serializable]
    public class Position
    {
        public float x;
        public float y;
        public float z;
    }

    [System.Serializable]
    public class Rotation
    {
        public float x;
        public float y;
        public float z;
        public float w;
    }

    [System.Serializable]
    public class Hotspot
    {
        public Position position;
        public Rotation rotation;
        public int connectedPanoramaIndex;
    }

    [System.Serializable]
    public class DollHouse
    {
        public Position position;
        public Rotation rotation;
    }

    [System.Serializable]
    public class Hotspots
    {
        public Hotspot[] hotspots;
        public DollHouse dollHouse;
    }

    [System.Serializable]
    public class RootObject
    {
        public Hotspots hotspots;
    }
}
