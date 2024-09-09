using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class BookingFetcher : MonoBehaviour
{
    public string baseURL = "https://theserver-tp6r.onrender.com";
    public string bookingKey = "R7PH5B"; // Example booking key

    public static BookingData bookingData; // Store the fetched booking data
    public static string FetchedRoomKey; // Store the fetched room key

    void Start()
    {
        StartCoroutine(FetchBookingByKey());
    }

    public void StartFetchingBooking(string bookingKey)
    {
        this.bookingKey = bookingKey;
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
                FetchedRoomKey = response.booking.key;
                Debug.Log("Fetched Room Key: " + FetchedRoomKey);
                SaveBookingDataToJSON();
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

    void SaveBookingDataToJSON()
    {
        string json = JsonUtility.ToJson(bookingData);
        string path = Path.Combine(Application.persistentDataPath, "BookingData.json");
        File.WriteAllText(path, json);
        Debug.Log("Booking data saved to: " + path);
    }

    public static BookingData LoadBookingDataFromJSON()
    {
        string path = Path.Combine(Application.persistentDataPath, "BookingData.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            BookingData data = JsonUtility.FromJson<BookingData>(json);
            Debug.Log("Booking data loaded from: " + path);
            return data;
        }
        else
        {
            Debug.LogWarning("No booking data file found.");
            return null;
        }
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
