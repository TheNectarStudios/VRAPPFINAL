using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LoginManager))]


public class LoginManagerEditor : Editor
{
    public override void OnInspectorGUI(){
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("This script is responsible for connecting to Photon Servers",MessageType.Info);

        LoginManager loginmanager = (LoginManager)target;
        if (GUILayout.Button("Connect Anonymously"))
        {
            loginmanager.ConnectAnonymously();
        }
    }
}
