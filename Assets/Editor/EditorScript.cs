//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(SceneManager))]
//public class EditorScript : Editor
//{
//    [MenuItem("MyMenu/Save")]
//    public static void Save()
//    {
//        if (GUILayout.Button("Save Scene"))
//        {
            
//            GUIUtility.ExitGUI();
//        }
//    }


//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        SceneManager sceneManager = (SceneManager)target;

//        if(GUILayout.Button("Save Scene"))
//        {
//            sceneManager.Save();
//            GUIUtility.ExitGUI();
//        }
        
//        if(GUILayout.Button("Load Scene"))
//        {
//            sceneManager.Load();
//            GUIUtility.ExitGUI();
//        }
//    }
//}
