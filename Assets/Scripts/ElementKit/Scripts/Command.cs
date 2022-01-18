using System;

namespace Codeplay
{
	public static class JsonHelper
	{
		public static T FromJson<T>(string json)
		{
			Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(json);
			return wrapper.Param;
		}

		public static string ToJson<T>(T param)
		{
			Wrapper<T> wrapper = new Wrapper<T>();
			wrapper.Param = param;
			return UnityEngine.JsonUtility.ToJson(wrapper);
		}

		public static string ToJson<T>(T param, bool prettyPrint)
		{
			Wrapper<T> wrapper = new Wrapper<T>();
			wrapper.Param = param;
			return UnityEngine.JsonUtility.ToJson(wrapper, prettyPrint);
		}

		[Serializable]
		private class Wrapper<T>
		{
			public T Param;
		}
	}
	public interface ICommand
	{
		string ID { get; }
		void Dispatch(string param);
	}

	public class Command : ICommand
	{
		public string ID { get { return _id; } }

		public delegate void NonParamAction();

		public Command(ElementModel model, string id)
		{
			_id = string.Format("{0}/{1}", model.GetType(), id);
			SceneElementConfig.CommandHistory.Regiser(this);
		}

		public Command(ElementModel model, string id, NonParamAction action)
				: this(model, id)
		{
			_action = action;
		}

		public Command(ElementModel model, NonParamAction action)
				: this(model, action.Method.Name, action) { }

		public void BindAction(NonParamAction action)
		{
			_action = action;
		}

		public void Dispatch(string param)
		{
			ForceExec();
		}

		public void Exec()
		{
#if UNITY_EDITOR
			if (SceneElementConfig.CommandHistory.IsInHistory())
			{
				return;
			}
			SceneElementConfig.CommandHistory.Push(_id);
#endif

			ForceExec();
		}

		private void ForceExec()
		{
			if (_action != null)
				_action();
			OnExecute();
		}

		private readonly string _id;
		private NonParamAction _action;
		private event Action OnExecute = delegate { };
	}

	public class Command<TParam> : ICommand
	{
		public string ID { get { return _id; } }

		public delegate void ParamAction(TParam param);

		public Command(ElementModel model, string id)
		{
			if (!typeof(TParam).IsSerializable)
			{
				throw new InvalidOperationException("A serializable type is required");
			}
			_id = string.Format("{0}/{1}", model.GetType(), id);
			SceneElementConfig.CommandHistory.Regiser(this);
		}

		public Command(ElementModel model, string id, ParamAction action)
				: this(model, id)
		{
			_action = action;
		}

		public Command(ElementModel model, ParamAction action)
				: this(model, action.Method.Name, action) { }

		public void BindAction(ParamAction action)
		{
			_action = action;
		}

		public void Dispatch(string param)
		{
			ForceExec(JsonHelper.FromJson<TParam>(param));
		}

		public void Exec(TParam value)
		{
#if UNITY_EDITOR
			if (SceneElementConfig.CommandHistory.IsInHistory())
			{
				return;
			}
			SceneElementConfig.CommandHistory.Push(_id, value);
#endif
			ForceExec(value);
		}

		private void ForceExec(TParam value = default(TParam))
		{
			if (_action != null)
				_action(value);
			OnExecute(value);
		}

		private readonly string _id;
		private ParamAction _action;
		private event Action<TParam> OnExecute = delegate { };
	}
}
