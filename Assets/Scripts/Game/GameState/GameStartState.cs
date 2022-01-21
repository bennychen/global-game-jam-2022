
using Codeplay;

namespace Game
{
    public class GameStartState : State<GameController>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            _context.GameStartScene.SetActive(true);
        }

        public override void OnExit()
        {
            base.OnExit();
            _context.GameStartScene.SetActive(false);
        }
    }
}