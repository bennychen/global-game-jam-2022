/// <summary>
/// AEP to native unity animation. 
/// © OneP Studio
/// email: onepstudio@gmail.com
/// </summary>


// Using NGUI and have already import NGUI plugin need to define AE_NGUI for export to NGUI
//#define AE_NGUI
#pragma warning disable 0618
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Pathfinding.Serialization.JsonFx;

using UnityEngine.UI;

#if UNITY_4_0_0 ||UNITY_4_0 || UNITY_4_0_1||UNITY_4_1||UNITY_4_2||UNITY_4_3||UNITY_4_4||UNITY_4_5||UNITY_4_6||UNITY_4_7||UNITY_4_8||UNITY_4_9
using OnePStudio.AEPToUnity4;
#else
using OnePStudio.AEPToUnity5;
#endif

public class AEPToNativeUnityAnimation : EditorWindow {

	internal static AEPToNativeUnityAnimation instance;

	[SerializeField]
	public int tabBuildAtlas=0; //setting tab build atlas

	[SerializeField]
	public SpriteType buildSpriteType=SpriteType.SpriteRenderer; // setting mode export

	[SerializeField]
	public string pathImages=""; // path contain all image to build
	
	[SerializeField]
	public TrimType trimType=TrimType.Trim2nTexture; // mode trim atlas

	[SerializeField]
	public BuildAtlasType buildAtlasType=BuildAtlasType.ReferenceImage; // mode collection resource in image folder

	[SerializeField]
	public int fps=30;

	// Main Texture Atlas
	public Texture2D mainTexture; // main texture of altas

	[SerializeField]
	public List<TextAsset> listAnimationInfo=new List<TextAsset>(){null};// list animation json file that export form AE
	[SerializeField]
	public List<AnimationStyle> listAnimationLoop=new List<AnimationStyle>{AnimationStyle.Normal};// setting each animation mode

	private const string DEFAULT_OUTPUT="Assets/AEP Output"; //  default output folder export
	private string pathOutputs=DEFAULT_OUTPUT; // setting  output folder export

	// custom UI
	private bool showAnimationFiles=true;
	private Vector2 mScroll=Vector2.zero;

	[SerializeField]
	private string aepName="AEP Animation";// name export animation in scene

	[SerializeField]
	private string atlasName="AEP_Atlas"; // name of export atlas


	[SerializeField]
	public bool autoOverride=true; // mode setting override current export.
	[SerializeField]
	public SortLayerType sortType=SortLayerType.Depth;// setting sorting layer by depth or Z transform

	[SerializeField]
	public int defaultSortDepthValue=0;// default value when sorting at minimum

	[SerializeField]
	public int padingSize=1;// default pading atlas texture (better is 2,4), should <=8 to saving resources


	[SerializeField]
	public float scaleTextureImage=1;// using when resize atlas
	private int tabBuild=0;

	private float scaleInfo=100;// value of scale object create in scene


	// control Variable
	private List<DataAnimAnalytics> listInfoFinal=new List<DataAnimAnalytics>();
	private List<string> listPathNotAtlas=new List<string>();

	// temp cache variable
	private GameObject objectRoot;
	private SpriteType cacheType=SpriteType.SpriteRenderer;


#if UNITY_4_0_0 ||UNITY_4_0 || UNITY_4_0_1||UNITY_4_1||UNITY_4_2||UNITY_4_3||UNITY_4_4||UNITY_4_5||UNITY_4_6||UNITY_4_7||UNITY_4_8||UNITY_4_9
	[MenuItem("Window/AEPToUnity/Unity 4")]
#else
	[MenuItem("Window/AEPToUnity/Unity 5 or 2017,2018,2019,2020")]
#endif
	public static void CreateWindowOldVersion()
	{
		CreateWindow("AE2Unity");
	}
	
	[MenuItem("Window/AEPToUnity/Unity 2017")]
	public static void CreateWindow2017()
	{
		CreateWindow("AE2Unity2017");
	}
	
	[MenuItem("Window/AEPToUnity/Unity 2018")]
	public static void CreateWindow2018()
	{
		CreateWindow("AE2Unity2018");
	}
	
	[MenuItem("Window/AEPToUnity/Unity 2019")]
	public static void CreateWindow2019()
	{
		CreateWindow("AE2Unity2019");
	}
	
	[MenuItem("Window/AEPToUnity/Unity 2020")]
	public static void CreateWindow2020()
	{
		CreateWindow("AE2Unity2020");
	}
	
	[MenuItem("Window/AEPToUnity/Unity 2021")]
	public static void CreateWindow2021()
	{
		CreateWindow("AE2Unity2021");
	}
	
