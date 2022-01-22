﻿using UnityEngine;

namespace Game
{
	public class DialogController : MonoBehaviour
	{
		public NpcDialogue npcDialogue;
		public NpcDialogueBubble tutorialDialogue;

		public void NpcDialog(string dialog)
		{
			Debug.Log("dialog:" + dialog);
			if (!npcDialogue)
			{
				npcDialogue = FindObjectOfType<NpcDialogue>(true);
			}
			if (npcDialogue)
			{
				npcDialogue.PopupDialogue(dialog);
			}
		}

		public void CharacterDialog(string dialog)
		{
			Debug.Log("CharacterDialog:" + dialog);
			if (!string.IsNullOrEmpty(dialog))
			{
				NpcDialog(dialog);
			}

			//GameObject.Find("TestCharDialog").GetComponent<Text>().text = dialog;
		}

		public void TutorialDialog(string dialog)
		{
			if (!string.IsNullOrEmpty(dialog))
			{
				tutorialDialogue.gameObject.SetActive(true);
				tutorialDialogue.text.text = dialog;
			}
			else
			{
				tutorialDialogue.gameObject.SetActive(false);
			}
		}

		public void PopupSummaryDialog()
		{
			CharacterDialog(GameController.Instance.GameLoopController.LevelModel.CurrentCharacterData.SummaryDialog);
		}
		public void PopupAgeOfDeathDialog()
		{
			CharacterDialog(GameController.Instance.GameLoopController.LevelModel.CurrentCharacterData.AgeOfDeathDialog);
		}
		public void PopupDeadReasonDialog()
		{
			CharacterDialog(GameController.Instance.GameLoopController.LevelModel.CurrentCharacterData.DeadReasonDialog);
		}
		public void PopupNumberOfChildDialog()
		{
			CharacterDialog(GameController.Instance.GameLoopController.LevelModel.CurrentCharacterData.NumberOfChildDialog);
		}
		public void PopupNumberOfKilledDialog()
		{
			CharacterDialog(GameController.Instance.GameLoopController.LevelModel.CurrentCharacterData.NumberOfKilledDialog);
		}
		public void PopupCrimeDialog()
		{
			CharacterDialog(GameController.Instance.GameLoopController.LevelModel.CurrentCharacterData.CrimeDialog);
		}
		public void PopupRewardDialog()
		{
			CharacterDialog(GameController.Instance.GameLoopController.LevelModel.CurrentCharacterData.RewardDialog);
		}
		public void PopupPenaltyDialog()
		{
			CharacterDialog(GameController.Instance.GameLoopController.LevelModel.CurrentCharacterData.PenaltyDialog);
		}
	}

}