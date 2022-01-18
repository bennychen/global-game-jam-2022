// This class is auto-generated do not modify
using System;
using UnityEngine;

namespace Codeplay
{

	public class ValueChangeCommand<TParam> : ICommand
	{
		public string ID { get { return _id; } }
		public TParam Value { get { return _value; } }

		public ValueChangeCommand(ElementModel model, string id)
		{
			if (!typeof(TParam).IsSerializable)
			{
				throw new InvalidOperationException("A serializable type is required");
			}
			_id = string.Format("{0}/{1}", model.GetType(), id);
			SceneElementConfig.CommandHistory.Regiser(this);
		}

		public void Dispatch(string param)
		{
			ForceExec(JsonHelper.FromJson<TParam>(param));
		}

		public void Exec(TParam value = default(TParam))
		{
			if (!value.Equals(_value))
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
		}

		public void ForceExec(TParam value = default(TParam))
		{
			var oldValue = _value;
			_value = value;
			OnValueChange(oldValue, _value);
		}

		private TParam _value;
		private readonly string _id;
		private event Action<TParam, TParam> OnValueChange = delegate { };
	}

	public class IntChangeCommand : ICommand
	{
		public string ID { get { return _id; } }
		public int Value { get { return _value; } }

		public IntChangeCommand(ElementModel model, string id)
		{
			_id = string.Format("{0}/{1}", model.GetType(), id);
			SceneElementConfig.CommandHistory.Regiser(this);
		}

		public void Dispatch(string param)
		{
			ForceExec(JsonHelper.FromJson<int>(param));
		}

		public void Exec(int value = default(int))
		{
			if (_value != value)
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
		}

		public void ForceExec(int value = default(int))
		{
			var oldValue = _value;
			_value = value;
			OnValueChange(oldValue, _value);
		}

		private int _value;
		private readonly string _id;
		private event Action<int, int> OnValueChange = delegate { };
	}

	public class FloatChangeCommand : ICommand
	{
		public string ID { get { return _id; } }
		public float Value { get { return _value; } }

		public FloatChangeCommand(ElementModel model, string id)
		{
			_id = string.Format("{0}/{1}", model.GetType(), id);
			SceneElementConfig.CommandHistory.Regiser(this);
		}

		public void Dispatch(string param)
		{
			ForceExec(JsonHelper.FromJson<float>(param));
		}

		public void Exec(float value = default(float))
		{
			if (_value != value)
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
		}

		public void ForceExec(float value = default(float))
		{
			var oldValue = _value;
			_value = value;
			OnValueChange(oldValue, _value);
		}

		private float _value;
		private readonly string _id;
		private event Action<float, float> OnValueChange = delegate { };
	}

	public class BoolChangeCommand : ICommand
	{
		public string ID { get { return _id; } }
		public bool Value { get { return _value; } }

		public BoolChangeCommand(ElementModel model, string id)
		{
			_id = string.Format("{0}/{1}", model.GetType(), id);
			SceneElementConfig.CommandHistory.Regiser(this);
		}

		public void Dispatch(string param)
		{
			ForceExec(JsonHelper.FromJson<bool>(param));
		}

		public void Exec(bool value = default(bool))
		{
			if (_value != value)
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
		}

		public void ForceExec(bool value = default(bool))
		{
			var oldValue = _value;
			_value = value;
			OnValueChange(oldValue, _value);
		}

		private bool _value;
		private readonly string _id;
		private event Action<bool, bool> OnValueChange = delegate { };
	}

	public class StringChangeCommand : ICommand
	{
		public string ID { get { return _id; } }
		public string Value { get { return _value; } }

		public StringChangeCommand(ElementModel model, string id)
		{
			_id = string.Format("{0}/{1}", model.GetType(), id);
			SceneElementConfig.CommandHistory.Regiser(this);
		}

		public void Dispatch(string param)
		{
			ForceExec(JsonHelper.FromJson<string>(param));
		}

		public void Exec(string value = default(string))
		{
			if (_value != value)
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
		}

		public void ForceExec(string value = default(string))
		{
			var oldValue = _value;
			_value = value;
			OnValueChange(oldValue, _value);
		}

		private string _value;
		private readonly string _id;
		private event Action<string, string> OnValueChange = delegate { };
	}

	public class Vector2ChangeCommand : ICommand
	{
		public string ID { get { return _id; } }
		public Vector2 Value { get { return _value; } }

		public Vector2ChangeCommand(ElementModel model, string id)
		{
			_id = string.Format("{0}/{1}", model.GetType(), id);
			SceneElementConfig.CommandHistory.Regiser(this);
		}

		public void Dispatch(string param)
		{
			ForceExec(JsonHelper.FromJson<Vector2>(param));
		}

		public void Exec(Vector2 value = default(Vector2))
		{
			if (_value != value)
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
		}

		public void ForceExec(Vector2 value = default(Vector2))
		{
			var oldValue = _value;
			_value = value;
			OnValueChange(oldValue, _value);
		}

		private Vector2 _value;
		private readonly string _id;
		private event Action<Vector2, Vector2> OnValueChange = delegate { };
	}

	public class Vector3ChangeCommand : ICommand
	{
		public string ID { get { return _id; } }
		public Vector3 Value { get { return _value; } }

		public Vector3ChangeCommand(ElementModel model, string id)
		{
			_id = string.Format("{0}/{1}", model.GetType(), id);
			SceneElementConfig.CommandHistory.Regiser(this);
		}

		public void Dispatch(string param)
		{
			ForceExec(JsonHelper.FromJson<Vector3>(param));
		}

		public void Exec(Vector3 value = default(Vector3))
		{
			if (_value != value)
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
		}

		public void ForceExec(Vector3 value = default(Vector3))
		{
			var oldValue = _value;
			_value = value;
			OnValueChange(oldValue, _value);
		}

		private Vector3 _value;
		private readonly string _id;
		private event Action<Vector3, Vector3> OnValueChange = delegate { };
	}

	public class QuaternionChangeCommand : ICommand
	{
		public string ID { get { return _id; } }
		public Quaternion Value { get { return _value; } }

		public QuaternionChangeCommand(ElementModel model, string id)
		{
			_id = string.Format("{0}/{1}", model.GetType(), id);
			SceneElementConfig.CommandHistory.Regiser(this);
		}

		public void Dispatch(string param)
		{
			ForceExec(JsonHelper.FromJson<Quaternion>(param));
		}

		public void Exec(Quaternion value = default(Quaternion))
		{
			if (_value != value)
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
		}

		public void ForceExec(Quaternion value = default(Quaternion))
		{
			var oldValue = _value;
			_value = value;
			OnValueChange(oldValue, _value);
		}

		private Quaternion _value;
		private readonly string _id;
		private event Action<Quaternion, Quaternion> OnValueChange = delegate { };
	}
}
