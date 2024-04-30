#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.EditorTools;
using WormGame.Core;
using WormGame.Core.Utils;
using WormGame.FactoryPattern;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System.Linq;

namespace WormGame.EditorTools
{
    [EditorTool("[ Worm Chain Generator Tool ]")]
    public class WormChainGeneratorTool : EditorTool
    {
        private string m_CurrentScene;
        private Factory m_WormConnectableFactory;
        private Factory m_WormDisconnectedFactory;
        private int m_ChainLength;
        private GUILayoutOption[] m_GuiLayoutOption = new GUILayoutOption[] { GUILayout.Width(250) };
        private GameObject[] _prefabs;
        private GameObject _prefabPreview;

        // We are going to use this list so that the placing of chains is not allowed when the
        // mouse is hovering over GUILayout elements
        private List<Rect> _buttonRects = new List<Rect>();
        
        private WormDataSO _wormDataSO;
        private CompositeCollider2D _spawnableArea;

        // Undo functionality for the chain generation
        private Stack<WormCommand> _commandStack = new Stack<WormCommand>();
        List<WormChainEntry> entries = new List<WormChainEntry>();

        public override void OnActivated()
        {
            entries = new List<WormChainEntry>();
            m_CurrentScene = SceneManager.GetActiveScene().name;

            EditorUtils.CreatePoolsIfNeeded();

            _buttonRects.Clear();

            // Find scriptable object of type WormDataSO in the Assets/ScriptableObjects folder
            string[] guids = 
                AssetDatabase.FindAssets("t:WormDataSO", new[] { "Assets/Resources" });
            // Load the first one using its GUID
            _wormDataSO = 
                AssetDatabase.LoadAssetAtPath<WormDataSO>(AssetDatabase.GUIDToAssetPath(guids[0]));

            // Find a game object with the tag "SpawnArea"
            GameObject spawnArea = GameObject.FindWithTag("SpawnArea");
            if (spawnArea != null)
            {
                // Get the composite collider 2D component from the game object
                _spawnableArea = spawnArea.GetComponent<CompositeCollider2D>();
            }

            // Get reference to the segment factories residing in scene
            GameObject wormConnectableFactoryGameObj = GameObject.FindWithTag("WormConnectableFactory");
            if (wormConnectableFactoryGameObj != null)
                m_WormConnectableFactory = wormConnectableFactoryGameObj.GetComponent<WormConnectableSegmentConcreteFactory>();

            GameObject wormDisconnectedFactoryGameObj = GameObject.FindWithTag("WormDisconnectedFactory");
            if (wormDisconnectedFactoryGameObj != null)
            {
                m_WormDisconnectedFactory =
                    GameObject.FindWithTag("WormDisconnectedFactory").GetComponent<WormDisconnectedSegmentConcreteFactory>();
            }



            for (int i = 0; i < 4; i++)
            {
                _buttonRects.Add(new Rect());
            }

            string[] guids1 = AssetDatabase.FindAssets("WormConnectableSegment t:prefab", new string[] { "Assets/Prefabs/WormSegments" });
            IEnumerable<string> paths = guids1.Select(AssetDatabase.GUIDToAssetPath);
            _prefabs = paths.Select(AssetDatabase.LoadAssetAtPath<GameObject>).ToArray();

            for (int i = 0; i < _prefabs.Length; i++)
            {
                // Spawn prefab
                _prefabPreview = Instantiate(_prefabs[i]);

                // Detect collisions with other placed objects during edit mode
                _prefabPreview.AddComponent<CollisionChecker2D>();

                // Change colliders to trigger
                Collider2D[] cols = _prefabPreview.GetComponentsInChildren<Collider2D>();

                foreach (Collider2D col in cols)
                {
                    col.isTrigger = true;
                }

                _prefabPreview.SetActive(true);
            }

            SceneView.duringSceneGui += DuringSceneViewGUI;
        }

        public override void OnWillBeDeactivated()
        {
            DestroyImmediate(_prefabPreview);
            SceneView.duringSceneGui -= DuringSceneViewGUI;
        }

        // Scene view code goes here
        void DuringSceneViewGUI(SceneView sceneView)
        {
            // Draw scene view overlay only if this is the active tool
            if (ToolManager.IsActiveTool(this))
            {
                DrawSceneViewOverlay();

                SceneInputAndDrawing(sceneView);
            }
        }

        void DrawSceneViewOverlay()
        {
            Handles.BeginGUI();

            DrawToolPanel();

            Handles.EndGUI();
        }

