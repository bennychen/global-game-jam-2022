
using System;
using Codeplay;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
	public class GameController : MonoSingleton<GameController>, Prime31.IObjectInspectable
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
		public StickFsm RewardButton;
		public StickFsm PenaltyButton;
		public GameObject HPRoot;
		public GameObject RuleBook;
		public Image NightMask;

		public StateMachine<GameController> GameStateMachine;

		public Sprite EndingBad;
		public Sprite EndingMed;
		public Sprite EndingGood;

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
			DialogController = gameObject.GetComponentAndCreateIfNonExist<DialogController>();
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

		[Prime31.MakeButton]
		public void ChangeToGuid()
		{
			GameStateMachine.ChangeState<GuidState>();
		}

		[Prime31.MakeButton]
		public void ChangeToGameLoop()
		{
			GameStateMachine.ChangeState<GameLoopState>();
		}

		[Prime31.MakeButton]
		public void ChangeToEnding()
		{
			GameStateMachine.ChangeState<EndingState>();
		}

		public void ChangeToCredits()
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("Credits",
					UnityEngine.SceneManagement.LoadSceneMode.Single);
		}


		private void Update()
		{
			GameStateMachine.OnUpdate(Time.deltaTime);
		}
	}
}