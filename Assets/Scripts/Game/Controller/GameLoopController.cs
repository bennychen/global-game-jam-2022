
using System;
using System.Data;
using System.Text;
using Codeplay;
using Game.Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class GameLoopController : MonoBehaviour
    {
        public StateMachine<GameLoopController> GameLoopStateMachine;

        public LevelModel LevelModel = new LevelModel();

        private void Awake()
        {
            CreateGameStateMachine();
        }

        public void CreateGameStateMachine()
        {
            GameLoopStateMachine = new StateMachine<GameLoopController>(this);
            GameLoopStateMachine.AddState(new FirstEnterGameState());
            GameLoopStateMachine.AddState(new CharacterEnterState());
            GameLoopStateMachine.AddState(new CharacterAwaitState());
            GameLoopStateMachine.AddState(new ChangeNextDayState());

            ChangeToFirstEnterGame();
            ResetLevelModel();
            // GameStateMachine.OnStateChanged += OnGameStateChanged;
        }

        private void ResetLevelModel()
        {
            LevelModel.HP = GameController.Instance.ConfigData.DefaultHP;
            
        }

        public void ChangeToFirstEnterGame()
        {
            GameLoopStateMachine.ChangeState<FirstEnterGameState>();
        }
        
        public void ChangeToCharacterEnter()
        {
            GameLoopStateMachine.ChangeState<CharacterEnterState>();
        }
        
        public void ChangeToAwait()
        {
            GameLoopStateMachine.ChangeState<CharacterAwaitState>();
        }
        
        public void ChangeToNextDay()
        {
            GameLoopStateMachine.ChangeState<ChangeNextDayState>();
        }
        
        public void PlayerJudge(bool toHeaven)
        {
            CharacterData character = LevelModel.CurrentCharacterData;
            var meetRule = MeetRule(character, toHeaven);
            if (meetRule)
            {
                LevelModel.CorrectScore++;
                if (!toHeaven && character.Ethics == EthicsType.Good)
                {
                    LevelModel.CorrectButNotEthicsScore++;
                }
            }
            else
            {
                LevelModel.CorrectScore--;
                LevelModel.HP--;
                if (toHeaven && character.Ethics == EthicsType.Good)
                {
                    LevelModel.EthicsButMistakeScore++;
                }
            }

            LevelModel.CurrentJudgeToHeaven = toHeaven;

            
            GameLoopStateMachine.ChangeState<CharacterLeaveState>();
        }

        private bool MeetRule(CharacterData character, bool toHeaven)
        {
            // TODO no rule
            return true;
        }

        public void DialogCurrentRule()
        {
            if (LevelModel.CurrentDay <= 0)
            {
                return;
            }
            StringBuilder buff = new StringBuilder();
            foreach (var rule in LevelModel.CurrentRule)
            {
                buff.Append(rule.Description);
                buff.Append("\r\n");
            }
            
            // simple use SendMessage
            GameController.Instance.SendMessage("NpcDialog", buff.ToString());
        }

        public void ResetRule()
        {
            LevelModel.CurrentLevel =
                GameController.Instance.ConfigData.AllLevel[LevelModel.CurrentDay];
            LevelModel.CurrentRule.Clear();
            foreach (var ruleID in GameController.Instance.ConfigData.DefaultRule)
            {
                foreach (var rule in GameController.Instance.ConfigData.AllRule)
                {
                    if (rule.ID == ruleID)
                    {
                        LevelModel.CurrentRule.Add(rule);
                    }
                }
            }
            if (LevelModel.CurrentDay > 0)
            {
                var randomIndex = Random.Range(0, GameController.Instance.ConfigData.AllRule.Count);
                LevelModel.CurrentRule.Add(GameController.Instance.ConfigData.AllRule[randomIndex]);
            }
        }
    }
}