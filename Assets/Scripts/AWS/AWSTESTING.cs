using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Dummiesman;
using TMPro;

public class AWSTESTING : MonoBehaviour
{
    string baseURL = "http://localhost:3000"; // Adjusted to exclude the endpoint path
    public Vector3 instantiatePosition = Vector3.zero; // Add a public Vector3 variable for the instantiation position
    public TMP_Text downloadStatusText; // TextMeshPro field to display download status
    private bool isDownloading = false; // Flag to ensure download coroutine runs only once
    private GameObject currentLoadedObject; // Track the currently loaded object
    public GameObject canvaskoudadenge;
    

    void Start()
    {
        FetchObjectsFromAPI();
    }

    public void FetchObjectsFromAPI()
    {
        if (!isDownloading)
        {
            StartCoroutine(GetObjectsFromAPI());
        }
    }
    public void baldevthedestroyer()
    {
        if (canvaskoudadenge != null)
        {
            Destroy(canvaskoudadenge);
        }
    }
    IEnumerator GetObjectsFromAPI()
    {
        string endpoint = "/objects"; // Replace with your API endpoint if different
        string url = baseURL + endpoint;

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("API Response: " + request.downloadHandler.text);
                ProcessAPIResponse(request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error fetching data: " + request.error);
            }
        }
    }

    void ProcessAPIResponse(string jsonResponse)
    {
        // Wrap the JSON response in an object structure for deserialization
        string wrappedJsonResponse = "{\"Items\":" + jsonResponse + "}";

        // Parse the wrapped JSON response
        S3ObjectWrapper wrapper = JsonUtility.FromJson<S3ObjectWrapper>(wrappedJsonResponse);
        S3Object[] s3Objects = wrapper.Items;

        foreach (S3Object s3Object in s3Objects)
        {
            Debug.Log("Key: " + s3Object.Key);
            Debug.Log("Last Modified: " + s3Object.LastModified);
            Debug.Log("ETag: " + s3Object.ETag);
            Debug.Log("Size: " + s3Object.Size);
            Debug.Log("Storage Class: " + s3Object.StorageClass);

            // If your API provides the file URL
            if (!string.IsNullOrEmpty(s3Object.Url) && !isDownloading)
            {
                StartCoroutine(DownloadAndInstantiateObject(s3Object.Url));
            }
            else if (!isDownloading)
            {
                // Construct URL if necessary
                string fileUrl = ConstructFileUrl(s3Object.Key);
                StartCoroutine(DownloadAndInstantiateObject(fileUrl));
            }
        }
    }

    string ConstructFileUrl(string key)
    {
        // Construct the file URL based on your S3 bucket structure
        // Example: https://your-bucket-name.s3.amazonaws.com/key
        return "https://ignitens.s3.amazonaws.com/" + key;
    }

    IEnumerator DownloadAndInstantiateObject(string fileUrl)
    {
        isDownloading = true;
        Debug.Log("Downloading file from: " + fileUrl);

        using (UnityWebRequest request = UnityWebRequest.Get(fileUrl))
        {
            request.SendWebRequest();

            float startTime = Time.time;
            while (!request.isDone)
            {
                float progress = request.downloadProgress;
                Debug.Log("Download Progress: " + (progress * 100).ToString("F2") + "%");
                if (downloadStatusText != null)
                {
                    downloadStatusText.text = "Download Progress: " + (progress * 100).ToString("F2") + "%";
                }
                yield return null;
            }

            float totalTime = Time.time - startTime;
            Debug.Log("Download completed in: " + totalTime.ToString("F2") + " seconds");
            if (downloadStatusText != null)
            {
                downloadStatusText.text = "Download completed in: " + totalTime.ToString("F2") + " seconds";
                baldevthedestroyer();

            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("File downloaded successfully.");
                byte[] fileData = request.downloadHandler.data;

                // Save the file to a temporary location for Dummiesman to load
                string tempFilePath = Path.Combine(Application.temporaryCachePath, "temp.obj");
                File.WriteAllBytes(tempFilePath, fileData);

                // Destroy the current loaded object if it exists
                if (currentLoadedObject != null)
                {
                    Destroy(currentLoadedObject);
                }

                // Load the OBJ model using the Dummiesman OBJ loader
                currentLoadedObject = LoadOBJFromFile(tempFilePath);
                if (currentLoadedObject != null)
                {
                    // Instantiate the new object and assign it to currentLoadedObject
                    currentLoadedObject.transform.position = instantiatePosition;
                }
                else
                {
                    Debug.LogError("Failed to load OBJ model.");
                }

                // Clean up temporary file
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
            else
            {
                Debug.LogError("Error downloading file: " + request.error);
            }
        }
        isDownloading = false;
    }

    GameObject LoadOBJFromFile(string filePath)
    {
        GameObject loadedObj = null;
        try
        {
            loadedObj = new OBJLoader().Load(filePath);
            if (loadedObj != null)
            {
                Debug.Log("Loaded OBJ from file: " + filePath);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Exception occurred while loading OBJ: " + e.Message);
        }
        return loadedObj;
    }
}

[System.Serializable]
public class S3Object
{
    public string Key;
    public string LastModified;
    public string ETag;
    public List<string> ChecksumAlgorithm;
    public long Size;
    public string StorageClass;
    public string Url; // Assuming your API provides the file URL
}

[System.Serializable]
public class S3ObjectWrapper
{
    public S3Object[] Items;
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
