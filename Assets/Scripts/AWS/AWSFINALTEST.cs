using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class FetchObjects : MonoBehaviour
{
    public string baseURL = "https://theserver-tp6r.onrender.com";
    public string organisationName = "ExampleOrganisation";
    public string parentPropertyName = "ExampleParentProperty";
    public string childPropertyName = "ExampleChildProperty";
    public GameObject importerObject;
    public GameObject anotherObject;
    public GameObject thirdObject;

    void Start()
    {
        if (importerObject != null) importerObject.SetActive(false);
        if (anotherObject != null) anotherObject.SetActive(false);
        if (thirdObject != null) thirdObject.SetActive(false);

        StartCoroutine(DownloadObjects());
    }

    IEnumerator DownloadObjects()
    {
        string url = baseURL + "/download/fetch-objects";

        FetchObjectsData data = new FetchObjectsData(organisationName, parentPropertyName, childPropertyName);
        string jsonData = JsonUtility.ToJson(data);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Request successful: " + request.downloadHandler.text);
            FilesResponse response = JsonUtility.FromJson<FilesResponse>(request.downloadHandler.text);

            if (response.files == null || response.files.Count == 0)
            {
                Debug.LogWarning("No files found in the response.");
            }
            else
            {
                foreach (var file in response.files)
                {
                    if (string.IsNullOrEmpty(file.error))
                    {
                        byte[] fileData = System.Convert.FromBase64String(file.data);
                        string filePath = Path.Combine(Application.persistentDataPath, file.key);

                        try
                        {
                            // Ensure the directory exists
                            string directoryPath = Path.GetDirectoryName(filePath);
                            if (!Directory.Exists(directoryPath))
                            {
                                Directory.CreateDirectory(directoryPath);
                            }

                            // Save the file to the local path
                            File.WriteAllBytes(filePath, fileData);
                            Debug.Log("Saved file to: " + filePath);
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            Debug.LogError("UnauthorizedAccessException: Access to the path is denied. " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError("Exception: " + ex.Message);
                        }
                    }
                    else
                    {
                        Debug.LogError("Error downloading file: " + file.error);
                    }
                }

                if (importerObject != null) importerObject.SetActive(true);
                if (anotherObject != null) anotherObject.SetActive(true);
                if (thirdObject != null) thirdObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("Request failed: " + request.error);
        }
    }

    [System.Serializable]
    public class FetchObjectsData
    {
        public string organisationName;
        public string parentPropertyName;
        public string childPropertyName;

        public FetchObjectsData(string org, string parentProp, string childProp)
        {
            organisationName = org;
            parentPropertyName = parentProp;
            childPropertyName = childProp;
        }
    }

    [System.Serializable]
    public class FileData
    {
        public string key;
        public string data;
        public string error;
    }

    [System.Serializable]
    public class FilesResponse
    {
        public string message;
        public List<FileData> files;
    }
}





// using UnityEngine;
// using UnityEngine.Networking;
// using System.Collections;
// using System.IO;

// public class FetchObjects : MonoBehaviour
// {
//     public string baseURL = "http://localhost:8081";
//     public string organisationName = "ExampleOrganisation";
//     public string parentPropertyName = "ExampleParentProperty";
//     public string childPropertyName = "ExampleChildProperty";
//     public string localPath = @"D:\TP";
//     public GameObject importerObject; // Reference to the object responsible for importing
//     public GameObject anotherObject; // Reference to another object to be enabled
//     public GameObject thirdObject; // Reference to the third object to be enabled

//     void Start()
//     {
//         if (importerObject != null)
//         {
//             importerObject.SetActive(false); // Ensure the importer object is initially disabled
//         }
//         else
//         {
//             Debug.LogWarning("Importer object is not assigned.");
//         }

//         if (anotherObject != null)
//         {
//             anotherObject.SetActive(false); // Ensure the other object is initially disabled
//         }
//         else
//         {
//             Debug.LogWarning("Another object is not assigned.");
//         }

//         if (thirdObject != null)
//         {
//             thirdObject.SetActive(false); // Ensure the third object is initially disabled
//         }
//         else
//         {
//             Debug.LogWarning("Third object is not assigned.");
//         }

//         StartCoroutine(CheckServerConnectivity());
//     }

//     IEnumerator CheckServerConnectivity()
//     {
//         UnityWebRequest request = UnityWebRequest.Get(baseURL);
//         yield return request.SendWebRequest();

//         if (request.result == UnityWebRequest.Result.Success)
//         {
//             Debug.Log("Server reachable: " + request.downloadHandler.text);
//             StartCoroutine(DownloadObjects()); // Proceed with the original request
//         }
//         else
//         {
//             Debug.LogError("Failed to reach server: " + request.error);
//         }
//     }

//     IEnumerator DownloadObjects()
//     {
//         string url = baseURL + "/download/fetch-objects";
        
//         // Create data object to send as JSON
//         FetchObjectsData data = new FetchObjectsData(organisationName, parentPropertyName, childPropertyName, localPath);
//         string jsonData = JsonUtility.ToJson(data);

//         // Create UnityWebRequest
//         UnityWebRequest request = new UnityWebRequest(url, "POST");
//         byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
//         request.uploadHandler = new UploadHandlerRaw(bodyRaw);
//         request.downloadHandler = new DownloadHandlerBuffer();
//         request.SetRequestHeader("Content-Type", "application/json");

//         // Log the URL
//         Debug.Log("Generated URL: " + url);

//         // Send the request
//         yield return request.SendWebRequest();

//         // Handle the response
//         if (request.result == UnityWebRequest.Result.Success)
//         {
//             Debug.Log("Request successful: " + request.downloadHandler.text);

//             // Save the response data to local storage
//             string filePath = Path.Combine(localPath, "responseData.json");
//             File.WriteAllText(filePath, request.downloadHandler.text);
//             Debug.Log("Data saved to: " + filePath);

//             // Enable the importer object
//             if (importerObject != null)
//             {
//                 importerObject.SetActive(true);
//                 Debug.Log("Importer object enabled.");
//             }

//             // Enable the other object
//             if (anotherObject != null)
//             {
//                 anotherObject.SetActive(true);
//                 Debug.Log("Another object enabled.");
//             }

//             // Enable the third object
//             if (thirdObject != null)
//             {
//                 thirdObject.SetActive(true);
//                 Debug.Log("Third object enabled.");
//             }
//         }
//         else
//         {
//             Debug.LogError($"Request failed: {request.error}\nResponse Code: {request.responseCode}\nResponse: {request.downloadHandler.text}");
//         }
//     }

//     [System.Serializable]
//     public class FetchObjectsData
//     {
//         public string organisationName;
//         public string parentPropertyName;
//         public string childPropertyName;
//         public string localPath;

//         public FetchObjectsData(string org, string parentProp, string childProp, string path)
//         {
//             organisationName = org;
//             parentPropertyName = parentProp;
//             childPropertyName = childProp;
//             localPath = path;
//         }
//     }
// }
