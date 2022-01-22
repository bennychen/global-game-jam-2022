using UnityEngine;

public class NpcDialogueBubble : MonoBehaviour
{
	public UnityEngine.UI.Text text;

	public void Awake()
	{
		text = GetComponentInChildren<UnityEngine.UI.Text>();
	}
}
