using System.Collections;
using UnityEngine;
using WormGame.Core;
using WormGame.Core.Containers;

namespace WormGame.VisualEffects
{
    /// <summary>
    /// This class plays the impact particle effect when it is enabled.
    /// </summary>
    /// <remarks>
    /// This class uses the ParticleSystem component to play the impact particle effect
    /// and uses the OnEnable method to trigger the effect when the game object is activated.
    /// It also uses a coroutine to send the particle effect to the object pool after it is finished playing.
    /// </remarks>
    /// 
    public class ImpactParticle : MonoBehaviour
    {
        [SerializeField] private ObjectPoolTagsContainerSO m_poolTagsContainerSO;
        [SerializeField] private ParticleSystem _hitParticleEffect;
        private readonly WaitForSecondsRealtime _backToPoolDelay = new WaitForSecondsRealtime(2f);

        private void OnEnable()
        {
            _hitParticleEffect.Play();
            StartCoroutine(SendMeBackToPoolCoroutine());
        }

        private IEnumerator SendMeBackToPoolCoroutine()
        {
            yield return _backToPoolDelay;

            ObjectPooler.Instance.ReturnObject(gameObject, m_poolTagsContainerSO.ImpactParticleTag);
        }
    }
}