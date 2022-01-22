using System.Collections;
using System.Collections.Generic;
using System.Text;
using Codeplay;
using Game.Model;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game
{
	public class GameLoopController : MonoBehaviour
	{
		public StateMachine<GameLoopController> GameLoopStateMachine;

		public LevelModel LevelModel = new LevelModel();

		public List<Transform> HPList = new List<Transform>();

		private void Awake()
		{
			CreateGameStateMachine();
		}

		public void CreateGameStateMachine()
		{
			GameLoopStateMachine = new StateMachine<GameLoopController>(this);
			GameLoopStateMachine.AddState(new FirstEnterGameState());
			GameLoopStateMachine.AddState(new CharacterEnterState());
			GameLoopStateMachine.AddState(new CharacterAwaitState());
			GameLoopStateMachine.AddState(new CharacterLeaveState());
			GameLoopStateMachine.AddState(new ChangeNextDayState());

			//ChangeToFirstEnterGame();
			ResetLevelModel();
			RegisterEvent();
			ResetHP();
			// GameStateMachine.OnStateChanged += OnGameStateChanged;
		}

		private void ResetHP()
		{
			for (int i = 0; i < GameController.Instance.HPRoot.transform.childCount; i++)
			{
				HPList.Add(GameController.Instance.HPRoot.transform.GetChild(i));
			}

			for (int i = GameController.Instance.ConfigData.DefaultHP; i < HPList.Count; i++)
			{
				HPList[i].gameObject.SetActive(false);
			}
		}

		public void UpdateHp()
		{

			for (int i = LevelModel.HP; i < GameController.Instance.ConfigData.DefaultHP; i++)
			{
				HPList[i].gameObject.SetActive(false);
			}
		}

		public void PopupRuleBook()
		{
			StringBuilder buff = new StringBuilder();
			foreach (var rule in LevelModel.CurrentRule)
			{
				buff.Append(rule.Description);
				buff.Append("\r\n");
			}

			var currentRule = buff.ToString();
			GameController.Instance.RuleBook.transform.GetChild(0).GetComponent<Text>().text = currentRule;
			GameController.Instance.RuleBook.SetActive(true);
		}

		public void CloseRuleBook()
		{
			GameController.Instance.RuleBook.SetActive(false);
			if (LevelModel.IsNeedACharacter)
			{
				LevelModel.IsNeedACharacter = false;
				GameLoopStateMachine.ChangeState<CharacterEnterState>();
			}
		}

		private void RegisterEvent()
		{
			GameController.Instance.RewardButton.OnDroppedOut += PlayerJudgeToHeaven;
			GameController.Instance.PenaltyButton.OnDroppedOut += PlayerJudgeToHell;
		}

		private void ResetLevelModel()
		{
			LevelModel.HP = GameController.Instance.ConfigData.DefaultHP;

		}

		public void ChangeToFirstEnterGame()
		{
			GameLoopStateMachine.ChangeState<FirstEnterGameState>();
		}

		public void ChangeToCharacterEnter()
		{
			GameLoopStateMachine.ChangeState<CharacterEnterState>();
		}

		public void ChangeToAwait()
		{
			GameLoopStateMachine.ChangeState<CharacterAwaitState>();
		}

		public void ChangeToNextDay()
		{
			GameLoopStateMachine.ChangeState<ChangeNextDayState>();
		}

		public void PlayerJudgeToHeaven()
		{
			PlayerJudge(true);
		}

		public void PlayerJudgeToHell()
		{
			PlayerJudge(false);
		}

		public void PlayerJudge(bool toHeaven)
		{
			CharacterData character = LevelModel.CurrentCharacterData;
			var meetRule = MeetRule(character, toHeaven);
			LevelModel.CurrentJudgeCorrect = meetRule;
			if (meetRule)
			{
				Debug.Log("正确");
				LevelModel.CorrectScore++;
				if (toHeaven && character.Ethics == EthicsType.Evil)
				{
					Debug.Log("坏人去天堂，秩序+1");
					LevelModel.CorrectButNotEthicsScore++;
				}
				if (!toHeaven && character.Ethics == EthicsType.Good)
				{
					Debug.Log("好人去地狱，秩序+1");
					LevelModel.CorrectButNotEthicsScore++;
				}
			}
			else
			{
				Debug.Log("错误");
				LevelModel.CorrectScore--;
				LevelModel.HP--;
				UpdateHp();
				if (toHeaven && character.Ethics == EthicsType.Good)
				{
					Debug.Log("好人去天堂，道德+1");
					LevelModel.EthicsButMistakeScore++;
				}
				if (!toHeaven && character.Ethics == EthicsType.Evil)
				{
					Debug.Log("坏人去地狱，道德+1");
					LevelModel.EthicsButMistakeScore++;
				}

			}

			if (toHeaven)
			{
				GameController.Instance.SendMessage("CharacterDialog", character.RewardDialog);
			}
			else
			{
				GameController.Instance.SendMessage("CharacterDialog", character.PenaltyDialog);
			}

			LevelModel.CurrentJudgeToHeaven = toHeaven;

			GameLoopStateMachine.ChangeState<CharacterLeaveState>();
			CheckFirstGuide();
		}

		private void CheckFirstGuide()
		{

			if (LevelModel.IsNeverUseReward && LevelModel.CurrentJudgeToHeaven)
			{
				LevelModel.IsNeverUseReward = false;
				GameController.Instance.DialogController.TutorialDialog(
						string.Format(GameController.Instance.ConfigData.GetDialogByKey("first_reward"),
								LevelModel.CurrentCharacterData.Name), this.ChangeNextCharacter);
			}

			if (LevelModel.IsNeverUsePenalty && !LevelModel.CurrentJudgeToHeaven)
			{
				LevelModel.IsNeverUsePenalty = false;
				GameController.Instance.DialogController.TutorialDialog(
						string.Format(GameController.Instance.ConfigData.GetDialogByKey("first_penalty"),
								LevelModel.CurrentCharacterData.Name), this.ChangeNextCharacter);
			}

			if (LevelModel.IsNeverMistake && !LevelModel.CurrentJudgeCorrect)
			{
				LevelModel.IsNeverMistake = false;
				GameController.Instance.DialogController.TutorialDialog(
						string.Format(GameController.Instance.ConfigData.GetDialogByKey("first_mistake"),
								LevelModel.CurrentCharacterData.Name), this.ChangeNextCharacter);
			}
		}


		public void ChangeNextCharacter()
		{
			LevelModel.CurrentCharacterIndex++;
			if (LevelModel.CurrentCharacterIndex >= LevelModel.CurrentLevel.CharacterList.Count)
			{
				GameLoopStateMachine.ChangeState<ChangeNextDayState>();
			}
			else
			{
				GameLoopStateMachine.ChangeState<CharacterEnterState>();
			}
		}

		private bool MeetRule(CharacterData character, bool toHeaven)
		{
			var result = true;
			foreach (var currentRule in LevelModel.CurrentRule)
			{
				result &= currentRule.IsMeet(character);
			}
			return result == toHeaven;
		}

		public void DialogCurrentRule()
		{
			if (LevelModel.CurrentDay <= 0)
			{
				return;
			}

			PopupRuleBook();
		}

		public void ResetRule()
		{
			LevelModel.CurrentLevel =
					GameController.Instance.ConfigData.AllLevel[LevelModel.CurrentDay];
			LevelModel.CurrentRule.Clear();
			foreach (var ruleID in GameController.Instance.ConfigData.DefaultRule)
			{
				foreach (var rule in GameController.Instance.ConfigData.AllRule)
				{
					if (rule.ID == ruleID)
					{
						LevelModel.CurrentRule.Add(rule);
					}
				}
			}
			if (LevelModel.CurrentDay > 0)
			{
				var randomIndex = Random.Range(0, GameController.Instance.ConfigData.AllRule.Count);
				LevelModel.CurrentRule.Add(GameController.Instance.ConfigData.AllRule[randomIndex]);
			}

			foreach (var rule in LevelModel.CurrentRule)
			{
				Debug.Log("current rule:" + rule.Description);
			}
		}
	}
}