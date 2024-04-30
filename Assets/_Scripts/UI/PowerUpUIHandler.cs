using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using WormGame.Collectables;
using UnityEngine.UI;
using WormGame.EventChannels;
using WormGame.Core;

namespace WormGame
{
    public class PowerUpUIHandler : MonoBehaviour
    {
        [SerializeField] private Image _panel;
        [SerializeField] RectTransform _panelRectTransform;
        [SerializeField] private GameObject _slotPrefab;
        private Color _initialPanelColor;
        [SerializeField] private Image _flashImage;
        [SerializeField] private Ease _ease;
        [SerializeField] private List<Image> powerUpSlots = new List<Image>();
        [SerializeField] private WormCollectionsDataSO _wormCollectionsDataSO;
        [Header("Broadcast on Event Channels")]
        [SerializeField] private VoidEventChannelSO _gotPickedSO;
        [SerializeField] private VoidEventChannelSO _playPoweredUpSoundSO;
        [Header("Listen to Event Channels")]
        [SerializeField] private PowerUpEventChannelSO _powerUpEventChannelSO;
        [SerializeField] private VoidEventChannelSO _applyLastPowerUpSO;
        [SerializeField] private VoidEventChannelSO _asyncLoadCompletedSO;

        private void Awake()
        {
            UpdatePowerUpDisplay();
            _initialPanelColor = _panel.color;
            Debug.Log("Is UI Power Up Panel Awaking");
        }

        // Call this method to update the UI when a power-up is collected or used
        private void UpdatePowerUpDisplay()
        {
            HideAllSlots();
            // Calculate the number of power-ups to display (up to 4)
            int countToDisplay = _wormCollectionsDataSO.PowerUpSOs.Count;
            //Mathf.Min(powerUpSlots.Count, _wormCollectionsDataSO.PowerUpSOs.Count);
            // Show power-up icons in the active slots, with the last added as the first
            for (int i = 0; i < countToDisplay; i++)
            {
                //int powerUpIndex = _wormCollectionsDataSO.PowerUpSOs.Count - 1 - i;
                powerUpSlots[i].gameObject.SetActive(true);
                powerUpSlots[i].sprite = _wormCollectionsDataSO.PowerUpSOs[i].Icon;
                powerUpSlots[i].color = _wormCollectionsDataSO.PowerUpSOs[i].Color;
            }
        }

        private void HideAllSlots()
        {
            // Hide all slots initially
            foreach (var slot in powerUpSlots)
            {
                slot.gameObject.SetActive(false);
            }
        }

        private void AddPowerUp(PowerUpSO powerUpSO)
        {
            _gotPickedSO.RaiseEvent();

            List<PowerUpSO> originalList = _wormCollectionsDataSO.PowerUpSOs;
            
            originalList.Insert(0, powerUpSO);

            // If the list exceeds the limit of 4, remove the first one added
            if (originalList.Count > 4)
            {
                originalList.RemoveAt(originalList.Count - 1);
            }     

            UpdatePowerUpDisplay();
        }

        private void RemoveLastPowerUp()
        {
            // Check if there are any power-ups to remove
            if (_wormCollectionsDataSO.PowerUpSOs.Count > 0)
            {
                ShowFlash();
                _playPoweredUpSoundSO.RaiseEvent();

                //_wormCollectionsDataSO.PowerUpSOs[_wormCollectionsDataSO.PowerUpSOs.Count - 1].Apply();
                _wormCollectionsDataSO.PowerUpSOs[0].Apply();

                ShowSlotCloneVisual();

                _wormCollectionsDataSO.PowerUpSOs.RemoveAt(0);
                // Remove the last power-up added from the list
                //_wormCollectionsDataSO.PowerUpSOs.RemoveAt(_wormCollectionsDataSO.PowerUpSOs.Count - 1);

                // Update the UI display
                UpdatePowerUpDisplay();

                AnimatePowerUpPanel();
            }
        }

        private void ShowFlash()
        {
            DOTween.Sequence().Append(_flashImage.DOFade(1, 0.1f)).
                Append(_flashImage.DOFade(0, 0.1f)).SetUpdate(true);
        }
        private void ShowSlotCloneVisual()
        {
            Vector2 slotPos = powerUpSlots[0].transform.position;
            GameObject slotClone = Instantiate(_slotPrefab, slotPos, Quaternion.identity, _panelRectTransform);
            slotClone.GetComponent<Image>().sprite = _wormCollectionsDataSO.PowerUpSOs[_wormCollectionsDataSO.PowerUpSOs.Count - 1].Icon;
            Destroy(slotClone, 0.5f);
            RectTransform myImage = slotClone.GetComponent<RectTransform>();

            // Animate the slot clone
            DOTween.Sequence().Append(myImage.DOAnchorPosY(myImage.anchoredPosition.y + 50, 0.75f)).
                Join(slotClone.GetComponent<Image>().DOFade(0, 0.5f));
        }

        private void AnimatePowerUpPanel()
        {
            DOTween.Sequence().Append(transform.DOScale(1.1f, 0.3f))
                    .Join(_panel.DOColor(Color.white, 0.3f))
                    .Append(transform.DOScale(1f, 0.3f))
                    .Join(_panel.DOColor(_initialPanelColor, 0.3f)).SetEase(_ease);
        }
        protected void OnPickedUpPowerUp()
        {
            if (_gotPickedSO != null)
                _gotPickedSO.RaiseEvent();
        }

        private void ClearPowerUpsCollection()
        {
            _wormCollectionsDataSO.ClearPowerUpsSOsCollection();
            HideAllSlots();
        }

        private void OnEnable()
        {
            _powerUpEventChannelSO.OnEventRaised += AddPowerUp;
            _applyLastPowerUpSO.OnEventRaised += RemoveLastPowerUp;
            _asyncLoadCompletedSO.OnEventRaised += ClearPowerUpsCollection;
        }

        private void OnDisable()
        {
            _powerUpEventChannelSO.OnEventRaised -= AddPowerUp;
            _applyLastPowerUpSO.OnEventRaised -= RemoveLastPowerUp;
            _asyncLoadCompletedSO.OnEventRaised -= ClearPowerUpsCollection;
        }
    }
}
