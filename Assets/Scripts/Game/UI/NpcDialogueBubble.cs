using System.Collections;
using UnityEngine;

public class NpcDialogueBubble : MonoBehaviour, Prime31.IObjectInspectable
{
	public UnityEngine.UI.Text text;

	public void Awake()
	{
		text = GetComponentInChildren<UnityEngine.UI.Text>();
	}

	public void Popup(string txt)
	{
		StartCoroutine(Typewriter(txt));
	}

	private IEnumerator Typewriter(string txt)
	{
		text.text = "";
		foreach (char c in txt)
		{
			text.text = text.text + c;
			yield return new WaitForSeconds(.03f);
		}
		yield return null;
	}
}
