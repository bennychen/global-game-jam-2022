
using System;

namespace Game
{
    public enum ConditionType
    {
        KillGreater,
        ChildLessEqualKill,
        KillNone,
        FromWar,
        FromAging,
        FromHungerThirsty,
    }
    public enum RelationType
    {
        And,
        Or,
    }

    public enum Destination
    {
        Heaven,
        Hell,
        Undefined,
    }
    
    [Serializable]
    public class ConditionData
    {
        public ConditionType ConditionType;
        public int NumberValue;
        public string StringValue;
        public RelationType RelationType = RelationType.And;
        // public Destination Destination;
        
        public bool IsMeet(CharacterData characterData)
        {
            switch (ConditionType)
            {
                case ConditionType.KillGreater:
                    return characterData.NumberOfKilled > NumberValue;
                case ConditionType.ChildLessEqualKill:
                    return characterData.NumberOfChild <= characterData.NumberOfKilled * NumberValue;
                case ConditionType.KillNone:
                    return characterData.NumberOfKilled <= 0;
                case ConditionType.FromWar:
                    return characterData.DeadReason == "刀兵";
                case ConditionType.FromAging:
                    return characterData.DeadReason == "寿终";
                case ConditionType.FromHungerThirsty:
                    return characterData.DeadReason == "饥渴";
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return true;
        }


    }
}