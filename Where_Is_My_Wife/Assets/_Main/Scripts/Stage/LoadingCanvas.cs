using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingCanvas : MonoBehaviour
{
   private void OnEnable()
   {
      DontDestroyOnLoad(gameObject);
   }

   public Loading _LoadingObject;
   
   public void LoadingScreenIn()
   {
      _LoadingObject.OnLoadingScreenSet();
   }
   public void LoadingScreenOut()
   {
      _LoadingObject.OnLoadingScreenOff();
      Destroy(gameObject);
   }
}
