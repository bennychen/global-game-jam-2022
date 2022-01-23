using Codeplay;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
	public class CharacterEnterState : State<GameLoopController>
	{
		public override void OnEnter()
		{
			base.OnEnter();
			var characterIndex = PickCharacter();
			_context.LevelModel.CurrentCharacterData =
					GameController.Instance.ConfigData.AllCharacter[characterIndex];
			_context.LevelModel.CurrentCharacter = LoadCharacter();
			RefreshDeathBook();
			GameController.Instance.RewardButton.ShowStick();
			GameController.Instance.PenaltyButton.ShowStick();

			Camera.main.GetComponent<PlayUISound>().PlayNextPage();

			_stateMachine.ChangeState<CharacterAwaitState>();
		}

		private void RefreshDeathBook()
		{
			var AgeOfDeathDialogGO = GameObject.Find("AgeOfDeathDialog");
			AgeOfDeathDialogGO.transform.Find("Value").GetComponent<Text>().text = _context.LevelModel.CurrentCharacterData.AgeOfDeath.ToString();
			var DeadReasonDialogGO = GameObject.Find("DeadReasonDialog");
			DeadReasonDialogGO.transform.Find("Value").GetComponent<Text>().text = _context.LevelModel.CurrentCharacterData.DeadReason;
			var NumberOfChildDialogGO = GameObject.Find("NumberOfChildDialog");
			NumberOfChildDialogGO.transform.Find("Value").GetComponent<Text>().text = _context.LevelModel.CurrentCharacterData.NumberOfChild.ToString();
			var NumberOfKilledDialogGO = GameObject.Find("NumberOfKilledDialog");
			NumberOfKilledDialogGO.transform.Find("Value").GetComponent<Text>().text = _context.LevelModel.CurrentCharacterData.NumberOfKilled.ToString();
			var CrimeDialogGO = GameObject.Find("CrimeDialog");
			CrimeDialogGO.transform.Find("Value").GetComponent<Text>().text = _context.LevelModel.CurrentCharacterData.Crime;
		}

		private int PickCharacter()
		{
			//Debug.Log("index:" + _context.LevelModel.CurrentCharacterIndex + " count:" + _context.LevelModel.CurrentLevel.CharacterList.Count);
			return _context.LevelModel.CharacterList[_context.LevelModel.CurrentDay][_context.LevelModel.CurrentCharacterIndex];
		}

		private Character LoadCharacter()
		{
			var characterGameObject = GameObject.FindObjectOfType<Character>(true).gameObject;
			if (!characterGameObject)
			{
				Debug.Log("not found character game object, create a new one");
				characterGameObject = new GameObject();
			}
			characterGameObject.gameObject.SetActive(true);
			characterGameObject.name = _context.LevelModel.CurrentDay + "-" + _context.LevelModel.CurrentCharacterIndex + " name:" + _context.LevelModel.CurrentCharacterData.Name;
			Debug.Log("Load:" + characterGameObject.name);
			var character = characterGameObject.GetComponentAndCreateIfNonExist<Character>();
			character.ResetDissolve();
			var characterData = _context.LevelModel.CurrentCharacterData;
			if (characterData != null)
			{
				character.ChangeSkin(characterData.Skin);
			}
			characterGameObject.transform.SetParent(GameController.Instance.CharacterSpawn.transform);
			return character;
		}

		public override void OnExit()
		{
			base.OnExit();
		}
	}
}