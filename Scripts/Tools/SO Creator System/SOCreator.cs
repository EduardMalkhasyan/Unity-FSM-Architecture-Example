#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace BugiGames.Tools
{
    public class SOCreator : MonoBehaviour
    {
        [MenuItem("Tools/ProjectLibrary/ScriptableObject/CreateSOs", priority = 1)]
        public static void Create()
        {
            AssetDatabase.Refresh();

            UnityEngine.Object[] SOs = Resources.LoadAll(SOProps.folderName, typeof(UnityEngine.ScriptableObject));
            Type[] types = Assembly.GetAssembly(typeof(SOLoader<UnityEngine.ScriptableObject>)).GetTypes();
            List<string> createdSOsNames = new List<string>();
            List<string> fullNames = new List<string>();

            for (int i = 0; i < SOs.Length; i++)
            {
                createdSOsNames.Add(SOs[i].GetType().FullName);
            }

            for (int i = 0; i < types.Length; i++)
            {
                if (types[i].BaseType?.GetInterface(nameof(ISOLoader)) != null)
                {
                    if (createdSOsNames.Contains(types[i].FullName) == false)
                    {
                        fullNames.Add(types[i].FullName);

                        var createdInstance = UnityEngine.ScriptableObject.CreateInstance(types[i].FullName);
                        AssetDatabase.CreateAsset(createdInstance, $"Assets/Resources/{SOProps.folderName}/{types[i].FullName}.asset");
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        EditorUtility.FocusProjectWindow();
                        Selection.activeObject = createdInstance;

                        Debug.Log(types[i].FullName + " created! as Scriptable Object");
                    }
                    else
                    {
                        Debug.LogWarning(types[i].FullName + " doesn't created! its already exists");
                    };
                }
            }
        }
    }
}
#endif