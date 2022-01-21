
using System;
using System.Collections.Generic;

namespace Game
{
    [Serializable]
    public class LevelData
    {
        public int Day = 0;
        // <0 is random
        public List<int> RuleIndex = new List<int>();
        
        
        
        //public List<CharacterData> CharacterList = new List<CharacterData>();
    }
}