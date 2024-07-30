//using UnityEngine;
//using UnityEngine.Networking;
//using System.Collections;

//public class FetchObjects : MonoBehaviour
//{
//    public string baseURL = "http://localhost:3000";
//    public string username = "tempUser";
//    public string propertyName = "tempProperty";
//    public string localPath = @"D:\TP";
//    public GameObject importerObject; // Reference to the object responsible for importing
//    public GameObject anotherObject; // Reference to another object to be enabled
//    public GameObject thirdObject; // Reference to the third object to be enabled

//    void Start()
//    {
//        if (importerObject != null)
//        {
//            importerObject.SetActive(false); // Ensure the importer object is initially disabled
//        }
//        else
//        {
//            Debug.LogWarning("Importer object is not assigned.");
//        }

//        if (anotherObject != null)
//        {
//            anotherObject.SetActive(false); // Ensure the other object is initially disabled
//        }
//        else
//        {
//            Debug.LogWarning("Another object is not assigned.");
//        }

//        if (thirdObject != null)
//        {
//            thirdObject.SetActive(false); // Ensure the third object is initially disabled
//        }
//        else
//        {
//            Debug.LogWarning("Third object is not assigned.");
//        }

//        StartCoroutine(DownloadObjects());
//    }

//    IEnumerator DownloadObjects()
//    {
//        string url = baseURL + "/download/fetch-objects";

//        // Create data object to send as JSON
//        FetchObjectsData data = new FetchObjectsData(username, propertyName, localPath);
//        string jsonData = JsonUtility.ToJson(data);

//        // Create UnityWebRequest
//        UnityWebRequest request = new UnityWebRequest(url, "POST");
//        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
//        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
//        request.downloadHandler = new DownloadHandlerBuffer();
//        request.SetRequestHeader("Content-Type", "application/json");

//        // Send the request
//        request.SendWebRequest();

//        // Track the progress of the request
//        while (!request.isDone)
//        {
//            if (request.uploadProgress < 1.0f)
//            {
//                Debug.Log($"Upload progress: {request.uploadProgress * 100}%");
//            }
//            else
//            {
//                if (request.downloadHandler != null && request.downloadHandler.data != null)
//                {
//                    Debug.Log($"Download progress: {request.downloadProgress * 100}%");
//                }
//                else
//                {
//                    Debug.Log("Waiting for response...");
//                }
//            }
//            yield return null;
//        }

//        if (request.result == UnityWebRequest.Result.Success)
//        {
//            Debug.Log("Request successful: " + request.downloadHandler.text);

//            // Enable the importer object
//            if (importerObject != null)
//            {
//                importerObject.SetActive(true);
//                Debug.Log("Importer object enabled.");
//            }

//            // Enable the other object
//            if (anotherObject != null)
//            {
//                anotherObject.SetActive(true);
//                Debug.Log("Another object enabled.");
//            }

//            // Enable the third object
//            if (thirdObject != null)
//            {
//                thirdObject.SetActive(true);
//                Debug.Log("Third object enabled.");
//            }
//        }
//        else
//        {
//            Debug.LogError("Request failed: " + request.error);
//        }
//    }

//    [System.Serializable]
//    public class FetchObjectsData
//    {
//        public string username;
//        public string propertyName;
//        public string localPath;

//        public FetchObjectsData(string user, string prop, string path)
//        {
//            username = user;
//            propertyName = prop;
//            localPath = path;
//        }
//    }
//}

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class FetchObjects : MonoBehaviour
{
    public string baseURL = "https://theserver-tp6r.onrender.com";
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
        Debug.Log($"Sending request to: {url}");

        // Create a new WWWForm for the multipart form data
        WWWForm form = new WWWForm();
        form.AddField("organisationName", organisationName);
        form.AddField("parentPropertyName", parentPropertyName);
        form.AddField("childPropertyName", childPropertyName);
        form.AddField("localPath", localPath);

        // Create UnityWebRequest and attach the form data
        UnityWebRequest request = UnityWebRequest.Post(url, form);

        // Send the request
        Debug.Log("Sending request...");
        yield return request.SendWebRequest();

        // Check for network errors or HTTP errors
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Request successful: " + request.downloadHandler.text);

            // Process the response
            ProcessResponse(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Request failed: " + request.error);
        }
    }

    void ProcessResponse(string jsonResponse)
    {
        // Assuming the response is a JSON object containing an array of files
        ResponseData responseData = JsonUtility.FromJson<ResponseData>(jsonResponse);

        if (responseData != null && responseData.files != null)
        {
            Debug.Log("Files downloaded successfully:");
            foreach (var file in responseData.files)
            {
                Debug.Log($"File: {file.key}, Data (Base64): {file.data.Substring(0, 30)}..."); // Show first 30 characters
            }

            // Enable the importer object
            if (importerObject != null)
            {
                importerObject.SetActive(true);
                Debug.Log("Importer object enabled.");
            }

            // Enable the other objects
            if (anotherObject != null)
            {
                anotherObject.SetActive(true);
                Debug.Log("Another object enabled.");
            }

            if (thirdObject != null)
            {
                thirdObject.SetActive(true);
                Debug.Log("Third object enabled.");
            }
        }
        else
        {
            Debug.LogError("Failed to parse response or no files found.");
        }
    }

    [System.Serializable]
    public class ResponseData
    {
        public string message;
        public FileData[] files;
    }

    [System.Serializable]
    public class FileData
    {
        public string key;
        public string data;
    }
}
