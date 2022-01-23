
using System.Collections;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

namespace Game
{
	public class Character : MonoBehaviour, Prime31.IObjectInspectable
	{
		public AudioClip chatter;
		public AudioClip heavenAudio;
		public AudioClip hellAudio;

		//move
		//speak
		[Prime31.MakeButton]
		public void SaySomething()
		{

		}

		public void ChangeSkin(string name)
		{
			if (skeletonAnimation == null)
			{
				skeletonAnimation = GetComponent<SkeletonAnimation>();
			}
			skeletonAnimation.Skeleton.SetSkin(name);
			skeletonAnimation.Skeleton.SetSlotsToSetupPose();

			if (name == "xiuxiu")
			{
				transform.SetLocalPositionY(-10f);
				transform.SetLocalPositionZ(-1);
			}
			else if (name == "yeweiyang")
			{
				transform.SetLocalPositionY(-10.5f);
				transform.SetLocalPositionZ(-0.1f);
			}
			else if (name == "limingji")
			{
				transform.SetLocalPositionY(-9.5f);
				transform.SetLocalPositionZ(-0.1f);
			}
			else
			{
				transform.SetLocalPositionY(-7.8f);
				transform.SetLocalPositionZ(-0.1f);
			}
		}

		[Prime31.MakeButton]
		public void PlayHellSound()
		{
			var source = this.GetComponent<AudioSource>();
			if (source.isPlaying)
			{
				source.Stop();
			}
			source.clip = hellAudio;
			source.Play();
		}

		[Prime31.MakeButton]
		public void PlayHeavenSound()
		{
			var source = this.GetComponent<AudioSource>();
			if (source.isPlaying)
			{
				source.Stop();
			}
			source.clip = heavenAudio;
			source.Play();
		}

		public void PlayChatter(int duration)
		{
			StopCoroutine("PlayChatterCoroutine");
			var source = this.GetComponent<AudioSource>();
			source.clip = chatter;
			source.loop = true;
			source.Play();
			StartCoroutine(PlayChatterCoroutine(duration));
		}

		private IEnumerator PlayChatterCoroutine(int duration)
		{
			yield return new WaitForSeconds(duration);
			this.GetComponent<AudioSource>().Stop();
		}


		[Prime31.MakeButton]
		public void DebugSkin()
		{
			ChangeSkin(_debugSkinID);
		}

		[Prime31.MakeButton]
		public void ResetDissolve()
		{
			GetComponent<MeshRenderer>().material.SetFloat("_DissolveAmount", 1.0f);
			GetComponent<MeshRenderer>().material.SetFloat("_FadeOutAmount", 1.0f);
		}
		[Prime31.MakeButton]
		public void FadeOut()
		{
			PlayHeavenSound();
			GetComponent<MeshRenderer>().material.DOFloat(1.02f, "_FadeOutAmount", 1f);
		}
		[Prime31.MakeButton]
		public void DissolveOut()
		{
			PlayHellSound();
			GetComponent<MeshRenderer>().material.DOFloat(1.0f, "_DissolveAmount", 1.5f);
		}

		[SerializeField]
		private string _debugSkinID = "xiuxiu";
		//public Material 

		private SkeletonAnimation skeletonAnimation;

	}
}