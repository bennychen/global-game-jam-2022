using System;

namespace Codeplay
{
	public class ElementModel : IElementComponent
	{
		public bool IsInited { get; private set; }

		public ElementModel()
		{
			_resetCommand = new Command(this, OnReset);
		}

		public void SetContext(SceneElementBase context, IElementComponents container)
		{
			_context = context;
			_components = container;
		}

		public void Init()
		{
			OnInit();
			IsInited = true;
		}

		public R GetModel<R>() where R : ElementModel
		{
			return _components.Get<R>() as R;
		}

		public bool IsActive { get { return true; } }
		public void Activate() { }
		public void Deactivate() { }

		public void Reset()
		{
			_resetCommand.Exec();
		}

		protected virtual void OnInit() { }
		protected virtual void OnReset() { }

		internal SceneElementBase _context;
		private IElementComponents _components;
		private Command _resetCommand;
	}

	public class ElementStateModel<T> : ElementModel where T : SceneElementBase
	{
		public Action<Type, Type> OnStateChange = delegate { };
		public bool PrintStateLog { get; set; }

		public Type CurrentState { get { return _currentState; } }
		public Type PreviousState { get { return _previousState; } }

		public ElementStateModel(StateMachine<T> stateMachine)
		{
			_stateMachine = stateMachine;
			_stateChangeCommand = new Command<Type>(this, ChangeState);
		}

		public void ChangeState<R>() where R : State<T>
		{
			_stateChangeCommand.Exec(typeof(R));
		}

		private void ChangeState(Type value)
		{
			if (!_currentState.Equals(value))
			{
				_previousState = _currentState;
				_currentState = value;
				ChangeStateInternal(_currentState);
				OnStateChange(_previousState, _currentState);
			}
		}

		private void ChangeStateInternal(Type newStateType)
		{
			_stateMachine.ChangeState(newStateType);

#if UNITY_EDITOR
			if (PrintStateLog)
			{
				UnityEngine.Debug.LogFormat("[{0}] changes from {1} to {2}",
								_context.ID, (_stateMachine.PreviousState != null ?
										 _stateMachine.PreviousState.ToString() : "None"),
								_stateMachine.CurrentState.ToString());
			}
#endif
		}

		private Type _currentState;
		private Type _previousState;
		private StateMachine<T> _stateMachine;
		private Command<Type> _stateChangeCommand;
	}
}
