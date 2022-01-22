
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


			_stateMachine.ChangeState<CharacterAwaitState>();
		}

		private int PickCharacter()
		{
			//Debug.Log("index:" + _context.LevelModel.CurrentCharacterIndex + " count:" + _context.LevelModel.CurrentLevel.CharacterList.Count);
			return _context.LevelModel.CurrentLevel.CharacterList[_context.LevelModel.CurrentCharacterIndex];
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
			var characterData = _context.LevelModel.CurrentCharacterData;
			if (characterData != null)
			{
				character.changeSkin(characterData.Skin);
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