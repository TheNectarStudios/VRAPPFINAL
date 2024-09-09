using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class AWSFINALTEST : MonoBehaviour
{
    public string baseURL = "https://theserver-tp6r.onrender.com";
    public GameObject importerObject;
    public GameObject anotherObject;
    public GameObject thirdObject;

    void Start()
    {
        Debug.Log("AWSFINALTEST script started.");

        if (importerObject != null) importerObject.SetActive(false);
        if (anotherObject != null) anotherObject.SetActive(false);
        if (thirdObject != null) thirdObject.SetActive(false);

        WatchlistItem item = LoadWatchlistItemFromPlayerPrefs();
        if (item != null)
        {
            Debug.Log("Watchlist item loaded: " + item.organisationName);
            SetWatchlistItem(item);
        }
        else
        {
            Debug.LogError("No watchlist item found in PlayerPrefs.");
        }
    }

    private WatchlistItem LoadWatchlistItemFromPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("CurrentWatchlistItem_organisationName"))
        {
            WatchlistItem item = new WatchlistItem
            {
                organisationName = PlayerPrefs.GetString("CurrentWatchlistItem_organisationName"),
                parentPropertyName = PlayerPrefs.GetString("CurrentWatchlistItem_parentPropertyName"),
                propertyName = PlayerPrefs.GetString("CurrentWatchlistItem_propertyName"),
                date = PlayerPrefs.GetString("CurrentWatchlistItem_date"),
                time = PlayerPrefs.GetString("CurrentWatchlistItem_time"),
                imageURL = PlayerPrefs.GetString("CurrentWatchlistItem_imageURL"),
                username = PlayerPrefs.GetString("CurrentWatchlistItem_username")
            };
            return item;
        }
        return null;
    }

    public void SetWatchlistItem(WatchlistItem item)
    {
        Debug.Log("Setting watchlist item in AWSFINALTEST: " + item.organisationName + ", " + item.parentPropertyName + ", " + item.propertyName);
        StartCoroutine(DownloadObjects(item));
    }

    IEnumerator DownloadObjects(WatchlistItem item)
    {
        string organisationName = item.organisationName;
        string parentPropertyName = item.parentPropertyName;
        string childPropertyName = item.propertyName;

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

    [System.Serializable]
    public class WatchlistItem
    {
        public string propertyName;
        public string parentPropertyName;
        public string organisationName;
        public string date;
        public string time;
        public string imageURL;
        public string username;
    }
}
