
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
				"Tada~你达到了这个游戏的真结局！恭喜你！\r\n并没有什么大老爷，这是我们为打到这里的有缘人留下的一封信。\r\n二元对立的方法解决问题看起来简单有效，可一旦问题变得复杂，\r\n做出决断的过程就会变得非常折磨人。\r\n然而不管问题多么复杂，在生活中我们迫于形势，\r\n却不得不常常这么做。\r\n这种折磨和苦恼正是我们想通过这个游戏表达的东西，\r\n如果我们成功用这个游戏把烦恼也带给了你，对不起！\r\n";
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