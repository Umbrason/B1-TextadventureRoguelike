using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    public static T Instance { get; protected set; }
    protected virtual void Awake()
    {
        Instance ??= (T)this;
    }

    protected virtual void OnDestroy()
    {
        Instance = this == Instance ? null : Instance;
    }
}