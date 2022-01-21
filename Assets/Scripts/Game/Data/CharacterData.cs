
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    
    [Serializable]
    public class CharacterData
    {
        public Sprite MainSprite;
        public List<string> MainStory;
        public string Age;
        public string DeadReason;
        public string Crime;
        public int NumberOfChild;
        public int NumberOfKill;

        public int Score = 1;
        public bool IsGoodMen;
    }
}