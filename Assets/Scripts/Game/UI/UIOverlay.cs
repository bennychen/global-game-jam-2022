using UnityEngine;
using UnityEngine.EventSystems;

public class UIOverlay : MonoBehaviour, IPointerClickHandler
{
	public void Show(System.Action action)
	{
		this._action = action;
		this.gameObject.SetActive(true);
	}

	public void Hide()
	{
		this._action = null;
		this.gameObject.SetActive(false);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (this._action != null)
		{
			this._action();
		}
	}

	private System.Action _action;
}
