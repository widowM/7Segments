using UnityEngine;
using WormGame.Core;

namespace WormGame.Managers
{
    /// <summary>
    /// Sets up the paddle, ball, wall, and goal instances for GameplayDataSO. This can load the level
    /// from either an external JSON file or a LevelLayout ScriptableObject.
    /// </summary>
    public class GameSetup : MonoBehaviour
    {
        // Parent instantiated walls/goals under a Transform of this name
        public const string ROOT_TRANSFORM = "Level";

        // Default filename/subfolder for saving persistent data
        private const string JSON_FILENAME = "LevelLayout.json";
        private const string JSON_SUBFOLDER = "Json";
        private const string LEVEL_LAYOUT_SO_NAME = "LevelLayoutFromJSON";

        // Enum to choose an external file format
        public enum Mode
        {
            ScriptableObject,
            Json
        }

        [Header("Prefabs")]

        [Header("Level Data")]
        [Tooltip("Use ScriptableObject or Json file")]
        [SerializeField] private Mode m_Mode;

        [Header("ScriptableObject Data")]

        // Private fields
        private GameplayDataSO _gameplayDataSO;


        // Parents a Transform to a specific transform if it already exists in the Hierarchy. Creates the GameObject if it doesn't exist. 
        private void SetTransformParent(Transform transformToParent, string newParentName)
        {
            GameObject newParent = GameObject.Find(newParentName);

            if (newParent == null)
            {
                newParent = new GameObject(newParentName);
            }

            transformToParent.SetParent(newParent.transform);
        }
    }
}