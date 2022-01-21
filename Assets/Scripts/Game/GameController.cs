﻿
using System;
using Codeplay;
using JumpExample;
using UnityEngine;

namespace Game
{
    public class GameController : MonoSingleton<GameController>
    {

        public ConfigData ConfigData;
        [HideInInspector]
        public GuideController GuideController;
        [HideInInspector]
        public GameLoopController GameLoopController;
        [HideInInspector]
        public PlayerController PlayerController;
        [HideInInspector]
        public DialogController DialogController;

        public GameObject GameStartScene;
        public GameObject GameGuideScene;
        public GameObject GameLoopScene;
        public GameObject GameEndingScene;
        public GameObject CharacterSpawn;
        
        public StateMachine<GameController> GameStateMachine;

        protected override void OnAwake()
        {
            CreateController();
            CreateGameStateMachine();
        }

        private void CreateController()
        {
            GuideController = gameObject.AddComponent<GuideController>();
            GameLoopController = gameObject.AddComponent<GameLoopController>();
            PlayerController = gameObject.AddComponent<PlayerController>();
            DialogController = gameObject.AddComponent<DialogController>();
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