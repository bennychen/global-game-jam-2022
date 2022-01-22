using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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

        public int DefaultHP = 5;
        // public int DayAmount = 10;
        public List<int> DefaultRule = new List<int>();
        public int EthicsEndingThreshold = 10;

        

        public string GetDialogByKey(string key)
        {
            if (keyToDialog == null)
            {
                keyToDialog = new Dictionary<string, DialogData>();
                foreach (var dialog in AllDialog)
                {
                    keyToDialog.Add(dialog.key, dialog);
                }
            }

            return keyToDialog[key].dialog;
        }
        private Dictionary<string, DialogData> keyToDialog;
    }


}