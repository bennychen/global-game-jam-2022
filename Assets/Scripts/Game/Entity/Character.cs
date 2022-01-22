
using Spine.Unity;
using UnityEngine;

namespace Game
{
	public class Character : MonoBehaviour, Prime31.IObjectInspectable
	{
		[SerializeField]
		[Range(1, 5)]
		public int debugSkinIndex = 1;

		//move
		//speak
		[Prime31.MakeButton]
		public void SaySomething()
		{

		}

		[Prime31.MakeButton]
		public void debugSkin()
		{
			this.changeSkin("pifu0" + debugSkinIndex);
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