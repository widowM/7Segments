using UnityEngine;
using TMPro;
using WormGame.EventChannels;
using System.Text;
using WormGame.Core;
using DG.Tweening;

namespace WormGame.UI
{

    /// <summary>
    /// This class is responsible for updating the UI text that shows the number of active segments of the worm.
    /// It has a reference to a TextMeshProUGUI component that displays the text. It listens to an event channel
    /// that passes an int value as a parameter. It uses the RefreshActiveSegmentsUI method to update the text
    /// with the current number of active segments. It subscribes and unsubscribes to the event channel when the
    /// class is enabled and disabled.
    /// </summary>
    public class ActiveWormSegmentsUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _activeSegmentsText;
        [SerializeField] private WormDataSO _wormDataSO;
        [SerializeField] private Transform _parentTransform;
        [Header("Listen to Event Channels")]
        [SerializeField] private IntEventChannelSO _updateActiveSegmentsUISO;

        private void RefreshActiveSegmentsUI(int activeNumOfSegments)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("x ");
            sb.Append(activeNumOfSegments);

            _activeSegmentsText.text = sb.ToString();
            Sequence seq = DOTween.Sequence();
            seq.Append(_parentTransform.DOScale(0.52f, 0.1f)).
                Append(_parentTransform.DOScale(0.5f, 0.1f)).
                SetRecyclable(true);

        }

        private void OnEnable()
        {
            _updateActiveSegmentsUISO.OnEventRaised += RefreshActiveSegmentsUI;
            RefreshActiveSegmentsUI(_wormDataSO.CurrentWormLength);
        }

        private void OnDisable()
        {
            _updateActiveSegmentsUISO.OnEventRaised -= RefreshActiveSegmentsUI;
        }
    }
}