        void DrawToolPanel()
        {
            Texture2D tex = new Texture2D(1, 1);

            // Create the color that you want to change to
            Color grey = Color.grey;
            Color g = new Color(grey.r, grey.g, grey.b, 1f);

            EditorUtils.SetTexturePixelsToColor(tex, g);

            // GUIStyle for the label
            GUIStyle guiStyle = new GUIStyle(GUI.skin.label);
            guiStyle.alignment = TextAnchor.MiddleCenter;
            guiStyle.fixedHeight = 30;
            guiStyle.fixedWidth = 250;
            guiStyle.fontStyle = FontStyle.Bold;
            guiStyle.wordWrap = true;
            guiStyle.fontSize = 16;

            guiStyle.richText = true;

            guiStyle.normal.background = tex;

            GUILayout.BeginVertical();

            GUILayout.Label("Worm Chain Generator v. 0.1", guiStyle);
            _buttonRects[0] = GUILayoutUtility.GetLastRect();

            m_ChainLength = Mathf.RoundToInt(GUILayout.HorizontalSlider(m_ChainLength, 1, 12, m_GuiLayoutOption));

            GUILayout.Space(15);
            
            // Create the color that you want to change to
            Color red = Color.red;
            Color redTransparent = new Color(red.r, red.g, red.b, 0.7f);

            EditorUtils.SetTexturePixelsToColor(tex, redTransparent);

            guiStyle.fixedWidth = 150;
            guiStyle.fontStyle = FontStyle.Normal;
            guiStyle.fontSize = 10;
            guiStyle.normal.background = tex;

            GUILayout.Label("Worm chain length: " + m_ChainLength.ToString(), guiStyle);
            _buttonRects[1] = GUILayoutUtility.GetLastRect();

            // Draw outcome label
            guiStyle.fixedHeight = 120;
            guiStyle.fontSize = 12;
            guiStyle.fontStyle = FontStyle.Italic;
            guiStyle.wordWrap = true;

            g.a = 0.8f;
            EditorUtils.SetTexturePixelsToColor(tex, g);

            guiStyle.normal.background = tex;

            GUILayout.Label("HOW TO: Using the slider, select the preferred length " +
                "of worm chain to place and left click anywhere in scene " +
                " / " +
                "Undo last action by pressing Ctrl + Z", guiStyle);

            _buttonRects[3] = GUILayoutUtility.GetLastRect();
            guiStyle = new GUIStyle(GUI.skin.button);
            guiStyle.fixedHeight = 30;
            guiStyle.fixedWidth = 150;
            guiStyle.fontSize = 10;
            guiStyle.wordWrap= true;

            if (GUILayout.Button("Delete Pools", guiStyle))
            {
                GameObject.Find("Manager_ObjectPooling").GetComponent<ObjectPooler>().ClearPools();
            }
            if (GUILayout.Button("Create Pools", guiStyle))
            {
                GameObject.Find("Manager_ObjectPooling").GetComponent<ObjectPooler>().CreatePools();
            }
            if (GUILayout.Button("Clear Chains Collections", guiStyle))
            {
                _wormDataSO.WormCollectionsDataSO.ClearWormChainsCollections();
                Debug.Log("Dictionary of worm chains in WormCollectionsDataSO was cleared.");
            }
            if (GUILayout.Button("Clear Saved Chains File", guiStyle))
            {
                entries.Clear();

                CreateFolderIfNeeded();

                string path = Path.Combine(Application.streamingAssetsPath, "Saves", "WormChainsSaves", m_CurrentScene + "_ChainsArrangement.txt");
                using (StreamWriter file = File.CreateText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, entries);
                }
                Debug.Log("File with saved worm chains positions, in path " + path + ", was cleared.");
            }
            if (GUILayout.Button("Spawn saved level objects", guiStyle))
            {
                //ObjectPooler objPool = GameObject.Find("Manager_ObjectPooling").GetComponent<ObjectPooler>();
                LevelEditorUtils.SpawnSavedLevelGameObjects();
                LevelEditorUtils.GenerateSavedChains();
            }

            GUILayout.EndVertical();
        }

