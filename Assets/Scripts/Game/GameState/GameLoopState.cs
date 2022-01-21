
using Codeplay;

namespace Game
{
    public class GameLoopState : State<GameController>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            _context.GameLoopScene.SetActive(true);
            _context.GameLoopController.GameLoopStateMachine.ChangeState<FirstEnterGameState>();
        }

        public override void OnExit()
        {
            base.OnExit();
            _context.GameLoopScene.SetActive(false);
        }
    }
}