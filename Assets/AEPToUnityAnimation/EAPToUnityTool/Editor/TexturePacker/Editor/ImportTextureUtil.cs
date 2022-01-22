/// <summary>
/// version 2.6
/// 2016-02-10
/// Copyright OnePStudio
/// mail: onepstudio@gmail.com
/// </summary>
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_4_0_0 ||UNITY_4_0 || UNITY_4_0_1||UNITY_4_1||UNITY_4_2||UNITY_4_3||UNITY_4_4||UNITY_4_5||UNITY_4_6||UNITY_4_7||UNITY_4_8||UNITY_4_9
namespace OnePStudio.AEPToUnity4
#else
namespace OnePStudio.AEPToUnity5
#endif
{
	public class ImportTextureUtil
	{
		public static Texture2D MakeTex(int width, int height, Color col)
		{
			Color[] pix = new Color[width*height];
			
			for(int i = 0; i < pix.Length; i++)
				pix[i] = col;
			
			Texture2D result = new Texture2D(width, height);
			result.SetPixels(pix);
			result.Apply();
			return result;
		}
		public static Texture2D[] MaxImportSettings(Texture2D[] imgs)
		{
			for(int s = 0; s < imgs.Length; s++)
			{
				if(AssetDatabase.GetAssetPath( (Texture2D)imgs[s]) != null)
				{
					TextureImporter tempImporter = TextureImporter.GetAtPath( AssetDatabase.GetAssetPath( (Texture2D)imgs[s]) ) as TextureImporter;
					tempImporter.isReadable = true;
					tempImporter.textureFormat = TextureImporterFormat.ARGB32;
					tempImporter.npotScale = TextureImporterNPOTScale.None;
					tempImporter.textureType = TextureImporterType.GUI;
					AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath((Texture2D)imgs[s]), ImportAssetOptions.ForceUpdate);
				}
			}
			return(imgs);
		}
		public static Texture2D MaxImportSettings(Texture2D img)
		{
			if(AssetDatabase.GetAssetPath( (Texture2D)img) != null)
			{
				TextureImporter tempImporter = TextureImporter.GetAtPath( AssetDatabase.GetAssetPath( (Texture2D)img) ) as TextureImporter;
				tempImporter.isReadable = true;
				tempImporter.textureFormat = TextureImporterFormat.ARGB32;
				tempImporter.npotScale = TextureImporterNPOTScale.None;
				tempImporter.textureType = TextureImporterType.Sprite;
				tempImporter.maxTextureSize=4096;
				AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath((Texture2D)img), ImportAssetOptions.ForceUpdate);
			}
			
			return(img);
		}

		public static Texture2D MaxImportSettings2(Texture2D img)
		{
			if(AssetDatabase.GetAssetPath( (Texture2D)img) != null)
			{
				TextureImporter tempImporter = TextureImporter.GetAtPath( AssetDatabase.GetAssetPath( (Texture2D)img) ) as TextureImporter;
				tempImporter.textureFormat = TextureImporterFormat.ARGB32;
				tempImporter.npotScale = TextureImporterNPOTScale.None;
				tempImporter.textureType = TextureImporterType.Sprite;
				tempImporter.isReadable = true;
				tempImporter.maxTextureSize=4096;

				AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath((Texture2D)img), ImportAssetOptions.ForceUpdate);
			}
			
			return(img);
		}
		public static Texture2D ReadAndUnScale(Texture2D img)
		{
			if(AssetDatabase.GetAssetPath( (Texture2D)img) != null)
			{
				TextureImporter tempImporter = TextureImporter.GetAtPath( AssetDatabase.GetAssetPath( (Texture2D)img) ) as TextureImporter;
				tempImporter.isReadable = true;
				tempImporter.textureFormat = TextureImporterFormat.ARGB32;
				tempImporter.npotScale = TextureImporterNPOTScale.None;
				tempImporter.maxTextureSize=4096;
				AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath((Texture2D)img), ImportAssetOptions.ForceUpdate);
				AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
			}
			
			return(img);
		}

		public static Texture2D MakeReadable(Texture2D img)
		{
			if(AssetDatabase.GetAssetPath( (Texture2D)img) != null)
			{
				TextureImporter tempImporter = TextureImporter.GetAtPath( AssetDatabase.GetAssetPath( (Texture2D)img) ) as TextureImporter;
				tempImporter.isReadable = true;
				AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath((Texture2D)img), ImportAssetOptions.ForceUpdate);
			}
			
			return(img);
		}
		public static Vector2 GetPivotFromMetaSprite(SpriteMetaData meta)
		{
			switch(meta.alignment)
			{
			case 0://center
				return new Vector2(0.5f,0.5f);
			case 1://topleft
				return new Vector2(0,1);
			case 2://top
				return new Vector2(0.5f,1);
			case 3://top Right
				return new Vector2(1,1);
			case 4://left
				return new Vector2(0,0.5f);
			case 5://right
				return new Vector2(1,0.5f);
			case 6://Bottom Left
				return new Vector2(0,0);
			case 7://Bottom 
				return new Vector2(0.5f,0);
			case 8://Bottom right
				return new Vector2(1,0);
			default:
				return meta.pivot;
			}
		}
		static public int GetAlignment(Vector2 vec)
		{
			float epsilon=0.0001f;
			int alignment=0;
			if(Mathf.Abs(vec.x-0.5f)<epsilon&&Mathf.Abs(vec.y-0.5f)<epsilon)//center
			{
				alignment=0;
			}
			else if(Mathf.Abs(vec.x)<epsilon&&Mathf.Abs(vec.y-1)<epsilon)//top left
			{
				alignment=1;
			}
			else if(Mathf.Abs(vec.x-0.5f)<epsilon&&Mathf.Abs(vec.y-1)<epsilon)//top
			{
				alignment=2;
			}
			else if(Mathf.Abs(vec.x-1)<epsilon&&Mathf.Abs(vec.y-1)<epsilon)//top Right
			{
				alignment=3;
			}
			else if(Mathf.Abs(vec.x-0)<epsilon && Mathf.Abs(vec.y-0.5f)<epsilon)//left
			{
				alignment=4;
			}
			else if(Mathf.Abs(vec.x-1)<epsilon&&Mathf.Abs(vec.y-0.5f)<epsilon)//left
			{
				alignment=5;
			}
			else if(Mathf.Abs(vec.x-0)<epsilon&&Mathf.Abs(vec.y-0)<epsilon)// bottom left
			{
				alignment=6;
			}
			else if(Mathf.Abs(vec.x-0.5f)<epsilon&&Mathf.Abs(vec.y-0)<epsilon)//bottom
			{
				alignment=7;
			}
			else if(Mathf.Abs(vec.x-1)<epsilon&&Mathf.Abs(vec.y-0)<epsilon)//bottom right
			{
				alignment=8;
			}
			else//custom
			{
				alignment=9;
			}
			return alignment;
		}
	}
}