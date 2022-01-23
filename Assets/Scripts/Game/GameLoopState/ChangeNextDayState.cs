using Codeplay;
using DG.Tweening;
using UnityEngine;

namespace Game
{
	public class ChangeNextDayState : State<GameLoopController>
	{
		public override void OnEnter()
		{
			Debug.Log("next day state");
			base.OnEnter();

			Camera.main.GetComponent<PlayUISound>().PlayNextDay();
			
			GameController.Instance.DialogController.TutorialDialog(
				string.Format(GameController.Instance.ConfigData.GetDialogByKey("before_rule_change"),
					_context.LevelModel.CurrentCharacterData.Name), this.ChangeToNight);
		}

		private void ChangeToNight()
		{
			var sequence = DG.Tweening.DOTween.Sequence();
			sequence.Append(GameController.Instance.NightMask.DOFade(1.0f, 0.5f));
			sequence.AppendInterval(1f);
			sequence.Append(GameController.Instance.NightMask.DOFade(0f, 0.5f));
			sequence.onComplete += AfterChangeDay;
			sequence.Play();
		}

		private void AfterChangeDay()
		{
			GameController.Instance.DialogController.TutorialDialog(
				string.Format(GameController.Instance.ConfigData.GetDialogByKey("rule_change"),
					_context.LevelModel.CurrentCharacterData.Name), this.ChangeDay);
		}

		private void ChangeDay()
		{
			_context.LevelModel.CurrentDay++;
			_context.LevelModel.CurrentCharacterIndex = 0;
			RecoverHP();
			if (_context.LevelModel.CurrentDay >= GameController.Instance.ConfigData.AllLevel[0].ChapterList.Count)
			{
				Debug.Log("AllLevelCount exceeded, going to ending state");
				GameController.Instance.GameStateMachine.ChangeState<EndingState>();
				return;
			}
			_context.ResetRule();
			_context.LevelModel.IsNeedACharacter = true;
			_context.DialogCurrentRule();
		}

		private void RecoverHP()
		{
			_context.LevelModel.HP += 2;
			_context.LevelModel.HP = Mathf.Clamp(_context.LevelModel.HP, 0, GameController.Instance.ConfigData.DefaultHP);
			_context.UpdateHp();
		}
	}
}