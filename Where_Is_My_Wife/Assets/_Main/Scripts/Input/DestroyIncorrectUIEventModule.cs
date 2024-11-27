using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class DestroyIncorrectUIEventModule : MonoBehaviour
{
    [SerializeField] private StandaloneInputModule _standaloneInputModule;
    [SerializeField] private InputSystemUIInputModule _inputSystemUiInputModule;
    
    private void Awake()
    {
        #if XBOX
        Destroy(_inputSystemUiInputModule);
        #endif
        
        #if !XBOX
        Destroy(_standaloneInputModule);
        #endif  
    }
}
