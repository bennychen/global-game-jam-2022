/// <summary>
/// version 2.6
/// 2016-02-10
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
	public enum SpriteBuildStatus
	{
		NEW=1,
		OLD=2,
		UPDATE=3,
	}

	public class SpriteRawData
	{
		public Sprite sprite=null;
		public int hashId=0;
		public Vector2 pivot;
		public int alignment;
		public string name;
		public bool isAttact=true;
		public SpriteBuildStatus spriteStaus=SpriteBuildStatus.OLD;
		public SpriteRawData(Sprite _sprite,string _name,Vector2 _pivot, int _alignment)
		{
			this.name=_name;
			this.sprite=_sprite;
			if(sprite!=null)
			{
				this.hashId=sprite.GetInstanceID();
			}
			this.pivot=_pivot;
			this.alignment=_alignment;
			this.spriteStaus=SpriteBuildStatus.OLD;
			this.isAttact=true;
		}
	}
	public enum TrimType
	{
		Trim2nTexture	=1,
		TrimMinimum		=2,
		NotTrimming		=3
	}
	public class IntRect
	{
		public int x;
		public int y;
		public int width;
		public int height;
		public IntRect()
		{
			
		}
		public IntRect(int _x,int _y,int _w,int _h)
		{
			this.x=_x;
			this.y=_y;
			this.width=_w;
			this.height=_h;
		}
		public string ToText()
		{
			return "{"+x+","+y+","+width+","+height+"}";
		}
	}
	public class SpriteElement
	{
		public Texture2D texture;
		public IntRect originalRect;
		public IntRect optimizeRect;
		public int startX;
		public int startY;
		public string name;
		public int alignment;
		public Vector2 pivot;
		private bool isOptimize;
		private bool needSetAuthority;

		public bool IsOptimize()
		{
			return isOptimize;
		}
		public void SetPivot(Vector2 _vecPivot)
		{
			this.pivot=_vecPivot;
			this.alignment=ImportTextureUtil.GetAlignment(this.pivot);
		}
		/*public SpriteElement(SpriteRawData spriteRaw,TrimType trimType)
		{
			Rect rect=spriteRaw.sprite.rect;
			if(rect.width<1)
			{
				rect.width=1;
			}
			if(rect.height<1)
			{
				rect.height=1;
			}
			this.texture=new Texture2D((int)rect.width,(int)rect.height);

			texture.SetPixels(spriteRaw.sprite.texture.GetPixels((int)rect.x,(int)rect.y,(int)rect.width,(int)rect.height));
			texture.Apply();
			this.name=spriteRaw.name;
			this.originalRect=new IntRect(0,0,(int)rect.width,(int)rect.height);
			this.optimizeRect=new IntRect(0,0,(int)rect.width,(int)rect.height);
			this.isOptimize=false;
			this.startX=0;
			this.startY=0;
			this.alignment=spriteRaw.alignment;
			this.pivot=spriteRaw.pivot;
			if(trimType==TrimType.Trim2nTexture||trimType==TrimType.TrimMinimum)
			{
				TrimTexture(true);
			}
		}*/
		public SpriteElement(Texture2D _texture,bool _needSetAuthority=true)
		{
			this.needSetAuthority=_needSetAuthority;
			if(this.needSetAuthority)
			{
				ImportTextureUtil.MaxImportSettings(_texture);
			}
			this.texture=_texture;
			this.name=texture.name;
			this.originalRect=new IntRect(0,0,texture.width,texture.height);
			this.optimizeRect=new IntRect(0,0,texture.width,texture.height);
			this.isOptimize=false;
			this.startX=0;
			this.startY=0;
			this.alignment=0;// o giua
			this.pivot=new Vector2(0.5f,0.5f);//default
			if(this.needSetAuthority)
			{
				string path=AssetDatabase.GetAssetPath(texture);
				TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
				if(ti.textureType==TextureImporterType.Sprite)
				{
					if(ti.spriteImportMode==SpriteImportMode.Single)
					{
						this.pivot=ti.spritePivot;
						this.alignment=ImportTextureUtil.GetAlignment(this.pivot);
					}
					else
					{
						if(ti.spritesheet!=null&&ti.spritesheet.Length>0)
						{
							this.alignment=ti.spritesheet[0].alignment;
							this.pivot=ti.spritesheet[0].pivot;
						}
					}
				}
			}
		}
		public void CloneFromOriginTexture()
		{
			if(this.needSetAuthority)
			{
				ImportTextureUtil.MaxImportSettings(texture);
				ImportTextureUtil.ReadAndUnScale(texture);
				Texture2D text2=new Texture2D(texture.width,texture.height);
				text2.SetPixels(texture.GetPixels(0,0,texture.width,texture.height));
				text2.Apply();
				this.texture=text2;
			}

		}
		public bool TrimTexture(bool deleteOld=false)
		{

			if(this.needSetAuthority)
			{
				ImportTextureUtil.MakeReadable(texture);
			}
			Color32[] pixels = texture.GetPixels32();
			int xmin = texture.width;
			int xmax = 1;
			int ymin = texture.height;
			int ymax = 1;
			int oldWidth = texture.width;
			int oldHeight = texture.height;
			
			// Trim solid pixels
			for (int y = 0, yw = oldHeight; y < yw; y++)
			{
				for (int x = 0, xw = oldWidth; x < xw; x++)
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
			if(xmin>0||ymin>0||xmax<originalRect.width||ymax<originalRect.height)
			{
				// co the optimize dc
				isOptimize=true;
				int width=xmax-xmin;
				int height=ymax-ymin;
				bool emptyImage=false;
				if(width<0)
				{
					xmin=0;
					width=1;
					emptyImage=true;
				}
				if(height<0)
				{
					ymin=0;
					height=1;
					emptyImage=true;
				}
				Texture2D text2=new Texture2D(xmax-xmin,height);
				text2.SetPixels(texture.GetPixels(xmin,ymin,width,height));
				text2.Apply();
				if(deleteOld)
				{
					GameObject.DestroyImmediate(texture);
				}
				this.texture=text2;
				if(!emptyImage)
				{	
					this.optimizeRect=new IntRect(0,0,texture.width,texture.height);
				}
				else
				{
					this.optimizeRect=new IntRect(xmin,ymin,width,height);
				}
				this.startX=xmin;
				this.startY=ymin;
				return true;
			}
			return false;
		}
		public void FreeMemory()
		{
			texture=null;
		}
		public Rect GetSpriteRect()
		{
			return new Rect(optimizeRect.x,optimizeRect.y,optimizeRect.width,optimizeRect.height);
		}
	}
}
