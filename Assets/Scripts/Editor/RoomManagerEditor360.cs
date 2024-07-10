using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Room360VR))]
public class RoomManagerEditor360 : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Room360VR roomManager = (Room360VR)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Join 360VR Room"))
        {
            roomManager.ChangeSceneTo360VR();
        }
    }
}
