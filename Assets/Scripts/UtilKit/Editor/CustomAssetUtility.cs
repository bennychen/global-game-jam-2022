using System.IO;
using UnityEditor;
using UnityEngine;

namespace Codeplay
{
    public static class CustomAssetUtility
    {
        public static void CreateAsset<T>(string fileName = null) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "New " + typeof(T).ToString();
            }
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + fileName + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }

        public static T CreateAssetAt<T>(string assetPathAndName) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            Selection.activeObject = asset;
            return asset;
        }
    }
}