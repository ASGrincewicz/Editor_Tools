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
        private AbilityType _abilityType;
        
        // GUI variables
        private Rect _mainAreaRect;
        private Rect _buttonAreaRect;
        private Rect _keywordListAreaRect;
        private Vector2 _scrollPosition;
        
        
        [MenuItem("Tools/Keyword Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<KeywordEditorWindow>();
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
            _mainAreaRect = new Rect(20,15, position.width * 0.90f, position.height * 0.25f);
            _buttonAreaRect = new Rect(20,_mainAreaRect.y + _mainAreaRect.height + 5, position.width * 0.90f, position.height * 0.15f);
            _keywordListAreaRect = new Rect(20, (_buttonAreaRect.y + _buttonAreaRect.height) + 5, position.width * 0.90f, position.height * 0.50f);
        }

        private void DrawMainArea()
        {
            GUILayout.BeginArea(_mainAreaRect);
            EditorGUIUtility.labelWidth = 100;
            _keywordName = EditorGUILayout.TextField("Keyword Name:",_keywordName);
            _keywordValue = EditorGUILayout.IntField("Keyword Value:",_keywordValue);
            _abilityType = (AbilityType)EditorGUILayout.EnumPopup("Ability Type:",_abilityType);
            GUILayout.EndArea();
        }

        private void DrawButtonArea()
        {
            GUILayout.BeginArea(_buttonAreaRect);
            // Implement Save functions
            GUILayout.Button("Save", GUILayout.Width(100));
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
            foreach (var keyword in _keywords)
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
                    _abilityType = keyword.abilityType;
                }
                EditorGUILayout.EndHorizontal();
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
    }
}