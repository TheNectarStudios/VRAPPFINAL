using UnityEngine;

public class SpectatorCameraController : MonoBehaviour
{
    [SerializeField] string playerTag = "Player"; // Tag of the player GameObjects to follow
    [SerializeField] float smoothSpeed = 0.125f; // Speed of camera movement

    void LateUpdate()
    {
        // Find the nearest player with the specified tag
        GameObject[] players = GameObject.FindGameObjectsWithTag(playerTag);
        GameObject nearestPlayer = FindNearestPlayer(players);

        if (nearestPlayer != null)
        {
            // Smoothly move the camera to follow the nearest player
            Vector3 desiredPosition = nearestPlayer.transform.position;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // Make the camera look at the nearest player
            transform.LookAt(nearestPlayer.transform);
        }
    }

    GameObject FindNearestPlayer(GameObject[] players)
    {
        GameObject nearestPlayer = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(player.transform.position, currentPosition);
            if (distance < minDistance)
            {
                nearestPlayer = player;
                minDistance = distance;
            }
        }

        return nearestPlayer;
    }

    // Other methods for switching views, adjusting camera settings, etc.
}
