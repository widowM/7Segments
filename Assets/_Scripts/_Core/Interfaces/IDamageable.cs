using UnityEngine;
using WormGame.Environment;

namespace WormGame.Core
{
    /// <summary>
    /// This interface defines a contract for damageable objects in the game.
    /// </summary>
    /// <remarks>
    /// This interface declares a method called TakeDamage(Vector hitPosition)
    /// that should be implemented by any class that can take damage from other sources.
    /// The method takes a vector parameter that represents the position or the velocity
    /// of the impact. The implementation of the method should handle the logic of reducing the health,
    /// applying visual  effects, and destroying the object if necessary.
    /// <see cref="Enemy"/> <see cref="Wall"/> <see cref="WormSegmentPlayableAbstract"/> 
    /// are examples of classes that implement this interface.
    /// </remarks>
    /// 
    public interface IDamageable
    {
        void TakeDamage(DamageInfo damageInfo);
    }

    public struct DamageInfo
    {
        public Vector2 hitPosition;
        public Vector2 projectileVelocity;

        public DamageInfo(Vector2 hitPos,  Vector2 projectileVel)
        {
            hitPosition = hitPos;
            projectileVelocity = projectileVel;
        }
    }
}