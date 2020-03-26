using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SmartDLL;

public class SceneManager : MonoBehaviour
{
    static public List<GameObject> objects  = new List<GameObject>();
    static public string SaveFileName = "newSave";
    static int i = 0;
    static public SmartFileExplorer fileExplorer = new SmartFileExplorer();

    static string path;
    static string loadFile;

    [MenuItem("SceneManager/Save")]
    public static void Save()
    {
        // 1. Create txt file 
        path = Application.dataPath + "/Scenes/" + SaveFileName + i + ".txt";
        i++;
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
                    tempData.type = obj.GetComponent<MeshFilter>().sharedMesh.name;
                    tempData.name = obj.name;
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

    [MenuItem("SceneManager/Load")]
    public static void Load()
    {
        ShowExplorer();
    }

    private static void ShowExplorer()
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

        if (fileExplorer.resultOK)
        {
            LoadData();
        }
    }

    private static void LoadData()
    {
        // 1. destroy all gameobjects
        foreach (GameObject obj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (obj.GetComponent<MeshFilter>() != null)
            {
                DestroyImmediate(obj);
                //Destroy(obj);
            }
        }
        objects.Clear();

        // 2. Read File to Json format
        string path = fileExplorer.fileName;
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                loadFile = reader.ReadToEnd();
            }
        }

        //Load Ojbects
        string[] lines = loadFile.Split('\n');
        foreach (string line in lines)
        {
            if (line.Length != 0)
            {
                ObjectData objData = JsonUtility.FromJson<ObjectData>(line);
                Debug.Log(line);
                LoadObject(objData);
            }
        }
    }

    static void LoadObject(ObjectData objData)
    {
        GameObject gameObject = null;

        switch (objData.type)
        {
            case "Cube Instance":
            case "Cube":
                gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                break;
            case "Cylinder Instance":
            case "Cylinder":
                gameObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                break;
            case "Capsule Instance":
            case "Capsule":
                gameObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                break;
            case "Sphere Instance":
            case "Sphere":
                gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                break;
            case "Plane Instance":
            case "Plane":
                gameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
                break;
            case "Quad Instance":
            case "Quad":
                gameObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
                break;
            default:
                break;
        }
        if (gameObject != null)
        {
            gameObject.name = objData.name;
            gameObject.transform.position = objData.position;
            gameObject.transform.rotation = Quaternion.Euler(objData.rotation);
            gameObject.transform.localScale = objData.scale;
        }
        //CloseMenuPanel();
    }

}
