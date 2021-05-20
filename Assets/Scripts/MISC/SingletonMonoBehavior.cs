using UnityEngine;

public abstract class SingletonMonoBehavior<T> : MonoBehaviour where T:MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    //Standard Unity method when game object is initialized
    //Protected, can be accessed by inherting classes
    //Virtual, can be overwritten by inherting classes
    protected virtual void Awake()
    {
        if(instance == null)
        {
            instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }

 
}
