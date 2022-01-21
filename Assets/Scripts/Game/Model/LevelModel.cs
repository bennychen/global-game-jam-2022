
using System.Collections.Generic;
using UnityEngine;

namespace Game.Model
{
    public class LevelModel
    {
        public int HP;
        public int CurrentDay = -1;

        public CharacterData CurrentCharacterData;
        public Character CurrentCharacter;
        public LevelData CurrentLevel;
        public List<RuleData> CurrentRule;
        
    }
}