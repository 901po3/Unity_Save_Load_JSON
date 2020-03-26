using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class FileManager : MonoBehaviour
{
    public List<GameObject> objects;
    public string SaveFileName;

    string path;

    private void Awake()
    {
        //Default name if you don't choose the file name;
        SaveFileName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    public void Save()
    {
        // 1. Create txt file 
        path = Application.dataPath + '/' + SaveFileName + ".txt";
        File.WriteAllText(path, "");

        // 2. Get all gameobjects data
        objects.Clear();

        FileStream fileStream = new FileStream(path, FileMode.Create);
        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            foreach (GameObject obj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (obj.GetComponent<MeshFilter>() != null)
                {
                    objects.Add(obj);
                    ObjectData tempData = new ObjectData();
                    tempData.name = obj.name;
                    tempData.type = obj.GetComponent<MeshFilter>().mesh;
                    tempData.position = obj.transform.position;
                    tempData.scale = obj.transform.localScale;
                    tempData.rotation = obj.transform.rotation.eulerAngles;

                    // 3. Parse datas to json and save them to file
                    string data = JsonUtility.ToJson(tempData);
                    writer.Write(data);
                    writer.Write("\n");
                }
            }
        }
    }

    public void Load()
    {
        path = EditorUtility.OpenFolderPanel("Load Scene", "", "");
    }
}
