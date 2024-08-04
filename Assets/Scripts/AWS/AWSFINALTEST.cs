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
        if (importerObject != null) importerObject.SetActive(false);
        if (anotherObject != null) anotherObject.SetActive(false);
        if (thirdObject != null) thirdObject.SetActive(false);

        StartCoroutine(DownloadObjects());
    }

    IEnumerator DownloadObjects()
    {
        // Wait until a watchlist item is available
        yield return new WaitUntil(() => PlayerPrefs.HasKey("WatchlistItem_0_propertyName"));

        WatchlistItem firstItem = LoadWatchlistItem(0);
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

    WatchlistItem LoadWatchlistItem(int index)
    {
        if (PlayerPrefs.HasKey("WatchlistItem_" + index + "_propertyName"))
        {
            WatchlistItem item = new WatchlistItem
            {
                propertyName = PlayerPrefs.GetString("WatchlistItem_" + index + "_propertyName"),
                parentPropertyName = PlayerPrefs.GetString("WatchlistItem_" + index + "_parentPropertyName"),
                organisationName = PlayerPrefs.GetString("WatchlistItem_" + index + "_organisationName"),
                date = PlayerPrefs.GetString("WatchlistItem_" + index + "_date"),
                time = PlayerPrefs.GetString("WatchlistItem_" + index + "_time"),
                imageURL = PlayerPrefs.GetString("WatchlistItem_" + index + "_imageURL"),
                username = PlayerPrefs.GetString("WatchlistItem_" + index + "_username")
            };
            return item;
        }
        return null;
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
