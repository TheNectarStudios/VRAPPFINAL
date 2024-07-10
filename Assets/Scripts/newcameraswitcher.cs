using UnityEngine;

public class newcameraswitcher : MonoBehaviour
{
    public GameObject rig1;
    public GameObject rig2;

    void Update()
    {
        // Switch cameras when the spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Toggle the active state of the rigs
            rig1.SetActive(!rig1.activeSelf);
            rig2.SetActive(rig2.activeSelf);
        }
    }
}
