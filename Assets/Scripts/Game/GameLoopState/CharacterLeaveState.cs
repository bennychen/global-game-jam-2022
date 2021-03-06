
using System.Collections;
using Codeplay;
using UnityEngine;

namespace Game
{
    public class CharacterLeaveState : State<GameLoopController>
    {
        public override void OnEnter()
        {
            GameController.Instance.PenaltyButton.DisableStick();
            GameController.Instance.RewardButton.DisableStick();
            
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