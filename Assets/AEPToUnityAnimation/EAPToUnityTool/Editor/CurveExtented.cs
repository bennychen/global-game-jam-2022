/// <summary>
/// AEP to native unity animation. 
/// © OneP Studio
/// email: onepstudio@gmail.com
/// </summary>
/// 
using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
#if UNITY_4_0_0 ||UNITY_4_0 || UNITY_4_0_1||UNITY_4_1||UNITY_4_2||UNITY_4_3||UNITY_4_4||UNITY_4_5||UNITY_4_6||UNITY_4_7||UNITY_4_8||UNITY_4_9
namespace OnePStudio.AEPToUnity4
#else
namespace OnePStudio.AEPToUnity5
#endif
{
	public enum TangentMode
	{
		Editable = 0,
		Smooth = 1,
		Linear = 2,
		Stepped = Linear | Smooth,
	}
	
	public enum TangentDirection
	{
		Left,
		Right
	}
	
	
	public class KeyframeUtil {

		public static TangentMode GetTangleMode(string str)
		{
			if(str.ToLower().Contains("stepped"))
			{
				return TangentMode.Stepped;
			}
			else if(str.ToLower().Contains("editable"))
			{
				return TangentMode.Editable;
			}
			else if(str.ToLower().Contains("Smooth"))
			{
				return TangentMode.Smooth;
			}
			else
			{
				return TangentMode.Linear;
			}
		}

		public static Keyframe GetNew( float time, float value, TangentMode leftAndRight){
			return GetNew(time,value, leftAndRight,leftAndRight);
		}
		
		public static Keyframe GetNew(float time, float value, TangentMode left, TangentMode right){
			object boxed = new Keyframe(time,value); // cant use struct in reflection			
			
			SetKeyBroken(boxed, true);
			SetKeyTangentMode(boxed, 0, left);
			SetKeyTangentMode(boxed, 1, right);
			
			Keyframe keyframe = (Keyframe)boxed;
			if (left == TangentMode.Stepped )
				keyframe.inTangent = float.PositiveInfinity;
			if (right == TangentMode.Stepped )
				keyframe.outTangent = float.PositiveInfinity;
			
			return keyframe;
		}
		
		
		// UnityEditor.CurveUtility.cs (c) Unity Technologies
		public static void SetKeyTangentMode(object keyframe, int leftRight, TangentMode mode)
		{
			
			Type t = typeof( UnityEngine.Keyframe );
			FieldInfo field = t.GetField( "m_TangentMode", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance );
			int tangentMode =  (int)field.GetValue(keyframe);
			
			if (leftRight == 0)
			{
				tangentMode &= -7;
				tangentMode |= (int) mode << 1;
			}
			else
			{
				tangentMode &= -25;
				tangentMode |= (int) mode << 3;
			}
			
			field.SetValue(keyframe, tangentMode);
			if (GetKeyTangentMode(tangentMode, leftRight) == mode)
				return;
			Debug.Log("bug"); 
		}
		
		// UnityEditor.CurveUtility.cs (c) Unity Technologies
		public static TangentMode GetKeyTangentMode(int tangentMode, int leftRight)
		{
			if (leftRight == 0)
				return (TangentMode) ((tangentMode & 6) >> 1);
			else
				return (TangentMode) ((tangentMode & 24) >> 3);
		}
		
		// UnityEditor.CurveUtility.cs (c) Unity Technologies
		public static TangentMode GetKeyTangentMode(Keyframe keyframe, int leftRight)
		{
			Type t = typeof( UnityEngine.Keyframe );
			FieldInfo field = t.GetField( "m_TangentMode", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance );
			int tangentMode =  (int)field.GetValue(keyframe);
			if (leftRight == 0)
				return (TangentMode) ((tangentMode & 6) >> 1);
			else
				return (TangentMode) ((tangentMode & 24) >> 3);
		}
		
		
		// UnityEditor.CurveUtility.cs (c) Unity Technologies
		public static void SetKeyBroken(object keyframe, bool broken)
		{
			Type t = typeof( UnityEngine.Keyframe );
			FieldInfo field = t.GetField( "m_TangentMode", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance );
			int tangentMode =  (int)field.GetValue(keyframe);
			
			if (broken)
				tangentMode |= 1;
			else
				tangentMode &= -2;
			field.SetValue(keyframe, tangentMode);
		}
		
	}
	public static class CurveExtension {
		
		public static void UpdateAllLinearTangents(this AnimationCurve curve){
			for (int i = 0; i < curve.keys.Length; i++) {
				UpdateTangentsFromMode(curve, i);
			}
		}
		
		// UnityEditor.CurveUtility.cs (c) Unity Technologies
		public static void UpdateTangentsFromMode(AnimationCurve curve, int index)
		{
			if (index < 0 || index >= curve.length)
				return;
			Keyframe key = curve[index];
			if (KeyframeUtil.GetKeyTangentMode(key, 0) == TangentMode.Linear && index >= 1)
			{
				key.inTangent = CalculateLinearTangent(curve, index, index - 1);
				curve.MoveKey(index, key);
			}
			if (KeyframeUtil.GetKeyTangentMode(key, 1) == TangentMode.Linear && index + 1 < curve.length)
			{
				key.outTangent = CalculateLinearTangent(curve, index, index + 1);
				curve.MoveKey(index, key);
			}
			if (KeyframeUtil.GetKeyTangentMode(key, 0) != TangentMode.Smooth && KeyframeUtil.GetKeyTangentMode(key, 1) != TangentMode.Smooth)
				return;
			curve.SmoothTangents(index, 0.0f);
		}
		
		// UnityEditor.CurveUtility.cs (c) Unity Technologies
		private static float CalculateLinearTangent(AnimationCurve curve, int index, int toIndex)
		{
			return (float) (((double) curve[index].value - (double) curve[toIndex].value) / ((double) curve[index].time - (double) curve[toIndex].time));
		}
		
	}
}