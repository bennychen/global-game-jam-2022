using UnityEngine;

public class NpcDialogueBubble : MonoBehaviour, Prime31.IObjectInspectable
{
	public UnityEngine.UI.Text text;

	public void Awake()
	{
		text = GetComponentInChildren<UnityEngine.UI.Text>();
	}
}
