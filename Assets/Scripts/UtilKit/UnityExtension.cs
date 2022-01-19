using UnityEngine;
using System;
using System.Text;
using System.Text.RegularExpressions;

public static class UnityExtension
{
    public static void SetLayerIncludeChild(this GameObject gameObject, int newLayer, int excludeLayer = -1, int excludeLayer2 = -1)
    {
        if (gameObject.layer != excludeLayer && gameObject.layer != excludeLayer2)
            gameObject.layer = newLayer;

        var tranform = gameObject.transform;
        for (int i = 0; i < tranform.childCount; i++)
        {
            SetLayerIncludeChild(tranform.GetChild(i).gameObject, newLayer, excludeLayer, excludeLayer2);
        }
    }

    public static string GetGameObjectPath(this GameObject gameObject)
    {
        StringBuilder path = new StringBuilder(gameObject.name);
        while (gameObject.transform.parent != null)
        {
            gameObject = gameObject.transform.parent.gameObject;
            path.Insert(0, "/");
            path.Insert(0, gameObject.name);
        }
        return path.ToString();
    }

    public static void SetActiveAvoidNullError(this GameObject gameObject, bool isActive)
    {
        if (gameObject && (gameObject.activeSelf ^ isActive))
        {
            gameObject.SetActive(isActive);
        }
    }

    public static T GetComponentAndCreateIfNonExist<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }
        return component;
    }

    public static void Reset(this Transform transform)
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    public static void SetPositionX(this Transform transform, float x)
    {
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    public static void SetPositionY(this Transform transform, float y)
    {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    public static void SetPositionZ(this Transform transform, float z)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }

    public static void SetLocalPositionX(this Transform transform, float x)
    {
        transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
    }

    public static void SetLocalPositionY(this Transform transform, float y)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
    }

    public static void SetLocalPositionZ(this Transform transform, float z)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
    }

    public static bool IsBumping(this CollisionFlags collisionFlags)
    {
        return (collisionFlags & (CollisionFlags.CollidedSides)) != 0;
    }

    public static bool IsGrounded(this CollisionFlags collisionFlags)
    {
        return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
    }

    public static bool IsHittingHead(this CollisionFlags collisionFlags)
    {
        return (collisionFlags & CollisionFlags.CollidedAbove) != 0;
    }

    public static void DestroyBasedOnRunning(this UnityEngine.Object anObject)
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

    public static bool ExistChildByName(this Transform parent, string name)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).name.Trim().Equals(name.Trim()))
            {
                return true;
            }
        }
        return false;
    }

    public static Transform GetChildByName(this Transform parent, string name)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).name.Trim().Equals(name.Trim()))
            {
                return parent.GetChild(i);
            }
        }
        return null;
    }

    public static Transform GetChildRecursionByName(this Transform parent, string name, bool fullSearch)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (fullSearch ?
                    parent.GetChild(i).name.Trim().Equals(name) :
                    parent.GetChild(i).name.Trim().Contains(name))
            {
                return parent.GetChild(i);
            }
        }

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform result = GetChildRecursionByName(parent.GetChild(i), name, fullSearch);

            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    public static void DebugChildRecursion(this Transform trans)
    {
        Debug.LogWarning(trans.name, trans.gameObject);
        for (int i = 0; i < trans.childCount; i++)
        {
            trans.GetChild(i).DebugChildRecursion();
        }
        Debug.LogWarning("-----------");
    }

    public static Material GetMaterial(this Renderer renderer)
    {
        #if UNITY_EDITOR
        return renderer.material;
        #else
        return renderer.sharedMaterial;
        #endif
    }

    public static Material[] GetMaterials(this Renderer renderer)
    {
        #if UNITY_EDITOR
        return renderer.materials;
        #else
        return renderer.sharedMaterials;
        #endif
    }

    public static void SetMaterial(this Renderer renderer, Material mat)
    {
        #if UNITY_EDITOR
        renderer.material = mat;;
        #else
        renderer.sharedMaterial = mat;
        #endif
    }
}
