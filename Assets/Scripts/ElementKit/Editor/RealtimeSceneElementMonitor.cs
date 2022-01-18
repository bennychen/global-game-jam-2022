using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using Codeplay;

public class RealTimeSceneElementMonitor : EditorWindow
{
	public RealTimeSceneElementMonitor()
	{
		_model = new RealTimeSceneElementModel();
	}

	[MenuItem("Codeplay/Scene Element/RealTime Monitor Panel %#&s")]
	public static void OpenDebugPanel()
	{
		GetWindow<RealTimeSceneElementMonitor>("Scene Elements Monitor");
	}

	private void Update()
	{
		_model.ResetAllElement(FindAllElement());

		Repaint();
	}

	private GUIStyle GetFoldoutStyle()
	{
		GUIStyle result = EditorStyles.foldout;
		result.stretchWidth = false;
		result.fixedWidth = 20f;
		return result;
	}

	private void OnGUI()
	{
		if (!Application.isPlaying) return;

		_scrollView = EditorGUILayout.BeginScrollView(_scrollView);
		for (int i = 0; i < _model.AllElement.Count; i++)
		{
			EditorGUILayout.BeginHorizontal();
			_model.AllElementFoldoutBool[i] =
					EditorGUILayout.Foldout(_model.AllElementFoldoutBool[i], " ", GetFoldoutStyle());
			EditorGUILayout.ObjectField(_model.AllElement[i], _model.AllElement[i].GetType()
					, true);

			EditorGUILayout.EndHorizontal();
			if (_model.AllElementFoldoutBool[i])
			{
				EditorGUI.indentLevel++;
				DrawSceneElementInfo(_model.AllElement[i]);
				EditorGUI.indentLevel--;
			}
		}

		EditorGUILayout.EndScrollView();
	}

	private void DrawSceneElementInfo(SceneElementBase element)
	{
		System.Type sceneElementType = element.GetType();
		System.Reflection.PropertyInfo stateMachineProperty = sceneElementType.GetProperty("StateMachine");
		if (stateMachineProperty != null)
		{
			object stateMachine = stateMachineProperty.GetValue(element, null);
			System.Type stateMachineType = stateMachine.GetType();

			object currentState = stateMachineType.GetProperty("currentState").GetValue(stateMachine, null);
			object previousState = stateMachineType.GetProperty("previousState").GetValue(stateMachine, null);
			EditorGUILayout.LabelField("Current State", currentState == null ? "null" : currentState.ToString());
			EditorGUILayout.LabelField("Previous State", previousState == null ? "null" : previousState.ToString());
			EditorGUILayout.Space();
		}
		else
		{
			System.Reflection.FieldInfo modelField = sceneElementType.GetField("_stateModel",
					System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

			if (modelField != null)
			{
				object model = modelField.GetValue(element);
				System.Type modelType = model.GetType();

				object currentState = modelType.GetProperty("CurrentState").GetValue(model, null);
				object previousState = modelType.GetProperty("PreviousState").GetValue(model, null);
				EditorGUILayout.LabelField("Current State", currentState == null ? "null" : currentState.ToString());
				EditorGUILayout.LabelField("Previous State", previousState == null ? "null" : previousState.ToString());
				EditorGUILayout.Space();
			}
		}

		System.Reflection.PropertyInfo controllersProperty = sceneElementType.GetProperty("Controllers");
		if (controllersProperty != null)
		{
			DrawComponents(element, controllersProperty, "Controllers");
		}

		System.Reflection.PropertyInfo viewsProperty = sceneElementType.GetProperty("Views");
		if (viewsProperty != null)
		{
			DrawComponents(element, viewsProperty, "Views");
		}

		System.Reflection.PropertyInfo modelsProperty = sceneElementType.GetProperty("Models");
		if (modelsProperty != null)
		{
			DrawModels(element, modelsProperty);
		}
	}

	private void DrawComponents(SceneElementBase element, System.Reflection.PropertyInfo componentsProperty, string label)
	{
		object components = componentsProperty.GetValue(element, null);
		System.Type componentsType = components.GetType();
		EditorGUILayout.LabelField(label);
		if (components != null)
		{
			System.Type elementComponentsType = componentsType;
			System.Reflection.FieldInfo viewsField = elementComponentsType.GetField("_components",
					System.Reflection.BindingFlags.NonPublic |
					System.Reflection.BindingFlags.Instance);

			IDictionary componentsDictionary =
					viewsField.GetValue(components) as IDictionary;
			if (componentsDictionary != null)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.LabelField("Number of " + label + ": " + componentsDictionary.Count.ToString());

				foreach (var component in componentsDictionary.Values)
				{
					IElementComponent c = component as IElementComponent;
					if (c != null)
					{
						GUI.enabled = c.IsActive;
						EditorGUILayout.LabelField(c.GetType().ToString());
						GUI.enabled = true;
					}
				}
				EditorGUI.indentLevel--;
			}
		}
	}

