using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.Core;

namespace WormGame.Core
{
    public interface IWormState
    {
        void TakeDamage(WormSegmentPlayableAbstract wormPlayableSegment, DamageInfo damageInfo);
    }
}