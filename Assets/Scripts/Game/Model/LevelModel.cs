
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
        public bool CurrentJudgeCorrect = true;

        public CharacterData CurrentCharacterData;
        public Character CurrentCharacter;
        public LevelData CurrentLevel;
        public List<RuleData> CurrentRule = new List<RuleData>();

        public bool IsNeverUseReward = true;
        public bool IsNeverUsePenalty = true;
        public bool IsNeverMistake = true;
        public bool IsNeedACharacter = false;
    }
}