using UnityEngine;
using System.Collections.Generic;

namespace Codeplay
{
	[System.Serializable]
	public class CommandHistory
	{
		public List<CommandHistoryEntry> Entries = new List<CommandHistoryEntry>();
		public float LargestTimestamp
		{
			get { return Entries.Count > 0 ? Entries[Entries.Count - 1].Timestamp : 0; }
		}
		public bool IsInHistory()
		{
			return Time.time - StartTime < LargestTimestamp;
		}

		public Dictionary<string, ICommand> Commands { get; private set; }

		public float StartTime { get; set; }
		public int CurrentPlayIndex { get; set; }

		public CommandHistory()
		{
			Commands = new Dictionary<string, ICommand>();
		}

		public void Reset()
		{
			Entries.Clear();
			StartTime = 0;
			CurrentPlayIndex = 0;
		}

		public void ReadFromFile(string filePath)
		{
			if (filePath.EndsWith(".json") && System.IO.File.Exists(filePath))
			{
				string jsonData = System.IO.File.ReadAllText(filePath);
				JsonUtility.FromJsonOverwrite(jsonData, this);
			}
			else
			{
				Debug.LogError("Failed to read command history file data from file " + filePath);
			}
		}

		internal void Tick(float time)
		{
			float currentTime = time - StartTime;
			if (currentTime < LargestTimestamp)
			{
				while (CurrentPlayIndex < Entries.Count &&
							 currentTime > Entries[CurrentPlayIndex].Timestamp)
				{
					var entry = Entries[CurrentPlayIndex];
					if (Commands.ContainsKey(entry.CommandID))
					{
						Commands[entry.CommandID].Dispatch(entry.Param);
						// Debug.Log("exec command " + entry.CommandID + "(" + entry.Param + ")");
					}
					CurrentPlayIndex++;
				}
			}
		}

		internal void Regiser(ICommand command)
		{
			if (Commands.ContainsKey(command.ID))
			{
				Debug.LogWarning("A command with ID [" + command.ID + "] already exists.");
			}
			else
			{
				Commands.Add(command.ID, command);
			}
		}

		internal void Push(string id)
		{
			Entries.Add(new CommandHistoryEntry()
			{
				CommandID = id,
				Timestamp = Time.time - StartTime,
			});
		}

		internal void Push<TParam>(string id, TParam param)
		{
			Entries.Add(new CommandHistoryEntry()
			{
				CommandID = id,
				Timestamp = Time.time - StartTime,
				Param = JsonHelper.ToJson(param),
			});
		}
	}

	[System.Serializable]
	public class CommandHistoryEntry
	{
		public float Timestamp;
		public string CommandID;
		public string Param;
	}

	public class SceneElementConfig : MonoBehaviour
	{
		public List<SceneElementBase> Elements { get { return _elements; } }

		private void Awake()
		{
#if UNITY_EDITOR
			_commandHistory.Reset();
			if (!string.IsNullOrEmpty(_commandHistoryFile))
			{
				_commandHistory.ReadFromFile(_commandHistoryFile);
			}
#endif

			GatherElementsFromChild(transform);

			for (int i = 0; i < _elements.Count; i++)
			{
				SceneElementManager.Instance.TryAddElementToDictioinary(_elements[i]);
			}

			for (int i = 0; i < _elements.Count; i++)
			{
				SceneElementManager.Instance.InjectForElement(_elements[i]);
			}

			for (int i = 0; i < _elements.Count; i++)
			{
				_elements[i].Init(null);
			}
		}

		private void GatherElementsFromChild(Transform transform)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform childTransform = transform.GetChild(i);
				SceneElementBase element = childTransform.GetComponent<SceneElementBase>();
				if (element != null)
				{
					_elements.Add(element);
				}

				GatherElementsFromChild(childTransform);
			}
		}

		[SerializeField]
		private List<SceneElementBase> _elements;
		[SerializeField]
		private string _commandHistoryFile;

#if UNITY_EDITOR
		public static CommandHistory CommandHistory { get { return _commandHistory; } }

		private void FixedUpdate()
		{
			_commandHistory.Tick(Time.time);
		}

		private static CommandHistory _commandHistory = new CommandHistory();
#endif
	}
}
