using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRecycler
{
#if UNITY_EDITOR
    public static bool WarningForInsufficientCapacity { get; set; }
    public List<RecyclableObject> ObjectList { get { return _objectList; } }
#endif

    public ObjectRecycler(GameObject gameObject, int initialCapacity,
        GameObject parentObject, Action<GameObject> onInstantiate)
    {
        _objectList = new List<RecyclableObject>(initialCapacity);
        _objectToRecycle = gameObject;
        _parentObject = parentObject;
        OnInstantiate = onInstantiate;

        for (int i = 0; i < initialCapacity; i++)
        {
            InstantiateNewRecyclable();
        }
    }

    public ObjectRecycler(GameObject gameObject, int initialCapacity,
                          GameObject parentObject)
    : this(gameObject, initialCapacity, parentObject, null) { }

    public GameObject GetNextFree(bool dontInstantiateNew = false)
    {
        RecyclableObject freeObject = null;

        for (int i = 0; i < _objectList.Count; i++)
        {
            if (_objectList[i].IsAvailabe == true)
            {
                freeObject = _objectList[i];
                break;
            }
        }

        if (freeObject == null && !dontInstantiateNew)
        {
#if UNITY_EDITOR
            if (WarningForInsufficientCapacity)
            {
                Debug.LogWarning(string.Format("Pool size for [{0}] is not sufficient, and instantiation is kicked out at run-time",
                                               _objectToRecycle.name));
            }
#endif
            freeObject = InstantiateNewRecyclable();
        }

        if (freeObject != null)
        {
            freeObject.IsAvailabe = false;
            return freeObject.gameObject;
        }
        else
        {
            return null;
        }
    }

    public void FreeObject(GameObject objectToFree)
    {
        if (objectToFree != null)
        {
            if (objectToFree.GetComponent<RecyclableObject>() != null)
            {
                objectToFree.GetComponent<RecyclableObject>().IsAvailabe = true;
            }
            else
            {
                Debug.LogError("Try to free [" + objectToFree.name + "] that is not recyclable.");
            }
        }
    }

    private RecyclableObject InstantiateNewRecyclable()
    {
        GameObject newGameObject = GameObject.Instantiate(_objectToRecycle) as GameObject;
        RecyclableObject recyclable = newGameObject.AddComponent<RecyclableObject>();

        if (_parentObject != null)
        {
            recyclable.transform.SetParent(_parentObject.transform, false);
        }

        recyclable.IsAvailabe = true;
        _objectList.Add(recyclable);

        if (OnInstantiate != null)
        {
            OnInstantiate(newGameObject);
        }

        return recyclable;
    }

    public void FreeAllObjects()
    {
        foreach (RecyclableObject recyclable in _objectList)
        {
            if (recyclable != null)
            {
                if (recyclable.transform.parent != _parentObject.transform)
                {
                    recyclable.transform.SetParent(_parentObject.transform);
                }
                recyclable.IsAvailabe = true;
            }
            else
            {
                Debug.LogWarning("An object from recycler [" + _parentObject.name + "] is already destroyed.");
            }
        }
    }

    public void DestroyAllObjects()
    {
        foreach (RecyclableObject recyclable in _objectList)
        {
            if (recyclable != null)
            {
                recyclable.gameObject.DestroyBasedOnRunning();
            }
        }
        if (_parentObject != null)
        {
            _parentObject.DestroyBasedOnRunning();
        }

        _objectList.Clear();
    }

    private void DestroyObject(GameObject anObject)
    {
        if (Application.isPlaying)
        {
            GameObject.Destroy(anObject);
        }
        else
        {
            GameObject.DestroyImmediate(anObject);
        }
    }

    private Action<GameObject> OnInstantiate { get; set; }

    private List<RecyclableObject> _objectList;
    private GameObject _objectToRecycle;
    private GameObject _parentObject;
}
