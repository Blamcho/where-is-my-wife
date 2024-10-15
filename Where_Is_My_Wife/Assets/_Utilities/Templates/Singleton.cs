using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    [SerializeField] private bool _shouldDestroyOnLoad = false;

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            if (!_shouldDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject); 
            }
        }
        else
        {
            Destroy(gameObject); 
        }
    }
}