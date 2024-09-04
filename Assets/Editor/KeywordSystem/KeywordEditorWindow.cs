using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor.KeywordSystem
{
    public class KeywordEditorWindow : EditorWindow
    {
        //Keyword Manager Asset
        [SerializeField] private KeywordManager _keywordManager;
        private List<Keyword> _keywords;
        
        //Class variables
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
        private static void ShowWindow()
        {
            KeywordEditorWindow window = GetWindow<KeywordEditorWindow>();
            window.titleContent = new GUIContent("Keyword Editor");
            window.position = new Rect(50, 50, 250, 600);
            window.Show();
        }

        private void OnEnable()
        {
            GetKeywords();
        }

        private void OnGUI()
        {
            SetupAreaRects();
            DrawMainArea();
            DrawButtonArea();
            DrawKeywordListArea();
        }

        private void SetupAreaRects()
        {
            _mainAreaRect = new Rect(20,15, position.width * 0.90f, position.height * 0.30f);
            _buttonAreaRect = new Rect(20,_mainAreaRect.y + _mainAreaRect.height + 5, position.width * 0.90f, position.height * 0.15f);
            _keywordListAreaRect = new Rect(20, (_buttonAreaRect.y + _buttonAreaRect.height) + 5, position.width * 0.90f, position.height * 0.50f);
        }

        private void DrawMainArea()
        {
            GUILayout.BeginArea(_mainAreaRect);
            EditorGUIUtility.labelWidth = 100;
            _keywordName = EditorGUILayout.TextField("Name:",_keywordName);
            _keywordValue = EditorGUILayout.IntField("Value:",_keywordValue);
            _keywordDefinition = EditorGUILayout.TextArea(_keywordDefinition, GUILayout.Height(50));
            _abilityType = (AbilityType)EditorGUILayout.EnumPopup("Ability Type:",_abilityType);
            GUILayout.EndArea();
        }

        private void DrawButtonArea()
        {
            GUILayout.BeginArea(_buttonAreaRect);
            // Implement Save functions

            if (GUILayout.Button("Save", GUILayout.Width(100)))
            {
                SaveKeywords();
            }

            if (GUILayout.Button("New Keyword", GUILayout.Width(100)))
            {
                InitializeNewKeyword();
            }
            if (GUILayout.Button("Reload Keyword List"))
            {
                GetKeywords();
            }
            GUILayout.EndArea();
        }

        private void DrawKeywordListArea()
        {
            GUILayout.BeginArea(_keywordListAreaRect);
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.ExpandHeight(true));
            Keyword itemToRemove = new Keyword();
            
            foreach (Keyword keyword in _keywords)
            {
                if (keyword.keywordName == "")
                {
                    continue;
                }
                    
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(keyword.keywordName);
                if (GUILayout.Button("Edit",GUILayout.Width(50)))
                {
                    _keywordName = keyword.keywordName;
                    _keywordValue = keyword.keywordValue;
                    _keywordDefinition = keyword.definition;
                    _abilityType = keyword.abilityType;
                }

                if (GUILayout.Button("Delete", GUILayout.Width(50)))
                {
                    // Confirm with popup window
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

        private void GetKeywords()
        {
            if (!ReferenceEquals(_keywordManager, null))
            {
                _keywords = _keywordManager.keywordList;
            }
        }

        private void SaveKeywords()
        {
            Undo.RecordObject(_keywordManager, "Save Keywords");
            Keyword editedKeyword = new Keyword
            {
                keywordName = _keywordName,
                keywordValue = _keywordValue,
                definition = _keywordDefinition,
                abilityType = _abilityType
            };
            // Check if _keywords is initialized
            if (_keywords == null)
            {
                Debug.LogError("_keywords list is not initialized");
                return;
            }

            // Overwrite data for edited keywords
            foreach (Keyword keyword in _keywords)
            {
                if (string.Equals(keyword.keywordName, _keywordName, StringComparison.OrdinalIgnoreCase))
                {
                    _keywords.Remove(_keywords.Find(x => x.keywordName == keyword.keywordName));
                    _keywords.Add(editedKeyword);
                    return;
                }
                // replace empty keyword if available
                Keyword emptyKeyword = _keywords.Find(x => string.IsNullOrEmpty(x.keywordName));
                if (emptyKeyword.keywordName != null)
                {
                    _keywords.Remove(emptyKeyword);
                    _keywords.Add(editedKeyword);
                    return;
                }
            }

            // Add new keywords to list if not found
            _keywords.Add(new Keyword
            {
                keywordName = _keywordName,
                keywordValue = _keywordValue,
                definition = _keywordDefinition,
                abilityType = _abilityType
            });
            EditorUtility.SetDirty(_keywordManager);
            AssetDatabase.SaveAssets();
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