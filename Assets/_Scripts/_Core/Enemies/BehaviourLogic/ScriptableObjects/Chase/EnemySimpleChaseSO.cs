using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.Core.Utils;

namespace WormGame.Core.Enemies
{
    [CreateAssetMenu(fileName = "Simple Chase Enemy Behaviour", menuName = "Enemy Behaviour/Chase Logic/Simple Chase")]
    public class EnemySimpleChaseSO : EnemyChaseSOBase
    {
        public override EnemyState RunCurrentState()
        {
            return base.RunCurrentState();
        }
    }
}