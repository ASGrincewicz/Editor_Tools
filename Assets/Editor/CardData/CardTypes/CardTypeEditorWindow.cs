using Editor.Channels;
using UnityEditor;
using UnityEngine;

namespace Editor.CardData.CardTypes
{
    public class CardTypeEditorWindow: EditorWindow
    {
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
            _typeEditorWindow = GetWindow<CardTypeEditorWindow>("Card Type Editor");
            _typeEditorWindow.position = new Rect(250f, 150f, 300f, 300f);
            _typeEditorWindow.Show();
        }
        
        private void OnEnable()
        {
            _editorWindowChannel.OnCardTypeEditorWindowRequested += OpenCardTypeInEditor;
        }

        private void OnDisable()
        {
            _editorWindowChannel.OnCardTypeEditorWindowRequested -= OpenCardTypeInEditor;
        }

        public void OpenCardTypeInEditor(CardTypeDataSO cardTypeData)
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
           _loadedType = (CardTypeDataSO)EditorGUILayout.ObjectField("Card Type Asset",_loadedType, typeof(CardTypeDataSO), false);
           _cardTypeName = EditorGUILayout.TextField("Card Type Name", _cardTypeName);
           _cardTypeIcon = (Texture2D)EditorGUILayout.ObjectField("Card Type Icon",_cardTypeIcon, typeof(Texture2D), false);
           _cardTypeColor = EditorGUILayout.ColorField("Card Type Color",_cardTypeColor);
           _hasStats = EditorGUILayout.Toggle("Stats", _hasStats);
           _hasCost = EditorGUILayout.Toggle("Cost", _hasCost);
           _hasKeywords = EditorGUILayout.Toggle("Keywords", _hasKeywords);
           _hasCardText = EditorGUILayout.Toggle("Card Text", _hasCardText);
           
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
        }

        private void HandleSaveButtonPressed()
        {
            Debug.Log("Save card type");
        }

        private void HandleLoadButtonPressed()
        {
            Debug.Log("Load card type");
            LoadCardTypeData();
        }

        private void HandleUnloadButtonPressed()
        {
            Debug.Log("Unload card type");
        }
        
        private void HandleDoneButtonPressed()
        {
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

    }
}