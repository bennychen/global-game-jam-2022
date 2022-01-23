using UnityEngine;

public class PlayUISound : MonoBehaviour
{
	public AudioClip buttonClick;
	public AudioClip nextPage;
	public AudioClip nextDay;

	public void Awake()
	{
		this._source = GetComponent<AudioSource>();
	}

	public void PlayButtonClick()
	{
		this._source.clip = buttonClick;
		this._source.loop = false;
		this._source.Play();
	}

	public void PlayNextPage()
	{
		this._source.clip = nextPage;
		this._source.loop = false;
		this._source.Play();
	}

	public void PlayNextDay()
	{
		this._source.clip = nextDay;
		this._source.loop = false;
		this._source.Play();
	}

	private AudioSource _source;
}
