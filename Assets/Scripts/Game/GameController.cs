
using System;
using Codeplay;
using JumpExample;
using UnityEngine;

namespace Game
{
    public class GameController : MonoSingleton<GameController>
    {

        public ConfigData ConfigData;
        public GuideController GuideController = new GuideController();
        public GameLoopController GameLoopController = new GameLoopController();
        public PlayerController PlayerController = new PlayerController();
        public CharacterController CharacterController = new CharacterController();


        public GameObject GameStartScene;
        public GameObject GameGuideScene;
        public GameObject GameLoopScene;
        public GameObject GameEndingScene;
        public GameObject CharacterSpawn;
        
        public StateMachine<GameController> GameStateMachine;

        public void OnAwake()
        {
            CreateGameStateMachine();
        }


        public void CreateGameStateMachine()
        {
            GameStateMachine = new StateMachine<GameController>(this);
            GameStateMachine.AddState(new GameStartState());
            GameStateMachine.AddState(new GameLoopState());
            GameStateMachine.AddState(new GuidState());
            GameStateMachine.AddState(new EndingState());
            
            GameStateMachine.ChangeState<GameStartState>();
            // GameStateMachine.OnStateChanged += OnGameStateChanged;
        }

        public void ChangeToGuid()
        {
            GameStateMachine.ChangeState<GuidState>();
        }
        
        public void ChangeToGameLoop()
        {
            GameStateMachine.ChangeState<GameLoopState>();
        }
        
        public void ChangeToEnding()
        {
            GameStateMachine.ChangeState<EndingState>();
        }
    }
}