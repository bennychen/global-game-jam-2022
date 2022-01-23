
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
        public Destination Destination;

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

        public bool MeetChoice(CharacterData characterData, Destination destination)
        {
            bool result = false;
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

            if (!result)
            {
                return true;
            }
            else
            {
                return this.Destination == destination;
            }
        }
        
        // public Destination CalcDest(CharacterData characterData)
        // {
        //     return IsMeet(characterData) ? Destination : Destination.Undefined;
        // }

        // public Destination CompDest(CharacterData characterData)
        // {
        //     foreach (var data in ConditionList)
        //     {
        //         var calcDest = data.CalcDest(characterData);
        //         if (calcDest!=Destination.Undefined)
        //         {
        //             return calcDest;
        //         }
        //     }
        //
        //     return Destination.Undefined;
        // }
    }
}