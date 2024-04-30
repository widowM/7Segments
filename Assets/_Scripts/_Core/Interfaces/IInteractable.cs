using UnityEngine;

namespace WormGame.Core
{
    public interface IInteractable
    {
        /// <summary>
        /// This interface defines a contract for objects that can be interacted with by the worm's head.
        /// </summary>
        /// <remarks>
        /// This interface declares a method called Interact() that should be implemented by any class 
        /// that represents an interactable object in the game. The implementation of the method should handle
        /// the logic of the interaction, such as changing the object's state, triggering an event or applying
        /// effects to the worm.
        /// </remarks>
        /// 
        void Interact(in GameObject head);
    }
}