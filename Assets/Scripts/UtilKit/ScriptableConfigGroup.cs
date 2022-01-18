using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ScriptableConfigGroup<T, R> :
    ScriptableObject, Prime31.IObjectInspectable
    where T : ScriptableConfig<T>
    where R : ScriptableConfigGroup<T, R>
{
    public List<T> All
    {
        get
        {
            return _configs;
        }
    }

    public abstract string FolderName { get; }

#if UNITY_EDITOR
    [Prime31.MakeButton]
    public void RefreshMapConfigsFormFolder()
    {
        if (_configs == null)
        {
            _configs = new List<T>();
        }
        _configs.Clear();

        T[] configs = Resources.LoadAll<T>(FolderName);
        for (int i = 0; i < configs.Length; i++)
        {
            _configs.Add(configs[i]);
        }

        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif

    public bool TryGetConfigByID(string id, out T config)
    {
#if UNITY_EDITOR
        RefreshIdToConfigMap();
#endif
        return _idToConfig.TryGetValue(id, out config);
    }

    public T GetConfigByID(string id)
    {
#if UNITY_EDITOR
        RefreshIdToConfigMap();
#endif
        if (_idToConfig.ContainsKey(id))
        {
            return _idToConfig[id];
        }
        return null;
    }

    private void OnEnable()
    {
        RefreshIdToConfigMap();
    }

    private void RefreshIdToConfigMap()
    {
        if (_idToConfig == null)
        {
            _idToConfig = new Dictionary<string, T>();
        }
        _idToConfig.Clear();

        for (int i = 0; i < _configs.Count; i++)
        {
            if (_configs[i] == null)
            {
                Debug.LogError("ScriptableConfigGroup [" + name + "]'s " + i + " item is null.");
            }
            else if (string.IsNullOrEmpty(_configs[i].ID))
            {
                Debug.LogError("ScriptableConfigGroup [" + name + "]'s " + i + " item's id is empty.");
            }
            else
            {
                if (_idToConfig.ContainsKey(_configs[i].ID))
                {
                    Debug.LogError(typeof(T) + " config with same ID: " + _configs[i].ID);
                }
                else
                {
                    _idToConfig.Add(_configs[i].ID, _configs[i]);
                }
            }
        }
    }

    [SerializeField]
    private List<T> _configs;

    private Dictionary<string, T> _idToConfig;

    #region ScriptableSingleton

    public static R Instance
    {
        get
        {
            if (cachedInstance == null)
            {
                cachedInstance = Resources.Load(ResourcePath) as R;
            }
#if UNITY_EDITOR
            if (cachedInstance == null)
            {
                cachedInstance = CreateAndSave();
            }
#endif
            if (cachedInstance == null)
            {
                Debug.LogWarning("No instance of " + FileName + " found, using default values");
                cachedInstance = ScriptableObject.CreateInstance<R>();
                cachedInstance.OnCreate();
            }
            return cachedInstance;
        }
    }

    private static string FileName
    {
        get
        {
            return typeof(R).Name;
        }
    }

#if UNITY_EDITOR
    private static string AssetPath
    {
        get
        {
            return "Assets/Resources/" + FileName + ".asset";
        }
    }
#endif

    private static string ResourcePath
    {
        get
        {
            return FileName;
        }
    }

    private static R cachedInstance;

#if UNITY_EDITOR
    protected static R CreateAndSave()
    {
        R instance = ScriptableObject.CreateInstance<R>();
        instance.OnCreate();
        //Saving during Awake() will crash Unity, delay saving until next editor frame
        if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
        {
            UnityEditor.EditorApplication.delayCall += () => SaveAsset(instance);
        }
        else
        {
            SaveAsset(instance);
        }
        return instance;
    }

    private static void SaveAsset(R obj)
    {
        string dirName = System.IO.Path.GetDirectoryName(AssetPath);
        if (!System.IO.Directory.Exists(dirName))
        {
            System.IO.Directory.CreateDirectory(dirName);
        }
        UnityEditor.AssetDatabase.CreateAsset(obj, AssetPath);
        UnityEditor.AssetDatabase.SaveAssets();
        Debug.Log("Saved " + FileName + " instance");
    }
#endif

    protected virtual void OnCreate()
    {
        // Do setup particular to your class here
    }

    #endregion
}
