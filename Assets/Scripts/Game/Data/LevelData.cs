
using System.Collections.Generic;

namespace Game
{
    public class LevelData
    {
        public int Day = 0;
        // <0 is random
        public int RuleIndex = -1;

        
        public List<CharacterData> CharacterList = new List<CharacterData>();
        
    }
}