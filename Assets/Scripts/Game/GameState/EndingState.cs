
using Codeplay;
using Game.Model;

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
            throw new System.NotImplementedException();
        }

        private void EthicsEnding()
        {
            throw new System.NotImplementedException();
        }

        private void BadEnding()
        {
            throw new System.NotImplementedException();
        }


        public override void OnExit()
        {
            base.OnExit();
            _context.GameEndingScene.SetActive(false);
        }
    }
}