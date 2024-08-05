using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputHandler : MonoBehaviour
{
    public TMP_InputField RoomKeyInputField; // Reference to the input field
    public Button EnterButton; // Reference to the Enter button

    public BookingFetcher bookingFetcher;
    public RoomManager roomManager;

    void Start()
    {
        EnterButton.onClick.AddListener(OnEnterButtonClick);
    }

    void OnEnterButtonClick()
    {
        string roomKey = RoomKeyInputField.text;
        if (!string.IsNullOrEmpty(roomKey))
        {
            PlayerPrefs.SetString("EnteredRoomKey", roomKey);
            PlayerPrefs.Save();
            Debug.Log("Room key entered: " + roomKey);

            if (bookingFetcher != null)
            {
                bookingFetcher.StartFetchingBooking(roomKey);
            }
        }
        else
        {
            Debug.LogWarning("Room key is empty!");
        }
    }
}
