
using Codeplay;

namespace Game
{
    public class EndingState : State<GameController>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            _context.GameEndingScene.SetActive(true);
        }

        public override void OnExit()
        {
            base.OnExit();
            _context.GameEndingScene.SetActive(false);
        }
    }
}