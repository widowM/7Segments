using System.Collections;
using UnityEngine;
using WormGame.EventChannels;
using WormGame.Core;
using WormGame.Variables;

namespace WormGame
{
    public class BombExplosion : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _circle;
        [SerializeField] private ParticleSystem _explosion;
        [SerializeField] private ParticleSystem _smoke;
        [SerializeField] private AudioClip _preExplosionSFX;
        [SerializeField] private AudioClip _explosionSFX;
        [SerializeField] private GameObject _areaOfDamage;
        [SerializeField] private StringVariableSO _bombExplosionTag;
        private WaitForSeconds _zeroThreeSeconds = new WaitForSeconds(0.3f);
        private WaitForSeconds _fourSeconds = new WaitForSeconds(4);
        private WaitForSeconds _zeroOneSeconds = new WaitForSeconds(0.1f);
        [Header("Broadcast on Event Channels")]
        [SerializeField] private FloatFloatEventChannelSO _shakeCameraSO;


        private void OnEnable()
        {
            StartCoroutine(ExplosionCoroutine());
        }

        IEnumerator ExplosionCoroutine()
        {
            yield return _zeroOneSeconds;
            AudioSource.PlayClipAtPoint(_explosionSFX, transform.position, 1.5f);
            _explosion.Emit(45);
            OnShakingCamera();
            _areaOfDamage.SetActive(true);
            yield return _zeroThreeSeconds;
            _smoke.Emit(30);
            _areaOfDamage.SetActive(false);
            yield return _fourSeconds;
            ObjectPooler.Instance.ReturnObject(gameObject, _bombExplosionTag.Value);
        }

        private void OnShakingCamera()
        {
            _shakeCameraSO.RaiseEvent(18.36f, 0.8f);
        }
    }
}
