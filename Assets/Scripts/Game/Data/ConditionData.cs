
using System;

namespace Game
{
    public enum ConditionType
    {
        KillGreater,
        ChildLessEqualKill,
    }
    public enum RelationType
    {
        And,
        Or,
    }
    
    [Serializable]
    public class ConditionData
    {
        public ConditionType ConditionType;
        public int NumberValue;
        public string StringValue;
        public RelationType RelationType = RelationType.And;
        
        public bool IsMeet(CharacterData characterData)
        {
            switch (ConditionType)
            {
                case ConditionType.KillGreater:
                    return characterData.NumberOfKilled > NumberValue;
                case ConditionType.ChildLessEqualKill:
                    return characterData.NumberOfChild <= characterData.NumberOfKilled * NumberValue;
            }
            return true;
        }
    }
}