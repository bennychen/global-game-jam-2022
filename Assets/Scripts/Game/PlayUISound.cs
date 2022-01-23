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

	private AudioSource _source;
}
