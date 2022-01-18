using UnityEngine;
using System.Collections;

public abstract class ScriptableConfig<T> : ScriptableObject where T : ScriptableObject 
{
    public abstract string ID { get; }
}
