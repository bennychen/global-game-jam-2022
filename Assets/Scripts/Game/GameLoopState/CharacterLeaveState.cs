
using Codeplay;
using UnityEngine;

namespace Game
{
    public class CharacterLeaveState : State<GameLoopController>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            // TODO some anim?
            if (_context.LevelModel.CurrentJudgeToHeaven)
            {
                GameObject.Destroy(_context.LevelModel.CurrentCharacter.gameObject);
            }
            else
            {
                GameObject.Destroy(_context.LevelModel.CurrentCharacter.gameObject);
            }
            OnExit();
        }

        public override void OnExit()
        {
            base.OnExit();
  
            if (_context.LevelModel.HP <= 0)
            {
                GameController.Instance.GameStateMachine.ChangeState<EndingState>();
                return;
            }
            
            _context.LevelModel.CurrentCharacterIndex++;
            if (_context.LevelModel.CurrentCharacterIndex >= _context.LevelModel.CurrentLevel.CharacterList.Count)
            {
                _stateMachine.ChangeState<ChangeNextDayState>();
            }
            else
            {
                _stateMachine.ChangeState<CharacterEnterState>();
            }
        }
    }
}