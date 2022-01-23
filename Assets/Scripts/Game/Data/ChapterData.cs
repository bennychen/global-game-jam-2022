using System;
using System.Collections.Generic;

namespace Game
{
    public enum GroupType
    {
        A,
        B,
        C,
    }
    [Serializable]
    public class ChapterData
    {
        public List<CharacterGroupData> groupList = new List<CharacterGroupData>();
    }

    [Serializable]
    public class CharacterGroupData
    {
        public GroupType Group;
        public int characterAmount;
    }
}