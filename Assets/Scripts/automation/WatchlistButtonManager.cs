using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class WatchlistButtonManager : MonoBehaviour
{
    public Button buttonPrefab; // Assign this in the Inspector
    public Transform buttonParent; // Assign this in the Inspector

    void Start()
    {
        LoadWatchlistButtons();
    }

    void LoadWatchlistButtons()
    {
        List<WatchlistItem> watchlistItems = LoadWatchlistItemsFromPlayerPrefs();

        if (watchlistItems.Count == 0)
        {
            Debug.LogWarning("No watchlist items found.");
            return;
        }

        for (int i = 0; i < watchlistItems.Count; i++)
        {
            WatchlistItem item = watchlistItems[i];
            if (buttonPrefab == null)
            {
                Debug.LogError("Button Prefab is not assigned!");
                return;
            }

            Button newButton = Instantiate(buttonPrefab, buttonParent);
            StartCoroutine(LoadImageFromURL(item.imageURL, newButton));

            // Set the first watchlist item as the default image on the button
            if (i == 0)
            {
                StartCoroutine(LoadImageFromURL(item.imageURL, buttonPrefab));
                SaveWatchlistItemToPlayerPrefs(item); // Save the item to PlayerPrefs
                AWSFINALTEST awsFinalTest = FindObjectOfType<AWSFINALTEST>();
                if (awsFinalTest != null)
                {
                    awsFinalTest.SetWatchlistItem(item);
                }
                else
                {
                    Debug.LogError("AWSFINALTEST script not found in the scene.");
                }
            }

            // Set up the button click listener
            newButton.onClick.AddListener(() =>
            {
                SaveWatchlistItemToPlayerPrefs(item); // Save the item to PlayerPrefs
                AWSFINALTEST awsFinalTest = FindObjectOfType<AWSFINALTEST>();
                if (awsFinalTest != null)
                {
                    awsFinalTest.SetWatchlistItem(item);
                }
                else
                {
                    Debug.LogError("AWSFINALTEST script not found in the scene.");
                }
            });
        }
    }

    void SaveWatchlistItemToPlayerPrefs(WatchlistItem item)
    {
        PlayerPrefs.SetString("CurrentWatchlistItem_organisationName", item.organisationName);
        PlayerPrefs.SetString("CurrentWatchlistItem_parentPropertyName", item.parentPropertyName);
        PlayerPrefs.SetString("CurrentWatchlistItem_propertyName", item.propertyName);
        PlayerPrefs.SetString("CurrentWatchlistItem_date", item.date);
        PlayerPrefs.SetString("CurrentWatchlistItem_time", item.time);
        PlayerPrefs.SetString("CurrentWatchlistItem_imageURL", item.imageURL);
        PlayerPrefs.SetString("CurrentWatchlistItem_username", item.username);
        PlayerPrefs.Save();
    }

    List<WatchlistItem> LoadWatchlistItemsFromPlayerPrefs()
    {
        List<WatchlistItem> watchlistItems = new List<WatchlistItem>();

        int index = 0;
        while (PlayerPrefs.HasKey($"WatchlistItem_{index}_propertyName"))
        {
            WatchlistItem item = new WatchlistItem
            {
                propertyName = PlayerPrefs.GetString($"WatchlistItem_{index}_propertyName"),
                parentPropertyName = PlayerPrefs.GetString($"WatchlistItem_{index}_parentPropertyName"),
                organisationName = PlayerPrefs.GetString($"WatchlistItem_{index}_organisationName"),
                date = PlayerPrefs.GetString($"WatchlistItem_{index}_date"),
                time = PlayerPrefs.GetString($"WatchlistItem_{index}_time"),
                imageURL = PlayerPrefs.GetString($"WatchlistItem_{index}_imageURL"),
                username = PlayerPrefs.GetString($"WatchlistItem_{index}_username")
            };
            watchlistItems.Add(item);
            index++;
        }

        return watchlistItems;
    }

    IEnumerator LoadImageFromURL(string url, Button button)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                button.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                Debug.LogError("Failed to load image from URL: " + request.error);
            }
        }
    }
}