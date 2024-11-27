using UnityEngine;

namespace WhereIsMyWife.Porting
{
    public class DestroyOnXbox : MonoBehaviour
    {
        private void Awake()
        {
            #if XBOX
                Destroy(gameObject);
            #endif
        }
    }
}