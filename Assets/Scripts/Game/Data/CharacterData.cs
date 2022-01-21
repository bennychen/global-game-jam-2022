
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    
    [Serializable]
    public class CharacterData
    {
        public string Name;
        public Sprite MainSprite;
        public List<string> MainStory;
        public string AgeOfDeath;
        public string DeadReason;
        public string Crime;
        public int NumberOfChild;
        public int NumberOfKilled;

       
        // 道德评判
        public bool IsGoodMen;
        // 秩序评判
        public List<int> MeetRules;
        
        public int Score = 1;
    }
}
