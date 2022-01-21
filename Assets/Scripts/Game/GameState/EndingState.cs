
using Codeplay;
using Game.Model;
using UnityEngine;

namespace Game
{
    public class EndingState : State<GameController>
    {
        private LevelModel _levelModel = GameController.Instance.GameLoopController.LevelModel;
        public override void OnEnter()
        {
            base.OnEnter();
            _context.GameEndingScene.SetActive(true);
            Clearing();
        }

        private void Clearing()
        {
            if (_levelModel.HP <= 0)
            {
                BadEnding();
            }
            else
            {
                if (_levelModel.EthicsButMistakeScore > GameController.Instance.ConfigData.EthicsEndingThreshold)
                {
                    EthicsEnding();
                }
                else
                {
                    GoodEnding();
                }
            }
        }

        private void GoodEnding()
        {
            GameController.Instance.SendMessage("NpcDialog", "GoodEnding");
            Debug.LogError("GoodEnding");
        }

        private void EthicsEnding()
        {
            GameController.Instance.SendMessage("NpcDialog", "EthicsEnding");
            Debug.LogError("EthicsEnding");
        }

        private void BadEnding()
        {
            GameController.Instance.SendMessage("NpcDialog", "BadEnding");
            Debug.LogError("BadEnding");
        }


        public override void OnExit()
        {
            base.OnExit();
            _context.GameEndingScene.SetActive(false);
        }
    }
}