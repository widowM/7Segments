using UnityEngine;
using UnityEngine.UI;
using WormGame.EventChannels;
using DG.Tweening;
namespace WormGame
{
    public class LevelFillingMeterLogic : MonoBehaviour
    {
        [SerializeField] GameObject _soulFillPanel;
        [SerializeField] private Image _fillingMeter;
        [Header("Listen to Event Channels")]
        [SerializeField] private FloatFloatEventChannelSO _fillingLevelMeterSO;
        [SerializeField] private VoidEventChannelSO _onGameStartedSO;

        private void Init()
        {
            _fillingMeter.fillAmount = 0;
        }

        private void FillMeterByPortionOfTotal(float soulsPicked, float totalSoulsNeeded)
        {
            float fillAmount = soulsPicked / totalSoulsNeeded;
            _fillingMeter.fillAmount = fillAmount;
            DOTween.Sequence().Append(_soulFillPanel.transform.DOScale(new Vector2(1.1f, 1.1f), 0.3f)).
                Append(_soulFillPanel.transform.DOScale(new Vector2(1, 1), 0.3f)).SetUpdate(true);
        }
       
        private void OnEnable()
        {
            _fillingLevelMeterSO.OnEventRaised += FillMeterByPortionOfTotal;
            _onGameStartedSO.OnEventRaised += Init;
        }

        private void OnDisable()
        {
            _fillingLevelMeterSO.OnEventRaised -= FillMeterByPortionOfTotal;
            _onGameStartedSO.OnEventRaised -= Init;
        }
    }
}
