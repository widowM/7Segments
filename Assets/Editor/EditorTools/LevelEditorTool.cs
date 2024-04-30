#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditor.EditorTools;
using WormGame.Core.Utils;
using WormGame.Core;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using WormGame.SceneManagement;

namespace WormGame.EditorTools
{
    // We need this type of command to implement Undo functionality for this tool
    public class LevelEditorCommand
    {
        public GameObject spawnedGO;
        public string poolTag;

        public LevelEditorCommand(GameObject spawnedGO, string poolTag)
        {
            this.spawnedGO = spawnedGO;
            this.poolTag = poolTag;
        }
    }

    /// <summary>
    /// This tool is responsible for placing assets in the map and creating map setups during edit mode.
    /// It can place anything (enemies, powerups, obstacles etc) in the map except for worm segments.
    /// The setups prepared during edit mode are saved in json format so they can be retrieved at
    /// runtime from the object pool.
    /// </summary>

    [EditorTool("[ Level Editor Tool ]")]
    public class LevelEditorTool : EditorTool
    {
        private string _currentScene;
        private Rect _panelRect;
        private GameObject[] _prefabs;
        private List<GameObject> _prefabPreviews = new List<GameObject>();

        // create a list to store the button rectangles
        private List<Rect> _buttonRects = new List<Rect>();

        private int _prefabPreviewIndex = 0;
        private System.Object _selectedPrefab;
        private Stack<LevelEditorCommand> m_CommandStack = new Stack<LevelEditorCommand>();

        // The list of placed objects to be stored in a save file
        private List<LevelEditorEntry> m_Entries = new List<LevelEditorEntry>();
        
        // Initialisation methods
        public override void OnActivated()
        {
            _panelRect = GetDefaultUIRect();
            SceneView.duringSceneGui += DuringSceneViewGUI;

            // We need the currently loaded scene name to store level setups for different scenes
            
            _currentScene = SceneManager.GetActiveScene().name;
            Debug.Log("Current scene = " + _currentScene);
            EditorUtils.CreatePoolsIfNeeded();
            CreateFolderIfNeeded();
            // Refreshing the collections and reinitialising them
            Debug.Log("Loading prefab previews...");
            ReloadPrefabs();
            
            // Spawning scene previews
            SpawnPrefabPreviews();
        }

        Rect GetDefaultUIRect()
        {
            // Set up where to draw the scene UI and its elements' default dimensions
            const int MARGIN = 4;
            Rect panelConfig = new Rect(MARGIN, MARGIN, 80, 80);

            return new Rect(MARGIN, panelConfig.yMax + MARGIN, panelConfig.width, panelConfig.height);
        }

        private void ReloadPrefabs()
        {
            _prefabPreviews.Clear();

            Debug.Log("*** Worm Level Editor Tool v. 0.001 ***");
            _selectedPrefab = null;

            // load _prefabs
            Debug.Log("Loading prefabs...");

            // Get all the _prefabs from Assets/Animations/Test folder
            string[] guids1 = AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/Prefabs/Enemies" });

            // Get a specific prefab from Assets/Models folder by its name
            string[] guids2 = AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/Prefabs/Powerups" });

            // Get another array of prefabs, for example, from Assets/Decorations folder
            string[] guids3 = AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/Prefabs/Weapons" });

            // Combine the three arrays into one
            string[] guids = guids1.Concat(guids2).Concat(guids3).ToArray();

            IEnumerable<string> paths = guids.Select(AssetDatabase.GUIDToAssetPath);
            _prefabs = paths.Select(AssetDatabase.LoadAssetAtPath<GameObject>).ToArray();
        }

        private void SpawnPrefabPreviews()
        {
            for (int i = 0; i < _prefabs.Length; i++)
            {
                // Spawn prefab
                GameObject spawnPrefabPreview = Instantiate(_prefabs[i]);

                // Detect collisions with other placed objects during edit mode
                spawnPrefabPreview.AddComponent<CollisionChecker2D>();

                // Populate the prefab preview list
                _prefabPreviews.Add(spawnPrefabPreview);

                // Add an empty rectangle to the list
                _buttonRects.Add(new Rect());

                // Change colliders to trigger
                Collider2D[] cols = spawnPrefabPreview.GetComponentsInChildren<Collider2D>();

                foreach (Collider2D col in cols)
                {
                    col.isTrigger = true;
                }

                spawnPrefabPreview.SetActive(false);
            }
        }

        // Deactivation methods

