using System;
using System.Collections.Generic;
using System.Text;
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

        [MenuItem("Temp/LoadCSV2DataAsset")]
        public static void LoadCsv()
        {
            ConfigData currentConfig = Resources.Load<ConfigData>("ConfigData");
            currentConfig.AllCharacter.Clear();
            
            var dataset = Resources.Load<TextAsset>("CharConfig");
            var datalines = dataset.text.Split('\n');
            for (int line = 2; line < datalines.Length; line++)//前两行是表头
            {
                int parseErrors = 0;
                var data = datalines[line].Split(',');
                var configdata= new CharacterData();
                configdata.Name = data[1];
                configdata.Skin = data[2];
                if(int.TryParse(data[3], out configdata.Complexity)==false) parseErrors+=1;
                if(Enum.TryParse(data[4], out configdata.Conflict)==false) parseErrors+=1;;
                if(int.TryParse(data[5], out configdata.AgeOfDeath)==false) parseErrors+=1;
                configdata.DeadReason = data[6];
                if(int.TryParse(data[7], out configdata.NumberOfChild)==false) parseErrors+=1;
                if(int.TryParse(data[8], out configdata.NumberOfKilled)==false) parseErrors+=1;
                configdata.Crime = data[9];
                if(Enum.TryParse(data[10], out configdata.Ethics)==false) parseErrors+=1;
                configdata.Comment = data[11];
                configdata.SummaryDialog = data[12];
                configdata.AgeOfDeathDialog = data[13];
                configdata.DeadReasonDialog = data[14];
                configdata.NumberOfChildDialog = data[15];
                configdata.NumberOfKilledDialog = data[16];
                configdata.CrimeDialog = data[17];
                configdata.RewardDialog = data[18];
                configdata.PenaltyDialog = data[19];

                if (parseErrors != 0)
                {
                    Debug.LogWarning("parse errors="+parseErrors+"in line ["+line+"]");
                    continue;
                }
                currentConfig.AllCharacter.Add(configdata);
//                StringBuilder sb=new StringBuilder();
//                for(int column = 0; column < data.Length; column++)
//                {
//                    sb.Append(data[column]+", ");
//                }
//                Debug.Log(sb); // what you get is split sequential data that is column-first, then row
            }
            Debug.Log(currentConfig.AllCharacter);
            AssetDatabase.SaveAssets();
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