        void SceneInputAndDrawing(SceneView sceneView)
        {
            if (Event.current.type == EventType.MouseMove)
            {
                sceneView.Repaint();
            }

            bool isOverGui = false;

            for (int i = 0; i < _buttonRects.Count; i++)
            {
                if (_buttonRects[i].Contains(Event.current.mousePosition))
                {
                    isOverGui = true;
                    break;
                }
            }

            Vector3 mouseWorldPos = EditorUtils.GetMouseWorldPos();

            Vector3 tileCenteredPosition = new Vector3(
                Mathf.Round(mouseWorldPos.x),
                Mathf.Round(mouseWorldPos.y),
                mouseWorldPos.z // Assuming you're working in 2D
            );

            _prefabPreview.transform.position = tileCenteredPosition;


            if (GUIUtility.hotControl != 0 || isOverGui)
            {
                // Do something if the mouse is over the GUILayout
                //Debug.Log("Cannot place gameobject");
            }
            else if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                if (!_prefabPreview.GetComponent<CollisionChecker2D>().CanSpawn)
                    return;

                // Spawn chain
                GenerateChainOfLengthAtPosition(m_ChainLength);
            }

            // Check if the event type is key down
            if (Event.current.type == EventType.KeyDown)
            {
                // Check if the modifier keys include Ctrl
                if ((Event.current.modifiers & EventModifiers.Control) != 0)
                {
                    // Check if the key code is Z
                    if (Event.current.keyCode == KeyCode.Z)
                    {
                        // Do something when Ctrl + Z is pressed
                        //Debug.Log("Ctrl + Z pressed");
                        UndoGenerateChainOfLengthAtPosition();
                    }
                }
            }
        }

        private void GenerateChainOfLengthAtPosition(int chainLength)
        {
            Vector2 initialSpawnPosition = EditorUtils.GetMouseWorldPos();

            List<GameObject> disconnectedChain = WormUtils.CreateDisconnectedChain();
            GameObject connectableSegment = WormUtils.CreateSegmentAtPosition(m_WormConnectableFactory, initialSpawnPosition);

            // Create a new command and push it to the stack
            WormCommand command = new WormCommand(connectableSegment, disconnectedChain);
            _commandStack.Push(command);

            // Register the undo operation with the Undo class
            Undo.RegisterCompleteObjectUndo(_wormDataSO, "Added chain to collection");

            WormUtils.AddNewEntryToDictionary(connectableSegment, disconnectedChain, _wormDataSO);
            WormUtils.CompleteDetachedChainGeneration(m_WormDisconnectedFactory, chainLength, disconnectedChain, _wormDataSO);
            List<Point> points = new List<Point>();
            points.Add(new Point(connectableSegment.transform.position.x, connectableSegment.transform.position.y));
            for (int i = 1; i < disconnectedChain.Count; i++)
            {
                points.Add(new Point(disconnectedChain[i].transform.position.x, disconnectedChain[i].transform.position.y));
            }
            WormChainEntry wormChainEntry = new WormChainEntry(chainLength, points);
            entries.Add(wormChainEntry);
            CreateFolderIfNeeded();
            //serialize JSON directly to a file
            string path = Path.Combine(Application.streamingAssetsPath, "Saves", "WormChainsSaves", m_CurrentScene + "_ChainsArrangement.txt");
            using (StreamWriter file = File.CreateText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, entries);
            }
        }

        private void UndoGenerateChainOfLengthAtPosition()
        {
            if (_commandStack.Count > 0)
            {
                // Pop the last command from the stack
                WormCommand command = _commandStack.Pop();

                ObjectPooler.Instance.ReturnObject(command.disconnectedChain[0], "WormConnectableSegment");
                for (int i = 1; i < command.disconnectedChain.Count; i++)
                {
                    ObjectPooler.Instance.ReturnObject(command.disconnectedChain[i], "WormDisconnectedSegment");
                }

                // Remove the dictionary entry
                _wormDataSO.
                    WormCollectionsDataSO.
                    WormDisconnectedChainsOfSegments.Remove(command.connectableSegment);
               
                entries.RemoveAt(entries.Count - 1);

                string path = Path.Combine(Application.streamingAssetsPath, "Saves", "WormChainsSaves", m_CurrentScene + "_ChainsArrangement.txt");
                //serialize JSON directly to a file
                using (StreamWriter file = File.CreateText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, entries);
                }
            }
        }

        private void CreateFolderIfNeeded()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "Saves", "WormChainsSaves");
            bool folderExists = Directory.Exists(path); // Check if the folder exists
            if (!folderExists)
            {
                Directory.CreateDirectory(path);
                Debug.Log("Created folder " + path);
            }
        }
    }
}
#endif