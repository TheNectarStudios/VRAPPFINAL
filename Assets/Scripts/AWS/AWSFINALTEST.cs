using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class FetchObjects : MonoBehaviour
{
    public string baseURL = "http://localhost:8080";
    public string organisationName = "ExampleOrganisation";
    public string parentPropertyName = "ExampleParentProperty";
    public string childPropertyName = "ExampleChildProperty";
    public string localPath = @"D:\TP";
    public GameObject importerObject; // Reference to the object responsible for importing
    public GameObject anotherObject; // Reference to another object to be enabled
    public GameObject thirdObject; // Reference to the third object to be enabled

    void Start()
    {
        if (importerObject != null)
        {
            importerObject.SetActive(false); // Ensure the importer object is initially disabled
        }
        else
        {
            Debug.LogWarning("Importer object is not assigned.");
        }

        if (anotherObject != null)
        {
            anotherObject.SetActive(false); // Ensure the other object is initially disabled
        }
        else
        {
            Debug.LogWarning("Another object is not assigned.");
        }

        if (thirdObject != null)
        {
            thirdObject.SetActive(false); // Ensure the third object is initially disabled
        }
        else
        {
            Debug.LogWarning("Third object is not assigned.");
        }

        StartCoroutine(DownloadObjects());
    }

    IEnumerator DownloadObjects()
    {
        string url = baseURL + "/download/fetch-objects";
        
        // Create data object to send as JSON
        FetchObjectsData data = new FetchObjectsData(organisationName, parentPropertyName, childPropertyName, localPath);
        string jsonData = JsonUtility.ToJson(data);

        // Create UnityWebRequest
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request
        request.SendWebRequest();

        // Track the progress of the request
        while (!request.isDone)
        {
            if (request.uploadProgress < 1.0f)
            {
                Debug.Log($"Upload progress: {request.uploadProgress * 100}%");
            }
            else
            {
                if (request.downloadHandler != null && request.downloadHandler.data != null)
                {
                    Debug.Log($"Download progress: {request.downloadProgress * 100}%");
                }
                else
                {
                    Debug.Log("Waiting for response...");
                }
            }
            yield return null;
        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Request successful: " + request.downloadHandler.text);
            
            // Enable the importer object
            if (importerObject != null)
            {
                importerObject.SetActive(true);
                Debug.Log("Importer object enabled.");
            }

            // Enable the other object
            if (anotherObject != null)
            {
                anotherObject.SetActive(true);
                Debug.Log("Another object enabled.");
            }

            // Enable the third object
            if (thirdObject != null)
            {
                thirdObject.SetActive(true);
                Debug.Log("Third object enabled.");
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
        public string localPath;

        public FetchObjectsData(string org, string parentProp, string childProp, string path)
        {
            organisationName = org;
            parentPropertyName = parentProp;
            childPropertyName = childProp;
            localPath = path;
        }
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
