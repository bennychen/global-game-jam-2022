
using Codeplay;
using Game.Model;
using UnityEngine;

namespace Game
{
	public class EndingState : State<GameController>
	{
		private LevelModel _levelModel = GameController.Instance.GameLoopController.LevelModel;
		public override void OnEnter()
		{
			base.OnEnter();
			_context.GameEndingScene.SetActive(true);
			Clearing();
		}

		private void Clearing()
		{
			if (_levelModel.HP <= 0)
			{
				BadEnding();
			}
			else
			{
				if (_levelModel.EthicsButMistakeScore > GameController.Instance.ConfigData.EthicsEndingThreshold)
				{
					EthicsEnding();
				}
				else
				{
					GoodEnding();
				}
			}
		}

		private void GoodEnding()
		{
			_context.GameEndingScene.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite =
					GameController.Instance.EndingGood;
			GameController.Instance.SendMessage("NpcDialog", "GoodEnding");
			Debug.LogError("GoodEnding");
			Camera.main.GetComponent<PlayUISound>().PlayGoodEnding();
		}

		private void EthicsEnding()
		{
			_context.GameEndingScene.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite =
					GameController.Instance.EndingMed;
			GameController.Instance.SendMessage("NpcDialog", "EthicsEnding");
			Debug.LogError("EthicsEnding");
			Camera.main.GetComponent<PlayUISound>().PlayEthicsEnding();
		}

		private void BadEnding()
		{
			_context.GameEndingScene.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite =
					GameController.Instance.EndingBad;
			GameController.Instance.SendMessage("NpcDialog", "BadEnding");
			Debug.LogError("BadEnding");
			Camera.main.GetComponent<PlayUISound>().PlayBadEnding();
		}


		public override void OnExit()
		{
			base.OnExit();
			_context.GameEndingScene.SetActive(false);
		}
	}
}