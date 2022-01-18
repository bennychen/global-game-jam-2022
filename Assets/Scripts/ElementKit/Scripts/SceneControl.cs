using System;

namespace Codeplay
{
	public abstract class SceneControl
	{
		public string SceneName { get { return _sceneName; } }
		public SceneElementBase Context { get { return _context; } }
		public bool IsLoaded { get; internal set; }

		public void Init(SceneElementBase context)
		{
			_context = context;
		}

		public T GetContext<T>() where T : SceneElementBase
		{
			return _context as T;
		}

		public SceneControl() { }

		public SceneControl(string sceneName)
		{
			_sceneName = sceneName;
		}

		protected virtual void OnSceneUnloaded() { }
		public virtual void PreEnter() { }
		public abstract void Load(float maxProgress, Action onLoaded, params object[] args);
		public abstract void Unload(float maxProgress, Action onLoaded = null);

		private SceneElementBase _context;
		private string _sceneName;
	}
}
