using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using WormGame.EventChannels;
using WormGame.Core.Utils;
using System;

namespace WormGame.SceneManagement
{

    /// <summary>
    /// Use this basic helper for loading scenes by name, index, etc.
    /// 
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        // Fields
        [Header("Listen to Event Channels")]
        [Tooltip("Loads a scene by its Scene path string")]
        [SerializeField, Optional] private StringEventChannelSO _loadSceneByNameEventChannelSO;
        [Tooltip("Reloads the current scene")]
        [SerializeField, Optional] private VoidEventChannelSO _reloadSceneEventChannelSO;
        [Tooltip("Loads the next scene by index in the Build Settings")]
        [SerializeField, Optional] private VoidEventChannelSO _loadNextSceneEventChannelSO;

        [Tooltip("Unloads the last scene, stops GameplayDataSO")]
        [SerializeField, Optional] private VoidEventChannelSO _lastSceneUnloaded;

        [Header("Broadcast on Event Channels")]
        [SerializeField] private VoidEventChannelSO _asyncSceneLoadCompletedSO;
        // Default loaded scene that serves as the entry point and does not unload
        private static string BOOTLOADER_SCENE = "BootloaderScene";

        // The previously loaded scene
        private string _lastLoadedScene;
        private int currentScene = 0;

        public static string BootstrapScene => BOOTLOADER_SCENE;

        private void OnEnable()
        {
            if (_loadSceneByNameEventChannelSO != null)
                _loadSceneByNameEventChannelSO.OnEventRaised += LoadSceneByName;

            if (_reloadSceneEventChannelSO != null)
                _reloadSceneEventChannelSO.OnEventRaised += ReloadScene;

            if (_lastSceneUnloaded != null)
                _lastSceneUnloaded.OnEventRaised += UnloadScene;
        }

        private void OnDisable()
        {
            if (_loadSceneByNameEventChannelSO != null)
                _loadSceneByNameEventChannelSO.OnEventRaised -= LoadSceneByName;

            if (_reloadSceneEventChannelSO != null)
                _reloadSceneEventChannelSO.OnEventRaised -= ReloadScene;

            if (_lastSceneUnloaded != null)
                _lastSceneUnloaded.OnEventRaised -= UnloadScene;
        }

        [ContextMenu("Load consecutive scenes")]
        public void LoadConsecutiveScenes()
        {
            currentScene++;
            string sceneName = SceneUtility.GetScenePathByBuildIndex(currentScene);
            sceneName = System.IO.Path.GetFileNameWithoutExtension(sceneName);
            LoadSceneByName(sceneName);
        }
        // Event-handling methods
        public void LoadSceneByName(string sceneName)
        {
            //
            GC.Collect();
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }

        public void UnloadScene()
        {
            StartCoroutine(UnloadLastSceneCoroutine());
        }

        // Reload the current scene
        public void ReloadScene()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }

        // Load the next scene by index in the Build Settings
        public void LoadNextScene()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
        }

        // Load a scene by its index number (non-additively)
        public void LoadScene(int buildIndex)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(buildIndex);

            if (string.IsNullOrEmpty(scenePath))
            {
                Debug.LogError("SceneLoader.LoadScene: invalid sceneBuildIndex");
                return;
            }

            SceneManager.LoadScene(scenePath);
        }


        // Coroutine to unload the previous scene and then load a new scene by scene path string
        private IEnumerator LoadSceneCoroutine(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("SceneLoader: Invalid scene name");
                yield break;
            }

            yield return UnloadLastSceneCoroutine();
            yield return LoadSceneAsyncCoroutine(sceneName);

        }

        // Coroutine to load a scene asynchronously by scene path string in Additive mode,
        // keeps the original scene as the active scene.
        private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
        {
            //Debug.Log("System IO path = " + Path.GetFileName(scenePath));

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (!asyncLoad.isDone)
            {
                float progress = asyncLoad.progress;
                yield return null;
            }

            _lastLoadedScene = sceneName;
            Debug.Log("Last loaded scene in Load Scene Async Coroutine: " + _lastLoadedScene);
            Scene sceneToActivate = SceneManager.GetSceneByName(sceneName);
            SceneManager.SetActiveScene(sceneToActivate);
            _asyncSceneLoadCompletedSO.RaiseEvent();
        }


        // Unloads the previously loaded scene if it's not the bootstrap scene
        private IEnumerator UnloadLastSceneCoroutine()
        {
            if (_lastLoadedScene != BOOTLOADER_SCENE)
                yield return UnloadSceneCoroutine(_lastLoadedScene);
        }

        // Coroutine to unload a specific Scene asynchronously
        private IEnumerator UnloadSceneCoroutine(string sceneName)
        {
            Scene lastScene = SceneManager.GetSceneByName(sceneName);
            if (!lastScene.IsValid())
                yield break;

            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);

            while (!asyncUnload.isDone)
            {
                yield return null;
            }
        }
    }
}