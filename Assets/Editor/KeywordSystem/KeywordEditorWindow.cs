using System;
using System.Collections.Generic;
using Editor.Channels;
using UnityEditor;
using UnityEngine;

namespace Editor.KeywordSystem
{
    public class KeywordEditorWindow : EditorWindow
    {
        [SerializeField] private EditorWindowChannel _editorWindowChannel;
        // Constants
        private const float MAIN_AREA_HEIGHT_RATIO = 0.30f;
        private const float BUTTON_AREA_HEIGHT_RATIO = 0.15f;
        private const float AREA_PADDING = 20f;
        private const float BUTTON_WIDTH = 100f;

        // Keyword Manager Asset
        [SerializeField] private KeywordManager _keywordManager;
        private List<Keyword> _keywords;

        // Class variables
        private string _keywordName = "";
        private int _keywordValue = 0;
        private string _keywordDefinition = "";
        private AbilityType _abilityType;
        
        // GUI variables
        private Rect _mainAreaRect;
        private Rect _buttonAreaRect;
        private Rect _keywordListAreaRect;
        private Vector2 _scrollPosition;

        [MenuItem("Tools/Keyword Editor")]
        private static void Init()
        {
            KeywordEditorWindow window = GetWindow<KeywordEditorWindow>();
            window.titleContent = new GUIContent("Keyword Editor");
            window.position = new Rect(50, 50, 250, 600);
            window.Show();
        }

        private void OnEnable()
        {
            _editorWindowChannel.OnKwywordEditorWindowRequested += OpenKeywordEditorWindow;
            LoadKeywords();
        }

        private void OnDisable()
        {
            _editorWindowChannel.OnKwywordEditorWindowRequested -= OpenKeywordEditorWindow;
        }

        private void OpenKeywordEditorWindow()
        {
            Init();
        }

        private void OnGUI()
        {
            InitializeAreaRects();
            DrawMainArea();
            DrawButtonArea();
            DrawKeywordListArea();
        }

        private void InitializeAreaRects()
        {
            float width = position.width * 0.90f;
            _mainAreaRect = new Rect(AREA_PADDING, 15, width, position.height * MAIN_AREA_HEIGHT_RATIO);
            _buttonAreaRect = new Rect(AREA_PADDING, _mainAreaRect.y + _mainAreaRect.height + 5, width, position.height * BUTTON_AREA_HEIGHT_RATIO);
            _keywordListAreaRect = new Rect(AREA_PADDING, _buttonAreaRect.y + _buttonAreaRect.height + 5, width, position.height * (1 - MAIN_AREA_HEIGHT_RATIO - BUTTON_AREA_HEIGHT_RATIO) - 40);
        }

        private void DrawMainArea()
        {
            GUILayout.BeginArea(_mainAreaRect);
            EditorGUIUtility.labelWidth = 100;
            _keywordName = EditorGUILayout.TextField("Name:", _keywordName);
            _keywordValue = EditorGUILayout.IntField("Value:", _keywordValue);
            _keywordDefinition = EditorGUILayout.TextArea(_keywordDefinition, GUILayout.Height(50));
            _abilityType = (AbilityType)EditorGUILayout.EnumPopup("Ability Type:", _abilityType);
            GUILayout.EndArea();
        }

        private void DrawButtonArea()
        {
            GUILayout.BeginArea(_buttonAreaRect);
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            if (GUILayout.Button("Save", EditorStyles.toolbarButton,GUILayout.Width(BUTTON_WIDTH)))
            {
                SaveKeywords();
            }
            if (GUILayout.Button("New Keyword",EditorStyles.toolbarButton,GUILayout.Width(BUTTON_WIDTH)))
            {
                InitializeNewKeyword();
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            if (GUILayout.Button("Reload Keyword List",EditorStyles.toolbarButton,GUILayout.Width(BUTTON_WIDTH * 2)))
            {
                LoadKeywords();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void DrawKeywordListArea()
        {
            GUILayout.BeginArea(_keywordListAreaRect);
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.ExpandHeight(true));
            Keyword itemToRemove = new Keyword();
            
            foreach (Keyword keyword in _keywords)
            {
                if (string.IsNullOrEmpty(keyword.keywordName))
                {
                    continue;
                }
                
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(keyword.keywordName);
                if (GUILayout.Button("Edit", GUILayout.Width(50)))
                {
                    EditKeyword(keyword);
                }
                if (GUILayout.Button("Delete", GUILayout.Width(50)))
                {
                    if (EditorUtility.DisplayDialog("Confirm Deletion", $"Are you sure you want to delete the keyword '{keyword.keywordName}'?", "Yes", "No"))
                    {
                        itemToRemove = keyword;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            if (!itemToRemove.IsDefault())
            {
                DeleteKeyword(itemToRemove);
            }
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
        

        private void LoadKeywords()
        {
            _keywords = _keywordManager?.keywordList ?? new List<Keyword>();
        }

        private void SaveKeywords()
        {
            Undo.RecordObject(_keywordManager, "Save Keywords");
            Keyword editedKeyword = new()
            {
                keywordName = _keywordName,
                keywordValue = _keywordValue,
                definition = _keywordDefinition,
                abilityType = _abilityType
            };
            
            if (_keywords == null)
            {
                Debug.LogError("_keywords list is not initialized");
                return;
            }
            foreach (Keyword keyword in _keywords)
            {
                if (string.Equals(keyword.keywordName, _keywordName, StringComparison.OrdinalIgnoreCase) 
                    || string.IsNullOrEmpty(keyword.keywordName))
                {
                    _keywords.Remove(keyword);
                    _keywords.Add(editedKeyword);
                    EditorUtility.SetDirty(_keywordManager);
                    AssetDatabase.SaveAssets();
                    return;
                }
            }
            _keywords.Add(editedKeyword);
            EditorUtility.SetDirty(_keywordManager);
            AssetDatabase.SaveAssets();
        }

        private void EditKeyword(Keyword keyword)
        {
            _keywordName = keyword.keywordName;
            _keywordValue = keyword.keywordValue;
            _keywordDefinition = keyword.definition;
            _abilityType = keyword.abilityType;
        }

        private void DeleteKeyword(Keyword keyword)
        {
            Undo.RecordObject(_keywordManager, "Delete Keyword");
            _keywords.Remove(keyword);
        }

        private void InitializeNewKeyword()
        {
            _keywordDefinition = "";
            _keywordValue = 0;
            _keywordName = "";
            _abilityType = AbilityType.None;
        }
    }
}