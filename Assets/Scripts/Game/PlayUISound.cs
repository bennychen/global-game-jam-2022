using System.Collections;
using UnityEngine;

public class PlayUISound : MonoBehaviour
{
	public AudioClip buttonClick;
	public AudioClip chatter;

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

	public void PlayChatter(int duration)
	{
		StopCoroutine("PlayChatterCoroutine");
		this._source.clip = chatter;
		this._source.loop = true;
		this._source.Play();
		StartCoroutine(PlayChatterCoroutine(duration));
	}

	private IEnumerator PlayChatterCoroutine(int duration)
	{
		yield return new WaitForSeconds(duration);
		this._source.Stop();
	}

	private AudioSource _source;
}
