using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System.Collections.Generic;

public class AWSFINALTEST : MonoBehaviour
{
    public string baseURL = "https://theserver-tp6r.onrender.com";
    public GameObject importerObject;
    public GameObject anotherObject;
    public GameObject thirdObject;

    private BookingFetcher bookingFetcher;

    void Start()
    {
        bookingFetcher = FindObjectOfType<BookingFetcher>();

        if (bookingFetcher == null)
        {
            Debug.LogError("BookingFetcher component not found.");
            return;
        }

        if (importerObject != null) importerObject.SetActive(false);
        if (anotherObject != null) anotherObject.SetActive(false);
        if (thirdObject != null) thirdObject.SetActive(false);

        StartCoroutine(DownloadObjects());
    }

    IEnumerator DownloadObjects()
    {
        // Wait for BookingFetcher to fetch data
        yield return new WaitUntil(() => bookingFetcher.WatchlistDictionary != null && bookingFetcher.WatchlistDictionary.Count > 0);

        WatchlistItem firstItem = bookingFetcher.GetFirstWatchlistItem();
        if (firstItem == null)
        {
            Debug.LogError("No watchlist item found.");
            yield break;
        }

        string organisationName = firstItem.organisationName;
        string parentPropertyName = firstItem.parentPropertyName;
        string childPropertyName = firstItem.propertyName;

        Debug.Log("Using data: " + organisationName + ", " + parentPropertyName + ", " + childPropertyName);

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
                        Debug.Log("File Key: " + file.key);
                        Debug.Log("File Data: " + file.data);
                    }
                    else
                    {
                        Debug.LogError("Error downloading file: " + file.error);
                    }
                }
            }

            if (importerObject != null) importerObject.SetActive(true);
            if (anotherObject != null) anotherObject.SetActive(true);
            if (thirdObject != null) thirdObject.SetActive(true);
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
