
using System;
using System.Collections.Generic;

namespace Game
{
    [Serializable]
    public class LevelData
    {
        // public int IndexIsDay = 0;
        // public int CharacterAmount = 3;
        public List<int> CharacterList = new List<int>();
        // <0 is random
        public List<int> RuleIndex = new List<int>();
        
        
        
        //public List<CharacterData> CharacterList = new List<CharacterData>();
    }
}