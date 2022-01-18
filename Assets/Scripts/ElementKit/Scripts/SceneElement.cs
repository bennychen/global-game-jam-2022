using System.Collections.Generic;
using UnityEngine;

namespace Codeplay
{
	public abstract class SceneElement<T> : SceneElementBase
			where T : SceneElementBase
	{
		public ElementStateModel<T> StateModel { get { return _stateModel; } }
		public ElementComponents<T> Models { get { return _models; } }
		public ElementComponents<T> Controllers { get { return _controllers; } }
		public ElementComponents<T> Views { get { return _views; } }

		public override void Init(object data)
		{
			if (transform.parent != null)
			{
				_parent = transform.parent.GetComponent<SceneElementBase>();
			}

			_stateMachine = new StateMachine<T>(this as T);
			_stateModel = new ElementStateModel<T>(_stateMachine);

			_models = new ElementComponents<T>(this as T);
			_controllers = new ElementComponents<T>(this as T);
			_views = new ElementComponents<T>(this as T);
			if (_viewsNode != null && !_viewsNode.IsInited)
			{
				_viewsNode.Init(_views);
			}
			OnInit(data);
			foreach (var model in _models.Components)
			{
				model.Init();
			}
			foreach (var controller in _controllers.Components)
			{
				controller.Init();
			}
			foreach (var view in _views.Components)
			{
				view.Init();
			}
		}

		public void ResetAllModels()
		{
			IEnumerable<IElementComponent> models = _models.Components;
			foreach (var model in models)
			{
				(model as ElementModel).Reset();
			}
		}

		public R CreateViewsNodeFromPrefab<R>(R prefab) where R : ElementComponentsNode
		{
			if (prefab != null)
			{
				_viewsNode = Instantiate(prefab);
				if (_viewsNode != null && !_viewsNode.IsInited)
				{
					_viewsNode.Init(_views);
				}
			}
			else
			{
				Debug.LogError("SceneElement::CreateViewsFromPrefab - Prefab is null.");
			}
			return _viewsNode as R;
		}

		public void AddState(State<T> state)
		{
			_stateMachine.AddState(state);
		}

		public void ChangeState<R>() where R : State<T>
		{
			_stateMachine.ChangeState<R>();
		}

		public R GetModel<R>() where R : class, IElementComponent
		{
			return _models.Get<R>();
		}

		public R GetView<R>() where R : class, IElementComponent
		{
			return _views.Get<R>();
		}

		public R GetController<R>() where R : class, IElementComponent
		{
			return _controllers.Get<R>();
		}

		protected abstract void OnInit(object data);

		[SerializeField]
		protected ElementComponentsNode _viewsNode;

		protected ElementComponents<T> _views;
		protected ElementComponents<T> _controllers;
		protected ElementComponents<T> _models;

		private StateMachine<T> _stateMachine;
		protected ElementStateModel<T> _stateModel;
	}
}
