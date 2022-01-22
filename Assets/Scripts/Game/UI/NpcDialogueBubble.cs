using System.Collections;
using UnityEngine;

public class NpcDialogueBubble : MonoBehaviour, Prime31.IObjectInspectable
{
	public UnityEngine.UI.Text text;

	public bool IsAnimating
	{
		get
		{
			return _isAnimating;
		}
	}

	public void Awake()
	{
		text = GetComponentInChildren<UnityEngine.UI.Text>();
	}

	public void PopupWithAnim(string txt, System.Action onComplete)
	{
		gameObject.SetActive(true);
		this._onAnimComplete = onComplete;
		StopAllCoroutines();
		StartCoroutine(Typewriter(txt));
	}

	public void Popup(string txt)
	{
		StopAllCoroutines();
		gameObject.SetActive(true);
		text.text = txt;
	}

	public void Hide()
	{
		StopAllCoroutines();
		gameObject.SetActive(false);
	}

	private IEnumerator Typewriter(string txt)
	{
		text.text = "";
		_isAnimating = true;
		foreach (char c in txt)
		{
			text.text = text.text + c;
			if (_isAnimating)
			{
				yield return new WaitForSeconds(.03f);
			}
			else
			{
				text.text = txt;
			}
		}
		if (this._onAnimComplete != null)
		{
			this._onAnimComplete();
		}
		yield return null;
	}

	private bool _isAnimating;
	private System.Action _onAnimComplete;
}
