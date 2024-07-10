using UnityEngine;
using TMPro;

public class RoomKeyStore : MonoBehaviour
{
    public TextMeshProUGUI RoomKeyText; // Assign this in the Inspector

    // Start is called before the first frame update
    void Start()
    {
        if (RoomKeyText != null)
        {
            RoomKeyText.text = "Room Key: " + RoomManager.RoomKey;
        }
    }
}
 