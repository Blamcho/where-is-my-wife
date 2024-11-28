using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace WhereIsMyWife.Porting
{
    public class UIInputSystemManager : MonoBehaviour
    {
        [SerializeField] private StandaloneInputModule _standaloneInputModule;
        [SerializeField] private InputSystemUIInputModule _inputSystemUiInputModule;
    
        private void Awake()
        {
            SelectInputSystemBasedOnPlatform();
        }
        
        private void SelectInputSystemBasedOnPlatform()
        {
            #if XBOX
                Destroy(_inputSystemUiInputModule);
            #endif
                            
            #if !XBOX
                Destroy(_standaloneInputModule);
            #endif  
        }
    }
}