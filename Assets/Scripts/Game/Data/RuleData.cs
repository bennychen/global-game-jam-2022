
using System;
using System.Collections.Generic;

namespace Game
{


    [Serializable]
    public class RuleData
    {
        public int ID;
        public string Description;
        public List<ConditionData> ConditionList = new List<ConditionData>();

        public bool IsMeet(CharacterData characterData)
        {
            bool result = true;
            for (int i = 0; i < ConditionList.Count; i++)
            {
                var condition = ConditionList[i];
                if (i == 0)
                {
                    result = condition.IsMeet(characterData);
                }
                else
                {
                    if (condition.RelationType == RelationType.And)
                    {
                        result &= condition.IsMeet(characterData);
                    }
                    else if (condition.RelationType == RelationType.Or)
                    {
                        result |= condition.IsMeet(characterData);
                    }
                }
            }

            return result;
        }
    }
}