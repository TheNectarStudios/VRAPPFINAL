using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; // Corrected namespace

[CustomEditor(typeof(RoomManager))]
public class RoomManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("This script is responsible for changes in Creating and Managing Rooms", MessageType.Info);

        RoomManager roommanager = (RoomManager)target;
        // if (GUILayout.Button("Join Random Room"))
        // {
        //     roommanager.JoinRandomRoom();
        // }

        if (GUILayout.Button("Join School Button"))
        {
            roommanager.OnEnterButtonClicked_School();
        }
        if (GUILayout.Button("Join School Outdoor"))
        {
            roommanager.OnEnterButtonClicked_Outdoor();
        }
    }
}
