using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WormGame.Core.Enemies
{
    public abstract class EnemyState : MonoBehaviour
    {
        public abstract EnemyState RunCurrentState();
    }
}