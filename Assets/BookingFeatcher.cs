using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class BookingFetcher : MonoBehaviour
{
    public string baseURL = "https://theserver-tp6r.onrender.com";
    public string bookingKey = "R7PH5B"; // Example booking key

    public BookingData bookingData;

    private Dictionary<string, WatchlistItem> watchlistDictionary;

    void Start()
    {
        watchlistDictionary = new Dictionary<string, WatchlistItem>();
        StartCoroutine(FetchBookingByKey());
    }

    IEnumerator FetchBookingByKey()
    {
        string url = baseURL + "/slots/bookings/key/" + bookingKey;

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Booking fetch successful: " + request.downloadHandler.text);
            BookingResponse response = JsonUtility.FromJson<BookingResponse>(request.downloadHandler.text);

            if (response.booking != null)
            {
                bookingData = response.booking;
                StoreWatchlistItems(response.booking.watchlist);
                Debug.Log("Booking Key: " + response.booking.key);
                foreach (var item in response.booking.watchlist)
                {
                    Debug.Log("Property Name: " + item.propertyName);
                    Debug.Log("Parent Property Name: " + item.parentPropertyName);
                    Debug.Log("Organisation Name: " + item.organisationName);
                    Debug.Log("Date: " + item.date);
                    Debug.Log("Time: " + item.time);
                    Debug.Log("Image URL: " + item.imageURL);
                }
            }
            else
            {
                Debug.LogWarning("No booking found with this key.");
            }
        }
        else
        {
            Debug.LogError("Request failed: " + request.error);
        }
    }

    void StoreWatchlistItems(List<WatchlistItem> watchlist)
    {
        foreach (var item in watchlist)
        {
            string key = GetCompositeKey(item);
            if (!watchlistDictionary.ContainsKey(key))
            {
                watchlistDictionary[key] = item;
                Debug.Log("Stored item with key: " + key);
            }
        }
    }

    string GetCompositeKey(WatchlistItem item)
    {
        return $"{item.organisationName}_{item.propertyName}_{item.parentPropertyName}";
    }

    public Dictionary<string, WatchlistItem> WatchlistDictionary
    {
        get { return watchlistDictionary; }
    }

    public WatchlistItem GetFirstWatchlistItem()
    {
        foreach (var item in watchlistDictionary.Values)
        {
            return item; // Return the first item
        }
        return null;
    }

    [System.Serializable]
    public class BookingData
    {
        public string _id;
        public string username;
        public List<WatchlistItem> watchlist;
        public string key;
        public string date;
    }

    [System.Serializable]
    public class BookingResponse
    {
        public BookingData booking;
    }
}
