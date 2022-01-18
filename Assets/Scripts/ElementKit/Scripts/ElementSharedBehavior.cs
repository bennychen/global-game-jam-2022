using UnityEngine;

namespace Codeplay
{
	public class ElementSharedBehavior : ElementBehaviorBase, IElementComponent
	{
		public bool IsInited { get; private set; }

		public SceneElementBase Context
		{
			get
			{
				return _context;
			}
			private set
			{
				_context = value;
			}
		}

		public bool IsActive { get { return enabled; } }

		public T CastContext<T>() where T : SceneElementBase
		{
			return (T)System.Convert.ChangeType(_context, typeof(T));
		}

		public void SetContext(SceneElementBase context, IElementComponents container)
		{
			_components = container;
			Context = context;
		}

		public void Init()
		{
			OnInit();
			IsInited = true;

			if (_needActiveWhenInit)
			{
				Activate();
			}
		}

		public R Get<R>() where R : class, IElementComponent
		{
			return _components.Get<R>() as R;
		}

		public virtual void Activate()
		{
			enabled = true;
		}

		public virtual void Deactivate()
		{
			enabled = false;
		}

		protected virtual void Awake()
		{
			if (!IsInited)
			{
				enabled = false;
				_needActiveWhenInit = true;
			}
		}

		protected virtual void OnInit() { }

		[SerializeField]
		[HideInInspector]
		private SceneElementBase _context;

		private IElementComponents _components;
		private bool _needActiveWhenInit = false;
	}
}
