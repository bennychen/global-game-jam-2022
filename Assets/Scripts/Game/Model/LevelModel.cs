
using System.Collections.Generic;
using UnityEngine;

namespace Game.Model
{
    public class LevelModel
    {
        public int HP;
        public int CurrentDay = -1;
        public int CurrentCharacterIndex = 0;
        public int CorrectScore;
        // 秩序分
        public int CorrectButNotEthicsScore;
        // 道德分
        public int EthicsButMistakeScore;
        public bool CurrentJudgeToHeaven;

        public CharacterData CurrentCharacterData;
        public Character CurrentCharacter;
        public LevelData CurrentLevel;
        public List<RuleData> CurrentRule;
        
    }
}