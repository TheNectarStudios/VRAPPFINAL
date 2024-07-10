using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float rotate_speed = 300.0f;   
    // Start is called before the first frame update
    private float zoomspeed = 600f;
    private float zoomAmount = 0.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x + Input.GetAxis("Mouse Y") * Time.deltaTime * rotate_speed,transform.localEulerAngles.y + Input.GetAxis("Mouse X") * Time.deltaTime * rotate_speed,0 );
        }

        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            zoomAmount = Mathf.Clamp(zoomAmount + Input.GetAxis("Mouse Y")* Time.deltaTime * zoomspeed, -5.0f, 5.0f);
            Camera.main.transform.localPosition  = new Vector3(0,0,zoomAmount);
        }
    }
}
