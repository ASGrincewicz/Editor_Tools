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

        [MenuItem("Tools/Card Type Editor")]
        public static void Init()
        {
            _typeEditorWindow = GetWindow<CardTypeEditorWindow>("Card Type Editor");
            _typeEditorWindow.position = new Rect(250f, 150f, 600f, 650f);
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
        }

        private void DrawControlButtons()
        {
           
        }
    }
}