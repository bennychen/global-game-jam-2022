using System.Collections;
using Codeplay;
using UnityEngine;

namespace Game
{
    public class FirstEnterGameState : State<GameLoopController>
    {
        public override void OnEnter()
        {
            base.OnEnter();

            _context.LevelModel.CurrentDay = 0;
   
            _context.ResetRule();

            Job.Make(AwaitToChangeState());

        }

        private IEnumerator AwaitToChangeState()
        {
            GameController.Instance.DialogController.TutorialDialog(
                GameController.Instance.ConfigData.GetDialogByKey("first_enter_game"));
            yield return new WaitForSeconds(2);
            _stateMachine.ChangeState<CharacterEnterState>();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}