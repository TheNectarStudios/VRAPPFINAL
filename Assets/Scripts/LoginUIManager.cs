using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ConnecOptionsPanelGameObject;
    public GameObject ConnectWithNamePanelGameObject;
    void Start()
    {
        ConnecOptionsPanelGameObject.SetActive(true);
        ConnectWithNamePanelGameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
