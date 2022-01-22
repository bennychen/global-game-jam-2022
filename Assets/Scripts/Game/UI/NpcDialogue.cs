using System.Collections.Generic;
using UnityEngine;

public class NpcDialogue : MonoBehaviour, Prime31.IObjectInspectable
{
	public List<NpcDialogueBubble> bubbles;

	public void Awake()
	{
		foreach (var bubble in bubbles)
		{
			_positions.Add(bubble.transform.localPosition.y);
			Debug.Log(bubble.transform.localPosition.y);
		}
		Reset();
	}

	private void Reset()
	{
		_availableBubbles.Clear();
		foreach (var bubble in bubbles)
		{
			bubble.gameObject.SetActive(false);
			_availableBubbles.Add(bubble);
		}
		_usedBubbles.Clear();
	}

	[Prime31.MakeButton]
	public void DebugDialogue()
	{
		this.PopupDialogue("大人饶命啊");
	}

	public void PopupDialogue(string text)
	{
		var bubble = fetchBubble();
		if (!bubble)
		{
			return;
		}

		bubble.text.text = text;
		bubble.transform.SetLocalPositionY(this._positions[0]);
		bubble.gameObject.SetActive(true);
		_usedBubbles.Insert(0, bubble);
		if (_usedBubbles.Count >= bubbles.Count)
		{
			recycleBubble(_usedBubbles[_usedBubbles.Count - 1]);
			_usedBubbles.RemoveAt(_usedBubbles.Count - 1);
		}
	}

	public void Update()
	{
		var n = 0;
		foreach (var bubble in _usedBubbles)
		{
			bubble.transform.SetLocalPositionY(
								Mathf.Lerp(bubble.transform.localPosition.y,
																this._positions[n + 1], Time.deltaTime * 5));
			n++;
		}
	}

	private NpcDialogueBubble fetchBubble()
	{
		if (_availableBubbles.Count > 0)
		{
			var bubble = _availableBubbles[0];
			_availableBubbles.RemoveAt(0);
			return bubble;
		}
		return null;
	}

	private void recycleBubble(NpcDialogueBubble bubble)
	{
		bubble.gameObject.SetActive(false);
		this._availableBubbles.Add(bubble);
	}

	private List<float> _positions = new List<float>();
	private List<NpcDialogueBubble> _usedBubbles = new List<NpcDialogueBubble>();
	private List<NpcDialogueBubble> _availableBubbles = new List<NpcDialogueBubble>();
}