	private void DrawModels(SceneElementBase element, System.Reflection.PropertyInfo modelsProperty)
	{
		object components = modelsProperty.GetValue(element, null);
		System.Type componentsType = components.GetType();
		EditorGUILayout.LabelField("Models");
		if (components != null)
		{
			System.Type elementComponentsType = componentsType;
			System.Reflection.FieldInfo viewsField = elementComponentsType.GetField("_components",
					System.Reflection.BindingFlags.NonPublic |
					System.Reflection.BindingFlags.Instance);

			IDictionary componentsDictionary =
					viewsField.GetValue(components) as IDictionary;
			if (componentsDictionary != null)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.LabelField("Number of Models: " + componentsDictionary.Count.ToString());

				foreach (var component in componentsDictionary.Values)
				{
					ElementModel model = component as ElementModel;
					if (model != null)
					{
						EditorGUILayout.LabelField(model.GetType().ToString());
						EditorGUI.indentLevel++;
						System.Type componentType = component.GetType();
						IEnumerable<System.Reflection.FieldInfo> fileds = componentType.GetFields(
								System.Reflection.BindingFlags.Instance |
								System.Reflection.BindingFlags.NonPublic |
								System.Reflection.BindingFlags.Public);
						foreach (var field in fileds)
						{
							ICommand command = field.GetValue(model) as ICommand;
							if (command != null)
							{
								EditorGUILayout.BeginHorizontal();
								//if (command is Command)
								//{
								//    GUILayout.Space(50);
								//    if (GUILayout.Button(command.ID, GUILayout.ExpandWidth(false)))
								//    {
								//        command.Publish();
								//    }
								//}
								//else
								//{
								EditorGUILayout.LabelField(command.ID);
								//}
								GUILayout.Space(20);
								EditorGUILayout.EndHorizontal();
							}
						}
						EditorGUI.indentLevel--;
					}
				}
				EditorGUI.indentLevel--;
			}
		}
	}

	private SceneElementBase[] FindAllElement()
	{
		return FindObjectsOfType<SceneElementBase>();
	}

	private RealTimeSceneElementModel _model;
	private Vector2 _scrollView;

	public class RealTimeSceneElementModel
	{
		public List<SceneElementBase> AllElement { get; private set; }

		public List<bool> AllElementFoldoutBool { get; set; }

		public void ResetAllElement(IEnumerable<SceneElementBase> data)
		{
			AllElement.Clear();
			AllElement.AddRange(data);

			while (AllElementFoldoutBool.Count < AllElement.Count)
			{
				AllElementFoldoutBool.Add(false);
			}

			while (AllElementFoldoutBool.Count > AllElement.Count)
			{
				AllElementFoldoutBool.RemoveAt(AllElementFoldoutBool.Count - 1);
			}
		}

		public RealTimeSceneElementModel()
		{
			AllElement = new List<SceneElementBase>();
			AllElementFoldoutBool = new List<bool>();
		}
	}
}
