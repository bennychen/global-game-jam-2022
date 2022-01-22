
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

		public void changeSkin(string name)
		{
			if (skeletonAnimation == null)
			{
				skeletonAnimation = GetComponent<SkeletonAnimation>();
			}
			skeletonAnimation.Skeleton.SetSkin(name);
			skeletonAnimation.Skeleton.SetSlotsToSetupPose();
		}

		private SkeletonAnimation skeletonAnimation;
	}
}