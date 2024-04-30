using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WormGame.EventChannels;
using DG.Tweening;

namespace WormGame
{
    public class LevelTitleTextUpdater : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshPro;
        [Header("Listen to Event Channels")]
        [SerializeField] private StringEventChannelSO _updateLevelTitleText;

        private void UpdateLevelTitleText(string text)
        {
            _textMeshPro.text = text;
            _textMeshPro.DOFade(1, 0.0001f);
            _textMeshPro.DOFade(0, 10);
        }

        private void OnEnable()
        {
            _updateLevelTitleText.OnEventRaised += UpdateLevelTitleText;
        }

        private void OnDisable()
        {
            _updateLevelTitleText.OnEventRaised -= UpdateLevelTitleText;
        }
    }
}
