using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject downloadManagerObject;
    public GameObject importManagerObject;

    void Start()
    {
        // Start with only the DownloadManager enabled
        downloadManagerObject.SetActive(true);
        importManagerObject.SetActive(false);
    }

    public void OnDownloadComplete()
    {
        // When download is complete, disable the DownloadManager and enable the ImportManager
        downloadManagerObject.SetActive(false);
        importManagerObject.SetActive(true);
    }
}
