using Codeplay;
using UnityEngine;

namespace Game
{
	public class ChangeNextDayState : State<GameLoopController>
	{
		public override void OnEnter()
		{
			base.OnEnter();

			_context.LevelModel.CurrentDay++;
			_context.LevelModel.CurrentCharacterIndex = 0;
			RecoverHP();
			if (_context.LevelModel.CurrentDay >= GameController.Instance.ConfigData.AllLevel.Count)
			{
				GameController.Instance.GameStateMachine.ChangeState<EndingState>();
				return;
			}
			_context.ResetRule();
			_context.LevelModel.IsNeedACharacter = true;
			_context.DialogCurrentRule();

			Camera.main.GetComponent<PlayUISound>().PlayNextDay();
		}

		private void RecoverHP()
		{
			_context.LevelModel.HP += 2;
			_context.LevelModel.HP = Mathf.Clamp(_context.LevelModel.HP, 0, GameController.Instance.ConfigData.DefaultHP);
			_context.UpdateHp();
		}


		public override void OnExit()
		{
			base.OnExit();
		}
	}
}