// using UnityEngine;
// using UnityEngine.UI;

// public class DownloadTrigger : MonoBehaviour
// {
//     public GameObject downloadCanvas; // Reference to the Canvas that should be shown

//     void Start()
//     {
//         if (downloadCanvas != null)
//         {
//             downloadCanvas.SetActive(false); // Ensure the canvas is hidden initially
//         }
//     }

//     void OnTriggerEnter(Collider other)
//     {
//         if (other.CompareTag("Player") && downloadCanvas != null)
//         {
//             downloadCanvas.SetActive(true);
//             // Optionally, start the download process here
//             AWSTESTING awsTesting = FindObjectOfType<AWSTESTING>();
//             if (awsTesting != null)
//             {
//                 awsTesting.FetchObjectsFromAPI();
//             }
//         }
//     }
// }
