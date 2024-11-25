using Editor.Channels;
using UnityEditor;
using UnityEngine;

namespace Editor.CardData.CardTypes
{
    public class CardTypeEditorWindow: EditorWindow
    {
        private const string WindowTitle = "Card Type Editor";
        private const string ResourcesPath = "Assets/Resources/Scriptable Objects/";
        private const string CardTypesPath = "Card Types/";
        private const string ObjectFieldLabel = "Card Type Asset:";
        private const string CardTypeNameFieldLabel = "Card Type Name:";
        private const string CardTypeIconFieldLabel = "Card Type Icon:";
        private const string CardTypeColorFieldLabel = "Card Type Color:";
        private const string HasStatsFieldLabel = "Has Stats:";
        private const string HasCostFieldLabel = "Has Cost:";
        private const string HasKeywordsFieldLabel = "Has Keywords:";
        private const string HasCardTextFieldLabel = "Has Card Text:";
        [SerializeField] private EditorWindowChannel _editorWindowChannel;

        private static EditorWindow _typeEditorWindow;
        
        private Vector2 ScrollPosition { get; set; }
        private Rect MainAreaRect { get; set; }

        private CardTypeDataSO _loadedType; 
        
        // Input Field Variables
        private string _cardTypeName;
        private Texture2D _cardTypeIcon;
        private Color _cardTypeColor;
        private bool _hasStats;
        private bool _hasCost;
        private bool _hasKeywords;
        private bool _hasCardText;

        [MenuItem("Tools/Card Type Editor")]
        public static void Init()
        {
            _typeEditorWindow = GetWindow<CardTypeEditorWindow>(WindowTitle);
            _typeEditorWindow.position = new Rect(250f, 150f, 300f, 300f);
            _typeEditorWindow.Show();
        }
        
        private void OnEnable()
        {
            _editorWindowChannel.OnCardTypeEditorWindowRequested += OpenCardTypeInEditor;
            VerifyDirectory();
        }

        private void OnDisable()
        {
            _editorWindowChannel.OnCardTypeEditorWindowRequested -= OpenCardTypeInEditor;
        }
        
        private void VerifyDirectory()
        {
            if (AssetDatabase.IsValidFolder(ResourcesPath+CardTypesPath))
            {
                return;
            }

            AssetDatabase.CreateFolder(ResourcesPath, CardTypesPath);

        }

        private void OpenCardTypeInEditor(CardTypeDataSO cardTypeData)
        {
            Init();
            _loadedType = cardTypeData;
            LoadCardTypeData();
        }
        
        private void OnGUI()
        {
            SetupAreaRects();
            DrawMainArea();
        }

        private void SetupAreaRects()
        {
            MainAreaRect= new Rect(5,5,position.width - 10,position.height - 25);
        }

        private void DrawMainArea()
        {
            GUILayout.BeginArea(MainAreaRect);
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            DrawControlButtons();
            GUILayout.EndHorizontal();
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition,GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            DrawEditableFields();
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private void DrawEditableFields()
        {
           _loadedType = (CardTypeDataSO)EditorGUILayout.ObjectField(ObjectFieldLabel,_loadedType, typeof(CardTypeDataSO), false);
           _cardTypeName = EditorGUILayout.TextField(CardTypeNameFieldLabel, _cardTypeName);
           _cardTypeIcon = (Texture2D)EditorGUILayout.ObjectField(CardTypeIconFieldLabel,_cardTypeIcon, typeof(Texture2D), false);
           _cardTypeColor = EditorGUILayout.ColorField(CardTypeColorFieldLabel,_cardTypeColor);
           _hasStats = EditorGUILayout.Toggle(HasStatsFieldLabel, _hasStats);
           _hasCost = EditorGUILayout.Toggle(HasCostFieldLabel, _hasCost);
           _hasKeywords = EditorGUILayout.Toggle(HasKeywordsFieldLabel, _hasKeywords);
           _hasCardText = EditorGUILayout.Toggle(HasCardTextFieldLabel, _hasCardText);
           
        }

        private void DrawControlButtons()
        {
            if (GUILayout.Button("Create", EditorStyles.toolbarButton))
            {
                HandleCreateButtonPressed();
            }
            if (GUILayout.Button("Load", EditorStyles.toolbarButton))
            {
                HandleLoadButtonPressed();
            }
            if (GUILayout.Button("Save", EditorStyles.toolbarButton))
            {
                HandleSaveButtonPressed();
            }
            if(GUILayout.Button("Unload", EditorStyles.toolbarButton) && !ReferenceEquals(_loadedType, null))
            {
                HandleUnloadButtonPressed();
            }

            if (GUILayout.Button("Done", EditorStyles.toolbarButton))
            {
                HandleDoneButtonPressed();
            }
        }

        private void HandleCreateButtonPressed()
        {
            Debug.Log("Create card type");
            CreateNewCardType();
        }

        private void HandleSaveButtonPressed()
        {
            Debug.Log("Save card type");
            if (!ReferenceEquals(_loadedType, null))
            {
                InitializeLoadedCardType();
                Undo.RecordObject(_loadedType, "Save card type");
                SaveAndRefreshAssets();
            }
        }

        private void HandleLoadButtonPressed()
        {
            Debug.Log("Load card type");
            LoadCardTypeData();
        }

        private void HandleUnloadButtonPressed()
        {
            Debug.Log("Unload card type");
            if (!ReferenceEquals(_loadedType, null))
            {
                _loadedType = null;
                _cardTypeName = string.Empty;
                _cardTypeIcon = null;
                _cardTypeColor = Color.white;
                _hasStats = false;
                _hasCost = false;
                _hasKeywords = false;
                _hasCardText = false;
            }
        }
        
        private void HandleDoneButtonPressed()
        {
            _typeEditorWindow ??= GetWindow<CardTypeEditorWindow>();

            _typeEditorWindow.Close();
        }

        private void LoadCardTypeData()
        {
            if (_loadedType == null)
            {
                return;
            }
            _cardTypeName = _loadedType.CardTypeName;
            _cardTypeIcon = _loadedType.CardTypeIcon;
            _cardTypeColor = _loadedType.CardTypeColor;
            _hasStats = _loadedType.HasStats;
            _hasCost = _loadedType.HasCost;
            _hasKeywords = _loadedType.HasKeywords;
            _hasCardText = _loadedType.HasCardText;
        }

        private void CreateNewCardType()
        {
            _loadedType = ScriptableObject.CreateInstance<CardTypeDataSO>();
            InitializeLoadedCardType();
            AssetDatabase.CreateAsset(_loadedType, ResourcesPath + CardTypesPath + _cardTypeName + "_CardType.asset");
            Undo.RecordObject(_loadedType, "Create card type");
            SaveAndRefreshAssets();
        }

        private void InitializeLoadedCardType()
        {
            _loadedType.CardTypeName = _cardTypeName;
            _loadedType.CardTypeIcon = _cardTypeIcon;
            _loadedType.CardTypeColor = _cardTypeColor;
            _loadedType.HasStats = _hasStats;
            _loadedType.HasCost = _hasCost;
            _loadedType.HasKeywords = _hasKeywords;
            _loadedType.HasCardText = _hasCardText;
        }

        private void SaveAndRefreshAssets()
        {
            EditorUtility.SetDirty(_loadedType);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

    }
}