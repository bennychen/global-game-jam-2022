
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public enum EthicsType
    {
        Evil = 1,
        Neutral,
        Good,
    }
    [Serializable]
    public class CharacterData
    {
        public string Name;
        public int AgeOfDeath;
        public string DeadReason;
        public int NumberOfChild;
        public int NumberOfKilled;
        public string Crime;
        // 道德评判
        public EthicsType Ethics;
        public string Comment;
        
        
        public string SummaryDialog;
        public string AgeOfDeathDialog;
        public string DeadReasonDialog;
        public string NumberOfChildDialog;
        public string NumberOfKilledDialog;
        public string RewardDialog;
        public string PenaltyDialog;
        
       
        
        public Sprite MainSprite;
        // public List<int> MeetRules;
        // public int Score = 1;
    }
}
