
using Codeplay;

namespace Game
{
    public class CharacterAwaitState : State<GameLoopController>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            GameController.Instance.RewardButton.EnableStick();
            GameController.Instance.PenaltyButton.EnableStick();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}