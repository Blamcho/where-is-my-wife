using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    public Loading _LoadingChange;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _LoadingChange.ChangeToLoading();
        }       
    }
}
