using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroText : MonoBehaviour
{
	public void Awake()
	{
		this._text = GetComponent<Text>();
		this._finalText = this._text.text;
	}


	public void Start()
	{
		StartCoroutine(Typewriter(this._finalText));
	}

	public void OnDisable()
	{
		this._isAnimating = false;
	}

	private IEnumerator Typewriter(string txt)
	{
		this._text.text = "";
		_isAnimating = true;
		foreach (char c in txt)
		{
			this._text.text = this._text.text + c;
			if (_isAnimating)
			{
				yield return new WaitForSeconds(.3f);
			}
			else
			{
				this._text.text = txt;
			}
		}
		this._isAnimating = false;
		yield return null;
	}


	private bool _isAnimating;
	private Text _text;
	private string _finalText;
}