	public static void CreateWindow(string version)
	{
		instance = GetWindow<AEPToNativeUnityAnimation>();
		instance.title = "AE To Unity "+version;
		instance.minSize = new Vector2(380, 450);
		instance.Show();
		instance.ShowUtility();
		instance.autoRepaintOnSceneChange = false;
		instance.wantsMouseMove = false;
		//Application.RegisterLogCallback(instance.LogCallback);

	}
	#region UI Util
	static public void DrawHeader (string text)
	{
		GUILayout.Space(3f);
		GUILayout.BeginHorizontal();
		GUILayout.Space(3f);
		
		GUI.changed = false;
		
		text = "<b><size=11>" + text + "</size></b>";
		
		text = "\u25BC " + text;
		GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f));
		GUILayout.Space(2f);
		GUILayout.EndHorizontal();
	}
	static public void BeginContents ()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Space(4f);
		EditorGUILayout.BeginHorizontal(GUILayout.MinHeight(10f));
		GUILayout.BeginVertical();
		GUILayout.Space(2f);
	}
	static public void BeginContentsMaxHeight ()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Space(4f);
		EditorGUILayout.BeginHorizontal(GUILayout.MaxHeight(20000f));
		GUILayout.BeginVertical();
		GUILayout.Space(2f);
	}
	static public void EndContents ()
	{
		GUILayout.Space(3f);
		GUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(3f);
		GUILayout.EndHorizontal();
		GUILayout.Space(3f);
	}
	#endregion
	void OnGUI()
	{
		if (instance == null) 
			CreateWindow("AE2Unity");
		DrawToolbar();
	}


	#region Change Path Folder Image
	private void ChangePathImage()
	{
		pathImages= EditorUtility.OpenFolderPanel("Directory All Fire","","");
		//Debug.LogError(pathImages);
		string currentPath=Application.dataPath;
		if(pathImages.Length<1)
		{
			pathImages="";
		}
		else if(!pathImages.Contains(currentPath))
		{
			pathImages="";
			EditorUtility.DisplayDialog("Error","Please Choose Folder Images inside Project","OK");

		}
		else
		{
			pathImages="Assets"+pathImages.Replace(currentPath,"");
			//Debug.LogError(pathImages);
			string[] file=Directory.GetFiles(pathImages);
			bool hasTextureInside=false;
			for(int i=0;i<file.Length;i++)
			{
				//Debug.LogError(file[i]);
				UnityEngine.Object obj=AssetDatabase.LoadAssetAtPath(file[i],typeof(UnityEngine.Object));
				if(obj!=null)
				{
					if(obj is Texture2D)
					{
						hasTextureInside=true;
					}
				}
			}
			if(!hasTextureInside)
			{
				pathImages="";
				EditorUtility.DisplayDialog("Error","Can not found any image texture in this folder","OK");
			}
		}
	}
	#endregion 

	#region Get All reference Image Name
	private HashSet<string> GetAllReferenceImage()
	{
		HashSet<string> hash=new HashSet<string>();
		if(listInfoFinal!=null)
		{
			for(int i=0;i<listInfoFinal.Count;i++)
			{
				DataAnimAnalytics data=listInfoFinal[i];
				foreach(KeyValuePair<string,EAPInfoAttachment> pair in data.jsonFinal.dicPivot)
				{
					if(!hash.Contains(pair.Value.spriteName))
					{
						hash.Add(pair.Value.spriteName);
					}
				}
			}
		}
		return hash;
	}
	#endregion

	#region Choose directory output
	private void ChooseOutput()
	{
		pathOutputs= EditorUtility.OpenFolderPanel("Choose Directory For  Output Data","","");
		//Debug.LogError(pathOutputs);
		string currentPath=Application.dataPath;
		if(pathOutputs.Length<1)
		{
			pathOutputs=DEFAULT_OUTPUT;
		}
		else if(!pathOutputs.Contains(currentPath))
		{
			pathOutputs=DEFAULT_OUTPUT;
			EditorUtility.DisplayDialog("Error","Please Choose Dicrectory Folder inside Project","OK");
			
		}
		else
		{
			pathOutputs="Assets"+pathOutputs.Replace(currentPath,"");
		}
	}
	#endregion

	#region GUI Show Editor Choose OR Create Atlas
	private void GUIShowEditorChooseOrCreateAtlas()
	{
		GUI.color = Color.cyan; 
		GUILayout.Label("SETTING IMAGES ATLAS BUILDER:",EditorStyles.boldLabel); 
		GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(2)});
		GUI.color = Color.white;
		tabBuildAtlas = GUILayout.Toolbar(tabBuildAtlas, new string[] {"Build New Atlas Sprite", "Use Exists Atlas Sprite"});
		if(tabBuildAtlas==0) 
		{
			GUI.color = Color.white;
			GUIShowEditorCreateAtlas();
		}
		else
		{
			GUIShowEditorChooseAtlas();
		}
		GUI.color = Color.cyan; 
		GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(2)});
		GUI.color = Color.white;
	}
	#endregion

	#region GUIShowEditorChooseAtlas
	private void GUIShowEditorChooseAtlas()
	{
		EditorGUILayout.BeginHorizontal();
		{
			if(mainTexture!=null)
			{
				string path=AssetDatabase.GetAssetPath(mainTexture);
				TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
				if(ti.textureType!=TextureImporterType.Sprite|| ti.spriteImportMode!=SpriteImportMode.Multiple)
				{
					EditorUtility.DisplayDialog("Texture Input Warning","Please choose Texture Sprite in multiple mode","OK");
					mainTexture=null;
				}
			}

			GUI.color = Color.white;
			if(mainTexture==null)
			{
				EditorGUILayout.HelpBox("Atlas Texture Sprite:\n(only allow Texture Sprite in Sprite Mode Multiple)\n", MessageType.Warning);
			}
			else
			{
				EditorGUILayout.HelpBox("Atlas Texture Sprite:\n(only allow Texture Sprite in Sprite Mode Multiple)\n", MessageType.Info);
			}
			if(mainTexture==null)
			{
				GUI.color=Color.red;
			}
			else
			{
				GUI.color=Color.white;
			}
			mainTexture = EditorGUILayout.ObjectField("",mainTexture, typeof(Texture2D),true,new GUILayoutOption[]{GUILayout.Width(70.0f),GUILayout.Height(70.0f)}) as Texture2D;

		}

		EditorGUILayout.EndHorizontal();
		
		GUI.color=Color.white;
		scaleTextureImage=EditorGUILayout.FloatField(new GUIContent("Resize Texture Ratio: ","scaleTextureImage Scale, Default is 1"),scaleTextureImage,GUILayout.MaxWidth(2000));
		if(scaleTextureImage<0)
		{
			scaleTextureImage=0.1f;
		}

	}
	#endregion

	#region GUIShowEditorCreateAtlas
	private void GUIShowEditorCreateAtlas()
	{
		if(pathImages.Length<1)
		{
			EditorGUILayout.BeginHorizontal();
			{
				GUI.color = Color.red;
				EditorGUILayout.HelpBox("No Images Choose ! Please Choose \nDirectory Folder Path that contain all texture !", MessageType.Warning,true);
				if(GUILayout.Button("Open Directory", GUILayout.Height(37f)))
				{
					ChangePathImage();
				}
			}
			EditorGUILayout.EndHorizontal();
		}
		else
		{
			BeginContents();
			EditorGUILayout.BeginHorizontal();
			{
				GUI.color = Color.white;
				EditorGUILayout.HelpBox("Directory Choose: "+ pathImages, MessageType.Info,true);
				//GUI.color = Color.yellow;
				if(GUILayout.Button("Change Directory", GUILayout.Height(37f)))
				{
					ChangePathImage();
				}

			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			{
				GUI.color = Color.white;
				EditorGUILayout.LabelField("Choose Option to Build Atlas:",EditorStyles.boldLabel);
				buildAtlasType= (BuildAtlasType)EditorGUILayout.EnumPopup(buildAtlasType, "DropDown", new GUILayoutOption[]{GUILayout.Width(150f),GUILayout.Height(20)});
			}
			EditorGUILayout.EndHorizontal();


			EditorGUILayout.BeginHorizontal();
			{
				GUI.color = Color.white;
				EditorGUILayout.LabelField("Choose Option Trimming:",EditorStyles.boldLabel);
				trimType= (TrimType)EditorGUILayout.EnumPopup(trimType, "DropDown", new GUILayoutOption[]{GUILayout.Width(150f),GUILayout.Height(20)});
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			{
				GUI.color = Color.white;
				padingSize=EditorGUILayout.IntField("Padding Size:",padingSize,new GUILayoutOption[]{GUILayout.MaxWidth(2000)});
				if(padingSize<1)
					padingSize=1;
				if(padingSize>8)
					padingSize=8;
			}
			EditorGUILayout.EndHorizontal();

			EndContents();
		}
		


	}
	#endregion

	#region Show Animation File Input
	private void GUIShowAnimationInfo()
	{
		GUI.color = Color.cyan;  
		GUILayout.Label("INPUT ANIMATIONS JSON FILE:",EditorStyles.boldLabel);
		GUI.color = Color.white;
		GUI.color = Color.white;
		EditorGUILayout.BeginHorizontal();
		{
			showAnimationFiles=EditorGUILayout.Foldout(showAnimationFiles,"List Animation Json Files");
		}
		EditorGUILayout.EndHorizontal();

		if(showAnimationFiles)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("\t",GUILayout.Width(20));
			int size=listAnimationInfo.Count;
			size=Mathf.Clamp(EditorGUILayout.IntField("Size:",size,GUILayout.MaxWidth(1000f)), 0, 50);
			if(size<0)
			{
				size=0;
			}
			if(size!=listAnimationInfo.Count)
			{
				if(size==0)
				{
					listAnimationInfo.Clear();
					listAnimationLoop.Clear();
				}
				else
				{
					if(size>listAnimationInfo.Count)
					{
						for(int i=listAnimationInfo.Count;i<size;i++)
						{
							listAnimationInfo.Add(null);
							listAnimationLoop.Add(AnimationStyle.Normal);
						}
					}
					else
					{
						int total=listAnimationInfo.Count;
						for(int i=size;i<total;i++)
						{
							listAnimationInfo.RemoveAt(listAnimationInfo.Count-1);
							listAnimationLoop.RemoveAt(listAnimationLoop.Count-1);
						}
					}
				}
			}
			EditorGUILayout.EndHorizontal();
			for(int i=0;i<listAnimationInfo.Count;i++)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("\t",GUILayout.Width(20));
				listAnimationInfo[i]=EditorGUILayout.ObjectField("Element "+(i+1).ToString(),listAnimationInfo[i], typeof(TextAsset),true,GUILayout.MaxWidth(1000f)) as TextAsset;
				
				listAnimationLoop[i]= (AnimationStyle)EditorGUILayout.EnumPopup(listAnimationLoop[i],GUILayout.Width(50));
				EditorGUILayout.EndHorizontal();
			}
		}
	}
	#endregion

	#region Show OutputData
	private void ShowOutputData()
	{
		GUI.color = Color.cyan;  
		GUILayout.Label("OUTPUT SETTING:",EditorStyles.boldLabel);
		GUI.color = Color.white;
		EditorGUILayout.BeginHorizontal();
		{
			EditorGUILayout.HelpBox("Choose Directory for all data output !\nCurrent: "+pathOutputs, MessageType.None,true);
			if(GUILayout.Button("Change Directory", new GUILayoutOption[]{GUILayout.Height(30f),GUILayout.Width(150.0f)}))
			{
				ChooseOutput();
			}
		}


		EditorGUILayout.EndHorizontal();

		fps=EditorGUILayout.IntField(new GUIContent("Samples FPS Animation","default is 30"),fps,GUILayout.MaxWidth(2000));
		if(fps<1)
		{
			fps=1;
		}
		if(fps>60)
		{
			fps=60;
		}

		buildSpriteType=(SpriteType)EditorGUILayout.EnumPopup(new GUIContent("Export Layer Type","Object Generate Scale, Default is 100"),buildSpriteType,GUILayout.MaxWidth(2000));
		if (buildSpriteType == SpriteType.NGUI) {
			EditorGUILayout.HelpBox("To export NGUI, please make sure you had already import NGUI plugins", MessageType.Warning,true);
		}

		//scaleInfo=EditorGUILayout.FloatField(new GUIContent("Scale Object Create","Object Generate Scale, Default is 100"),scaleInfo,GUILayout.MaxWidth(2000));
		if (buildSpriteType == SpriteType.SpriteRenderer) {
			scaleInfo=EditorGUILayout.FloatField(new GUIContent("Scale SpriteRenderer","Object Generate Scale, Default is 100"),scaleInfo,GUILayout.MaxWidth(2000));
			
		}
		else if(buildSpriteType == SpriteType.UGUI)
		{
			scaleInfo=EditorGUILayout.FloatField(new GUIContent("Scale UGUI Image","Object Generate Scale, Default is 1"),scaleInfo,GUILayout.MaxWidth(2000));
		}
		else{
			scaleInfo=EditorGUILayout.FloatField(new GUIContent("Scale NGUI Image","Object Generate Scale, Default is 1"),scaleInfo,GUILayout.MaxWidth(2000));
		}


		if(scaleInfo<0||cacheType != buildSpriteType) {
			cacheType=buildSpriteType;
			if (buildSpriteType == SpriteType.SpriteRenderer) {
				scaleInfo = 100;
			} else {
				scaleInfo = 1;
			}
		}
		aepName=EditorGUILayout.TextField("Name Object Create",aepName,GUILayout.MaxWidth(2000));
		if(aepName.Length==0)
		{
			aepName="AEP Animation";
		}
		atlasName=EditorGUILayout.TextField("Name Atlas Image Create ",atlasName,GUILayout.MaxWidth(2000));
		if(atlasName.Length==0)
		{
			atlasName="AEP_Atlas";
		}
		
		defaultSortDepthValue=EditorGUILayout.IntField("Default Sorting Layer Value:",defaultSortDepthValue,GUILayout.MaxHeight(2000));
		EditorGUILayout.BeginHorizontal();
		autoOverride=EditorGUILayout.Toggle("Auto Override Output Data",autoOverride,GUILayout.MaxWidth(2000));
		GUILayout.Label("Sort Layer By",GUILayout.Width(75));
		sortType=(SortLayerType)EditorGUILayout.EnumPopup(sortType,GUILayout.Width(60));
		EditorGUILayout.EndHorizontal();
	}
	#endregion

	#region Show Setting Build Animation
	private void ShowSettingBuildAnimation()
	{
		GUI.color = Color.cyan;  
		GUILayout.Label("BUILD SETTING",EditorStyles.boldLabel);
		GUI.color = Color.white; 
		tabBuild = GUILayout.Toolbar(tabBuild, new string[] {"Auto Build", "Custom Build"});
		if(tabBuild==0) 
		{
			GUI.color = Color.green; 
			if(GUILayout.Button("BUILD", new GUILayoutOption[]{GUILayout.Height(40f),GUILayout.MaxWidth(2000.0f)}))
			{
				if(Mathf.Abs(scaleTextureImage-1)>Mathf.Epsilon)
				{
					if(tabBuildAtlas==1)
					{
						if(!EditorUtility.DisplayDialog("Warning","Your Scale Texture Image is not 1, Make sure you scale atlas image by yourseft first?.\n Press YES to continue","YES","Let's me resize image first"))
						{
							return;
						}
					}
				}
				bool result=false;
				result=BuildSprite();
				if(result)
				{
					result=CreateBoneSkeleton();
				}
				if(result)
				{
					result=BuildAnimation();
				}
				if(result)
				{
					scaleTextureImage=1;
					EditorUtility.DisplayDialog("Finish","Skeleton "+aepName+" had created in scene, all files reference in "+pathOutputs,"OK");
				}
			}
		}
		else
		{
			GUI.color = Color.green; 
			/*EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Read Input Info", new GUILayoutOption[]{GUILayout.Height(40f),GUILayout.MaxWidth(2000.0f)}))
			{
				ReadInfo();
			}
			EditorGUILayout.EndHorizontal();*/

			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Build Atlas Sprite", new GUILayoutOption[]{GUILayout.Height(40f),GUILayout.MaxWidth(2000.0f)}))
			{
				BuildSprite();
			}
			if(GUILayout.Button("Build Skeleton", new GUILayoutOption[]{GUILayout.Height(40f),GUILayout.MaxWidth(2000.0f)}))
			{
				CreateBoneSkeleton();
			}
			if(GUILayout.Button("Build Animation", new GUILayoutOption[]{GUILayout.Height(40f),GUILayout.MaxWidth(2000.0f)}))
			{
				BuildAnimation();
			}
			EditorGUILayout.EndHorizontal();
		}
	}
	#endregion

	void DrawToolbar()
	{
		mScroll = GUILayout.BeginScrollView(mScroll);
		EditorGUILayout.BeginVertical();
		GUI.color = Color.green;  
		GUILayout.Label("CONVERT ADOBE AFTER EFFECT TO NATIVE UNITY ANIMATION",EditorStyles.boldLabel);
		GUI.color = Color.cyan; 
		GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(2)});

		GUI.color = Color.white;
		GUIShowEditorChooseOrCreateAtlas();
		GUIShowAnimationInfo();
		GUI.color = Color.cyan; 
		GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(2)});
		GUI.color = Color.white;
		ShowOutputData();
		GUI.color = Color.cyan; 
		GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(2)});
		ShowSettingBuildAnimation();
		          
		EditorGUILayout.EndVertical();
		GUILayout.EndScrollView();

	}
	void Test()
	{
		AnimationClip anim=AssetDatabase.LoadAssetAtPath("Assets/Test/Testing.anim",typeof(AnimationClip)) as AnimationClip;
		if(anim==null)
		{
			Debug.LogError("Anim Null");
		}
		else
		{
			AnimationClipCurveData[] clipData= AnimationUtility.GetAllCurves(anim,true);
			Debug.LogError(JsonWriter.Serialize(clipData));
			EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings (anim);
			Debug.LogError(JsonWriter.Serialize(curveBindings));
		}
	}

	#region Read Info
	private bool ReadInfo(bool isShowError=true)
	{
		
		bool error=false;
		if(listAnimationInfo.Count<1)
		{
			error=true;
		}
		else
		{
			listInfoFinal.Clear();
			for(int i=0;i<listAnimationInfo.Count;i++)
			{
				TextAsset textAsset=listAnimationInfo[i];
				if(textAsset!=null)
				{				
					#region File Anim
						AnimationStyle stype=AnimationStyle.Normal;
						if(i<listAnimationLoop.Count)
						{
							stype=listAnimationLoop[i];
						}
						string json=textAsset.text;
						string filename=AssetDatabase.GetAssetPath(textAsset);
						int index=filename.LastIndexOf("/");
						filename=filename.Substring(index+1,filename.Length-index-6);// subject .json
						RawAEPJson jsonRaw = JsonReader.Deserialize(json, typeof(RawAEPJson)) as RawAEPJson;
						AEPJsonFinal jsonFinal=new AEPJsonFinal(jsonRaw);
						if(jsonFinal!=null)
						{
							listInfoFinal.Add(new DataAnimAnalytics(jsonFinal,filename,stype));
						}
						float percent=(i+1)/listAnimationInfo.Count;
						EditorUtility.DisplayCancelableProgressBar("Read Animation Json Info","processing...", percent);

						#endregion
				}
			}
		}

		EditorUtility.ClearProgressBar();
		if(error)
		{
			if(isShowError)
			{
				EditorUtility.DisplayDialog("Error Input Data","Data Animation is not correct format !","OK");
			}
			return false;
		}
		return true;
	}
	#endregion

	#region Build SpriteRenderer
	private bool BuildSprite()
	{
		Debug.Log("BuildSprite");
		if(!Directory.Exists(pathOutputs))
		{
			Directory.CreateDirectory(pathOutputs);	
		}
		if(!ReadInfo())
		{
			return false;
		}

		if(tabBuildAtlas==0)
		{
			if(pathImages.Length<1)
			{
				EditorUtility.DisplayDialog("Error Input Data","Folder directory Path is empty !","OK");
				return false;
			}
			string[] file=Directory.GetFiles(pathImages);
			bool hasTextureInside=false;
			HashSet<string> hashRefImage=GetAllReferenceImage();
			if(buildAtlasType==BuildAtlasType.AllImageInDirectory||
			   buildAtlasType==BuildAtlasType.ReferenceImage)
			{
				List<Texture2D> listTextureBuildAtlas=new List<Texture2D>();
				for(int i=0;i<file.Length;i++)
				{
					UnityEngine.Object obj=AssetDatabase.LoadAssetAtPath(file[i],typeof(UnityEngine.Object));
					if(obj!=null)
					{
						if(obj is Texture2D)
						{
							Texture2D textureFol=(Texture2D)obj;
							if(buildAtlasType==BuildAtlasType.AllImageInDirectory)
							{
								hasTextureInside=true;
								listTextureBuildAtlas.Add(textureFol);
							}
							else if(buildAtlasType==BuildAtlasType.ReferenceImage)
							{
								if(hashRefImage.Contains(textureFol.name))
								{
									hasTextureInside=true;
									listTextureBuildAtlas.Add(textureFol);
								}
								else
								{
									Debug.LogError("khong co ref:"+textureFol.name);
								}
							}
						}
					}
				}
				if(!hasTextureInside)
				{
					pathImages="";
					EditorUtility.DisplayDialog("Error Input Data","Can not found any image texture in folder directory "+pathImages,"OK");
				}
				string pathAtlas=pathOutputs+"/"+atlasName+".png";
				if(File.Exists(pathAtlas))
				{
					File.Delete(pathAtlas);
				}
				bool result =TexturePacker.AutoBuildAtlasFromListTexture(listTextureBuildAtlas,listInfoFinal,trimType,pathAtlas,padingSize);
				if(result && listInfoFinal.Count>0)
				{
					result=TexturePacker.UpdateAtlasSpriteInfo(pathAtlas,listInfoFinal,1);
				}
				UnityEngine.Object obj2=AssetDatabase.LoadAssetAtPath(pathAtlas,typeof(UnityEngine.Object));
				if(obj2!=null)
				{
					if(obj2 is Texture2D)
					{
						mainTexture=(Texture2D)obj2;
					}
				}
				return result;
			}
			else if(buildAtlasType==BuildAtlasType.NotBuildAtlas)
			{
				List<Texture2D> listRefImage=new List<Texture2D>();

				for(int i=0;i<file.Length;i++)
				{
					UnityEngine.Object obj=AssetDatabase.LoadAssetAtPath(file[i],typeof(UnityEngine.Object));
					if(obj!=null)
					{
						if(obj is Texture2D)
						{
							Texture2D textureFol=(Texture2D)obj;
							if(hashRefImage.Contains(textureFol.name))
							{
								hasTextureInside=true;
								listRefImage.Add(textureFol);
							}
							else
							{
								Debug.LogError("khong co ref:"+textureFol.name);
							}

						}
					}
				}
				bool result=TexturePacker.BuildToEachTexture(listRefImage,listInfoFinal,trimType,pathOutputs);
				listPathNotAtlas.Clear();
				if(result)
				{
					for(int i=0;i<listRefImage.Count;i++)
					{
						listPathNotAtlas.Add(pathOutputs+"/"+listRefImage[i].name+".png");
					}
				}
				return result;
			}
			else
			{
				Debug.LogError("Not Support Yet:");
				return false;

			}
		}
		else
		{
			if(mainTexture==null)
			{
				EditorUtility.DisplayDialog("Error Input Data","Texture Sprite is Null, Please choose text Atlas Sprite first !","OK");
				return false;
			}
			if(listInfoFinal.Count<1)
			{
				EditorUtility.DisplayDialog("Error Input Data","Animation json input files are empty!","OK");
				return false;
			}
			string pathAtlas=AssetDatabase.GetAssetPath(mainTexture);

			bool result=TexturePacker.UpdateAtlasSpriteInfo(pathAtlas,listInfoFinal,scaleTextureImage);
			return result;
		}
	}
	#endregion

	#region Build Bone
	private bool CreateBoneSkeleton()
	{
		#region setting conditional
		Debug.Log("CreateBoneSkeleton");
		if(!Directory.Exists(pathOutputs))
		{
			Directory.CreateDirectory(pathOutputs);	
		}
		ReadInfo(false);
		
		if(listInfoFinal.Count<1)
		{
			EditorUtility.DisplayDialog("Error Input Data","Animation Json Files input are empty!","OK");
			return false;
		}
		Dictionary<string,Sprite> dicSprite=new Dictionary<string, Sprite>();
		
		if(buildAtlasType==BuildAtlasType.AllImageInDirectory||
		   buildAtlasType==BuildAtlasType.ReferenceImage)
		{
			string pathAtlas=pathOutputs+"/"+atlasName+".png";
			if(tabBuildAtlas==1)
			{
				if(mainTexture==null)
				{
					EditorUtility.DisplayDialog("Error Input Data","Texture Sprite is Null, Please choose text Atlas Sprite first !"+pathImages,"OK");
					return false;
				}
				pathAtlas=AssetDatabase.GetAssetPath(mainTexture);
			}
			
			Sprite[] spritesTemp = AssetDatabase.LoadAllAssetsAtPath(pathAtlas)
				.OfType<Sprite>().ToArray();
			for(int i=0;i<spritesTemp.Length;i++)
			{
				dicSprite[spritesTemp[i].name]=spritesTemp[i];
			}
		}
		else if(buildAtlasType==BuildAtlasType.NotBuildAtlas)
		{
			for(int x=0;x<listPathNotAtlas.Count;x++)
			{
				string pathAtlas=listPathNotAtlas[x];
				Sprite[] spritesTemp = AssetDatabase.LoadAllAssetsAtPath(pathAtlas)
					.OfType<Sprite>().ToArray();
				for(int i=0;i<spritesTemp.Length;i++)
				{
					dicSprite[spritesTemp[i].name]=spritesTemp[i];
				}
				//Debug.LogError(pathAtlas);
			}
		}
		
		#endregion
		GameObject canvasUI = null;
		GameObject transObj = null;
		if (buildSpriteType == SpriteType.UGUI) {
			canvasUI=GameObject.Find("CanvasUI");
			if(canvasUI==null){
				canvasUI=new GameObject();
				canvasUI.name="CanvasUI";
				Canvas canvas=canvasUI.AddComponent<Canvas>();
				canvas.renderMode=RenderMode.WorldSpace;
				CanvasScaler canvasScaler=canvasUI.AddComponent<CanvasScaler>();
				canvasScaler.uiScaleMode=CanvasScaler.ScaleMode.ConstantPhysicalSize;
				canvasUI.AddComponent<GraphicRaycaster>();
			}
			if (canvasUI != null) {
				transObj = GameObject.Find (aepName);
			}
		}
		#if AE_NGUI
		else if (buildSpriteType == SpriteType.NGUI) {
			canvasUI=GameObject.Find("2DUI");
			if(canvasUI==null){
				canvasUI=new GameObject();
				canvasUI.name="2DUI";
				UIPanel canvas=canvasUI.AddComponent<UIPanel>();
				UIRoot root=canvasUI.AddComponent<UIRoot>();
				root.scalingStyle = UIRoot.Scaling.Flexible;
				root.minimumHeight = 320;
				root.manualHeight = 4096;
				Rigidbody regidbody=canvasUI.AddComponent<Rigidbody>();
				regidbody.mass=1;
				regidbody.angularDrag=0.05f;
				regidbody.isKinematic=true;
				regidbody.angularDrag=0;
				regidbody.collisionDetectionMode=CollisionDetectionMode.Discrete;
				regidbody.interpolation=RigidbodyInterpolation.None;
				//add camera
				//				Camera camera=canvasUI.GetComponentInChildren<Camera>();
				//				if(camera==null){
				//					Debug.LogError("Here");
				//					GameObject obj=new GameObject();
				//					obj.transform.SetParent(canvasUI.transform);
				//					obj.transform.localPosition=Vector3.zero;
				//					obj.transform.localScale=Vector3.one;
				//					camera=obj.AddComponent<Camera>();
				//					camera.orthographic=true;
				//					UICamera uiCamera=obj.AddComponent<UICamera>();
				//				}
			}
			if (canvasUI != null) {
				if(canvasUI.transform.Find (aepName)!=null)
				{
					transObj = canvasUI.transform.Find (aepName).gameObject;
				}
				if(transObj==null)
				{
					transObj = GameObject.Find (aepName);
				}
			}
		}
		#endif
		else {
			transObj= GameObject.Find(aepName);
		}
		
		#region remove Object
		if(transObj!=null)
		{
			if(!autoOverride)
			{
				if(!EditorUtility.DisplayDialog("Warning","Object "+aepName+" has exist in scene,Do you want replace this file?","YES","NO"))
				{
					return false;
				}
				else
				{
					GameObject.DestroyImmediate(transObj);
					transObj=null;
				}
			}
			else
			{
				GameObject.DestroyImmediate(transObj);
				transObj=null;
			}
		}
		#endregion
		
		//cache data
		Dictionary<string,GameObject> cache=new Dictionary<string, GameObject>();
		
		Dictionary<string,string> objHideForAllAnim=new Dictionary<string, string>();
		Dictionary<string,string> objShowForAllAnim=new Dictionary<string, string>();
		
		GameObject rootSkeleton;
		for(int x=0;x<listInfoFinal.Count*2;x++)//quet lai 2 lan
		{
			rootSkeleton=transObj= GameObject.Find(aepName);
			DataAnimAnalytics dataAnimAnalytics=listInfoFinal[x%listInfoFinal.Count];
			Dictionary<string,string> objHideWhenStartAnim=new Dictionary<string, string>();
			Dictionary<string,string> objShowWhenStartAnim=new Dictionary<string, string>();
			//Dictionary<string,string> objForAttachment=new Dictionary<string, string>();
			
			if(transObj!=null)
			{
				cache.Clear();
				for(int i=0;i<dataAnimAnalytics.jsonFinal.bones.Count;i++)
				{
					BoneElement bone=dataAnimAnalytics.jsonFinal.bones[i];
					GameObject obj=AEPAnimationClipElement.GetRefenreceObject(dataAnimAnalytics.jsonFinal.GetFullPathBone(bone.name),transObj);
					if(obj!=null)
					{
						cache[bone.name]=obj;
					}
				}
			}
			
			#region An nhung doi tuong khong co trong anim
			if(transObj!=null)
			{
				Transform[] allTrans=transObj.GetComponentsInChildren<Transform>(true);
				for(int i=0;i<allTrans.Length;i++)
				{
					GameObject obj=allTrans[i].gameObject;
					if(transObj!=allTrans[i].gameObject)// chi lay pivot
					{
						if(buildSpriteType==SpriteType.SpriteRenderer)
						{
							if(obj.GetComponent<SpriteRenderer>()!=null)//khong lay sprite
							{
								continue;
							}
						}
						#if AE_NGUI
						else if(buildSpriteType==SpriteType.NGUI)
						{
							if(obj.GetComponent<UI2DSprite>()!=null)//khong lay UIsprite
							{
								continue;
							}
						}
						#endif
						else
						{
							if(obj.GetComponent<Image>()!=null)//khong lay sprite
							{
								continue;
							}
						}
						bool isSlot=false;
						for (int xx= 0; xx < transObj.transform.childCount; xx++)
						{
							Transform traChild=transObj.transform.GetChild(xx);
							if(traChild.GetComponent<Image>()!=null)
							{
								isSlot=true;
								break;
							}
						}
						if(isSlot)
						{
							continue;
						}
						if(!dataAnimAnalytics.jsonFinal.dicBones.ContainsKey(allTrans[i].name))
						{
							string fullPath=allTrans[i].name;
							Transform tran=allTrans[i];
							while(true)
							{
								if(tran.parent==null||tran.parent==transObj.transform)
								{
									break;
								}
								else
								{
									fullPath=tran.parent.name+"/"+ fullPath;
									tran=tran.parent;
								}
							}
							//hide object 
							objHideWhenStartAnim[allTrans[i].name]=fullPath;
						}
					}
				}
			}
			#endregion
			
			#region Build New Bone
			for(int i=0;i<dataAnimAnalytics.jsonFinal.bones.Count;i++)
			{
				BoneElement bone=dataAnimAnalytics.jsonFinal.bones[i];
				if(bone.parent==null)
				{
					if(transObj==null)
					{
						GameObject obj=new GameObject();
						if(buildSpriteType==SpriteType.UGUI)
						{
							obj.transform.parent=canvasUI.transform;
							obj.AddComponent<RectTransform>();
						}
						if(buildSpriteType==SpriteType.NGUI)
						{
							obj.transform.SetParent(canvasUI.transform);
							obj.transform.localScale=Vector3.one;
						}
						rootSkeleton=obj;
						obj.transform.localPosition=new Vector3(0,0,0);
						obj.name=bone.name;
						cache[bone.name]=obj;
						objectRoot=obj;
						if(aepName.Length>0)
						{
							obj.name=aepName;
						}
						List<SlotElelement> listSlot=dataAnimAnalytics.jsonFinal.GetSlotMappingWithBone(bone.name);
						for(int xx=0;xx<listSlot.Count;xx++)
						{
							BuildSlotAndAttachment(listSlot[xx],obj,rootSkeleton,dicSprite, dataAnimAnalytics,buildSpriteType, ref objHideForAllAnim,ref objShowForAllAnim);
						}
						
					}
				}
				else 
				{
					GameObject parent=null;
					if(cache.TryGetValue(bone.parent,out parent))
					{
						GameObject objReference=null;
						cache.TryGetValue(bone.name, out objReference);
						if(objReference==null)// append new
						{
							GameObject obj=new GameObject();
							obj.transform.parent=parent.transform;
							obj.transform.localPosition=new Vector3(bone.x,bone.y,0);
							obj.transform.localScale=new Vector3(bone.scaleX,bone.scaleY,1);
							Quaternion quad=obj.transform.localRotation;
							quad.eulerAngles=new Vector3(0,0,bone.rotation);
							obj.transform.localRotation=quad;
							obj.name=bone.name;
							if(buildSpriteType==SpriteType.UGUI)
							{
								int subIndex=bone.index;
								int boneNowIndex=0;
								for(int cx=0;cx<parent.transform.childCount;cx++){
									Transform eleTran=parent.transform.GetChild(cx);
									BoneElement eleBone=null;
									if(dataAnimAnalytics.jsonFinal.dicBones.TryGetValue(eleTran.name,out eleBone)){
										if(eleBone.index>subIndex){
											boneNowIndex++;
										}
									}
								}
								//for new UI
								obj.transform.SetSiblingIndex(boneNowIndex);
								obj.AddComponent<RectTransform>();
							}
							cache[bone.name]=obj;
							List<SlotElelement> listSlot=dataAnimAnalytics.jsonFinal.GetSlotMappingWithBone(bone.name);
							for(int xx=0;xx<listSlot.Count;xx++)
							{
								BuildSlotAndAttachment(listSlot[xx],obj,rootSkeleton,dicSprite,dataAnimAnalytics,buildSpriteType,ref objHideForAllAnim,ref objShowForAllAnim);
							}
							
							if(transObj!=null)
							{
								obj.SetActive(false);
							}
							objReference=obj;
						}
					}
					else
					{
						Debug.LogWarning("Parent Null: "+bone.name);
					}
				}
			}
			#endregion
			
			dataAnimAnalytics.AddObjectHideWhenStartAnim(objHideWhenStartAnim);
			//dataAnimAnalytics.AddObjectHideWhenStartAnim(objHideForAllAnim);
			
			dataAnimAnalytics.AddObjectShowWhenStartAnim(objShowWhenStartAnim);
			//dataAnimAnalytics.AddObjectShowWhenStartAnim(objShowForAllAnim);
		}
		return true;
	}
	
	
	public void BuildSlotAndAttachment(SlotElelement slot,GameObject objBone,
	                                   GameObject rootSkeleton,
	                                   Dictionary<string,Sprite> dicSprite,
	                                   DataAnimAnalytics dataAnimAnalytics,
	                                   SpriteType buildSpriteType,
	                                   ref Dictionary<string,string> objHideForAllAnim,
	                                   ref Dictionary<string,string> objShowForAllAnim
	                                   )
	{
		//GameObject obj=new GameObject();
		if(objBone==null)
		{
			return;
		}
		/*obj.transform.parent=objBone.transform;
		obj.transform.localPosition=new Vector3(0,0,0);
		obj.transform.localScale=new Vector3(1,1,1);
		Quaternion quad=obj.transform.localRotation;
		quad.eulerAngles=new Vector3(0,0,0);
		obj.transform.localRotation=quad;
		obj.name=slot.name;*/
		for(int k=0;k<slot.listAcceptAttachment.Count;k++)
		{
			EAPInfoAttachment attachElement=slot.listAcceptObj[k];
			//string nameInAtlas=slot.mapNameInAtlas[slot.listAcceptAttachment[k]];
			
			EAPInfoAttachment attachElementSave=null;
			dataAnimAnalytics.jsonFinal.dicPivot.TryGetValue(attachElement.spriteName,out attachElementSave);
			if(attachElement!=null)
			{
				GameObject obj2=new GameObject();
				obj2.transform.parent=objBone.transform;
				obj2.transform.localScale=new Vector3(scaleInfo/scaleTextureImage*attachElement.scaleX,scaleInfo/scaleTextureImage*attachElement.scaleY,scaleInfo/scaleTextureImage);
				Quaternion quad2=obj2.transform.localRotation;
				obj2.transform.localPosition=new Vector3(0,0,0);
				if(attachElementSave!=null)
				{
					Vector3 vec=Vector3.zero;
					vec.x=attachElement.offsetX-attachElementSave.offsetX;
					vec.y=attachElement.offsetY-attachElementSave.offsetY;
					obj2.transform.localPosition=vec;
				}
				obj2.name=attachElement.spriteName;
				quad2.eulerAngles=new Vector3(0,0,attachElement.rotation);
				obj2.transform.localRotation=quad2;
				if(buildSpriteType==SpriteType.SpriteRenderer)
				{
					obj2.AddComponent<SpriteRenderer>();
					SpriteRenderer sprite=obj2.GetComponent<SpriteRenderer>();
					if(dicSprite.ContainsKey(attachElement.spriteName))	
					{
						sprite.sprite=dicSprite[attachElement.spriteName];
					}
					else if(dicSprite.ContainsKey(attachElement.slotName))	
					{
						sprite.sprite=dicSprite[attachElement.slotName];
					}
					else
					{
						Debug.LogWarning("not found suitable spite:"+attachElement.spriteName+" in "+attachElement.slotName);
					}
					if(sortType==SortLayerType.Depth)
					{
						sprite.sortingOrder=defaultSortDepthValue-slot.index;
					}
					else
					{
						sprite.sortingOrder=defaultSortDepthValue;
						float z=0.1f*slot.index;
						Vector3 vecObj=obj2.transform.localPosition;
						obj2.transform.localPosition=new Vector3(vecObj.x,vecObj.y,z);
					}
					sprite.color = EditorUtil.HexToColor(slot.color);
				}
				#if AE_NGUI
				else if(buildSpriteType==SpriteType.NGUI)
				{
					
					UI2DSprite sprite=obj2.AddComponent<UI2DSprite>();
					if(dicSprite.ContainsKey(attachElement.spriteName))	
					{
						Sprite spriteData=dicSprite[attachElement.spriteName];
						sprite.width=(int)spriteData.rect.width;
						sprite.height=(int)spriteData.rect.height;
						Bounds bounds = spriteData.bounds;
						var X = (bounds.center.x / bounds.extents.x / 2 )*spriteData.rect.width;
						var Y = ( bounds.center.y / bounds.extents.y / 2 )*spriteData.rect.height;

						Vector3 vecObjSprite=sprite.transform.localPosition;
						sprite.transform.localPosition=new Vector3(X,Y,vecObjSprite.z);
					}
					if(dicSprite.ContainsKey(attachElement.spriteName))	
					{
						sprite.sprite2D=dicSprite[attachElement.spriteName];
					}
					else if(dicSprite.ContainsKey(attachElement.slotName))	
					{
						sprite.sprite2D=dicSprite[attachElement.slotName];
					}
					else
					{
						Debug.LogWarning("not found suitable spite:"+attachElement.spriteName+" in "+attachElement.slotName);
					}
					if(sortType==SortLayerType.Depth)
					{
						sprite.depth=defaultSortDepthValue-slot.index;
					}
					else
					{
						sprite.depth=defaultSortDepthValue;
						float z=0.1f*slot.index;
						Vector3 vecObj=obj2.transform.localPosition;
						obj2.transform.localPosition=new Vector3(vecObj.x,vecObj.y,z);
					}
					sprite.color = HexToColor(slot.color);
				}
				#endif
				else{
					obj2.AddComponent<RectTransform>();
					obj2.AddComponent<Image>();
					Image sprite=obj2.GetComponent<Image>();
					if(dicSprite.ContainsKey(attachElement.spriteName))	
					{
						Sprite spriteData=dicSprite[attachElement.spriteName];
						OneP.RectTransformExtensions.SetWidth(sprite.rectTransform,spriteData.rect.width);
						OneP.RectTransformExtensions.SetHeight(sprite.rectTransform,spriteData.rect.height);
						Bounds bounds = spriteData.bounds;
						var pivotX = - bounds.center.x / bounds.extents.x / 2 + 0.5f;
						var pivotY = - bounds.center.y / bounds.extents.y / 2 + 0.5f;
						Vector2 vecAnchor=new Vector2(pivotX,pivotY);
						sprite.rectTransform.pivot=vecAnchor;
						//sprite.rectTransform.anchoredPosition=spriteData.
						//obj2.transform.localPosition=new Vector3(0,0,0);
						Vector3 vec=Vector3.zero;
						vec.x=attachElement.offsetX-attachElementSave.offsetX;
						vec.y=attachElement.offsetY-attachElementSave.offsetY;
						obj2.transform.localPosition=vec;
						sprite.sprite=dicSprite[attachElement.spriteName];
					}
					else if(dicSprite.ContainsKey(attachElement.slotName))	
					{
						sprite.sprite=dicSprite[attachElement.slotName];
					}
					else
					{
						Debug.LogWarning("not found suitable spite:"+attachElement.spriteName+" in "+attachElement.slotName);
					}
					if(sortType==SortLayerType.Depth)
					{
						//sprite.sortingOrder=defaultSortDepthValue-slot.index;
					}
					else
					{
						//int index = transform.GetSiblingIndex();
						//transform.SetSiblingIndex (index + delta);
						//sprite.sortingOrder=defaultSortDepthValue;
						float z=0.1f*slot.index;
						Vector3 vecObj=obj2.transform.localPosition;
						obj2.transform.localPosition=new Vector3(vecObj.x,vecObj.y,z);
					}
					sprite.color =EditorUtil.HexToColor(slot.color);
				}
				
				string fullPath=EditorUtil.GetFullPathBone(rootSkeleton.transform,obj2.transform);
				//objForAttachment[fullPath]=fullPath;
				if(slot.attachment!=null&&attachElement.spriteName==slot.attachment)//default hien
				{
					objShowForAllAnim[fullPath]=fullPath;
				}
				else
				{
					//Debug.LogError(slot.attachment+","+attachElement.name);
					obj2.SetActive(false);
					objHideForAllAnim[fullPath]=fullPath;
				}
			}
			else
			{
			}
		}
	}
	#endregion

	#region Build Animation
	private bool BuildAnimation()
	{

		Debug.Log("BuildAnimation");

		if(!Directory.Exists(pathOutputs))
		{
			Directory.CreateDirectory(pathOutputs);	
		}
		if(listInfoFinal.Count<1)
		{
			CreateBoneSkeleton();
		}
		GameObject transObj= GameObject.Find(aepName);
		objectRoot=transObj;
		string folder=pathOutputs;
		if(objectRoot==null)
		{
			EditorUtility.DisplayDialog("Reference object Error","Can not find Object name "+aepName+" in scene","OK");
			return false;
		}
		if(objectRoot!=null)
		{
			for(int x=0;x<listInfoFinal.Count;x++)
			{
				DataAnimAnalytics dataAnimAnalytics=listInfoFinal[x];
				string animatorName=aepName+".controller";;
				string animatorPathFile=folder+"/"+animatorName;
				Animator animator=objectRoot.GetComponent<Animator>();
				if(animator==null)
				{
					objectRoot.AddComponent<Animator>();
					animator=objectRoot.GetComponent<Animator>();
				}
#if UNITY_4_0_0 ||UNITY_4_0 || UNITY_4_0_1||UNITY_4_1||UNITY_4_2||UNITY_4_3||UNITY_4_4||UNITY_4_5||UNITY_4_6||UNITY_4_7||UNITY_4_8||UNITY_4_9
				UnityEditorInternal.AnimatorController runtimeControler=AssetDatabase.LoadAssetAtPath(animatorPathFile,typeof(UnityEditorInternal.AnimatorController)) as UnityEditorInternal.AnimatorController;
#else
				UnityEditor.Animations.AnimatorController runtimeControler=AssetDatabase.LoadAssetAtPath(animatorPathFile,typeof(UnityEditor.Animations.AnimatorController)) as UnityEditor.Animations.AnimatorController;
#endif
				if(runtimeControler==null)
				{
					runtimeControler=OnProcessCreateAnimatorController(animatorName,animatorPathFile);
					animator.runtimeAnimatorController=runtimeControler;
				}
				else
				{
					if(x==0)
					{
						bool isContinue=false;
						if(autoOverride)
						{
							isContinue=true;
						}
						else
						{
							if(EditorUtility.DisplayDialog("Confimation","File Anim "+ animatorPathFile+ " has already Exist, do you want to replace","YES","NO"))
							{
								isContinue=true;
							}
						}
						if(isContinue)
						{
							AssetDatabase.DeleteAsset(animatorPathFile);
							AssetDatabase.Refresh();
							runtimeControler=OnProcessCreateAnimatorController(animatorName,animatorPathFile);
						}
						else
						{
							continue;
						}
					}
					animator.runtimeAnimatorController=runtimeControler;
				}
				string animName=dataAnimAnalytics.filename+".anim";
				string animPath=folder+"/"+animName;

				AnimationClip anim=AssetDatabase.LoadAssetAtPath(animPath,typeof(AnimationClip)) as AnimationClip;
				if(anim==null)
				{
					anim=OnProcessAnimFile(dataAnimAnalytics,animName,animPath);
				}
				else
				{
					bool isContinue=false;
					if(autoOverride)
					{
						isContinue=true;
					}
					else
					{
						if(EditorUtility.DisplayDialog("Confimation","File Anim "+ animPath+ " has already Exist, do you want to replace","YES","NO"))
						{
							isContinue=true;
						}
					}
					if(isContinue)
					{
						AssetDatabase.DeleteAsset(animPath);
						AssetDatabase.Refresh();
						anim=OnProcessAnimFile(dataAnimAnalytics,animName,animPath);
					}
					else
					{
						continue;
					}
				}
				if(anim!=null)
				{
#if UNITY_4_0_0 ||UNITY_4_0 || UNITY_4_0_1||UNITY_4_1||UNITY_4_2||UNITY_4_3||UNITY_4_4||UNITY_4_5||UNITY_4_6||UNITY_4_7||UNITY_4_8||UNITY_4_9
					UnityEditorInternal.StateMachine sm = runtimeControler.GetLayer(0).stateMachine;
					//UnityEditor.Animations.ChildAnimatorState[] childs=sm.GetState;
					
					bool isExist=false;
					for(int i=0;i<sm.stateCount;i++)
					{
						if(sm.GetState(i).name==anim.name)
						{
							sm.GetState(i).SetAnimationClip(anim);
							isExist=true;
							sm.defaultState=sm.GetState(i);
							sm.anyStatePosition=new Vector3(0,80*i,0);
							break;
						}
					}
					if(!isExist)
					{
						UnityEditorInternal.State state=sm.AddState(anim.name);
						state.name=anim.name;
						state.SetAnimationClip(anim);
						sm.defaultState=state;
						runtimeControler.AddParameter(anim.name,UnityEditorInternal.AnimatorControllerParameterType.Trigger);

						UnityEditorInternal.Transition trans=sm.AddAnyStateTransition(state);
						trans.name=anim.name;
						trans.duration=0;
						trans.canTransitionToSelf=true;

						//trans.AddCondition(UnityEditorInternal.AnimatorCondition,0,anim.name);
						//trans.AddCondition(condi);
						trans.GetCondition(0);
						UnityEditorInternal.AnimatorCondition condi=trans.GetCondition(0);
						condi.exitTime=1;
						condi.mode=UnityEditorInternal.TransitionConditionMode.If;
						condi.parameter=anim.name;
					}
#else
					UnityEditor.Animations.AnimatorStateMachine sm = runtimeControler.layers[0].stateMachine;
					UnityEditor.Animations.ChildAnimatorState[] childs=sm.states;
					bool isExist=false;
					for(int i=0;i<childs.Length;i++)
					{
						if(childs[i].state.name==anim.name)
						{
							childs[i].state.motion=anim;
							isExist=true;
							sm.defaultState=childs[i].state;
							break;
						}
					}
					if(!isExist)
					{
						UnityEditor.Animations.AnimatorState state=sm.AddState(anim.name);
						state.name=anim.name;
						state.motion=anim;
						sm.defaultState=state;
						
						runtimeControler.AddParameter(anim.name,AnimatorControllerParameterType.Trigger);
						
						UnityEditor.Animations.AnimatorStateTransition trans=sm.AddAnyStateTransition(state);
						trans.name=anim.name;
						trans.duration=0;
						trans.exitTime=1;
						trans.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If,0,anim.name);
					}
#endif
				}
			}
		}
		return true;
	}
	#endregion

	#region Create .Anim File
	AnimationClip OnProcessAnimFile(DataAnimAnalytics dataAnimAnalytics,string name,string animPath)
	{
		AnimationClip anim=new AnimationClip();
		anim.name=name;
		anim.frameRate=fps;
		if(dataAnimAnalytics.animationStyle==AnimationStyle.Loop)
		{
			SerializedObject serializedClip = new SerializedObject(anim);
			EditorUtil.AnimationClipSettings clipSettings = new EditorUtil.AnimationClipSettings(serializedClip.FindProperty("m_AnimationClipSettings"));
			clipSettings.loopTime = true;
			serializedClip.ApplyModifiedProperties();
		}
		for(int xx=0;xx<dataAnimAnalytics.jsonFinal.bones.Count;xx++)
		{
			BoneElement boneE=dataAnimAnalytics.jsonFinal.bones[xx];
			string pathE=dataAnimAnalytics.jsonFinal.GetFullPathBone(boneE.name);
			GameObject objReference=AEPAnimationClipElement.GetRefenreceObject(pathE,objectRoot);
			if(objReference!=null&&objReference.activeSelf==false)
			{
				//Debug.LogError(objReference.name);
				AEPAnimationClipElement aepClip=new AEPAnimationClipElement();
				aepClip.AddStartVisible(boneE.name,pathE,objectRoot,dataAnimAnalytics.jsonFinal,true);
				for(int i=0;i<aepClip.listCurve.Count;i++)
				{
					//Debug.LogError(JsonWriter.Serialize(aepClip.listCurve[i].binding));
					AnimationUtility.SetEditorCurve(anim, aepClip.listCurve[i].binding,aepClip.listCurve[i].curve);
				}
			}
		}
		foreach(KeyValuePair<string,string> pair in dataAnimAnalytics.objHideWhenStartAnim)
		{
			AEPAnimationClipElement aepClip=new AEPAnimationClipElement();
			aepClip.AddStartVisible(pair.Key,pair.Value,objectRoot,dataAnimAnalytics.jsonFinal,false);
			for(int i=0;i<aepClip.listCurve.Count;i++)
			{
				//Debug.LogError(JsonWriter.Serialize(aepClip.listCurve[i].binding));
				AnimationUtility.SetEditorCurve(anim, aepClip.listCurve[i].binding,aepClip.listCurve[i].curve);
			}
		}

		foreach(KeyValuePair<string,string> pair in dataAnimAnalytics.objShowWhenStartAnim)
		{
			AEPAnimationClipElement aepClip=new AEPAnimationClipElement();
			aepClip.AddStartVisible(pair.Key,pair.Value,objectRoot,dataAnimAnalytics.jsonFinal,true);
			for(int i=0;i<aepClip.listCurve.Count;i++)
			{
				AnimationUtility.SetEditorCurve(anim, aepClip.listCurve[i].binding,aepClip.listCurve[i].curve);
			}
		}
		foreach(KeyValuePair<string,AEPBoneAnimationElement> pair in dataAnimAnalytics.jsonFinal.dicAnimation)
		{
			string pathProperty=dataAnimAnalytics.jsonFinal.GetFullPathBone(pair.Key);
			AEPAnimationClipElement aepClip=new AEPAnimationClipElement();
			aepClip.AddTranformAnimation(pair.Value,pathProperty,objectRoot,dataAnimAnalytics.jsonFinal);
			for(int i=0;i<aepClip.listCurve.Count;i++)
			{
				AnimationUtility.SetEditorCurve(anim, aepClip.listCurve[i].binding,aepClip.listCurve[i].curve);
			}
		}

		foreach(KeyValuePair<string,AEPSlotAnimationElement> pair in dataAnimAnalytics.jsonFinal.dicSlotAttactment)
		{
			string pathProperty=dataAnimAnalytics.jsonFinal.GetFullPathBone(pair.Key);
			AEPAnimationClipElement aepClip=new AEPAnimationClipElement();
			aepClip.AddAttactmentAnimation(pair.Value,pathProperty,objectRoot,dataAnimAnalytics.jsonFinal,buildSpriteType);

			for(int i=0;i<aepClip.listCurve.Count;i++)
			{
				AnimationUtility.SetEditorCurve(anim, aepClip.listCurve[i].binding,aepClip.listCurve[i].curve);
			}
		}

		EditorUtility.SetDirty(anim);
		AssetDatabase.CreateAsset(anim,animPath);
		AssetDatabase.ImportAsset(animPath);
		AssetDatabase.Refresh();
#if UNITY_4_0_0 ||UNITY_4_0 || UNITY_4_0_1||UNITY_4_1||UNITY_4_2||UNITY_4_3||UNITY_4_4||UNITY_4_5||UNITY_4_6||UNITY_4_7||UNITY_4_8||UNITY_4_9
		AnimationUtility.SetAnimationType(anim, ModelImporterAnimationType.Generic); 
#endif
		return anim;
	}
	#endregion

	#region Create Animator Controller File
