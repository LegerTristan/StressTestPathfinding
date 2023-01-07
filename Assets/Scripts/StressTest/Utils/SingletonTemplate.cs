using UnityEngine;

/// <summary>
/// Template class for Singleton pattern.
/// </summary>
/// <typeparam name="TSingleton">Singleton class</typeparam>
public class SingletonTemplate<TSingleton> : MonoBehaviour where TSingleton : MonoBehaviour
{
    #region F/P
    static TSingleton instance = null;

    public static TSingleton Instance => instance;
    #endregion

    #region Methods
    void Awake() => InitSingleton();

    void InitSingleton()
    {
        if(instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this as TSingleton;
    }
    #endregion
}
