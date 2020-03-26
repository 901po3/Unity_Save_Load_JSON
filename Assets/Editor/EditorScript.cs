using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneManager))]
public class EditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SceneManager sceneManager = (SceneManager)target;

        if(GUILayout.Button("Save Scene"))
        {
            sceneManager.Save();
            GUIUtility.ExitGUI();
        }
        
        if(GUILayout.Button("Load Scene"))
        {
            sceneManager.Load();
            GUIUtility.ExitGUI();
        }
    }
}