#if UNITY_4_0_0 ||UNITY_4_0 || UNITY_4_0_1||UNITY_4_1||UNITY_4_2||UNITY_4_3||UNITY_4_4||UNITY_4_5||UNITY_4_6||UNITY_4_7||UNITY_4_8||UNITY_4_9
	UnityEditorInternal.AnimatorController OnProcessCreateAnimatorController(string name,string animPath)
	{
		UnityEditorInternal.AnimatorController.CreateAnimatorControllerAtPath(animPath);
		UnityEditorInternal.AnimatorController runtimeControler=AssetDatabase.LoadAssetAtPath(animPath,typeof(UnityEditorInternal.AnimatorController)) as UnityEditorInternal.AnimatorController;
		AssetDatabase.Refresh();
		return runtimeControler;
	}
#else
	UnityEditor.Animations.AnimatorController OnProcessCreateAnimatorController(string name,string animPath)
	{
		UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(animPath);
		UnityEditor.Animations.AnimatorController runtimeControler=AssetDatabase.LoadAssetAtPath(animPath,typeof(UnityEditor.Animations.AnimatorController)) as UnityEditor.Animations.AnimatorController;
		AssetDatabase.Refresh();
		return runtimeControler;
	}
#endif
	#endregion
}
#region Animation Data Analytics

