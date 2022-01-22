
using Codeplay;

namespace Game
{
    public class CharacterAwaitState : State<GameLoopController>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            
            GameController.Instance.DialogController.PopupSummaryDialog();
            
            GameController.Instance.RewardButton.ShowStick();
            GameController.Instance.RewardButton.EnableStick();
            GameController.Instance.PenaltyButton.ShowStick();
            GameController.Instance.PenaltyButton.EnableStick();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}