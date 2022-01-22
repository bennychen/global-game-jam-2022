
using Spine.Unity;
using UnityEngine;

namespace Game
{
	public class Character : MonoBehaviour, Prime31.IObjectInspectable
	{
		[SerializeField]
		[Range(1, 5)]
		public int SkinIndex = 1;

		public void Awake()
		{
			skeletonAnimation = GetComponent<SkeletonAnimation>();
		}

		//move
		//speak
		[Prime31.MakeButton]
		public void SaySomething()
		{

		}

		[Prime31.MakeButton]
		public void changeSkin()
		{
			skeletonAnimation.Skeleton.SetSkin("pifu0" + SkinIndex);
			skeletonAnimation.Skeleton.SetSlotsToSetupPose();
		}

		private SkeletonAnimation skeletonAnimation;
	}
}