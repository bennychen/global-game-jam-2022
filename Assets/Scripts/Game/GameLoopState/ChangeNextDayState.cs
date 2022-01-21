
using System.Text;
using Codeplay;
using UnityEngine;

namespace Game
{
    public class ChangeNextDayState : State<GameLoopController>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            GameObject.Destroy(_context.LevelModel.CurrentCharacter.gameObject);
            _context.LevelModel.CurrentDay++;
            _context.ResetRule();
            _context.DialogCurrentRule();
        }



        public override void OnExit()
        {
            base.OnExit();
        }
    }
}