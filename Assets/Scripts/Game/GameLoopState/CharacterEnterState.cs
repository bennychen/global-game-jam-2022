
using Codeplay;
using UnityEngine;

namespace Game
{
    public class CharacterEnterState : State<GameLoopController>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            _context.LevelModel.CurrentCharacterData =
                GameController.Instance.ConfigData.AllCharacter[_context.LevelModel.CurrentDay];
            _context.LevelModel.CurrentCharacter = LoadCharacter();
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