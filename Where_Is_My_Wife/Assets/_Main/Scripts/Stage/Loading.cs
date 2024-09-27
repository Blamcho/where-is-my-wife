using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{ 
   enum LOADING_PARTS
   {
      LOADING_SCENE,
      UNLOAD_SCENE,
      LOAD_NEXT,
      NONE,
   }

   public string _SceneToLoad = "";
   
   public int _SceneToLoadNum = -1;

   private const string _LoadingSceneName = " LoadingScreen";

   private LOADING_PARTS _NextLoad = LOADING_PARTS.NONE;

   public Animation _AnimationLoadScreen;
   
   public event Action _onStartChangingScene;

    int _CurrentScene;

   public void Awake()
   {
      _NextLoad = LOADING_PARTS.NONE;
   }

   public void ChangeToLoading() //cambias de escena a la de loading
   {
      DontDestroyOnLoad(gameObject); //no destruyes el canvas
      _CurrentScene = SceneManager.GetActiveScene().buildIndex; 
      SceneManager.sceneUnloaded += OnSceneUnLoaded;//eventos que llaman a las escenas de cargar o descargar escenas para volver a llamarlos
      _AnimationLoadScreen.gameObject.SetActive(true); //activas la animacion
      _AnimationLoadScreen.Play("LoadingAnim");
      //
   }

   public void OnLoadingScreenSet() // cuando termine el changetoloading se llama a esta funcion, y se agrega la escena de loading
   {
      _NextLoad = LOADING_PARTS.LOADING_SCENE; //Cargamos la escena de loading
      
      if(_onStartChangingScene != null) //llamamos al evento
         _onStartChangingScene.Invoke();

      SceneManager.LoadSceneAsync(_LoadingSceneName,LoadSceneMode.Additive);
   }
   void OnSceneLoaded(Scene _scene, LoadSceneMode _load) //se llama automaticamente por el scenemanager
   {
      StartCoroutine(WaitOnLevelLoaded());
   }

   IEnumerator WaitOnLevelLoaded()
   {
      if (_NextLoad == LOADING_PARTS.LOADING_SCENE) //tenemos la escena 1 y la de loading screen
      {
         _NextLoad = LOADING_PARTS.UNLOAD_SCENE; //descargas la siguiente escena
      
         while (SceneManager.sceneCount < 2)
         {
            yield return new WaitForEndOfFrame();
         }
         SceneManager.UnloadSceneAsync(_CurrentScene); // quitamos la escena 1 y se pone la loading screen
      }
      else if (_NextLoad == LOADING_PARTS.LOAD_NEXT)
      {
         _NextLoad = LOADING_PARTS.NONE;
         while (SceneManager.sceneCount < 2)
         {
            yield return new WaitForEndOfFrame();
         }
         _AnimationLoadScreen.Play("LoadingAnimOut");
         SceneManager.UnloadSceneAsync(_LoadingSceneName);
      }
   }
   void OnSceneUnLoaded(Scene _scene)//se llama automaticamente por el scenemanager
   {
      if (_NextLoad!=LOADING_PARTS.UNLOAD_SCENE)
         return;         
      
      _NextLoad = LOADING_PARTS.LOAD_NEXT;
      SceneManager.sceneUnloaded -= OnSceneUnLoaded;
      if (_SceneToLoadNum > 0)
      {
         SceneManager.LoadSceneAsync(_SceneToLoadNum,LoadSceneMode.Additive);
      }
      else
      {
         SceneManager.LoadSceneAsync(_SceneToLoad,LoadSceneMode.Additive);
      }
   }

   public void OnLoadingScreenOff()
   {
      Destroy(gameObject);
   }
}
