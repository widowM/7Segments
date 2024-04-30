using UnityEngine;

namespace WormGame.Core.Utils
{
    public static class GameLog
    {
        // Just in case you need to use GameLog.LogMessage
        // for debugging, you can go to Project Settings >
        // Player > Other Settings > Scripting Define Symbols
        // and add TEST_MODE to the list of symbols

        [System.Diagnostics.Conditional("TEST_MODE")]
        public static void LogMessage(string message)
        {
            Debug.Log(message);
        }

        [System.Diagnostics.Conditional("GameplayDataSO_TEST_MODE")]
        public static void GameplayInfoMessage(string message)
        {
            Debug.Log(message);
        }

        [System.Diagnostics.Conditional("TEST_MODE")]
        public static void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }

        [System.Diagnostics.Conditional("TEST_MODE")]
        public static void LogError(string message)
        {
            Debug.LogWarning(message);
        }
    }
}