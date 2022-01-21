using Codeplay;

namespace Game
{
    public class FirstEnterGameState : State<GameLoopController>
    {
        public override void OnEnter()
        {
            base.OnEnter();

            _context.LevelModel.CurrentDay = 0;
   
            _context.ResetRule();
            _stateMachine.ChangeState<CharacterEnterState>();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}