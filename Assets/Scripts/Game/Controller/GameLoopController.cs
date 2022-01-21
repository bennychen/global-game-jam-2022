
using System.Data;
using Codeplay;
using Game.Model;
using UnityEngine;

namespace Game
{
    public class GameLoopController
    {
        public StateMachine<GameLoopController> GameLoopStateMachine;

        public LevelModel LevelModel = new LevelModel();
        

        public void CreateGameStateMachine()
        {
            GameLoopStateMachine = new StateMachine<GameLoopController>(this);
            GameLoopStateMachine.AddState(new CharacterEnterState());
            GameLoopStateMachine.AddState(new CharacterAwaitState());
            GameLoopStateMachine.AddState(new ChangeNextDayState());

            ChangeToCharacterEnter();
            // GameStateMachine.OnStateChanged += OnGameStateChanged;
        }
        
        public void ChangeToCharacterEnter()
        {
            GameLoopStateMachine.ChangeState<CharacterEnterState>();
        }
        
        public void ChangeToAwait()
        {
            GameLoopStateMachine.ChangeState<CharacterAwaitState>();
        }
        
        public void ChangeToNextDay()
        {
            GameLoopStateMachine.ChangeState<ChangeNextDayState>();
        }
    }
}