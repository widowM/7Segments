using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using DG.Tweening;
using WormGame.Core;
using WormGame.EventChannels;

namespace WormGame.Core.Enemies
{
    public class EnemyLaserLineGenerator : MonoBehaviour
    {
        [SerializeField] private WormDataSO m_wormDataSO;
        [SerializeField] private Vector2EventChannelSO m_enemyReadyToAttack;
        private Color _initialColor;
        private Color _endColor;
        
        private void DrawEnemyLaserLine(Vector2 startPoint)
        {
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.startColor = _initialColor;
            lineRenderer.endColor = _initialColor;
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, m_wormDataSO.Head.transform.position);
            lineRenderer.DOColor(new Color2(_initialColor, _initialColor), new Color2(_endColor, _endColor), 3);
        }

        private void OnEnable()
        {
            _initialColor = Color.red;
            _endColor = new Color(_initialColor.r, _initialColor.g, _initialColor.b, 0);
            m_enemyReadyToAttack.OnEventRaised += DrawEnemyLaserLine;
        }

        private void OnDisable()
        {
            m_enemyReadyToAttack.OnEventRaised -= DrawEnemyLaserLine;
        }
    }
}
