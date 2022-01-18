using System.Collections.Generic;
using UnityEngine;
using System;

namespace Codeplay
{
	public class App
	{
		public static App Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new App();
				}
				return _instance;
			}
		}

		public Action OnSceneLoaded = delegate { };

		public SceneControl CurrentScene { get { return _currentScene; } }
		public SceneControl PreviousScene { get { return _previousScene; } }

		public void RegisterScene<T>(T scene) where T : SceneControl
		{
			string id = typeof(T).ToString();
			if (_idToScene.ContainsKey(id))
			{
				Debug.LogError("Duplicated register of scene: " + id);
			}
			else
			{
				_idToScene.Add(id, scene);
			}
		}

		public void RegisterSystem<T>(T system) where T : Subsystem
		{
			Type t = typeof(T);
			if (_typeToSystem.ContainsKey(t))
			{
				Debug.LogError("System already registered: " + t.ToString());
			}
			else
			{
				_typeToSystem.Add(t, system);
				system.Init();
			}
		}

		public void UnregisterSystem<T>() where T : Subsystem
		{
			Type t = typeof(T);
			if (_typeToSystem.ContainsKey(t))
			{
				_typeToSystem[t].Destroy();
				_typeToSystem.Remove(t);
			}
		}

		public SceneControl GetScene(string id)
		{
			return _idToScene[id];
		}

		public T GetScene<T>() where T : SceneControl
		{
			return _idToScene[typeof(T).ToString()] as T;
		}

		public T GetSystem<T>() where T : Subsystem
		{
			return _typeToSystem[typeof(T)] as T;
		}

		public void EnterScene(SceneControl scene, Action onEntered, params object[] args)
		{
			_previousScene = _currentScene;
			_currentScene = scene;
			_currentScene.PreEnter();
			_currentScene.IsLoaded = false;
			float progressForUnload = _previousScene == null ? 0 : 0.3f;
			float progressForLoad = 1 - progressForUnload;
			if (_previousScene != null)
			{
				_previousScene.Unload(progressForUnload);
			}
			_currentScene.Load(progressForLoad, () =>
			{
				scene.IsLoaded = true;
				OnSceneLoaded();

				if (onEntered != null)
				{
					onEntered();
				}
			}, args);
		}

		public void EnterScene(string id, Action onEntered, params object[] args)
		{
			if (_idToScene.ContainsKey(id))
			{
				EnterScene(_idToScene[id], onEntered, args);
			}
			else
			{
				Debug.LogError("EnterScene failed, scene [" + id + "] is not registered.");
			}
		}

		public void EnterScene<T>(Action onEntered, params object[] args) where T : SceneControl
		{
			EnterScene(typeof(T).ToString(), onEntered, args);
		}

		protected App()
		{
			_idToScene = new Dictionary<string, SceneControl>();
			_typeToSystem = new Dictionary<Type, Subsystem>();
			_previousScene = null;
			_currentScene = null;
		}

		private Dictionary<string, SceneControl> _idToScene;
		private SceneControl _previousScene;
		private SceneControl _currentScene;

		private Dictionary<Type, Subsystem> _typeToSystem;

		private static App _instance = null;
	};
}
