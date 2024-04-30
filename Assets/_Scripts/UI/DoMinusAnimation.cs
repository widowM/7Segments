using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using WormGame.EventChannels;
namespace WormGame
{
    public class DoMinusAnimation : MonoBehaviour
    {
        [SerializeField] private RectTransform _myTransform;
        private Vector2 _initialPosition;
        private Color _initialColor;
        [Header("Listen to Event Channels")]
        [SerializeField] private VoidEventChannelSO _executingLoseOnePartSO;

        // Start is called before the first frame update
        void Start()
        {
            _initialPosition = _myTransform.anchoredPosition;
            _initialColor = GetComponent<Image>().color;

        }

        private void Initialize()
        {
            _myTransform.anchoredPosition = _initialPosition;
            GetComponent<Image>().color = _initialColor;
        }

        private void DoMinusAnimationSequence()
        {
            Initialize();

            DOTween.Sequence().Append(_myTransform.DOAnchorPosY(_myTransform.anchoredPosition.y - 25, 1f)).
                Join(_myTransform.GetComponent<Image>().DOFade(0, 1f)).SetEase(Ease.InSine).SetUpdate(true);
            //Append(_myTransform.DOAnchorPosY(_initialPosition.y, 0.001f)).
            //    Join(_myTransform.GetComponent<Image>().DOFade(_initialColor.a, 0.0001f))
        }

        private void OnEnable()
        {
            _executingLoseOnePartSO.OnEventRaised += DoMinusAnimationSequence;
        }

        private void OnDisable()
        {
            _executingLoseOnePartSO.OnEventRaised += DoMinusAnimationSequence;
        }
    }
}
