using System.Collections;
using UnityEngine;
using WormGame.Core;
using WormGame.Core.Containers;

namespace WormGame.FactoryPattern
{
    /// <summary>
    /// This class is a subclass of IProduct that represents the worm stare line that is spawned
    /// when the worm head looks at a body part. It uses the worm data and the line renderer to access
    /// and modify the color and position of the line. It also uses a coroutine to fade out the line
    /// over time and return it to the object pooler.
    /// </summary>
    public class WormStareLineProduct : MonoBehaviour, IProduct
    {
        [SerializeField] private string _productName = "Worm Stare Line Product";
        [SerializeField] private ObjectPoolTagsContainerSO _objectPoolTagsContainerSO;
        [SerializeField] private LineRenderer _lineRend;

        public string ProductName { get => _productName; set => _productName = value; }
        public GameObject GameObject => gameObject;

        public void Initialize()
        {
            _lineRend.colorGradient.SetKeys
            (
                _lineRend.colorGradient.colorKeys,
                new GradientAlphaKey[] { new GradientAlphaKey(1f, 1f) }
            );

            StartCoroutine(FadeOutLineCoroutine());
        }

        private IEnumerator FadeOutLineCoroutine()
        {
            Gradient lineRendererGradient = new Gradient();

            float fadeSpeed = 3f;
            float timeElapsed = 0f;
            float alpha;

            while (timeElapsed < fadeSpeed)
            {
                alpha = Mathf.Lerp(1f, 0f, timeElapsed / fadeSpeed);
                SetAlphaValues(lineRendererGradient, alpha);
                _lineRend.colorGradient = lineRendererGradient;

                timeElapsed += Time.deltaTime;

                yield return null;
            }

            ObjectPooler.Instance.ReturnObject(gameObject, _objectPoolTagsContainerSO.WormStareLineTag);

            // Reset alpha values -> alpha = 1
            SetAlphaValues(lineRendererGradient);
        }

        private void SetAlphaValues(Gradient lineRendererGradient, float alpha = 1)
        {
            lineRendererGradient.SetKeys
                (
                    _lineRend.colorGradient.colorKeys,
                    new GradientAlphaKey[] { new GradientAlphaKey(alpha, 1f) }
                );
        }
    }
}