
using Codeplay;

namespace Game
{
    public class GuidState : State<GameController>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            _context.GameGuideScene.SetActive(true);
        }

        public override void OnExit()
        {
            base.OnExit();
            _context.GameGuideScene.SetActive(false);
        }
    }
}