namespace Codeplay
{
	public class ElementComponentsNode : UnityEngine.MonoBehaviour, IElementComponents
	{
		public bool IsInited { get { return _delegate != null; } }

		public ElementComponents<T> Components<T>() where T : SceneElementBase
		{
			return _delegate as ElementComponents<T>;
		}

		public T GetContext<T>() where T : SceneElementBase
		{
			return (_delegate as ElementComponents<T>).Context;
		}

		public void Init<T>(ElementComponents<T> components) where T : SceneElementBase
		{
			_delegate = components;
			OnInit();
		}

		public void Add(IElementComponent component)
		{
			_delegate.Add(component);
		}

		public void Remove(IElementComponent component)
		{
			_delegate.Remove(component);
		}

		public R Get<R>() where R : class, IElementComponent
		{
			return _delegate.Get<R>();
		}

		protected virtual void OnInit() { }

		private IElementComponents _delegate;
	}
}