public class AEPAnimationCurve
{
	public EditorCurveBinding binding;
	public AnimationCurve curve;
	public AEPAnimationCurve(EditorCurveBinding _binding,AnimationCurve _curve)
	{
		this.binding=_binding;
		this.curve=_curve;
	}
}

public class AEPAnimationClipElement
{
	public List<AEPAnimationCurve> listCurve=new List<AEPAnimationCurve>();
	public static GameObject GetRefenreceObject(string path,GameObject aepRoot)
	{
		if(path.Length<1)
		{
			return aepRoot;
		}
		else
		{
			string[] sub=path.Split(new char[]{'/'});
			GameObject obj=aepRoot;
			for(int i=0;i<sub.Length;i++)
			{
				Transform trans=obj.transform.Find(sub[i]);
				if(trans!=null)
				{
					obj=trans.gameObject;
				}
				else
				{
					return null;
				}
			}
			return obj;
		}
	}
	public AEPAnimationClipElement()
	{

	}
	public void AddTranformAnimation(AEPBoneAnimationElement animInfo,string path,GameObject aepRoot,AEPJsonFinal jsonFinal)
	{
		GameObject objReference=GetRefenreceObject(path,aepRoot);
		if(objReference==null)
		{
			Debug.LogError("Not have object reference:"+path);
			return;
		}
		if(animInfo.translate!=null)
		{
			Vector3 vecTran=Vector3.zero;// objReference.transform.localPosition;

			// co tranlate
			//x
			{
				EditorCurveBinding binding=new EditorCurveBinding();
				AnimationCurve curve=new AnimationCurve();
				binding.type=typeof(Transform);
				binding.path=path;
				binding.propertyName="m_LocalPosition.x";
				for(int i=0;i<animInfo.translate.Count;i++)
				{
					AEPAnimationTranslate translate=animInfo.translate[i];
					/*Keyframe k=new Keyframe();
					k.inTangent=0;
					k.outTangent=0;
					k.tangentMode=0;
					k.time=translate.time;
					k.value=translate.x+vecTran.x;
					curve.AddKey(k);*/
					curve.AddKey(KeyframeUtil.GetNew(translate.time,translate.x+vecTran.x ,KeyframeUtil.GetTangleMode(translate.tangentType)));

				}
				AEPAnimationCurve curveData=new AEPAnimationCurve(binding,curve);
				listCurve.Add(curveData);
			}
			//y
			{
				EditorCurveBinding binding2=new EditorCurveBinding();
				AnimationCurve curve2=new AnimationCurve();
				binding2.type=typeof(Transform);
				binding2.path=path;
				binding2.propertyName="m_LocalPosition.y";
				for(int i=0;i<animInfo.translate.Count;i++)
				{
					AEPAnimationTranslate translate=animInfo.translate[i];
					/*Keyframe k=new Keyframe();
					k.inTangent=0;
					k.outTangent=0;
					k.tangentMode=0;
					k.time=translate.time;
					k.value=translate.y+vecTran.y;
					curve2.AddKey(k);*/
					curve2.AddKey(KeyframeUtil.GetNew(translate.time,translate.y+vecTran.y ,KeyframeUtil.GetTangleMode(translate.tangentType)));

				}
				AEPAnimationCurve curveData2=new AEPAnimationCurve(binding2,curve2);
				listCurve.Add(curveData2);
			}
			//z
			{
				EditorCurveBinding binding3=new EditorCurveBinding();
				AnimationCurve curve3=new AnimationCurve();
				binding3.type=typeof(Transform);
				binding3.path=path;
				binding3.propertyName="m_LocalPosition.z";
				for(int i=0;i<animInfo.translate.Count;i++)
				{
					AEPAnimationTranslate translate=animInfo.translate[i];
					/*Keyframe k=new Keyframe();
					k.inTangent=0;
					k.outTangent=0;
					k.tangentMode=0;
					k.time=translate.time;
					k.value=0+vecTran.z;
					curve3.AddKey(k);*/
					curve3.AddKey(KeyframeUtil.GetNew(translate.time,0+vecTran.z ,KeyframeUtil.GetTangleMode(translate.tangentType)));

				}
				
				AEPAnimationCurve curveData3=new AEPAnimationCurve(binding3,curve3);
				listCurve.Add(curveData3);
			}
		}
		if(animInfo.rotate!=null)
		{
			float lastAngleCi=0;
			for(int ci=0;ci<animInfo.rotate.Count;ci++)
			{
				if(ci==0)
				{
					lastAngleCi=animInfo.rotate[ci].angle;
					animInfo.rotate[ci].angleChange=lastAngleCi;
				}
				else
				{
					float curAngle=animInfo.rotate[ci].angle;

					if(curAngle-lastAngleCi>180)
					{
						curAngle=curAngle-360;
					}
					else if(lastAngleCi-curAngle>180)
					{
						curAngle=curAngle+360;
					}
					float change=curAngle-lastAngleCi;
					lastAngleCi=animInfo.rotate[ci].angle;
					animInfo.rotate[ci].angleChange=change;
				}
			}
			BoneElement boneElement=jsonFinal.GetBoneElement(animInfo.name);
			if(boneElement!=null)
			{
				float frameStartAngle=boneElement.rotation;
				//x
				{
					EditorCurveBinding bindingx=new EditorCurveBinding();
					AnimationCurve curvex=new AnimationCurve();
					bindingx.type=typeof(Transform);
					bindingx.path=path;
					bindingx.propertyName="m_LocalRotation.x";
					for(int i=0;i<animInfo.rotate.Count;i++)
					{
						AEPAnimationRotate rotate=animInfo.rotate[i];
						/*Keyframe k=new Keyframe();
						k.inTangent=0;
						k.outTangent=0;
						k.tangentMode=0;
						k.time=rotate.time;
						k.value=0;
						curvex.AddKey(k);*/
						curvex.AddKey(KeyframeUtil.GetNew(rotate.time,0 ,KeyframeUtil.GetTangleMode(rotate.tangentType)));

					}
					AEPAnimationCurve curveDatax=new AEPAnimationCurve(bindingx,curvex);
					listCurve.Add(curveDatax);
				}
				//y
				{
					EditorCurveBinding bindingy=new EditorCurveBinding();
					AnimationCurve curvey=new AnimationCurve();
					bindingy.type=typeof(Transform);
					bindingy.path=path;
					bindingy.propertyName="m_LocalRotation.y";
					for(int i=0;i<animInfo.rotate.Count;i++)
					{
						AEPAnimationRotate rotate=animInfo.rotate[i];
						/*Keyframe k=new Keyframe();
						k.inTangent=0;
						k.outTangent=0;
						k.tangentMode=0;
						k.time=rotate.time;
						k.value=0;
						curvey.AddKey(k);*/
						curvey.AddKey(KeyframeUtil.GetNew(rotate.time,0 ,KeyframeUtil.GetTangleMode(rotate.tangentType)));
					}
					AEPAnimationCurve curveDatay=new AEPAnimationCurve(bindingy,curvey);
					listCurve.Add(curveDatay);
				}

				//z
				{
					EditorCurveBinding bindingz=new EditorCurveBinding();
					AnimationCurve curvez=new AnimationCurve();
					bindingz.type=typeof(Transform);
					bindingz.path=path;
					bindingz.propertyName="m_LocalRotation.z";
					float beforeAngle=0;
					for(int i=0;i<animInfo.rotate.Count;i++)
					{

						AEPAnimationRotate rotate=animInfo.rotate[i];
						float angle=beforeAngle+rotate.angleChange+frameStartAngle;
						beforeAngle+=rotate.angleChange;
						/*Keyframe k=new Keyframe();
						k.inTangent=0;
						k.outTangent=0;
						k.tangentMode=0;
						k.time=rotate.time;
						k.value= Mathf.Sin((180+angle/2)*Mathf.Deg2Rad);
						curvez.AddKey(k);*/

						curvez.AddKey(KeyframeUtil.GetNew(rotate.time,Mathf.Sin((180+angle/2)*Mathf.Deg2Rad) ,KeyframeUtil.GetTangleMode(rotate.tangentType)));
					}
					AEPAnimationCurve curveDataz=new AEPAnimationCurve(bindingz,curvez);
					listCurve.Add(curveDataz);
				}

				//w
				{
					EditorCurveBinding bindingw=new EditorCurveBinding();
					AnimationCurve curvew=new AnimationCurve();
					bindingw.type=typeof(Transform);
					bindingw.path=path;
					bindingw.propertyName="m_LocalRotation.w";
					float beforeAngle=0;
					for(int i=0;i<animInfo.rotate.Count;i++)
					{
						AEPAnimationRotate rotate=animInfo.rotate[i];
						float angle=beforeAngle+rotate.angleChange+frameStartAngle;
						beforeAngle+=rotate.angleChange;
						/*Keyframe k=new Keyframe();
						k.inTangent=0;
						k.outTangent=0;
						k.tangentMode=0;
						k.time=rotate.time;
						k.value=Mathf.Cos((180+angle/2)*Mathf.Deg2Rad);
						curvew.AddKey(k);*/
						curvew.AddKey(KeyframeUtil.GetNew(rotate.time,Mathf.Cos((180+angle/2)*Mathf.Deg2Rad) ,KeyframeUtil.GetTangleMode(rotate.tangentType)));

					}
					AEPAnimationCurve curveDataw=new AEPAnimationCurve(bindingw,curvew);
					listCurve.Add(curveDataw);
				}
			}
			else{
				Debug.LogWarning("Can not find reference object for Rotation:"+boneElement.name);
			}
		}

		if(animInfo.scale!=null)
		{
			BoneElement boneElement=jsonFinal.GetBoneElement(animInfo.name);
			if(boneElement!=null)
			{
				if (animInfo.scale.Count > 0) {
					AEPAnimationScale scale=animInfo.scale[0];
					if (scale.time > 0) {
						AEPAnimationScale scale0=new AEPAnimationScale();
						scale0.x = scale.x;
						scale0.y = scale.y;
						scale0.time = 0;
						animInfo.scale.Insert (0, scale0);
					}
				}

				Vector2 scaleFrameStart = Vector2.one;// new Vector2(boneElement.scaleX,boneElement.scaleY);
				//x
				{
					EditorCurveBinding bindingx=new EditorCurveBinding();
					AnimationCurve curvex=new AnimationCurve();
					bindingx.type=typeof(Transform);
					bindingx.path=path;
					bindingx.propertyName="m_LocalScale.x";
					for(int i=0;i<animInfo.scale.Count;i++)
					{
						AEPAnimationScale scale=animInfo.scale[i];
						/*Keyframe k=new Keyframe();
						k.inTangent=0;
						k.outTangent=0;
						k.tangentMode=0;
						k.time=scale.time;
						k.value=scale.x*scaleFrameStart.x;
						curvex.AddKey(k);*/
						curvex.AddKey(KeyframeUtil.GetNew(scale.time,scale.x*scaleFrameStart.x ,KeyframeUtil.GetTangleMode(scale.tangentType)));
					}
					AEPAnimationCurve curveDatax=new AEPAnimationCurve(bindingx,curvex);
					listCurve.Add(curveDatax);
				}
				//y
				{
					EditorCurveBinding bindingy=new EditorCurveBinding();
					AnimationCurve curvey=new AnimationCurve();
					bindingy.type=typeof(Transform);
					bindingy.path=path;
					bindingy.propertyName="m_LocalScale.y";
					for(int i=0;i<animInfo.scale.Count;i++)
					{
						AEPAnimationScale scale=animInfo.scale[i];
						/*Keyframe k=new Keyframe();
						k.inTangent=0;
						k.outTangent=0;
						k.tangentMode=0;
						k.time=scale.time;
						k.value=scale.y*scaleFrameStart.y;
						curvey.AddKey(k);*/
						curvey.AddKey(KeyframeUtil.GetNew(scale.time,scale.y*scaleFrameStart.y ,KeyframeUtil.GetTangleMode(scale.tangentType)));

					}
					AEPAnimationCurve curveDatax=new AEPAnimationCurve(bindingy,curvey);
					listCurve.Add(curveDatax);
				}
				//z
				{
					EditorCurveBinding bindingz=new EditorCurveBinding();
					AnimationCurve curvez=new AnimationCurve();
					bindingz.type=typeof(Transform);
					bindingz.path=path;
					bindingz.propertyName="m_LocalScale.z";
					for(int i=0;i<animInfo.scale.Count;i++)
					{
						AEPAnimationScale scale=animInfo.scale[i];
						/*Keyframe k=new Keyframe();
						k.inTangent=0;
						k.outTangent=0;
						k.tangentMode=0;
						k.time=scale.time;
						k.value=1;
						curvez.AddKey(k);*/
						curvez.AddKey(KeyframeUtil.GetNew(scale.time,1 ,KeyframeUtil.GetTangleMode(scale.tangentType)));

					}
					AEPAnimationCurve curveDataz=new AEPAnimationCurve(bindingz,curvez);
					listCurve.Add(curveDataz);
				}
			}
			else{
				Debug.LogWarning("Can not find reference object for Scale:"+boneElement.name);
			}

		}
	}

