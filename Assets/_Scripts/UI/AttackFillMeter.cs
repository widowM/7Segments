using System.Collections;
using TMPro;
using UnityEngine;
using WormGame.EventChannels;
using UnityEngine.UI;
using DG.Tweening;

namespace WormGame.UI
{
    public class AttackFillMeter : MonoBehaviour
    {
        [SerializeField] private Image _attackMeter;
        [SerializeField] private Transform _myTransform;
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
        [SerializeField] private float _cooldownTime;

        [Header("Broadcast on Event Channels")]
        [SerializeField] private VoidEventChannelSO _attackMeterFilled;

        [Header("Listen to Event Channels")]
        [SerializeField] private StringEventChannelSO _attackUINameChangedSO;
        [SerializeField] private FloatEventChannelSO _passCooldownValueSO;

        private void SetCooldown(float value)
        {
            _cooldownTime = value;
        }

        public void ResetMeter()
        {
            StartCoroutine(FillAttackMeterCoroutine());
        }

        private void OnAttackMeterFilled()
        {
            _attackMeterFilled.RaiseEvent();
        }

        private IEnumerator FillAttackMeterCoroutine()
        {
            _attackMeter.fillAmount = 0;

            while (_attackMeter.fillAmount < 1)
            {
                _attackMeter.fillAmount += Time.unscaledDeltaTime / _cooldownTime;
                yield return null;
            }
            
            OnAttackMeterFilled();

            // UI Short animation
            Sequence seq = DOTween.Sequence();
            seq.Append(_myTransform.DOScale(1.1f, 0.1f)).
                Append(_myTransform.DOScale(1f, 0.1f)).
                Join(_attackMeter.DOFade(1, 0.05f)).
                Append(_attackMeter.DOFade(0.7843137f, 0.05f)).
                SetRecyclable(true).SetUpdate(true);
        }

        private void ChangeAttackMeterName(string newName)
        {
            _textMeshProUGUI.text = newName;
            Debug.Log("Changed weapon name");
        }    
        private void OnEnable()
        {
            _attackUINameChangedSO.OnEventRaised += ChangeAttackMeterName;
            _passCooldownValueSO.OnEventRaised += SetCooldown;
        }

        private void OnDisable()
        {
            _attackUINameChangedSO.OnEventRaised -= ChangeAttackMeterName;
            _passCooldownValueSO.OnEventRaised -= SetCooldown;
        }
    }
}