        public override void OnWillBeDeactivated()
        {
            SceneView.duringSceneGui -= DuringSceneViewGUI;
            
            DestroyPrefabPreviews();

            GameObject.Find("Manager_ObjectPooling").GetComponent<ObjectPooler>().ClearPools();


            Debug.Log("*** Level Editor disabled. ***");
        }

        private void DestroyPrefabPreviews()
        {
            foreach (GameObject prefabPreview in _prefabPreviews)
            {
                DestroyImmediate(prefabPreview);
            }
        }

        // Drawing in scene view methods

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

            DrawPrefabPickingPanel(_panelRect);
            DrawUtilityPanel(_panelRect);

            Handles.EndGUI();
        }

        void DrawPrefabPickingPanel(Rect rect)
        {
            GUILayout.BeginArea(rect);

            // Start a horizontal layout group
            GUILayout.BeginHorizontal();
            Texture2D tex = new Texture2D(1, 1);

            // Create the color that you want to change to
            Color grey = Color.grey;
            Color g = new Color(grey.r, grey.g, grey.b, 0.7f);

            EditorUtils.SetTexturePixelsToColor(tex, g);

            // GUIStyle for the label
            GUIStyle guiStyle = new GUIStyle(GUI.skin.label);
            guiStyle.fixedHeight = rect.height;
            guiStyle.fixedWidth = rect.width * 2f;
            guiStyle.fontStyle = FontStyle.Bold;
            guiStyle.wordWrap = true;
            guiStyle.fontSize = 20;
            guiStyle.alignment = TextAnchor.MiddleCenter;

            // Set label background color
            tex.Apply();
            guiStyle.normal.background = tex;

            GUILayout.Label("Level Editor Tool", guiStyle);

            // GUIStyle for the buttons
            guiStyle = new GUIStyle(GUI.skin.button);
            guiStyle.fixedHeight = rect.height;
            guiStyle.fixedWidth = rect.width;
            guiStyle.fontSize = 9;
            guiStyle.imagePosition = ImagePosition.ImageAbove;

            DrawPrefabPickingButtons(guiStyle);

            GUILayout.EndArea();
        }

        void DrawPrefabPickingButtons(GUIStyle guiStyle)
        {
            for (int i = 0; i < _prefabs.Length; i++)
            {
                GameObject prefab = _prefabs[i];

                prefab.SetActive(true);
                Texture icon = AssetPreview.GetAssetPreview(prefab);

                if (GUILayout.Button(new GUIContent(prefab.name, icon), guiStyle))
                {
                    if (_selectedPrefab != null)
                    {
                        _prefabPreviews[_prefabPreviewIndex].SetActive(false);
                    }

                    _selectedPrefab = prefab;
                    _prefabPreviewIndex = i;
                    _prefabPreviews[_prefabPreviewIndex].SetActive(true);

                    Debug.Log($"You selected the {prefab.name} prefab");
                }

                _buttonRects[i] = GUILayoutUtility.GetLastRect();
            }
        }

        // Draw utility buttons
        void DrawUtilityPanel(Rect rect)
        {
            GUILayout.BeginVertical();

            // GUIStyle for the label
            GUIStyle guiStyle = new GUIStyle(GUI.skin.button);
            guiStyle.fixedHeight = 30;
            guiStyle.fixedWidth = rect.width * 2;
            guiStyle.fontSize = 12;
            guiStyle.wordWrap = true;
            guiStyle.alignment = TextAnchor.MiddleCenter;

            if (GUILayout.Button("Delete Pools", guiStyle))
            {
                GameObject.Find("Manager_ObjectPooling").GetComponent<ObjectPooler>().ClearPools();
            }
            if (GUILayout.Button("Create Pools", guiStyle))
            {
                GameObject.Find("Manager_ObjectPooling").GetComponent<ObjectPooler>().CreatePools();
            }
            if (GUILayout.Button("Spawn saved level objects", guiStyle))
            {
                //ObjectPooler objPool = GameObject.Find("Manager_ObjectPooling").GetComponent<ObjectPooler>();
                LevelEditorUtils.SpawnSavedLevelGameObjects();
                LevelEditorUtils.GenerateSavedChains();
            }
            if (GUILayout.Button("Clear Saved Objects File", guiStyle))
            {
                m_Entries.Clear();

                CreateFolderIfNeeded();

                string path = Path.Combine(Application.streamingAssetsPath, "Saves", "LevelEditorSaves", _currentScene + "_SceneSetup.txt");
                using (StreamWriter file = File.CreateText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, m_Entries);
                }
            }

