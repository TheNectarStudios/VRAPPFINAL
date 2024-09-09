// using UnityEngine;
// using System.Collections;

// public class Automation : MonoBehaviour
// {
//     private BookingFetcher bookingFetcher;

//     void Start()
//     {
//         bookingFetcher = FindObjectOfType<BookingFetcher>();
//         if (bookingFetcher != null)
//         {
//             var watchlistDictionary = bookingFetcher.WatchlistDictionary;
//             Debug.Log("WatchlistDictionary Count: " + watchlistDictionary.Count);
//             // Further processing
//         }
//         else
//         {
//             Debug.LogError("BookingFetcher component not found.");
//         }
//     }
// }
