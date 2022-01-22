/// <summary>
/// AEP to native unity animation. 
/// © OneP Studio
/// email: onepstudio@gmail.com
/// </summary>
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
#if UNITY_4_0_0 ||UNITY_4_0 || UNITY_4_0_1||UNITY_4_1||UNITY_4_2||UNITY_4_3||UNITY_4_4||UNITY_4_5||UNITY_4_6||UNITY_4_7||UNITY_4_8||UNITY_4_9
namespace OnePStudio.AEPToUnity4
#else
namespace OnePStudio.AEPToUnity5
#endif
{
	public class DataAnimAnalytics  
	{
		public Dictionary<string,string> objShowWhenStartAnim=new Dictionary<string,string> ();
		public Dictionary<string,string> objHideWhenStartAnim=new Dictionary<string,string>();
		public AEPJsonFinal jsonFinal;
		public string filename;
		public AnimationStyle animationStyle;
		public DataAnimAnalytics(AEPJsonFinal _jsonFinal,string _filename,AnimationStyle _animationStyle)
		{
			this.jsonFinal=_jsonFinal;
			this.filename=_filename;
			animationStyle=_animationStyle;
		}
		public void AddObjectShowWhenStartAnim(Dictionary<string,string> _objShowWhenStartAnim)
		{
			foreach(KeyValuePair<string,string> pair in _objShowWhenStartAnim)
			{
				objShowWhenStartAnim[pair.Key]=pair.Value;
			}
		}
		public void AddObjectHideWhenStartAnim(Dictionary<string,string> _objHideWhenStartAnim)
		{
			foreach(KeyValuePair<string,string> pair in _objHideWhenStartAnim)
			{
				objHideWhenStartAnim[pair.Key]=pair.Value;
			}
		}
	}
}