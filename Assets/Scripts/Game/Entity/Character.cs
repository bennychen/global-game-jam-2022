
using System;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

namespace Game
{
	public class Character : MonoBehaviour, Prime31.IObjectInspectable
	{
		
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
			else
			{
				transform.SetLocalPositionY(-7.8f);
				transform.SetLocalPositionZ(-0.1f);
			}
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
		}
		[Prime31.MakeButton]
		public void FadeOut()
		{
			GetComponent<MeshRenderer>().material.DOFloat(1.0f, "_DissolveAmount", 1.0f);
		}

		[SerializeField]
		private string _debugSkinID = "xiuxiu";
		//public Material 

		private SkeletonAnimation skeletonAnimation;

	}
}