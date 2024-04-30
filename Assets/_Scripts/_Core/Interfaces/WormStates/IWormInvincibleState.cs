using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.Core;

namespace WormGame.Core
{
    public class IWormInvincibleState : IWormState
    {
        public void TakeDamage(WormSegmentPlayableAbstract wormPlayableSegment, DamageInfo damageInfo)
        {
            // Worm in the Invincible state does not take damage
        }
    }

}
