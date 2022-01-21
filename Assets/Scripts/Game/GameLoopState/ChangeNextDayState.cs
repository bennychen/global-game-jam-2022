﻿
using System.Text;
using Codeplay;
using UnityEngine;

namespace Game
{
    public class ChangeNextDayState : State<GameLoopController>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            
            _context.LevelModel.CurrentDay++;
            _context.LevelModel.CurrentCharacterIndex = 0;
            if (_context.LevelModel.CurrentDay >= GameController.Instance.ConfigData.AllLevel.Count)
            {
                GameController.Instance.GameStateMachine.ChangeState<EndingState>();
                return;
            }
            _context.ResetRule();
            _context.DialogCurrentRule();
        }


        public override void OnExit()
        {
            base.OnExit();
        }
    }
}