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

	public void PopupWithAnim(string txt, System.Action onComplete = null)
	{
		gameObject.SetActive(true);
		Game.GameController.Instance.DialogController.overlay.Show(this.onClickOverlay);
		this._onComplete = onComplete;
		StopAllCoroutines();
		StartCoroutine(Typewriter(txt));
	}

	public void Popup(string txt)
	{
		StopAllCoroutines();
		Game.GameController.Instance.DialogController.overlay.Show(this.onClickOverlay);
		gameObject.SetActive(true);
		text.text = txt;
	}

	public void Hide()
	{
		StopAllCoroutines();
		gameObject.SetActive(false);
	}

	private void onClickOverlay()
	{
		Debug.Log("onclick" + this._isAnimating + this._onComplete);
		if (this._isAnimating)
		{
			this._isAnimating = false;
		}
		else
		{
			if (this._onComplete != null)
			{
				this._onComplete();
			}
			this.Hide();
		}
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
		this._isAnimating = false;
		yield return null;
	}

	private bool _isAnimating;
	private System.Action _onComplete;
}
