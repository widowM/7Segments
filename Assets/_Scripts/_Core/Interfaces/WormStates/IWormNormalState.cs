using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.Core;

namespace WormGame.Core
{
    public class IWormNormalState : IWormState
    {
        public void TakeDamage(WormSegmentPlayableAbstract wormPlayableSegment, DamageInfo damageInfo)
        {
            // During Normal state worm takes damage
            wormPlayableSegment.ProcessDamage(damageInfo);
        }
    }
}

