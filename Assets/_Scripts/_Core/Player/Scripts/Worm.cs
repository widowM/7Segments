using UnityEngine;
using WormGame.Core.Containers;

namespace WormGame.Core.Player
{
    /// <summary>
    /// This class is responsible for managing the components and data of the worm.
    /// It has references to several scriptable objects and scripts that provide the functionality and behavior of the worm.
    /// It has the following fields and properties: - WormBuilder: a script that builds the initial worm’s structure and appearance
    /// using the Worm stores the currently connected segment in the collection.
    /// - WormInput: a script that handles the user input - WormMovement: a script that implements the logic for the worm’s movement
    /// and rotation using the WormInput and the Worm. /- WormInputActions: a scriptable object that defines the input actions for the worm.
    /// WormPhysics: a script that handles the collisions of the worm.
    /// 
    /// The Worm class uses the Awake method to store a reference to this game object in the Worm scriptable object,
    /// which allows easy access to the worm without using GameObject.Find().
    /// // It also disables the runtime UI of the DebugManager class.
    /// </summary>
    /// 
    public class Worm : MonoBehaviour
    {
        // Fields
        [SerializeField] private WormDataSO _wormDataSO;
        [SerializeField] private WormCollectionsDataSO _wormCollectionsDataSO;
        [SerializeField] private GameplayDataSO _wormGameplayDataSO;
        [SerializeField] private WormStructureDataSO _wormStructureDataSO;
        [SerializeField] private WormPhysicsDataSO _wormPhysicsDataSO;
        [SerializeField] private ObjectPoolTagsContainerSO _objectPoolTagsContainerSO;
        [SerializeField] private WormBuilder _wormBuilder;
        [SerializeField] private WormInput _wormInput;
        [SerializeField] private WormMovement m_WormMovement;
        [SerializeField] private WormInputActions _wormInputActions;
        [SerializeField] private WormPhysics _wormPhysics;

        // Properties
        public WormDataSO WormDataSO => _wormDataSO;
        public WormCollectionsDataSO WormCollectionsDataSO => _wormCollectionsDataSO;
        public GameplayDataSO WormGameplayDataSO => _wormGameplayDataSO;
        public WormStructureDataSO WormStructureDataSO => _wormStructureDataSO;
        public WormPhysicsDataSO WormPhysicsDataSO => _wormPhysicsDataSO;
        public ObjectPoolTagsContainerSO ObjectPoolTagsContainerSO => _objectPoolTagsContainerSO;
        public WormBuilder WormBuilder => _wormBuilder;
        public WormInput WormInput => _wormInput;
        public WormMovement WormMovement => m_WormMovement;
        public WormInputActions WormInputActions => _wormInputActions;
        public WormPhysics WormPhysics => _wormPhysics;
        //public WormMechanicHelper WormMechanicHelper => _wormMechanicHelper;

        // Methods
        private void Awake()
        {
            // Store a reference to the player game object that resides in the scene
            // to the Worm Data scriptable object
            // This allows easy access to the Player game object without using GameObject.Find()

            _wormDataSO.ResetWormReferences();
            _wormDataSO.Player = gameObject;
            ////
            UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;
        }
    }
}