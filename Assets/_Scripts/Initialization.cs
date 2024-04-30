using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.Core;

namespace WormGame
{
    public class Initialization : MonoBehaviour
    {
        [SerializeField] private WormDataSO _wormDataSO;

        private void Awake()
        {
            _wormDataSO.InitializeCooldowns();
        }
    }
}
