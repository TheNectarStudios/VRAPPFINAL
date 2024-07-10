// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class LocalUIScriptManager : MonoBehaviour
// {
//     [SerializeField] GameObject GoHome_Button;
//     // Start is called before the first frame update
//     void Start()
//     {
//         GoHome_Button.GetComponent<Button>().OnClick.AddListener(VirtualWorldManager.Instance.LeaveRoomAndLoadHomeScene);
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Added namespace for Button class

public class LocalUIScriptManager : MonoBehaviour
{
    [SerializeField] GameObject GoHome_Button;

    // Start is called before the first frame update
    void Start()
    {
        // Check if GoHome_Button has a Button component attached
        Button button = GoHome_Button.GetComponent<Button>();
        if (button != null)
        {
            // Add a listener to the onClick event of the button
            button.onClick.AddListener(() => VirtualWorldManager.Instance.LeaveRoomAndLoadHomeScene());
        }
        else
        {
            Debug.LogError("GoHome_Button does not have a Button component attached.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

