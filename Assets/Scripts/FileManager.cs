using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using SmartDLL;

public class FileManager : MonoBehaviour
{
    public List<GameObject> objects;
    public string SaveFileName;
    public SmartFileExplorer fileExplorer = new SmartFileExplorer();

    string path;
    string loadFile;

    private void Awake()
    {
        //Default name if you don't choose the file name;
        SaveFileName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    public void Save()
    {
        // 1. Create txt file 
        path = Application.dataPath + "/Scenes/" + SaveFileName + ".txt";
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
        ShowExplorer();
    }

    private void ShowExplorer()
    {
        string initialDir = Application.dataPath + "/Scenes/";
        bool restoreDir = true;
        string title = "Open a Save Scene file";
        string defExt = "txt";
        string filter = "txt files (*.txt)|*.txt";

        /// <summary>
        /// --OpenExplorer--
        /// Call this function for open explorer
        /// In Unity Editor, It will throw 2 errors because form classed are not unregistered when Unity Editor Stops. 
        /// But it won't throw any kind of error in Build.
        /// </summary>
        fileExplorer.OpenExplorer(initialDir, restoreDir, title, defExt, filter);

        if(fileExplorer.resultOK)
        {
            ReadText(fileExplorer.fileName);
        }
    }

    private void ReadText(string path)
    {
        Debug.Log(path);
        loadFile = File.ReadAllText(path);
        Debug.Log(loadFile);
    }
}
