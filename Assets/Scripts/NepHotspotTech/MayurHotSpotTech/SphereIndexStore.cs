using UnityEngine;

public class SphereIndexStore : MonoBehaviour
{
    public int index; // Public field to store the index

    void OnMouseDown()
    {
        float targetX = index * 50f;
        TeleportPlayersToXCoordinate(targetX);
    }

    void TeleportPlayersToXCoordinate(float x)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            Vector3 newPosition = new Vector3(x, player.transform.position.y, player.transform.position.z);
            player.transform.position = newPosition;
        }
    }
}
