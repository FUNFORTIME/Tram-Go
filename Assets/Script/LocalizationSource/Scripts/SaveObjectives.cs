//using System.IO;
//using UnityEditor.SceneManagement;
//using UnityEditor;
//using UnityEngine;


//public static class ObjectSaver
//{
//    /// <summary>
//    /// Save an object as a prefab
//    /// </summary>
//    /// <param name="obj">The object to save</param>
//    /// <param name="path">The save path (e.g., "Assets/Prefabs/MyPrefab.prefab")</param>
//    public static void SaveAsPrefab(GameObject obj, string path)
//    {
//#if UNITY_EDITOR
//        if (obj == null)
//        {
//            Debug.LogError("Object is null, cannot save as prefab!");
//            return;
//        }

//        if (string.IsNullOrEmpty(path))
//        {
//            Debug.LogError("Path is null or empty, cannot save as prefab!");
//            return;
//        }

//        // Ensure the path ends with ".prefab"
//        if (!path.EndsWith(".prefab"))
//        {
//            path += ".prefab";
//        }

//        // Ensure the directory exists
//        string directory = Path.GetDirectoryName(path);
//        if (!Directory.Exists(directory))
//        {
//            Directory.CreateDirectory(directory);
//        }

//        // Save as prefab
//        PrefabUtility.SaveAsPrefabAsset(obj, path);
//        Debug.Log($"Prefab saved to: {path}");
//#endif
//    }

//    /// <summary>
//    /// Save the current scene
//    /// </summary>
//    public static void SaveScene()
//    {
//#if UNITY_EDITOR
//        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
//        Debug.Log("Scene saved!");
//#endif
//    }

//    /// <summary>
//    /// Mark an object as 'dirty' so it is saved when the scene is saved
//    /// </summary>
//    /// <param name="obj">The object to mark</param>
//    public static void MarkAsDirty(GameObject obj)
//    {
//#if UNITY_EDITOR
//        if (obj == null)
//        {
//            Debug.LogError("Object is null, cannot mark as dirty!");
//            return;
//        }

//        EditorUtility.SetDirty(obj);
//        Debug.Log($"Object {obj.name} has been marked as dirty!");
//#endif
//    }
//}