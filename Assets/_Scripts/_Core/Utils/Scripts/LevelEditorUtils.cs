using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using WormGame.Core;
using WormGame.Core.Utils;
using System.IO;

public static class LevelEditorUtils
{
    public static string _currentScene;

    public static void GenerateSavedChains()
    {

        // read file into a string and deserialize JSON to a list of WormChainEntry
        // Assuming the second scene is loaded additively

        if (SceneManager.loadedSceneCount > 1)
        {
            Scene scene = SceneManager.GetSceneAt(1);
            _currentScene = scene.name;
        }
        else
        {
            _currentScene = SceneManager.GetActiveScene().name;
        }
        Debug.Log(_currentScene);

#if UNITY_EDITOR
        string path = Path.Combine(Application.streamingAssetsPath, "Saves", "WormChainsSaves", _currentScene + "_ChainsArrangement.txt");
        string json = File.ReadAllText(path);
        WormChainEntry[] entries = JsonConvert.DeserializeObject<WormChainEntry[]>(json);
        foreach (WormChainEntry entry in entries)
        {
            WormUtils.GenerateSavedChainOfLengthAtPosition(entry.length, entry.points);
        }
#else
            string pathStandalone = Path.Combine(Application.streamingAssetsPath, "Saves", "WormChainsSaves", _currentScene + "_ChainsArrangement.txt");

            string jsonStandalone = File.ReadAllText(pathStandalone);
            WormChainEntry[] entriesStandalone = JsonConvert.DeserializeObject<WormChainEntry[]>(jsonStandalone);
            foreach (WormChainEntry entryStandalone in entriesStandalone)
            {
                WormUtils.GenerateSavedChainOfLengthAtPosition(entryStandalone.length, entryStandalone.points);
            }
#endif
    }

    public static void SpawnSavedLevelGameObjects()
    {
        // read file into a string and deserialize JSON to a list of WormChainEntry
        _currentScene = SceneManager.GetActiveScene().name;
#if UNITY_EDITOR
        string path = Path.Combine(Application.streamingAssetsPath, "Saves", "LevelEditorSaves", _currentScene + "_SceneSetup.txt");
        string json = File.ReadAllText(path);
        LevelEditorEntry[] entries = JsonConvert.DeserializeObject<LevelEditorEntry[]>(json);
        foreach (LevelEditorEntry entry in entries)
        {
            Vector2 pos = new Vector2(entry.pos.x, entry.pos.y);
            ObjectPooler.Instance.GetObject(entry.poolTag, pos, Quaternion.identity);
        }

#else
            string pathStandalone = Path.Combine(Application.streamingAssetsPath, "Saves", "LevelEditorSaves", _currentScene + "_SceneSetup.txt");
            string jsonStandalone = File.ReadAllText(pathStandalone);
            LevelEditorEntry[] entriesStandalone = JsonConvert.DeserializeObject<LevelEditorEntry[]>(jsonStandalone);
            foreach (LevelEditorEntry entryStandalone in entriesStandalone)
            {
                Vector2 pos = new Vector2(entryStandalone.pos.x, entryStandalone.pos.y);
                ObjectPooler.Instance.GetObject(entryStandalone.poolTag, pos, Quaternion.identity);
            }
#endif
    }
}
