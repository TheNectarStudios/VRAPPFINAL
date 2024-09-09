using UnityEditor;
using UnityEngine;

public class PlayerPrefsViewer : EditorWindow
{
    [MenuItem("Window/PlayerPrefs Viewer")]
    public static void ShowWindow()
    {
        GetWindow<PlayerPrefsViewer>("PlayerPrefs Viewer");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Refresh PlayerPrefs"))
        {
            Repaint();
        }

        GUILayout.Label("PlayerPrefs:");

        foreach (var key in PlayerPrefs.GetString("PlayerPrefsKeys").Split(','))
        {
            if (string.IsNullOrEmpty(key)) continue;

            if (PlayerPrefs.HasKey(key))
            {
                string value = PlayerPrefs.GetString(key);
                GUILayout.Label($"{key}: {value}");
            }  
        }
    }

    private void OnEnable()
    {
        // Initialize the keys list
        var keys = "";
        foreach (var key in PlayerPrefs.GetString("PlayerPrefsKeys").Split(','))
        {
            if (string.IsNullOrEmpty(key)) continue;

            keys += key + ",";
        }
        PlayerPrefs.SetString("PlayerPrefsKeys", keys);
        PlayerPrefs.Save();
    }
}
