using UnityEngine;
using WormGame.EventChannels;
using WormGame.Variables;

namespace WormGame
{
    [System.Serializable]
    public struct LevelInfo
    {
        public string levelScene;
        public int difficulty;
    }

    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelInfo[] levels;
        private int levelsWon = 0;
        //
        private int levelsWonInChain = 0;
        private static int currentDifficulty = 0;

        [Header("Broadcast on Event Channels")]
        [SerializeField] private StringEventChannelSO _loadSceneByNameSO;
        [SerializeField] private StringEventChannelSO _updateLevelTitleText;
        [SerializeField] private VoidEventChannelSO _gameFinishedScreenShownSO;

        [Header("Listen to Event Channels")]
        [SerializeField] private VoidEventChannelSO _selectInitialSceneSO;
        [SerializeField] private VoidEventChannelSO _selectNextSceneSO;
        [SerializeField] private VoidEventChannelSO _resetSceneSO;

        [ContextMenu("Finishing Game")]
        private void FinishGame()
        {
            levelsWon = 2;
            currentDifficulty = 2;
            GetRandomLevelAndIncreaseLevelsWonCount(2);

        }
        private void Initialize()
        {
            currentDifficulty = 0;
            levelsWonInChain = 0;
            string sceneToLoad = GetRandomLevelSameDifficulty();
            // Message the scene loader
            _loadSceneByNameSO.RaiseEvent(sceneToLoad);
        }
        
        private void ResetLevel()
        {
            string sceneToLoad = GetRandomLevelSameDifficulty();
            // Message the scene loader
            _loadSceneByNameSO.RaiseEvent(sceneToLoad);
        }
        private void SelectNextLevel()
        {
            string sceneToLoad = GetRandomLevelAndIncreaseLevelsWonCount(currentDifficulty);

            if (sceneToLoad == "finished")
            {
                return;
            }

            _loadSceneByNameSO.RaiseEvent(sceneToLoad);
        }

        private string GetRandomLevelAndIncreaseLevelsWonCount(int difficulty)
        {
            // Filter levels based on the current difficulty
            LevelInfo[] filteredLevels = System.Array.FindAll(levels, l => l.difficulty == difficulty);

            // Randomly select a level from the filtered levels
            int randomIndex = Random.Range(0, filteredLevels.Length);

            levelsWon++;
            levelsWonInChain++;
            // Every three levels increase the difficulty
            // and maybe stats - TODO
            if (levelsWon % 3 == 0)
            {
                Debug.Log("Entered the first nest");
                currentDifficulty++;
                Debug.Log("Current difficulty is: " + currentDifficulty);
                if(currentDifficulty > 2)
                {
                    _gameFinishedScreenShownSO.RaiseEvent();
                    return "finished";
                }
                levelsWonInChain = 0;
            }

            string levelTitle = $"{currentDifficulty}_{levelsWonInChain}";
            _updateLevelTitleText.RaiseEvent(levelTitle);

            return filteredLevels[randomIndex].levelScene;
        }

        private string GetRandomLevelSameDifficulty()
        {
            // Filter levels based on the current difficulty
            LevelInfo[] filteredLevels = System.Array.FindAll(levels, l => l.difficulty == currentDifficulty);

            // Randomly select a level from the filtered levels
            int randomIndex = Random.Range(0, filteredLevels.Length);

            string levelTitle = $"{currentDifficulty}_{levelsWonInChain}";
            _updateLevelTitleText.RaiseEvent(levelTitle);

            return filteredLevels[randomIndex].levelScene;
        }

        private void OnEnable()
        {
            _selectInitialSceneSO.OnEventRaised += Initialize;
            _selectNextSceneSO.OnEventRaised += SelectNextLevel;
            _resetSceneSO.OnEventRaised += ResetLevel;
        }

        private void OnDisable()
        {
            _selectInitialSceneSO.OnEventRaised -= Initialize;
            _selectNextSceneSO.OnEventRaised -= SelectNextLevel;
            _resetSceneSO.OnEventRaised -= ResetLevel;
        }
    }
}
