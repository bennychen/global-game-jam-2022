using UnityEngine;
using UnityEditor;

namespace Codeplay
{
	[CustomEditor(typeof(SceneElementConfig))]
	public class SceneElementConfigEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			SceneElementConfig config = target as SceneElementConfig;
			var serializedObject = new SerializedObject(config);
			if (GUILayout.Button("Select Command History File"))
			{
				string path = EditorUtility.OpenFilePanel("command history", Application.temporaryCachePath, "json");
				if (string.IsNullOrEmpty(path)) return;
				if (path.Contains(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name))
				{
					serializedObject.FindProperty("_commandHistoryFile").stringValue = path;
					serializedObject.ApplyModifiedProperties();
				}
				else
				{
					EditorUtility.DisplayDialog("Incompatible File",
							"The command file doesn't belong to current scene.", "OK");
				}
			}
			if (GUILayout.Button("Open Command History File Folder"))
			{
				var filePath = Application.temporaryCachePath;
				filePath = filePath.Replace(@"/", @"\");   // explorer doesn't like front slashes
				System.Diagnostics.Process.Start("explorer.exe", filePath);
			}

			EditorGUILayout.Space();

			if (Application.isPlaying)
			{
				DrawCommandHistory();
			}

			Repaint();
		}

		private void DrawCommandHistory()
		{
			EditorGUILayout.LabelField("Command History");
			if (GUILayout.Button("Save History"))
			{
				string data = JsonUtility.ToJson(SceneElementConfig.CommandHistory);
				Debug.Log(data);
				string path = GetNewFilePath();
				System.IO.File.WriteAllText(path, data);
			}


			if (SceneElementConfig.CommandHistory.IsInHistory())
			{
				EditorGUILayout.LabelField("In History");
			}
			if (EditorApplication.isPaused)
			{
				_currentTime = EditorGUILayout.Slider("Current Time",
						_currentTime, 0, Mathf.Max(SceneElementConfig.CommandHistory.LargestTimestamp, _currentTime));
			}
			else
			{
				_currentTime = Time.time - SceneElementConfig.CommandHistory.StartTime;
				float currentTime = EditorGUILayout.Slider("Current Time",
						_currentTime, 0, Mathf.Max(SceneElementConfig.CommandHistory.LargestTimestamp, _currentTime));
				if (currentTime != _currentTime)
				{
					EditorApplication.isPaused = true;
				}
			}

			EditorGUI.indentLevel++;
			_scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
			int totalCount = SceneElementConfig.CommandHistory.Entries.Count;
			int totalPage = Mathf.Max(0, (totalCount - 1) / EntriesPerPage);
			_currentPage =
					EditorGUILayout.IntSlider("Current Page", _currentPage, 0, totalPage);
			for (int i = EntriesPerPage * _currentPage;
					i < EntriesPerPage * (_currentPage + 1) && i < totalCount; i++)
			{
				var command = SceneElementConfig.CommandHistory.Entries[i];
				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(
						command.Timestamp + ": " + command.CommandID + "(" + command.Param + ")");
				GUILayout.EndHorizontal();
			}
			EditorGUI.indentLevel--;
			EditorGUILayout.EndScrollView();
		}

		private string GetNewFilePath()
		{
			return System.IO.Path.Combine(Application.temporaryCachePath,
					UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "_commandhistory.json");
		}

		private float _currentTime;
		private Vector2 _scrollPosition;
		private int _currentPage;
		private const int EntriesPerPage = 50;
	}
}
