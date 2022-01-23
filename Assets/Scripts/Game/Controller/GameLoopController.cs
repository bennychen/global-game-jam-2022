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
		private int _randomIndexDay1;

		private void Awake()
		{
			CreateGameStateMachine();
			ResetLevelModel();
			RegisterEvent();
			ResetHP();
			
		}

		public void CreateGameStateMachine()
		{
			GameLoopStateMachine = new StateMachine<GameLoopController>(this);
			GameLoopStateMachine.AddState(new FirstEnterGameState());
			GameLoopStateMachine.AddState(new CharacterEnterState());
			GameLoopStateMachine.AddState(new CharacterAwaitState());
			GameLoopStateMachine.AddState(new CharacterLeaveState());
			GameLoopStateMachine.AddState(new ChangeNextDayState());
			
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
			GenerateCharacterList();
		}

		private void GenerateCharacterList()
		{
			//CharacterList
			/**
			 * A组-Complexity==1
				B组-Complexity==2 AND Conflict==Neutral
				C组-Complexity==2 AND （Conflict==GoodChaos OR Conflict==EvilOrder）
			 */
			var configData = GameController.Instance.ConfigData;
			List<int> groupA = new List<int>();
			List<int> groupB = new List<int>();
			List<int> groupC = new List<int>();
			for (int i = 0; i < configData.AllCharacter.Count; i++)
			{
				var characterData = configData.AllCharacter[i];
				if (characterData.Complexity == 1)
				{
					groupA.Add(i);
				}
				else if (characterData.Complexity == 2 && characterData.Conflict == ConflictType.Neutral)
				{
					groupB.Add(i);
				}
				else if (characterData.Complexity == 2 && (characterData.Conflict == ConflictType.GoodChaos || characterData.Conflict == ConflictType.EvilOrder))
				{
					groupC.Add(i);
				}
			}
			
			LevelModel.CharacterList.Clear();
			foreach (var levelData in configData.AllLevel)
			{
				for (int i = 0; i < levelData.ChapterList.Count; i++)
				{
					var chapterData = levelData.ChapterList[i];
					LevelModel.CharacterList.Add(new List<int>());
					foreach (var groupData in chapterData.groupList)
					{
						if (groupData.Group == GroupType.A)
						{
							for (int j = 0; j < groupData.characterAmount; j++)
							{
								int randomIndex = Random.Range(0, groupA.Count);
								LevelModel.CharacterList[i].Add(groupA[randomIndex]);
								groupA.RemoveAt(randomIndex);
							}
						}
						else if (groupData.Group == GroupType.B)
						{
							for (int j = 0; j < groupData.characterAmount; j++)
							{
								int randomIndex = Random.Range(0, groupB.Count);
								LevelModel.CharacterList[i].Add(groupB[randomIndex]);
								groupB.RemoveAt(randomIndex);
							}
						}
						else if (groupData.Group == GroupType.C)
						{
							for (int j = 0; j < groupData.characterAmount; j++)
							{
								int randomIndex = Random.Range(0, groupC.Count);
								LevelModel.CharacterList[i].Add(groupC[randomIndex]);
								groupC.RemoveAt(randomIndex);
							}
						}
					}
					LevelModel.CharacterList[i].Sort((a, b)=> 1 - 2 * Random.Range(0, 1));
					Debug.Log("day:"+i + "  character count:" + LevelModel.CharacterList[i].Count);
				}
			}
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
			var playerChoice = toHeaven ? Destination.Heaven : Destination.Hell;
			CharacterData character = LevelModel.CurrentCharacterData;
			var meetRule = MeetRule(character, playerChoice);
			// var dest = LevelModel.CurrentRule.
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
			StartCoroutine(this.FadeOutCurrentCharacter());
		}

		private IEnumerator FadeOutCurrentCharacter()
		{
			if (!LevelModel.CurrentJudgeToHeaven)
			{
				LevelModel.CurrentCharacter.DissolveOut();
				yield return new WaitForSeconds(1.5f);
			}
			else
			{
				LevelModel.CurrentCharacter.FadeOut();
				yield return new WaitForSeconds(1.5f);
			}
			this.CheckFirstGuide();
		}

		private void CheckFirstGuide()
		{
			if (LevelModel.IsNeverUseReward && LevelModel.CurrentJudgeToHeaven)
			{
				LevelModel.IsNeverUseReward = false;
				GameController.Instance.DialogController.TutorialDialog(
						string.Format(GameController.Instance.ConfigData.GetDialogByKey("first_reward"),
								LevelModel.CurrentCharacterData.Name), this.ChangeNextCharacter);
				return;
			}

			if (LevelModel.IsNeverUsePenalty && !LevelModel.CurrentJudgeToHeaven)
			{
				LevelModel.IsNeverUsePenalty = false;
				GameController.Instance.DialogController.TutorialDialog(
						string.Format(GameController.Instance.ConfigData.GetDialogByKey("first_penalty"),
								LevelModel.CurrentCharacterData.Name), this.ChangeNextCharacter);
				return;
			}

			if (LevelModel.IsNeverMistake && !LevelModel.CurrentJudgeCorrect)
			{
				LevelModel.IsNeverMistake = false;
				GameController.Instance.DialogController.TutorialDialog(
						string.Format(GameController.Instance.ConfigData.GetDialogByKey("first_mistake"),
								LevelModel.CurrentCharacterData.Name), this.ChangeNextCharacter);
				return;
			}

			ChangeNextCharacter();
		}

		public void ChangeNextCharacter()
		{
			Game.GameController.Instance.DialogController.npcDialogue.Reset();
			LevelModel.CurrentCharacterIndex++;
			if (LevelModel.CurrentCharacterIndex >= LevelModel.CharacterList[LevelModel.CurrentDay].Count)
			{
				GameLoopStateMachine.ChangeState<ChangeNextDayState>();
			}
			else
			{
				GameLoopStateMachine.ChangeState<CharacterEnterState>();
			}
		}

		private bool MeetRule(CharacterData character, Destination destination)
		{
			var result = true;
			foreach (var currentRule in LevelModel.CurrentRule)
			{
				result &= currentRule.MeetChoice(character, destination);
			}
			return result;
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
			if (LevelModel.CurrentDay == 1)
			{
				_randomIndexDay1 = Random.Range(2, GameController.Instance.ConfigData.AllRule.Count);
				LevelModel.CurrentRule.Add(GameController.Instance.ConfigData.AllRule[_randomIndexDay1]);
			}

			int randomIndexDay2 = -1;
			if (LevelModel.CurrentDay == 2)
			{
				var bFound = false;
				while (!bFound)
				{
					randomIndexDay2 = Random.Range(2, GameController.Instance.ConfigData.AllRule.Count);
					if (randomIndexDay2 != _randomIndexDay1)
					{
						bFound = true;
					}
				}

				if (randomIndexDay2 >= 0 && GameController.Instance.ConfigData.AllRule.Count > randomIndexDay2)
					LevelModel.CurrentRule.Add(GameController.Instance.ConfigData.AllRule[randomIndexDay2]);
			}

			foreach (var rule in LevelModel.CurrentRule)
			{
				Debug.Log("current rule:" + rule.Description);
			}
		}
	}
}