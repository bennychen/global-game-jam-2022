using System.Collections.Generic;
using UnityEngine;

namespace Codeplay
{
	public class ElementComponents<T> : IElementComponents where T : SceneElementBase
	{
		public T Context { get { return _context; } }

		public IEnumerable<IElementComponent> Components { get { return _components.Values; } }

		public bool IsInited { get { return _components != null; } }

		public ElementComponents(SceneElementBase context)
		{
			_components = new Dictionary<System.Type, IElementComponent>();
			_context = context as T;

			OnInit();
		}

		public void Add(IElementComponent component)
		{
			if (component != null)
			{
				component.SetContext(_context, this);
				_components[component.GetType()] = component;
			}
			else
			{
				Debug.LogError("Tried to add a null view for Element type [" + _context.GetType() + "].");
			}
		}

		public void Remove(IElementComponent component)
		{
			if (component != null)
			{
				_components.Remove(component.GetType());
			}
			else
			{
				Debug.LogError("Tried to remove a null component.");
			}
		}

		public R Get<R>() where R : class, IElementComponent
		{
			System.Type viewType = typeof(R);

#if UNITY_EDITOR
			if (!_components.ContainsKey(viewType))
			{
				var error = GetType() + ": view " +
						viewType + " does not exist. Did you forget to add it by calling Add?";
				Debug.LogError(error);
				throw new System.Exception(error);
			}
#endif

			return _components[viewType] as R;
		}

		public bool Contains<R>() where R : IElementComponent
		{
			System.Type viewType = typeof(R);
			return _components.ContainsKey(viewType);
		}

		protected virtual void OnInit() { }

		private T _context;
		private Dictionary<System.Type, IElementComponent> _components;
	}
}
