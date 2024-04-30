using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.Core.Enemies;

namespace WormGame.Core.Utils
{
    // Needed for diagey enemy scriptable object behaviour
    public class CustomTimer : MonoBehaviour
    {
        public float timer = 2;
        private WaitForSeconds _waitForTimerDuration;
        private WaitForSeconds _waitForTimerDurationPlus10;
        private EnemyDiageyChaseSO _enemyDiageyChaseSOInstance;

        private void Start()
        {
            _waitForTimerDuration = new WaitForSeconds(timer);
            _waitForTimerDurationPlus10 = new WaitForSeconds(timer + 10);
        }
        public void ResetTimer()
        {
            timer = 2;
        }

        public void StartTheTimer(Vector2 chargeDirection)
        {
            StartCoroutine(StartCountdown(chargeDirection));
        }

        public void StartSecondTimer()
        {
            StartCoroutine(StartHasChargedCountdown());
        }

        public IEnumerator StartCountdown(Vector2 chargeDirection)
        {
            yield return _waitForTimerDuration;
            _enemyDiageyChaseSOInstance = (EnemyDiageyChaseSO)GetComponent<EnemyChaseState>().EnemyChaseSOBaseInstance;
            _enemyDiageyChaseSOInstance.ChargeToHead(chargeDirection);

        }

        public IEnumerator StartHasChargedCountdown()
        {
            yield return _waitForTimerDurationPlus10;
            _enemyDiageyChaseSOInstance = (EnemyDiageyChaseSO)GetComponent<EnemyChaseState>().EnemyChaseSOBaseInstance;
            _enemyDiageyChaseSOInstance.ResetHasChargedFlag();
        }
    }
}