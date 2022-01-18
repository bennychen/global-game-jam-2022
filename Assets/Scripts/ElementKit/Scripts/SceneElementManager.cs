using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;

namespace Codeplay
{
	public sealed class SceneElementManager
	{
		public static SceneElementManager Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new SceneElementManager();

				}
				return _instance;
			}
		}

		public SceneElementBase GetElement(string id)
		{
			if (_idToElement.ContainsKey(id))
			{
				return _idToElement[id];
			}
			return null;
		}

		public void AddElement(SceneElementBase element, object data = null)
		{
			if (TryAddElementToDictioinary(element))
			{
				InjectForElement(element);
				element.Init(data);
			}
		}

		public void RemoveElement(SceneElementBase element)
		{
			if (_idToElement.ContainsKey(element.ID))
			{
				element.Free();

				_idToElement.Remove(element.ID);
				if (_typeToElements.ContainsKey(element.GetType()))
				{
					_typeToElements[element.GetType()].Remove(element);
					if (_typeToElements.Count == 0)
					{
						_typeToElements.Remove(element.GetType());
					}
				}
			}
			else
			{
				Debug.LogError("Element manager doesn't contain an element with ID [" + element.ID + "]");
			}
		}

		public SceneElementBase Resolve(Type type, string id)
		{
			if (_typeToElements.ContainsKey(type))
			{
				List<SceneElementBase> elements = _typeToElements[type];
				if (elements.Count == 1 || string.IsNullOrEmpty(id))
				{
					return elements[0];
				}
				else
				{
					return _idToElement.ContainsKey(id) ? _idToElement[id] : null;
				}
			}
			return null;
		}

		internal bool TryAddElementToDictioinary(SceneElementBase element)
		{
			if (_idToElement.ContainsKey(element.ID))
			{
				Debug.LogError("An element with the same id [" +
						element.ID + "] already exists.");
				return false;
			}
			else
			{
				if (!_typeToElements.ContainsKey(element.GetType()))
				{
					_typeToElements.Add(element.GetType(), new List<SceneElementBase>());
				}
				_typeToElements[element.GetType()].Add(element);
				_idToElement.Add(element.ID, element);

				return true;
			}
		}

		internal void InjectForElement(SceneElementBase element)
		{
			if (element == null) return;

			var members = element.GetType().GetMembers();
			foreach (var memberInfo in members)
			{
				var injectAttribute =
						memberInfo.GetCustomAttributes(
								typeof(InjectElementAttribute), true).FirstOrDefault() as InjectElementAttribute;
				if (injectAttribute != null)
				{
					if (memberInfo is PropertyInfo)
					{
						var propertyInfo = memberInfo as PropertyInfo;

						propertyInfo.SetValue(element,
								Resolve(propertyInfo.PropertyType, injectAttribute.Name), null);
					}
					else if (memberInfo is FieldInfo)
					{
						var fieldInfo = memberInfo as FieldInfo;
						fieldInfo.SetValue(element,
								Resolve(fieldInfo.FieldType, injectAttribute.Name));
					}
				}
			}
		}

		private SceneElementManager()
		{
			_typeToElements = new Dictionary<Type, List<SceneElementBase>>();
			_idToElement = new Dictionary<string, SceneElementBase>();
		}

		private Dictionary<Type, List<SceneElementBase>> _typeToElements;
		private Dictionary<string, SceneElementBase> _idToElement;

		private static SceneElementManager _instance = null;
	}
}
