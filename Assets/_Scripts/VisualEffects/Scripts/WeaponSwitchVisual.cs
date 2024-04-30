using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WormGame.EventChannels;
using DG.Tweening;
using UnityEngine.InputSystem.Controls;

namespace WormGame
{
    public class WeaponSwitchVisual : MonoBehaviour
    {
        [SerializeField] private GameObject _weaponVisual;
        [SerializeField] private Sprite[] _weaponIcons;
        [SerializeField] private Ease _ease;
        [Header("Listen to Event Channels")]
        [SerializeField] private IntEventChannelSO _showWeaponVisualsSO;
        //[SerializeField] private VoidEventChannelSO _cinemachineCameraReadySO;


        //private void Initialize()
        //{
        //    Vector2 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f);

        //    _weaponVisual.transform.position = Camera.main.ScreenToWorldPoint(screenCenter);
        //}

        private void ShowWeaponSwitchVisual(int weaponId)
        {
            Image weaponVisualImage = _weaponVisual.GetComponent<Image>();

            switch (weaponId)
            {
                case 0:
                weaponVisualImage.sprite = _weaponIcons[0];
                DoFadeAndScaleTweenSequence(weaponVisualImage);
                break;
                case 1:
                weaponVisualImage.sprite = _weaponIcons[1];
                DoFadeAndScaleTweenSequence(weaponVisualImage);
                break;
                case 2:
                weaponVisualImage.sprite = _weaponIcons[2];
                DoFadeAndScaleTweenSequence(weaponVisualImage);
                break;
                default:
                    Debug.Log("Wrong weapon id.");
                    break;
            }
        }

        //[ContextMenu("Do show example")]
        //public void ExampleWeaponVisualShow()
        //{
        //    ShowWeaponSwitchVisual(0);
        //}
        private void DoFadeAndScaleTweenSequence(Image weaponVisualImage)
        {
            DOTween.Sequence().Append(weaponVisualImage.DOFade(1, 0.001f)).
                    Append(_weaponVisual.transform.DOScale(new Vector3(1, 1, 1), 0.5f)).
                    Join(weaponVisualImage.DOFade(0, 0.5f)).SetEase(_ease).
                    Append(_weaponVisual.transform.DOScale(Vector3.zero, 0.001f)).SetUpdate(true);
        }

        private void OnEnable()
        {
            _showWeaponVisualsSO.OnEventRaised += ShowWeaponSwitchVisual;
            //_cinemachineCameraReadySO.OnEventRaised += Initialize;
        }

        private void OnDisable()
        {
            _showWeaponVisualsSO.OnEventRaised -= ShowWeaponSwitchVisual;
            //_cinemachineCameraReadySO.OnEventRaised -= Initialize;
        }
    }
}
