
using Codeplay;
using Game.Model;
using UnityEngine;
using UnityEngine.UI;

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
				if (_levelModel.EthicsButMistakeScore >= GameController.Instance.ConfigData.EthicsEndingThreshold)
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
			_context.GameEndingScene.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = 
				"于是 你成功完成了大老爷的任务\r\n顺利转世投胎去了";
			
			Camera.main.GetComponent<PlayUISound>().PlayGoodEnding();
		}

		private void EthicsEnding()
		{
			_context.GameEndingScene.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite =
					GameController.Instance.EndingMed;
			_context.GameEndingScene.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = 
				"你成功的完成了大老爷的任务\r\n可以转世投胎了\r\n此外 大老爷似乎给你留了一封信";
			Camera.main.GetComponent<PlayUISound>().PlayEthicsEnding();
		}

		private void BadEnding()
		{
			_context.GameEndingScene.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite =
					GameController.Instance.EndingBad;
			_context.GameEndingScene.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = 
				"你没能完成大老爷的嘱托\r\n下狱受罚去了";
			Camera.main.GetComponent<PlayUISound>().PlayBadEnding();
		}


		public override void OnExit()
		{
			base.OnExit();
			_context.GameEndingScene.SetActive(false);
		}
	}
}