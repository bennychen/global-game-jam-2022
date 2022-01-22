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

			AwaitToChangeState();

		}

		private void AwaitToChangeState()
		{
			GameController.Instance.DialogController.TutorialDialog(
					GameController.Instance.ConfigData.GetDialogByKey("first_enter_game"),
										this.onAnimComplete);
		}

		private void onAnimComplete()
		{
			_stateMachine.ChangeState<CharacterEnterState>();
		}

		public override void OnExit()
		{
			base.OnExit();
		}
	}
}