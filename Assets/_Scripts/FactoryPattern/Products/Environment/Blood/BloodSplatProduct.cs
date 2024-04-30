using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using WormGame.Variables;
using WormGame.Core;

namespace WormGame.FactoryPattern
{
    public class BloodSplatProduct : MonoBehaviour, IProduct
    {
        [SerializeField] private string _productName = "Blood Splat Product";
        [SerializeField] private StringVariableSO _bloodPoolTag;
        private float _fadeOutDuration = 10;
        private TweenCallback _returnToPoolCallback;
        [SerializeField] private Color _defaultColor;

        public string ProductName { get => _productName; set => _productName = value; }
        public GameObject GameObject => gameObject;

        private void Start()
        {
            _returnToPoolCallback = ReturnToPool;
        }

        public void Initialize()
        {
            Quaternion rotation = GetRandomRotation();
            Vector2 scale = GetRandomScale();

            transform.rotation = rotation;
            transform.localScale = scale;

            FadeOut(_fadeOutDuration);
        }

        private void FadeOut(float duration)
        {
            SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
            DOTween.Sequence().
                Append(sr.DOFade(1, duration)).
                Append(sr.DOFade(0, duration).
                OnComplete(_returnToPoolCallback)); 
        }

        private Quaternion GetRandomRotation()
        {
            float zRotation = Random.Range(0f, 360f);
            Quaternion randomRotation = Quaternion.Euler(0f, 0f, zRotation);

            return randomRotation;
        }

        private Vector2 GetRandomScale()
        {
            float bloodSplatScale = Random.Range(0.3f, 0.6f);
            Vector2 randomScale = new Vector2(bloodSplatScale, bloodSplatScale);

            return randomScale;
        }

        private void ReturnToPool()
        {
            ObjectPooler.Instance.ReturnObject(gameObject, _bloodPoolTag.Value);
        }

        private void ResetColor()
        {
            GetComponentInChildren<SpriteRenderer>().color = _defaultColor;
        }

        private void OnEnable()
        {
            ResetColor();
        }
    }
}