using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using WormGame.EventChannels;
using WormGame.Variables;
using WormGame.Core;

namespace WormGame
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private StringVariableSO _bombExplosionTag;
        [SerializeField] private StringVariableSO _actualBombTag;
        [SerializeField] private AudioClip _tickingClip;
        [SerializeField] private AudioSource _audioSource;

        private void OnEnable()
        {
            DOTween.Sequence().Append(_spriteRenderer.DOFade(1, 0.001f)).
                Append(_spriteRenderer.DOFade(0, 1).OnStepComplete(OnBombTicking).SetLoops(2, LoopType.Restart)).
                Append(_spriteRenderer.DOFade(1, 0.0001f)).
                Append(_spriteRenderer.DOFade(0, 0.5f).OnStepComplete(OnBombTicking).SetLoops(4, LoopType.Restart)).
                Append(_spriteRenderer.DOFade(1, 0.0001f)).
                Append(_spriteRenderer.DOFade(0, 0.25f).OnStepComplete(OnBombTicking).SetLoops(8, LoopType.Restart)).
                Append(_spriteRenderer.DOFade(1, 0.0001f)).
                Append(_spriteRenderer.DOFade(0, 0.125f).OnStepComplete(OnBombTicking).SetLoops(16, LoopType.Restart)).OnComplete(Detonate);
        }

        private void OnBombTicking()
        {
            float randomVolume = Random.Range(0.7f, 0.9f);
            float randomPitch = Random.Range(0.85f, 1);
            _audioSource.volume = randomVolume;
            _audioSource.pitch = randomPitch;
            _audioSource.PlayOneShot(_tickingClip);
        }
        private void Detonate()
        {
            SpawnExplosion();
            ObjectPooler.Instance.ReturnObject(gameObject, _actualBombTag.Value);
        }

        private void SpawnExplosion()
        {
            ObjectPooler.Instance.GetObject(_bombExplosionTag.Value, transform.position, Quaternion.identity);
        }
    }
}
