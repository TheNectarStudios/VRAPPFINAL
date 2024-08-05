using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class BookingFetcher : MonoBehaviour
{
    public string baseURL = "https://theserver-tp6r.onrender.com";
    public string bookingKey = "R7PH5B"; // Example booking key

    public BookingData bookingData;

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
