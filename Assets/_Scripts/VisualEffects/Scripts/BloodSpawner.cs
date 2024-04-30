using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.EventChannels;
using WormGame.FactoryPattern;

namespace WormGame.VisualEffects
{

    /// <summary>
    /// This class is responsible for spawning blood splats on the scene when an event is raised.
    /// It has a reference to a blood splat concrete factory that creates different types of blood
    /// splats based on the blood splat data scriptable object. It listens to an event channel that
    /// passes a vector2 value as a parameter, representing the position of the blood splat.
    /// It uses the SpawnBlood method to get a blood splat product from the factory and place it on the scene.
    /// It subscribes and unsubscribes to the event channel when the class is enabled and disabled.
    /// </summary>
    public class BloodSpawner : MonoBehaviour
    {
        [SerializeField] private BloodSplatConcreteFactory _bloodSplatFactory;

        [Header("Listen to Event Channels")]
        [SerializeField] private Vector2EventChannelSO _spawningBloodSO;

        private void SpawnBlood(Vector2 position)
        {
            Vector2[] positions = { position };
            _bloodSplatFactory.GetProduct<IProduct>(positions);
        }

        private void OnEnable()
        {
            _spawningBloodSO.OnEventRaised += SpawnBlood;
        }

        private void OnDisable()
        {
            _spawningBloodSO.OnEventRaised -= SpawnBlood;
        }
    }
}