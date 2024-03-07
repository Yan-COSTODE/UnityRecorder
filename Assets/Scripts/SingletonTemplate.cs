using UnityEngine;

public class SingletonTemplate<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;

    public static T Instance => instance;

    protected virtual void Awake() => InitSingleton();

    private void InitSingleton()
    {
        if (instance)
        {
            Debug.LogWarning($"More than one {typeof(T)} instance found");
            Destroy(this);
            return;
        }

        instance = this as T;
        name += " [MANAGER]";
    }
}
