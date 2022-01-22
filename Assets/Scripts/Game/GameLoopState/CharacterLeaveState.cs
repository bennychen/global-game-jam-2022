
using Codeplay;
using UnityEngine;

namespace Game
{
    public class CharacterLeaveState : State<GameLoopController>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            
            if (_context.LevelModel.HP <= 0)
            {
                GameController.Instance.GameStateMachine.ChangeState<EndingState>();
                return;
            }
            // TODO play anim?
        }

        public override void OnExit()
        {
            base.OnExit();
            _context.LevelModel.CurrentCharacter.gameObject.SetActive(false);
        }
    }
}