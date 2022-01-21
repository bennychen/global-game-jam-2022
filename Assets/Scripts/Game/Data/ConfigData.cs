using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    [Serializable]
    public class ConfigData : ScriptableObject
    {
        [MenuItem("Temp/CreateCharacterDataAsset")]
        public static void CreateMyAsset()
        {
            ConfigData asset = ScriptableObject.CreateInstance<ConfigData>();

            AssetDatabase.CreateAsset(asset, "Assets/Resources/ConfigData.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }

        public List<CharacterData> AllCharacter;
        public List<LevelData> AllLevel;
        public List<RuleData> AllRule;
        public List<DialogData> AllDialog;

        public int AllScore = 10;
        public int LevelAmount = 30;
        public List<int> DefaultRule = new List<int>();

        

        public DialogData GetDialogByKey(string key)
        {
            if (keyToDialog == null)
            {
                keyToDialog = new Dictionary<string, DialogData>();
                foreach (var dialog in AllDialog)
                {
                    keyToDialog.Add(dialog.key, dialog);
                }
            }

            return keyToDialog[key];
        }
        private Dictionary<string, DialogData> keyToDialog;
    }


}