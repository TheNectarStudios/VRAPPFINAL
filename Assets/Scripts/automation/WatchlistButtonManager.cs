using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class WatchlistButtonManager : MonoBehaviour
{
    public Button buttonPrefab; // Assign this in the Inspector
    public Transform buttonParent; // Assign this in the Inspector
    public TextMeshProUGUI statusText; // Assign this in the Inspector

    private List<Button> buttons = new List<Button>();
    private bool isSceneChanging = false;

    void Start()
    {
        LoadWatchlistButtons();
    }

    void LoadWatchlistButtons()
    {
        if (BookingFetcher.bookingData == null || BookingFetcher.bookingData.watchlist == null)
        {
            Debug.LogWarning("No watchlist items found.");
            if (statusText != null)
            {
                statusText.text = "No watchlist items found.";
            }
            return;
        }

        List<BookingFetcher.WatchlistItem> watchlistItems = BookingFetcher.bookingData.watchlist;

        for (int i = 0; i < watchlistItems.Count; i++)
        {
            BookingFetcher.WatchlistItem item = watchlistItems[i];
            if (buttonPrefab == null)
            {
                Debug.LogError("Button Prefab is not assigned!");
                if (statusText != null)
                {
                    statusText.text = "Button Prefab is not assigned!";
                }
                return;
            }

            Button newButton = Instantiate(buttonPrefab, buttonParent);
            buttons.Add(newButton);
            StartCoroutine(LoadImageFromURL(item.imageURL, newButton));

            // Set up the button click listener
            int index = i; // Capture the current index
            newButton.onClick.AddListener(() =>
            {
                SaveWatchlistItemToPrefs(item);
                if (!isSceneChanging)
                {
                    isSceneChanging = true;
                    Room360VR room360VR = FindObjectOfType<Room360VR>();
                    if (room360VR != null)
                    {
                        room360VR.ChangeSceneTo360VR();
                    }
                    else
                    {
                        Debug.LogError("Room360VR script not found in the scene.");
                        if (statusText != null)
                        {
                            statusText.text = "Room360VR script not found in the scene.";
                        }
                    }
                }

                ToggleButtons(index);
            });
        }

        // Initially set the first button as active and others as inactive
        if (buttons.Count > 0)
        {
            buttons[0].gameObject.SetActive(true);
            for (int i = 1; i < buttons.Count; i++)
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }

    void SaveWatchlistItemToPrefs(BookingFetcher.WatchlistItem item)
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

    IEnumerator LoadImageFromURL(string url, Button button)
    {
        Debug.Log("Attempting to load image from URL: " + url);

        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                button.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                Debug.Log("Successfully loaded image from URL: " + url);
                if (statusText != null)
                {
                    statusText.text = "Successfully loaded image from URL: " + url;
                }
            }
            else
            {
                Debug.LogError("Failed to load image from URL: " + url + " - " + request.error);
                if (statusText != null)
                {
                    statusText.text = "Failed to load image from URL: " + url + " - " + request.error;
                }
            }
        }
    }

    void ToggleButtons(int clickedIndex)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].gameObject.SetActive(i == clickedIndex);
        }
    }

    // Public method to be called from UI buttons
    public void ToggleToFirstButton()
    {
        ToggleButtons(0);
    }

    public void ToggleToSecondButton()
    {
        ToggleButtons(1);
    }
}