	public void AddAttactmentAnimation(AEPSlotAnimationElement animInfo,string path,GameObject aepRoot,AEPJsonFinal jsonFinal, SpriteType buildSpriteType)
	{
		GameObject objReference=GetRefenreceObject(path,aepRoot);
		if(objReference==null)
		{
			Debug.LogError("Not have object reference:"+path);
			return;
		}
		if(animInfo.attachment!=null)
		{
			//seting dafault
			EditorCurveBinding bindingSlot=new EditorCurveBinding();
			AnimationCurve curveSlot=new AnimationCurve();
			bindingSlot.type=typeof(GameObject);
			bindingSlot.path=path;
			bindingSlot.propertyName="m_IsActive";
			if(animInfo.attachment.Count>0)// gia tri default
			{

				Keyframe k=new Keyframe();
				//k.inTangent=1000;
				//k.outTangent=1000;
				k.tangentMode=31;
				k.time=0;
				k.value=1;
				curveSlot.AddKey(KeyframeUtil.GetNew(0, k.value , TangentMode.Stepped));
				AEPAnimationCurve curveData=new AEPAnimationCurve(bindingSlot,curveSlot);
				listCurve.Add(curveData);
			}
			for(int xx=0;xx<objReference.transform.childCount;xx++)
			{
				GameObject childObj=objReference.transform.GetChild(xx).gameObject;
				bool checkCondition=false;
				if(buildSpriteType==SpriteType.SpriteRenderer)
				{
					if(childObj.GetComponent<SpriteRenderer>()!=null){
						checkCondition=true;
					}
				}
				else
				{
					if(childObj.GetComponent<Image>()!=null)
					{
						checkCondition=true;
					}
				}
				if(checkCondition)
				{

					string pathChild=path+"/"+childObj.name;
					EditorCurveBinding binding=new EditorCurveBinding();
					AnimationCurve curve=new AnimationCurve();
					binding.type=typeof(GameObject);
					binding.path=pathChild;
					binding.propertyName="m_IsActive";
					if(animInfo.attachment.Count>0)// gia tri default
					{
						if(animInfo.attachment[0].time>0)
						{
							Keyframe k=new Keyframe();
							//k.inTangent=1000;
							//k.outTangent=1000;
							k.tangentMode=31;
							k.time=0;
							k.value=0;
							curve.AddKey(KeyframeUtil.GetNew(0, k.value , TangentMode.Stepped));
						}
					}
					for(int i=0;i<animInfo.attachment.Count;i++)
					{
						AEPAnimationAttachment attact=animInfo.attachment[i];
						Keyframe k=new Keyframe();
						//k.inTangent=1000;
						//k.outTangent=1000;
						k.tangentMode=31;
						k.time=attact.time;
						if(attact.name==null||attact.name.Length<1)
							k.value=0;
						else
						{
							if(attact.name==childObj.name)
							{
								k.value=1;
							}
							else
							{
								k.value=0;
							}
						}
						curve.AddKey(KeyframeUtil.GetNew(attact.time, k.value , TangentMode.Stepped));
					}
					//curve.
					AEPAnimationCurve curveData=new AEPAnimationCurve(binding,curve);
					listCurve.Add(curveData);
				}
			}
		}
		if(animInfo.color!=null)
		{
			for(int xx=0;xx<objReference.transform.childCount;xx++)
			{
				GameObject childObj=objReference.transform.GetChild(xx).gameObject;
				bool checkCondition=false;
				if(buildSpriteType==SpriteType.SpriteRenderer)
				{
					if(childObj.GetComponent<SpriteRenderer>()!=null){
						checkCondition=true;
					}
				}
				else
				{
					if(childObj.GetComponent<Image>()!=null)
					{
						checkCondition=true;
					}
				}
				if(checkCondition)
				{
					string pathChild=path+"/"+childObj.name;
					//r
					{
						EditorCurveBinding binding=new EditorCurveBinding();
						AnimationCurve curve=new AnimationCurve();
						if(buildSpriteType==SpriteType.SpriteRenderer){
								binding.type=typeof(SpriteRenderer);
						}
						else{
							binding.type=typeof(Image);
						}
						binding.path=pathChild;
						binding.propertyName="m_Color.r";
						for(int i=0;i<animInfo.color.Count;i++)
						{
							Color color=EditorUtil.HexToColor(animInfo.color[i].color);
							curve.AddKey(KeyframeUtil.GetNew(animInfo.color[i].time, color.r ,KeyframeUtil.GetTangleMode(animInfo.color[i].tangentType)));
						}
						AEPAnimationCurve curveData=new AEPAnimationCurve(binding,curve);
						listCurve.Add(curveData);
					}
					//b
					{
						EditorCurveBinding binding=new EditorCurveBinding();
						AnimationCurve curve=new AnimationCurve();
						if(buildSpriteType==SpriteType.SpriteRenderer){
							binding.type=typeof(SpriteRenderer);
						}
						else{
							binding.type=typeof(Image);
						}
						
						binding.path=pathChild;
						binding.propertyName="m_Color.b";
						for(int i=0;i<animInfo.color.Count;i++)
						{
							Color color=EditorUtil.HexToColor(animInfo.color[i].color);
							curve.AddKey(KeyframeUtil.GetNew(animInfo.color[i].time, color.b ,KeyframeUtil.GetTangleMode(animInfo.color[i].tangentType)));
						}
						AEPAnimationCurve curveData=new AEPAnimationCurve(binding,curve);
						listCurve.Add(curveData);
					}
					//g
					{
						EditorCurveBinding binding=new EditorCurveBinding();
						AnimationCurve curve=new AnimationCurve();
						if(buildSpriteType==SpriteType.SpriteRenderer){
							binding.type=typeof(SpriteRenderer);
						}
						else{
							binding.type=typeof(Image);
						}
						binding.path=pathChild;
						binding.propertyName="m_Color.g";
						for(int i=0;i<animInfo.color.Count;i++)
						{
							Color color=EditorUtil.HexToColor(animInfo.color[i].color);
							curve.AddKey(KeyframeUtil.GetNew(animInfo.color[i].time, color.g ,KeyframeUtil.GetTangleMode(animInfo.color[i].tangentType)));
						}
						AEPAnimationCurve curveData=new AEPAnimationCurve(binding,curve);
						listCurve.Add(curveData);
					}
					//a
					{
						EditorCurveBinding binding=new EditorCurveBinding();
						AnimationCurve curve=new AnimationCurve();
						if(buildSpriteType==SpriteType.SpriteRenderer){
							binding.type=typeof(SpriteRenderer);
						}
						else{
							binding.type=typeof(Image);
						}
						binding.path=pathChild;
						binding.propertyName="m_Color.a";
						for(int i=0;i<animInfo.color.Count;i++)
						{
							Color color=EditorUtil.HexToColor(animInfo.color[i].color);
							curve.AddKey(KeyframeUtil.GetNew(animInfo.color[i].time, color.a ,KeyframeUtil.GetTangleMode(animInfo.color[i].tangentType)));
						}
						AEPAnimationCurve curveData=new AEPAnimationCurve(binding,curve);
						listCurve.Add(curveData);
					}
				}
			}
		}
	}

