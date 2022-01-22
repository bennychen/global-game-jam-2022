using UnityEngine;

namespace Game
{
	public class DialogController : MonoBehaviour, Prime31.IObjectInspectable
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
			if (!tutorialDialogue)
			{
				tutorialDialogue = FindObjectOfType<NpcDialogueBubble>(true);
			}
			if (!string.IsNullOrEmpty(dialog))
			{
				Debug.Log("TutorialDialog:" + dialog);
				tutorialDialogue.PopupWithAnim(dialog);
			}
			else
			{
				tutorialDialogue.Hide();
			}
		}

		[Prime31.MakeButton]
		public void HideTutorial()
		{
			TutorialDialog("");
		}

		[Prime31.MakeButton]
		public void DebugTutorial()
		{
			TutorialDialog("大人刚用了地狱签，这位李鑫将会被投入地狱，受刑后方能再次投胎做人。");
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