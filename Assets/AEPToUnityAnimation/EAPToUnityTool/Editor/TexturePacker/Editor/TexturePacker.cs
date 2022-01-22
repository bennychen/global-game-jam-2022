/// <summary>
/// version 3.2
/// 2017-02-10
/// Copyright OnePStudio
/// mail: onepstudio@gmail.com
/// </summary>
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if UNITY_4_0_0 ||UNITY_4_0 || UNITY_4_0_1||UNITY_4_1||UNITY_4_2||UNITY_4_3||UNITY_4_4||UNITY_4_5||UNITY_4_6||UNITY_4_7||UNITY_4_8||UNITY_4_9
namespace OnePStudio.AEPToUnity4
#else
namespace OnePStudio.AEPToUnity5
#endif
{
	public class TexturePacker 
	{
		#region Update Atlas Sprite Info
		static public bool UpdateAtlasSpriteInfo(string pathOutput,List<DataAnimAnalytics> listAnim,float scale)
		{
			//Debug.LogError(scale);
			if(listAnim.Count<1)
				return false;

			Dictionary<string,EAPInfoAttachment> dicPivotCache=new Dictionary<string, EAPInfoAttachment>();
			for(int i=0;i<listAnim.Count;i++)
			{
				DataAnimAnalytics dataAnalytic=listAnim[i];
				//Debug.LogError(Pathfinding.Serialization.JsonFx.JsonWriter.Serialize(dataAnalytic.jsonFinal));
				foreach(KeyValuePair<string,EAPInfoAttachment> pair in dataAnalytic.jsonFinal.dicPivot)
				{
					dicPivotCache[pair.Value.spriteName]=pair.Value;
				}
			}

			Dictionary<string,List<EAPInfoAttachment>> dicPivot=new Dictionary<string, List<EAPInfoAttachment>>();
			for(int i=0;i<listAnim.Count;i++)
			{
				DataAnimAnalytics dataAnalytic=listAnim[i];
				foreach(KeyValuePair<string,EAPInfoAttachment> pair in dataAnalytic.jsonFinal.dicPivot)
				{

					List<EAPInfoAttachment> list=null;
					dicPivot.TryGetValue(pair.Value.spriteName,out list);
					if(list==null)
					{
						list=new List<EAPInfoAttachment>();
					}
					bool haveExist=false;
					for(int x=0;x<list.Count;x++)
					{
						if(list[x].spriteName==pair.Key)
						{
							haveExist=true;
							break;
						}
					}
					if(!haveExist)
						list.Add(pair.Value);
					dicPivot[pair.Value.spriteName]=list;
				}
			}

			TextureImporter ti = AssetImporter.GetAtPath(pathOutput) as TextureImporter;
			TextureImporterSettings settings = new TextureImporterSettings();
			ti.ReadTextureSettings(settings);
			SpriteMetaData[] lstMetaSprite=ti.spritesheet;
			Dictionary<string,SpriteMetaData> dicSpriteMeta=new Dictionary<string,SpriteMetaData>();
			bool haveNew=false;

			for(int i=0;i<lstMetaSprite.Length;i++)
			{
				SpriteMetaData spriteMetaData=lstMetaSprite[i];
				if (Mathf.Abs (scale - 1f) > Mathf.Epsilon)
				{
					Rect rect = spriteMetaData.rect;
					rect.x= rect.x * scale;
					rect.y=rect.y * scale;
					rect.width= rect.width * scale;
					rect.height=rect.height* scale;
					spriteMetaData.rect = rect;
					haveNew=true;
				}
				dicSpriteMeta[lstMetaSprite[i].name]=spriteMetaData;
			}
			foreach(KeyValuePair<string,List<EAPInfoAttachment>> pair in dicPivot)
			{
				List<EAPInfoAttachment> list=pair.Value;
				for(int i=0;i<list.Count;i++)
				{
					if(!dicSpriteMeta.ContainsKey(list[i].spriteName))// sprite new
					{
						if(dicSpriteMeta.ContainsKey(list[i].spriteName))
						{
							SpriteMetaData currentMeta=dicSpriteMeta[list[i].spriteName];
							SpriteMetaData metaSprite=new SpriteMetaData();
							metaSprite.name=list[i].spriteName;
							metaSprite.rect=currentMeta.rect;
							metaSprite.alignment=currentMeta.alignment;

							EAPInfoAttachment pivotCache=null;
							dicPivotCache.TryGetValue(list[i].spriteName,out pivotCache);
							if(pivotCache==null)
							{
								pivotCache=list[i];
							}
							//Debug.LogError(pivotCache.name+","+list[i].name+","+pivotCache.name+","+pivotCache.isOptimze);

							if(!pivotCache.isOptimze)
							{
								metaSprite.pivot=new Vector2(list[i].x,list[i].y);//currentMeta.pivot;
							}
							else
							{
								float pivotX=list[i].x*pivotCache.originalRect.width*scale;
								float pivotY=list[i].y*pivotCache.originalRect.height*scale;
								float oWidth=pivotCache.optimizeRect.width*scale;
								float oHeight=pivotCache.optimizeRect.height*scale;
								if(oWidth<1)
									oWidth=1;
								if(oHeight<1)
									oHeight=1;
								pivotX=pivotX-pivotCache.startX*scale;
								pivotY=pivotY-pivotCache.startY*scale;
								pivotX=pivotX/oWidth;
								pivotY=pivotY/oHeight;
								metaSprite.pivot=new Vector2(pivotX,pivotY);
							}
							dicSpriteMeta[metaSprite.name]=metaSprite;
							haveNew=true;
						}
					}
				}
			}
			if(haveNew)
			{
				Texture2D mainTexture=AssetDatabase.LoadAssetAtPath(pathOutput,typeof(Texture2D)) as Texture2D;
				lstMetaSprite=new SpriteMetaData[dicSpriteMeta.Count];
				int count=0;
				foreach(KeyValuePair<string,SpriteMetaData> pair in dicSpriteMeta)
				{
					lstMetaSprite[count]=pair.Value;
					count++;
				}
				ti.isReadable=true;
				ti.mipmapEnabled=false;
				ti.spritesheet=lstMetaSprite;
				ti.textureType=TextureImporterType.Sprite;
				ti.spriteImportMode=SpriteImportMode.Multiple;
				ti.spritePixelsPerUnit=100;
				settings.textureFormat = TextureImporterFormat.ARGB32;
				settings.npotScale = TextureImporterNPOTScale.None;
				settings.alphaIsTransparency = true;
				ti.SetTextureSettings(settings);
				ti.maxTextureSize=4096;
				ti.mipmapEnabled=false;
				ti.spriteImportMode=SpriteImportMode.Multiple;
				AssetDatabase.ImportAsset(pathOutput);
				EditorUtility.SetDirty(mainTexture);
				AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
				AssetDatabase.ImportAsset(pathOutput);
			}
			return true;
		}
		#endregion

		#region Build To each Texture
		static public bool BuildToEachTexture(List<Texture2D> listTexture,List<DataAnimAnalytics> listJsonAnim, TrimType trimType, string folderOutPut)
		{
			float prog =0.0f;
			EditorUtility.DisplayCancelableProgressBar("Collecting Textures", "Process...", prog);
			try
			{
				Dictionary<string,EAPInfoAttachment> dicPivot=new Dictionary<string, EAPInfoAttachment>();
				for(int i=0;i<listJsonAnim.Count;i++)
				{
					DataAnimAnalytics dataAnalytic=listJsonAnim[i];
					foreach(KeyValuePair<string,EAPInfoAttachment> pair in dataAnalytic.jsonFinal.dicPivot)
					{
						dicPivot[pair.Value.spriteName]=pair.Value;
						//Debug.LogError(pair.Value.name+","+pair.Value.spriteName);
					}
				}
				bool resultFinal=false;
				for(int i=0;i<listTexture.Count;i++)
				{
					List<SpriteElement> listSprite=new List<SpriteElement>();
					Object obj=listTexture[i];
					if(obj is Texture2D)
					{
						Texture2D tex=(Texture2D)obj;
						SpriteElement element=new SpriteElement(tex);
						if(trimType==TrimType.Trim2nTexture||trimType==TrimType.TrimMinimum)
						{
							if(!element.TrimTexture())
							{
								element.CloneFromOriginTexture();
							}
						}
						else
						{
							element.CloneFromOriginTexture();
						}
						foreach(KeyValuePair<string,EAPInfoAttachment> pair in dicPivot)
						{
							if(pair.Value.spriteName==tex.name)
							{
								element.SetPivot(new Vector2(pair.Value.x,pair.Value.y));
								break;
							}
						}
						string texturePath=folderOutPut+"/"+element.name+".png";

						if(trimType==TrimType.Trim2nTexture||trimType==TrimType.TrimMinimum)
						{
							listSprite.Add(element);
							if(listSprite.Count>0)
							{
								bool result= BuildAtlas(trimType,listSprite,dicPivot,texturePath,0);
								if(!resultFinal)
								{
									resultFinal=result;
								}
								// GC memory
								for(int ci=0;ci<listSprite.Count;ci++)
								{
									GameObject.DestroyImmediate(listSprite[ci].texture);
									listSprite[ci]=null;
								}
							}
						}
						else
						{
							JustSaveNew(element,texturePath);
							GameObject.DestroyImmediate(element.texture);
							if(!resultFinal)
							{
								resultFinal=true;
							}
						}
						prog =(float)(i+1)/listTexture.Count;
						if(EditorUtility.DisplayCancelableProgressBar("Build Textures", "Process...", prog))
						{
							return false;
						}
					}
					
				}
				return resultFinal;

			}
			catch(System.Exception ex)
			{
				Debug.LogError("Error:"+ex.Message);
				EditorUtility.ClearProgressBar();
				return false;
			}
		}
		#endregion

		#region void JustSave()
		public static void JustSaveNew(SpriteElement spriteElement,string texturePath)
		{
			#region Write New File
			byte[] byt = spriteElement.texture.EncodeToPNG();
			if (texturePath != "") 
			{
				System.IO.File.WriteAllBytes(texturePath, byt);
				AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);         
			}
			AssetDatabase.ImportAsset(texturePath);
			Texture2D mainTexture=AssetDatabase.LoadAssetAtPath(texturePath,typeof(Texture2D)) as Texture2D;
			TextureImporter ti = AssetImporter.GetAtPath(texturePath) as TextureImporter;
			TextureImporterSettings settings = new TextureImporterSettings();
			ti.ReadTextureSettings(settings);

			SpriteMetaData[] lstMetaSprite=new SpriteMetaData[1];
			SpriteMetaData metaSprite=new SpriteMetaData();
			metaSprite.name=spriteElement.name;
			metaSprite.rect=spriteElement.GetSpriteRect();
			metaSprite.pivot=spriteElement.pivot;
			metaSprite.alignment=spriteElement.alignment;
			lstMetaSprite[0]=metaSprite;

			ti.isReadable=true;
			ti.mipmapEnabled=false;
			ti.spritesheet=lstMetaSprite;
			ti.textureType=TextureImporterType.Sprite;
			ti.spriteImportMode=SpriteImportMode.Multiple;
			ti.spritePixelsPerUnit=100;
			settings.textureFormat = TextureImporterFormat.ARGB32;
			settings.npotScale = TextureImporterNPOTScale.None;
			settings.alphaIsTransparency = true;
			ti.SetTextureSettings(settings);
			ti.maxTextureSize=4096;
			ti.mipmapEnabled=false;
			ti.spriteImportMode=SpriteImportMode.Multiple;
			AssetDatabase.ImportAsset(texturePath);
			EditorUtility.SetDirty(mainTexture);
			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
			AssetDatabase.ImportAsset(texturePath);

			#endregion

		}
		#endregion

		#region Build Atlas From List Texture
		static public bool AutoBuildAtlasFromListTexture(List<Texture2D> listTexture,List<DataAnimAnalytics> listJsonAnim, TrimType trimType, string texturePath,int pading)
		{
			float prog =0.0f;
			EditorUtility.DisplayCancelableProgressBar("Collecting Textures", "Process...", prog);
			try
			{
				Dictionary<string,EAPInfoAttachment> dicPivot=new Dictionary<string, EAPInfoAttachment>();
				for(int i=0;i<listJsonAnim.Count;i++)
				{
					DataAnimAnalytics dataAnalytic=listJsonAnim[i];
					foreach(KeyValuePair<string,EAPInfoAttachment> pair in dataAnalytic.jsonFinal.dicPivot)
					{
						dicPivot[pair.Value.spriteName]=pair.Value;
					}
				}

				List<SpriteElement> listSprite=new List<SpriteElement>();
				for(int i=0;i<listTexture.Count;i++)
				{
					Object obj=listTexture[i];
					if(obj is Texture2D)
					{
						Texture2D tex=(Texture2D)obj;
			
						SpriteElement element=new SpriteElement(tex);
						if(trimType==TrimType.Trim2nTexture||trimType==TrimType.TrimMinimum)
						{
							if(!element.TrimTexture())
							{
								element.CloneFromOriginTexture();
							}
						}
						else
						{
							element.CloneFromOriginTexture();
						}
						foreach(KeyValuePair<string,EAPInfoAttachment> pair in dicPivot)
						{
							if(pair.Value.spriteName==tex.name)
							{
								//Debug.LogError(pair.Value.spriteName);
								element.SetPivot(new Vector2(pair.Value.x,pair.Value.y));
								break;
							}
						}
						listSprite.Add(element);
						prog =(float)(i+1)/listTexture.Count;
						EditorUtility.DisplayCancelableProgressBar("Collecting Textures", "Process...", prog);
					}
			
				}
				if(listSprite.Count>0)
				{
					bool result= BuildAtlas(trimType,listSprite,dicPivot,texturePath,pading);
					// GC memory
					for(int i=0;i<listSprite.Count;i++)
					{
						GameObject.DestroyImmediate(listSprite[i].texture);
						listSprite[i]=null;
					}
					return result;
				}
				return false;
			}
			catch(System.Exception ex)
			{
				Debug.LogError("Error:"+ex.Message);
				EditorUtility.ClearProgressBar();
				return false;
			}
		}
		#endregion

		#region Build Atlas
		static public bool BuildAtlas(TrimType trimType,List<SpriteElement> listSprite,Dictionary<string,EAPInfoAttachment> dicPivot,string texturePath,int padingSize,bool append=false)
		{
			try
			{
				//bool checkAppend=append;
				float prog =0.2f;
				EditorUtility.DisplayCancelableProgressBar("Creating Spritesheet", "Auto Build Atlas Sprites", prog);
				Texture2D[] textArray=new Texture2D[listSprite.Count];
				for(int i=0;i<textArray.Length;i++)
				{
					textArray[i]=listSprite[i].texture;
				}
				Texture2D mainTexture=new Texture2D(8192,8192);
					Rect[] rects = mainTexture.PackTextures(textArray,padingSize, 8192,false);
				mainTexture.Apply();
				//ImportTextureUtil.MaxImportSettings(mainTexture);
				int xmin =0;
				int ymin =0;
				int cacheWidth=mainTexture.width;
				int cacheHeight=mainTexture.height;

				int optimizeWidth=cacheWidth;
				int optimizeHeight=cacheHeight;
				Texture2D mainTexture2=null;
				prog =0.4f;
				EditorUtility.DisplayCancelableProgressBar("Creating Spritesheet", "Auto Build Atlas Sprites", prog);
				#region Trim to minimum Texture
				if(trimType==TrimType.TrimMinimum&&rects.Length>0)
				{
					float rectMinX=rects[0].xMin;
					float rectMinY=rects[0].yMin;
					float rectMaxX=rects[0].xMax;
					float rectMaxY=rects[0].yMax;
					for(int i=1;i<rects.Length;i++)
					{
						if(rects[i].xMin<rectMinX)
						{
							rectMinX=rects[i].xMin;
						}
						if(rects[i].yMin<rectMinY)
						{
							rectMinY=rects[i].yMin;
						}
						if(rects[i].xMax<rectMaxX)
						{
							rectMaxX=rects[i].xMax;
						}
						if(rects[i].yMax<rectMaxY)
						{
							rectMaxY=rects[i].yMax;
						}
					}
					int intRectMinX=(int)(rectMinX*cacheWidth);
					int intRectMinY=(int)(rectMinY*cacheHeight);
					int intRectMaxX=(int)(rectMaxX*cacheWidth);
					int intRectMaxY=(int)(rectMaxY*cacheHeight);

					Color32[] pixels = mainTexture.GetPixels32();
					xmin = mainTexture.width;
					int xmax = 0;
					ymin = mainTexture.height;
					int ymax = 0;
					int oldWidth = mainTexture.width;
					int oldHeight = mainTexture.height;
				
					// Trim solid pixels
					for (int y = 0, yw = oldHeight; y < yw; ++y)
					{
						for (int x = 0, xw = oldWidth; x < xw; ++x)
						{
							Color32 c = pixels[y * xw + x];
						
							if (c.a != 0)
							{
								if (y < ymin) ymin = y;
								if (y > ymax-1) ymax = y+1;
								if (x < xmin) xmin = x;
								if (x > xmax-1) xmax = x+1;
							}
						}
					}
					if(xmin>intRectMinX)
					{
						xmin=intRectMinX;
					}
					if(ymin>intRectMinY)
					{
						ymin=intRectMinY;
					}
					if(xmax<intRectMaxX)
					{
						xmax=intRectMaxX;
					}
					if(ymax<intRectMaxY)
					{
						ymax=intRectMaxY;
					}
					if(xmax-xmin>0&&ymax-ymin>0)
					{
						optimizeWidth=xmax-xmin;
						optimizeHeight=ymax-ymin;
						mainTexture2=new Texture2D(xmax-xmin,ymax-ymin);
						mainTexture2.SetPixels(mainTexture.GetPixels(xmin,ymin,xmax-xmin,ymax-ymin));
						mainTexture2.Apply();
						GameObject.DestroyImmediate(mainTexture);
						mainTexture=mainTexture2;
					}
				}
		
				#endregion

				prog =0.5f;
				EditorUtility.DisplayCancelableProgressBar("Creating Spritesheet", "Auto Build Atlas Sprites", prog);
				#region Write New File
				byte[] byt = mainTexture.EncodeToPNG();
				if (texturePath != "") 
				{
					System.IO.File.WriteAllBytes(texturePath, byt);
					AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);         
				}
				AssetDatabase.ImportAsset(texturePath);
				#endregion
				EditorUtility.ClearProgressBar();
				prog =0.6f;
				EditorUtility.DisplayCancelableProgressBar("Creating Spritesheet", "Auto Build Atlas Sprites", prog);
				mainTexture=AssetDatabase.LoadAssetAtPath(texturePath,typeof(Texture2D)) as Texture2D;
				TextureImporter ti = AssetImporter.GetAtPath(texturePath) as TextureImporter;
				TextureImporterSettings settings = new TextureImporterSettings();
				ti.ReadTextureSettings(settings);
				SpriteMetaData[] lstMetaSprite=new SpriteMetaData[listSprite.Count];
				if(append)
				{
					if(ti.spritesheet!=null&&ti.spritesheet.Length>0)
					{
						append=true;
						lstMetaSprite=ti.spritesheet;
					}
					else
					{
						append=false;
					}
				}
				for(int i=0;i<lstMetaSprite.Length;i++)
				{
					if(i<rects.Length)
					{
						SpriteMetaData metaSprite=new SpriteMetaData();
						if(append)
						{
							metaSprite=lstMetaSprite[i];
						}
						metaSprite.name=listSprite[i].name;
						Rect rectInfo=listSprite[i].GetSpriteRect();
				
						Rect rect=new Rect(rects[i].x*cacheWidth-xmin,rects[i].y*cacheHeight-ymin,rectInfo.width,rectInfo.height);
						if(rect.x+rect.width>optimizeWidth)
						{
							rect.width=optimizeWidth-rect.x;
						}
						if(rect.y+rect.height>optimizeHeight)
						{
							rect.height=optimizeHeight-rect.y;
						}
						metaSprite.rect=rect;
						int oWidth=listSprite[i].originalRect.width;
						int oHeight=listSprite[i].originalRect.height;
						if(oWidth<1)
							oWidth=1;
						if(oHeight<1)
							oHeight=1;
						int xLeft=listSprite[i].startX;
						int yTop=listSprite[i].startY;

						if(listSprite[i].IsOptimize())
						{
							float pivotX=listSprite[i].pivot.x*listSprite[i].originalRect.width;
							float pivotY=listSprite[i].pivot.y*listSprite[i].originalRect.height;
							pivotX=pivotX-xLeft;
							pivotY=pivotY-yTop;
							pivotX=pivotX/listSprite[i].optimizeRect.width;
							pivotY=pivotY/listSprite[i].optimizeRect.height;

							listSprite[i].SetPivot(new Vector2(pivotX,pivotY));
							metaSprite.pivot=new Vector2(pivotX,pivotY);
							metaSprite.alignment=ImportTextureUtil.GetAlignment(metaSprite.pivot);// listSprite[i].alignment;
							if(dicPivot!=null)
							{
								foreach(KeyValuePair<string,EAPInfoAttachment> pair in dicPivot)
								{
									if(pair.Value.spriteName==metaSprite.name)
									{
										pair.Value.SetCache(xLeft,yTop,listSprite[i].originalRect,listSprite[i].optimizeRect);
									}
								}
							}
						}
						else
						{
							metaSprite.pivot=listSprite[i].pivot;
							metaSprite.alignment=ImportTextureUtil.GetAlignment(metaSprite.pivot);
						}
						lstMetaSprite[i]=metaSprite;
					}
				}
				prog =0.7f;
				EditorUtility.DisplayCancelableProgressBar("Creating Spritesheet", "Auto Build Atlas Sprites", prog);
				ti.isReadable=true;
				ti.mipmapEnabled=false;
				ti.spritesheet=lstMetaSprite;
				ti.textureType=TextureImporterType.Sprite;
				ti.spriteImportMode=SpriteImportMode.Multiple;
				ti.spritePixelsPerUnit=100;
				settings.textureFormat = TextureImporterFormat.ARGB32;
				settings.npotScale = TextureImporterNPOTScale.None;
				settings.alphaIsTransparency = true;
				ti.SetTextureSettings(settings);
				ti.maxTextureSize=4096;
				ti.mipmapEnabled=false;
				ti.spriteImportMode=SpriteImportMode.Multiple;
				AssetDatabase.ImportAsset(texturePath);
				EditorUtility.SetDirty(mainTexture);
				AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
				AssetDatabase.ImportAsset(texturePath);
				prog =1.0f;
				EditorUtility.DisplayCancelableProgressBar("Creating Spritesheet", "Auto Build Atlas Sprites", prog);
				EditorUtility.ClearProgressBar();

				// douple setting for fix Unity 5.5
				ti.textureType=TextureImporterType.Sprite;
				ti.spriteImportMode=SpriteImportMode.Multiple;
				EditorUtility.SetDirty(mainTexture);
				AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
				AssetDatabase.ImportAsset(texturePath);

				/*for(int i=0;i<listSprite.Count;i++)
				{
					listSprite[i].FreeMemory();
				}*/
				System.GC.Collect();
				return true;
	
			}
			catch(UnityException ex)
			{
				Debug.LogError("Error:"+ex.Message);
				EditorUtility.ClearProgressBar();
				return false;
			}
			catch(System.Exception ex)
			{
				Debug.LogError("Error:"+ex.Message);
				EditorUtility.ClearProgressBar();
				return false;
			}
		}
		#endregion
	}
}