            GUILayout.EndVertical();

        }

        // Draw the last part of this tool, which is prefab preview in scene
        void SceneInputAndDrawing(SceneView sceneView)
        {
            if (Event.current.type == EventType.MouseMove)
            {
                sceneView.Repaint();
            }

            bool mouseOverButton = false;

            for (int i = 0; i < _buttonRects.Count; i++)
            {
                // check if the mouse position is inside the button rectangle
                if (_buttonRects[i].Contains(Event.current.mousePosition))
                {
                    mouseOverButton = true;
                    //Debug.Log("Mouse over GUI layout");
                    return;
                }

                mouseOverButton = false;
            }

            if (!mouseOverButton)
            {
                Vector3 mouseWorldPos = EditorUtils.GetMouseWorldPos();

                Vector3 tileCenteredPosition = new Vector3(
                    Mathf.Round(mouseWorldPos.x),
                    Mathf.Round(mouseWorldPos.y),
                    mouseWorldPos.z // Assuming you're working in 2D
                );

                if (_selectedPrefab == null)
                {
                    //Handles.DrawWireArc(mouseWorldPos, Vector3.forward, Vector3.right, 360, 0.5f);
                }
                else
                {
                    // Draw spawn preview
                    _prefabPreviews[_prefabPreviewIndex].transform.position = tileCenteredPosition;
                }

                // If left mouse button is pressed
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                {
                    if (_selectedPrefab == null)
                        return;
                    if (!_prefabPreviews[_prefabPreviewIndex].GetComponent<CollisionChecker2D>().CanSpawn)
                        return;

                    // Spawning the object in sceneview during edit mode
                    GameObject goPrefab = (GameObject)_selectedPrefab;
                    string poolStr = goPrefab.GetComponent<PoolNameHolder>().PoolName.Value;

                    SpawnObjectAtPosition(poolStr, tileCenteredPosition);
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
                            UndoSpawnObject();
                        }
                    }
                }
            }
        }

        private void SpawnObjectAtPosition(string poolTag, Vector2 position)
        {
            GameObject spawnedGO = ObjectPooler.Instance.GetObject(poolTag);
            spawnedGO.transform.position = position;// EditorUtils.GetMouseWorldPos();

            // Keep the necessary values to spawn this object in playmode
            LevelEditorEntry lvlEditorEntry = new LevelEditorEntry(
                poolTag,
                new Point(spawnedGO.transform.position.x, spawnedGO.transform.position.y)
                );
            m_Entries.Add(lvlEditorEntry);

            CreateFolderIfNeeded();

            // Create a new command and push it to the stack
            LevelEditorCommand command = new LevelEditorCommand(spawnedGO, poolTag);
            m_CommandStack.Push(command);
            string path = Path.Combine(Application.streamingAssetsPath, "Saves", "LevelEditorSaves", _currentScene + "_SceneSetup.txt");
            using (StreamWriter file = File.CreateText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, m_Entries);
            }
            //string json = JsonConvert.SerializeObject(m_Entries); // Convert the m_Entries object to a JSON string
            //File.WriteAllText("Assets/LevelEditorSaves/save.txt", json); // Write the JSON string to the file

            CollisionChecker2D cc = spawnedGO.GetComponent<CollisionChecker2D>();
            DestroyImmediate(cc);

            Debug.Log("Grabbed a : " + spawnedGO.name + " from the object pool.");
        }

        private void UndoSpawnObject()
        {
            if (m_CommandStack.Count > 0)
            {
                // Pop the last command from the stack
                LevelEditorCommand command = m_CommandStack.Pop();

                ObjectPooler.Instance.ReturnObject(command.spawnedGO, command.poolTag);

                m_Entries.RemoveAt(m_Entries.Count - 1);

                CreateFolderIfNeeded();
                string path = Path.Combine(Application.streamingAssetsPath, "Saves", "LevelEditorSaves", _currentScene + "_SceneSetup.txt");
                using (StreamWriter file = File.CreateText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, m_Entries);
                }
            }
        }

        private void CreateFolderIfNeeded()
        {
            // Create the path for the folder and the file
            string path = Path.Combine(Application.streamingAssetsPath, "Saves", "LevelEditorSaves");

            // Check if the folder exists
            bool folderExists = Directory.Exists(path);

            if (!folderExists)
            {
                Directory.CreateDirectory(path);
                Debug.Log("Created folder " + path);
            }
        }
    }
}
#endif