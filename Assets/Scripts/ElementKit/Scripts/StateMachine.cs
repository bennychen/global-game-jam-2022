using System;
using System.Collections.Generic;
using UnityEngine;

namespace Codeplay
{
	public sealed class StateMachine<T>
	{
#pragma warning disable
		public event Action OnStateChanged = delegate { };
#pragma warning restore

		public State<T> CurrentState { get { return _currentState; } }
		public State<T> PreviousState { get { return _previousState; } }

		public StateMachine(T context)
		{
			_registeredStates = new Dictionary<Type, State<T>>();
			_context = context;
		}

		public bool IsInState<R>() where R : State<T>
		{
			return _currentState != null && _currentState.GetType() == typeof(R);
		}

		public void AddState(State<T> state)
		{
			state.Init(this, _context);
			_registeredStates[state.GetType()] = state;
		}

		public void OnUpdate(float deltaTime)
		{
			if (_currentState != null)
			{
				_currentState.OnUpdate(deltaTime);
			}
		}

		public bool TryGetState<R>(out State<T> state) where R : State<T>
		{
			return _registeredStates.TryGetValue(typeof(R), out state);
		}

		public R GetState<R>() where R : State<T>
		{
			State<T> state = null;
			_registeredStates.TryGetValue(typeof(R), out state);
			return state as R;
		}

		public bool ContainsState<R>() where R : State<T>
		{
			Type stateType = typeof(R);
			return _registeredStates.ContainsKey(stateType);
		}

		public State<T> RevertToPreviousState()
		{
			if (PreviousState != null)
			{
				if (_currentState != null)
				{
					_currentState.OnExit();
				}

				State<T> oldState = _previousState;
				_previousState = _currentState;
				_currentState = oldState;
				_currentState.OnEnter();

				if (OnStateChanged != null)
				{
					OnStateChanged();
				}
			}

			return _currentState;
		}

		public void ChangeState<R>() where R : State<T>
		{
			var newType = typeof(R);
			ChangeState(newType);
		}

		public void ChangeState(Type newType)
		{
			Debug.Log("change to:" + newType.ToString());
			if (_currentState != null && _currentState.GetType() == newType)
			{
				return;
			}

			if (_currentState != null)
			{
				_currentState.OnExit();
			}

#if UNITY_EDITOR
			if (!_registeredStates.ContainsKey(newType))
			{
				throw new Exception("ElementStateMachine::ChangeState failed - cannot find state [" + typeof(T) +
						"] from state machine");
			}
#endif

			_previousState = _currentState;
			_currentState = _registeredStates[newType];
			_currentState.OnEnter();

			if (OnStateChanged != null)
			{
				OnStateChanged();
			}
		}

		private T _context;

		private Dictionary<Type, State<T>> _registeredStates;
		private State<T> _currentState;
		private State<T> _previousState;
	}
}