using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.EventChannels;
using DG.Tweening;

namespace WormGame.Core.Enemies
{
    /// <summary>
    /// This class controls the visual appearance of the enemy.
    /// </summary>
    /// <remarks>
    /// This class uses the SpriteRenderer component to change the
    /// enemy's color to red for a duration when it takes damage.
    /// </remarks>
    /// 
    public class EnemyVisuals : MonoBehaviour
    {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private SpriteRenderer _spriteRend;
        private WaitForSeconds _waitFor03;
        [Header("Broadcast on Event Channels")]
        [SerializeField] private Vector2EventChannelSO _enemyHitSO;
        [SerializeField] private IntVector2EventChannelSO _showingDamagePopupSO;

        private void Start()
        {
            _waitFor03 = new WaitForSeconds(0.3f);
        }

        public void ShowDamageEffect()
        {
            if (gameObject.activeSelf)
            {
                StartCoroutine(ShowDamageEffectCoroutine());
                transform.DOShakeScale(1.1f, 0.3f);
            }
        }

        private IEnumerator ShowDamageEffectCoroutine()
        {
            ObjectPooler.Instance.GetObject("DamagePopUpTag", transform.position, Quaternion.identity);
            OnEnemyHit(transform.position);
            OnShowingDamagePopup(8, transform.position);

            // Setting the boolean value that controls the branching in enemy's shader, to show that he was damaged
            _spriteRend.material.SetFloat("_IsDamaged", 1);

            yield return _waitFor03;

            _spriteRend.material.SetFloat("_IsDamaged", 0);
        }

        private void OnEnemyHit(Vector2 spawnPosition)
        {
            _enemyHitSO.RaiseEvent(spawnPosition);         
        }

        private void OnShowingDamagePopup(int damageDone, Vector2 spawnPosition)
        {
            _showingDamagePopupSO.RaiseEvent(damageDone, spawnPosition);
        }


        private void OnEnable()
        {
            _spriteRend.material.SetFloat("_IsDamaged", 0);
        }
    }
}