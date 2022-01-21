
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
            GameObject go = new GameObject();
            go.name = _context.LevelModel.CurrentDay + "-" + _context.LevelModel.CurrentCharacterIndex + " name:" + _context.LevelModel.CurrentCharacterData.Name;
            GameObject.Find("CharacterInfo").GetComponent<Text>().text = go.name;
            var sprite = go.AddComponent<SpriteRenderer>();
            sprite.sprite = _context.LevelModel.CurrentCharacterData.MainSprite;
            var character = go.AddComponent<Character>();
            go.transform.SetParent(GameController.Instance.CharacterSpawn.transform);
            return character;
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}