	public void AddAttactmentAnimationNGUI(AEPSlotAnimationElement animInfo,string path,GameObject aepRoot,AEPJsonFinal jsonFinal, SpriteType buildSpriteType)
	{
		GameObject objReference=GetRefenreceObject(path,aepRoot);
		if(objReference==null)
		{
			Debug.LogError("Not have object reference:"+path);
			return;
		}
		if(animInfo.attachment!=null)
		{
			//Debug.LogError(path+","+objReference.transform.childCount);
			//seting dafault
			EditorCurveBinding bindingSlot=new EditorCurveBinding();
			AnimationCurve curveSlot=new AnimationCurve();
			bindingSlot.type=typeof(GameObject);
			bindingSlot.path=path;
			bindingSlot.propertyName="m_IsActive";
			if(animInfo.attachment.Count>0)// gia tri default
			{
				
				Keyframe k=new Keyframe();
				//k.inTangent=1000;
				//k.outTangent=1000;
				k.tangentMode=31;
				k.time=0;
				k.value=1;
				curveSlot.AddKey(KeyframeUtil.GetNew(0, k.value , TangentMode.Stepped));
				AEPAnimationCurve curveData=new AEPAnimationCurve(bindingSlot,curveSlot);
				listCurve.Add(curveData);
			}
			for(int xx=0;xx<objReference.transform.childCount;xx++)
			{
				GameObject childObj=objReference.transform.GetChild(xx).gameObject;
				bool checkCondition=false;
				if(buildSpriteType==SpriteType.SpriteRenderer)
				{
					if(childObj.GetComponent<SpriteRenderer>()!=null){
						checkCondition=true;
					}
				}
				#if AE_NGUI
				else if(buildSpriteType==SpriteType.NGUI)
				{
					if(childObj.GetComponent<UI2DSprite>()!=null){
						checkCondition=true;
					}
				}
				#endif
				else
				{
					if(childObj.GetComponent<Image>()!=null)
					{
						checkCondition=true;
					}
				}
				if(checkCondition)
				{
					
					string pathChild=path+"/"+childObj.name;
					EditorCurveBinding binding=new EditorCurveBinding();
					AnimationCurve curve=new AnimationCurve();
					binding.type=typeof(GameObject);
					binding.path=pathChild;
					binding.propertyName="m_IsActive";
					if(animInfo.attachment.Count>0)// gia tri default
					{
						if(animInfo.attachment[0].time>0)
						{
							Keyframe k=new Keyframe();
							//k.inTangent=1000;
							//k.outTangent=1000;
							k.tangentMode=31;
							k.time=0;
							k.value=0;
							curve.AddKey(KeyframeUtil.GetNew(0, k.value , TangentMode.Stepped));
						}
					}
					for(int i=0;i<animInfo.attachment.Count;i++)
					{
						AEPAnimationAttachment attact=animInfo.attachment[i];
						Keyframe k=new Keyframe();
						//k.inTangent=1000;
						//k.outTangent=1000;
						k.tangentMode=31;
						k.time=attact.time;
						if(attact.name==null||attact.name.Length<1)
							k.value=0;
						else
						{
							if(attact.name==childObj.name)
							{
								k.value=1;
							}
							else
							{
								k.value=0;
							}
						}
						curve.AddKey(KeyframeUtil.GetNew(attact.time, k.value , TangentMode.Stepped));
					}
					//curve.
					AEPAnimationCurve curveData=new AEPAnimationCurve(binding,curve);
					listCurve.Add(curveData);
				}
			}
		}
		if(animInfo.color!=null)
		{
			for(int xx=0;xx<objReference.transform.childCount;xx++)
			{
				GameObject childObj=objReference.transform.GetChild(xx).gameObject;
				bool checkCondition=false;
				if(buildSpriteType==SpriteType.SpriteRenderer)
				{
					if(childObj.GetComponent<SpriteRenderer>()!=null){
						checkCondition=true;
					}
				}
				#if AE_NGUI
				else if(buildSpriteType==SpriteType.NGUI)
				{
					if(childObj.GetComponent<UI2DSprite>()!=null){
						checkCondition=true;
					}
				}
				#endif
				else
				{
					if(childObj.GetComponent<Image>()!=null)
					{
						checkCondition=true;
					}
				}
				if(checkCondition)
				{
					string pathChild=path+"/"+childObj.name;
					//r
					{
						EditorCurveBinding binding=new EditorCurveBinding();
						AnimationCurve curve=new AnimationCurve();
						binding.propertyName="m_Color.r";
						if(buildSpriteType==SpriteType.SpriteRenderer){
							binding.type=typeof(SpriteRenderer);
						}
						#if AE_NGUI
						else if(buildSpriteType==SpriteType.NGUI){
							binding.type=typeof(UI2DSprite);
							binding.propertyName="mColor.r";
						}
						#endif
						else{
							binding.type=typeof(Image);
						}
						binding.path=pathChild;


						for(int i=0;i<animInfo.color.Count;i++)
						{
							Color color=EditorUtil.HexToColor(animInfo.color[i].color);
							curve.AddKey(KeyframeUtil.GetNew(animInfo.color[i].time, color.r ,KeyframeUtil.GetTangleMode(animInfo.color[i].tangentType)));
						}
						AEPAnimationCurve curveData=new AEPAnimationCurve(binding,curve);
						listCurve.Add(curveData);
					}
					//b
					{
						EditorCurveBinding binding=new EditorCurveBinding();
						AnimationCurve curve=new AnimationCurve();
						binding.propertyName="m_Color.b";
						if(buildSpriteType==SpriteType.SpriteRenderer){
							binding.type=typeof(SpriteRenderer);
						}
						#if AE_NGUI
						else if(buildSpriteType==SpriteType.NGUI){
							binding.type=typeof(UI2DSprite);
							binding.propertyName="mColor.b";

						}
						#endif
						else{
							binding.type=typeof(Image);
						}
						
						binding.path=pathChild;
						for(int i=0;i<animInfo.color.Count;i++)
						{
							Color color=EditorUtil.HexToColor(animInfo.color[i].color);
							curve.AddKey(KeyframeUtil.GetNew(animInfo.color[i].time, color.b ,KeyframeUtil.GetTangleMode(animInfo.color[i].tangentType)));
						}
						AEPAnimationCurve curveData=new AEPAnimationCurve(binding,curve);
						listCurve.Add(curveData);
					}
					//g
					{

						EditorCurveBinding binding=new EditorCurveBinding();
						AnimationCurve curve=new AnimationCurve();
						binding.propertyName="m_Color.g";
						if(buildSpriteType==SpriteType.SpriteRenderer){
							binding.type=typeof(SpriteRenderer);
						}
						#if AE_NGUI
						if(buildSpriteType==SpriteType.NGUI){
							binding.type=typeof(UI2DSprite);
							binding.propertyName="mColor.g";
						}
						#endif
						else{
							binding.type=typeof(Image);
						}
						binding.path=pathChild;

						for(int i=0;i<animInfo.color.Count;i++)
						{
							Color color=EditorUtil.HexToColor(animInfo.color[i].color);
							curve.AddKey(KeyframeUtil.GetNew(animInfo.color[i].time, color.g ,KeyframeUtil.GetTangleMode(animInfo.color[i].tangentType)));
						}
						AEPAnimationCurve curveData=new AEPAnimationCurve(binding,curve);
						listCurve.Add(curveData);
					}
					//a
					{
						EditorCurveBinding binding=new EditorCurveBinding();
						AnimationCurve curve=new AnimationCurve();
						binding.propertyName="m_Color.a";
						if(buildSpriteType==SpriteType.SpriteRenderer){
							binding.type=typeof(SpriteRenderer);
						}
						#if AE_NGUI
						else if(buildSpriteType==SpriteType.NGUI){
							binding.type=typeof(UI2DSprite);
							binding.propertyName="mColor.a";
						}
						#endif
						else{
							binding.type=typeof(Image);
						}
						binding.path=pathChild;
						for(int i=0;i<animInfo.color.Count;i++)
						{
							Color color=EditorUtil.HexToColor(animInfo.color[i].color);
							curve.AddKey(KeyframeUtil.GetNew(animInfo.color[i].time, color.a ,KeyframeUtil.GetTangleMode(animInfo.color[i].tangentType)));
						}
						AEPAnimationCurve curveData=new AEPAnimationCurve(binding,curve);
						listCurve.Add(curveData);
					}
				}
			}
		}
	}


	public void AddStartVisible(string boneName,string path,GameObject aepRoot,AEPJsonFinal jsonFinal,bool isVisible)
	{
		EditorCurveBinding binding=new EditorCurveBinding();
		AnimationCurve curve=new AnimationCurve();
		binding.type=typeof(GameObject);
		binding.path=path;
		binding.propertyName="m_IsActive";
		if(isVisible)
		{
			curve.AddKey(KeyframeUtil.GetNew(0, 1, TangentMode.Stepped));
		}
		else 
		{
			curve.AddKey(KeyframeUtil.GetNew(0, 0, TangentMode.Stepped));
		}
		AEPAnimationCurve curveData=new AEPAnimationCurve(binding,curve);
		listCurve.Add(curveData);
	}
}
#pragma warning restore 0618
#endregion
