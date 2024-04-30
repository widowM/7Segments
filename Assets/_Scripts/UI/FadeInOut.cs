using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WormGame.Core;
using WormGame.EventChannels;

namespace WormGame
{
    public class FadeInOut : MonoBehaviour
    {
        private Image rend;
        [SerializeField] private GameObject fadeObject;
        private float aspect = 1;
        private float offsetX = 0.5f;
        private float offsetY = 0.5f;
        private float tilingX = 1;
        private float tilingY = 1;
        private float hypotenuse = 1;

        private float fadeAmount = 1;

        private float scrollScale = 0.02f;
        private Vector3 targetPosition = new Vector3(0.5f, 0.5f);
        [SerializeField] private WormDataSO _wormDataSO;
        private float changeSpeed = 0.3f;
        private bool _isTracking = true;
        private WaitForSeconds sixSeconds = new WaitForSeconds(6f);
        [Header("Listen to Event Channels")]
        [SerializeField] private VoidEventChannelSO _worldGeneratedSO;
        [Header("Broadcast on EventChannels")]
        [SerializeField] private VoidEventChannelSO _selectedNextSceneSO;
        [SerializeField] private VoidEventChannelSO _playWinSoundSO;
        private Vector2 _centerScreenPos;

        // Start is called before the first frame update
        void Start()
        {
            rend = GetComponent<Image>();
            _centerScreenPos = new Vector2(Screen.width / 2f, Screen.height / 2f);
        }

        private void DoFadeOut()
        {
            //Debug.Log("Fade out started SO");
            _isTracking = true;
            StartCoroutine(FadeOutCoroutine());
        }

        public void DoFadeIn()
        {
            _isTracking = true;
            fadeAmount = 0;
            //Debug.Log("Fade in started SO");
            StartCoroutine(FadeInCoroutine());
        }
        private void OnEnable()
        {
            _worldGeneratedSO.OnEventRaised += DoFadeOut;
        }

        private void OnDisable()
        {
            _worldGeneratedSO.OnEventRaised -= DoFadeOut;
        }
        IEnumerator FadeOutCoroutine()
        {
            //Debug.Log("Started fade out coroutine");
            while (_isTracking)
            {
                // Increment fadeAmount towards 1 by changeSpeed every second
                fadeAmount -= changeSpeed * Time.deltaTime;
                fadeAmount = Mathf.Clamp(fadeAmount, -0.1f, 1.1f);

                TrackObject();

                // Check if fadeAmount reaches 0
                if (fadeAmount <= 0f)
                {
                    StopTracking();
                }

                yield return null;
            }
        }

        IEnumerator FadeInCoroutine()
        {
            _playWinSoundSO.RaiseEvent();
            yield return sixSeconds;
            while (_isTracking)
            {
                // Increment fadeAmount towards 1 by changeSpeed every second
                fadeAmount += changeSpeed * Time.deltaTime;
                fadeAmount = Mathf.Clamp(fadeAmount, -0.1f, 1.1f);

                TrackObject();

                // Check if fadeAmount reaches 0
                if (fadeAmount >= 1f)
                {
                    StopTracking();
                    _selectedNextSceneSO.RaiseEvent();
                }

                yield return null;
            }
        }

        private void StopTracking()
        {
            _isTracking = false;
            // Reset values for next fadeout
            fadeAmount = 1;
            
            //Debug.Log("Tracking stopped.");
        }
        void TrackObject()
        {
            Aspect();
            NormalizePosition();
            Hypotenuse();
            SetMaterialProperties();
        }

        void NormalizePosition()
        {
            Vector2 screenPos = _centerScreenPos;
            offsetX = screenPos.x / Screen.width;
            offsetY = screenPos.y / Screen.height;
            if (Screen.width > Screen.height)
            {
                offsetX *= aspect;
            }
            else
            {
                offsetY *= aspect;
            }
        }

        void Aspect()
        {
            if (Screen.width > Screen.height)
            {
                aspect = (float)Screen.width / Screen.height;
                tilingX = aspect;
                tilingY = 1;
            }
            else
            {
                aspect = (float)Screen.height / Screen.width;
                tilingY = aspect;
                tilingX = 1;
            }
        }

        void Hypotenuse()
        {
            float x = offsetX;
            float y = offsetY;

            float midX = 0.5f;
            float midY = 0.5f;

            if (Screen.width > Screen.height)
                midX *= aspect;
            else
                midY *= aspect;

            // determines which quadrant of the screen the point is in
            if (x < midX)
                x = midX * 2 - x;
            if (y < midY)
                y = midY * 2 - y;

            hypotenuse = Mathf.Sqrt(x * x + y * y);
        }

        void SetMaterialProperties()
        {
            rend.material.SetVector("_Offset", new Vector4(offsetX, offsetY, 0, 0));
            rend.material.SetVector("_Tiling", new Vector4(tilingX, tilingY, 0, 0));
            rend.material.SetFloat("_Hypotenuse", hypotenuse);
            rend.material.SetFloat("_FadeAmount", fadeAmount);
        }
    }
}
