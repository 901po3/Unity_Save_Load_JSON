using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class FileManager : MonoBehaviour
{
    public List<GameObject> objects;
    public List<string> names;
    public List<Vector3> positions;
    public List<Vector3> scales;
    public List<Quaternion> rotations;

    string path;

    public void Save()
    {
        // 1. Create txt file 
        path = Application.dataPath + '/' + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + ".txt";
        File.WriteAllText(path, "");

        // 2. Get all gameobjects data
        objects.Clear();
        foreach (GameObject obj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            objects.Add(obj);
            names.Add(obj.GetType().ToString());
        }


    }

    public void Load()
    {
        path = EditorUtility.OpenFolderPanel("Load Scene", "", "");
    }
}
