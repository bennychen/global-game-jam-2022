
using Codeplay;
using UnityEngine;

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
            return _context.LevelModel.CurrentLevel.CharacterList[_context.LevelModel.CurrentCharacterIndex];
        }

        private Character LoadCharacter()
        {
            GameObject go = new GameObject();
            go.name = "character-" + _context.LevelModel.CurrentDay;
            var sprite = go.AddComponent<SpriteRenderer>();
            sprite.sprite = _context.LevelModel.CurrentCharacterData.MainSprite;
            var character = go.AddComponent<Character>();
            return character;
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}