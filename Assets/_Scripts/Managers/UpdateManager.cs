using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.Core;

namespace WormGame.Managers
{
    /// <summary>
    /// Singleton class that manages the update logic.
    /// </summary>
    public class UpdateManager : MonoBehaviour
    {
        // Use a static instance to access the manager from anywhere
        public static UpdateManager Instance { get; private set; }

        // Use a list to store the updatable objects
        private List<IUpdatable> _updatables;
        private List<IFixedUpdatable> _fixedUpdatables;

        // Initialize the instance and the list in Awake
        private void Awake()
        {
            Instance = this;
            _updatables = new List<IUpdatable>();
            _fixedUpdatables = new List<IFixedUpdatable>();
        }

        // Loop through the list and call the UpdateMe methods in Update
        private void Update()
        {
            for (int i = 0; i < _updatables.Count; i++)
            {
                _updatables[i].UpdateMe();
            }
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < _fixedUpdatables.Count; i++)
            {
                _fixedUpdatables[i].FixedUpdateMe();
            }
        }

        // Add an object to the list
        public void Register(IUpdatable updatable)
        {
            _updatables.Add(updatable);
        }

        // Remove an object from the list
        public void Unregister(IUpdatable updatable)
        {
            _updatables.Remove(updatable);
        }
        
        public void Register(IFixedUpdatable fixedUpdatable)
        {
            _fixedUpdatables.Add(fixedUpdatable);
        }

        public void Unregister(IFixedUpdatable fixedUpdatable)
        {
            _fixedUpdatables.Remove(fixedUpdatable);
        }
    }
}