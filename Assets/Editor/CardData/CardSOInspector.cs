﻿using System.Collections.Generic;
using Editor.CardData.CardTypes;
using Editor.CardData.Stats;
using Editor.Channels;
using Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace Editor.CardData
{
    [CustomEditor(typeof(CardDataSO))]
    public class CardSoInspector: UnityEditor.Editor
    {
        // Logic helper
        private CardInspectorLogic _cardInspectorLogic;
        // Property Names
        [SerializeField] private EditorWindowChannel _editorWindowChannel;
        private const string CardSetNamePropertyName = "_cardSetName";
        private const string CardNumberPropertyName = "_cardNumber";
        private const string RarityPropertyName = "_rarity";
        private const string CostPropertyName = "_cost";
        private const string CardTypeDataPropertyName = "_cardTypeData";
        private const string CardNamePropertyName = "_cardName";
        private const string ArtWorkPropertyName = "_artWork";
        private const string KeywordsPropertyName = "_keywords";
        private const string CardTextPropertyName = "_cardText";
        private const string StatsPropertyName = "_stats";
        
        private SerializedProperty CardSetNameProperty { get; set; }
        private SerializedProperty CardNumberProperty { get; set; }
        private SerializedProperty RarityProperty { get; set; }
        private SerializedProperty CostProperty { get; set; }
        private SerializedProperty CardTypeDataProperty { get; set; }
        private string CardTypeNameProperty { get; set; }
        private bool HasStatsProperty { get; set; }
        private bool HasKeywordsProperty { get; set; }
        private bool HasCostProperty { get; set; }
        private bool HasCardTextProperty { get; set; }
        
        private CardTypeDataSO CardTypeData { get; set; }
        private SerializedProperty CardNameProperty { get; set; }
        private SerializedProperty ArtWorkProperty { get; set; }
        private SerializedProperty KeywordsProperty { get; set; }
        private SerializedProperty CardTextProperty { get; set; }
        private SerializedProperty StatsProperty { get; set; }
        
        private List<CardStat> CardStats { get; set; }
        
        
        
        // GUI Properties
        private bool IsOpenInCardEditorButtonPressed => GUILayout.Button("Open in Card Editor", EditorStyles.toolbarButton);
        private bool IsOpenInCostCalculatorButtonPressed => GUILayout.Button("Open in Cost Calculator", EditorStyles.toolbarButton);

        private void OnEnable()
        {
            _cardInspectorLogic = new CardInspectorLogic();
            CardSetNameProperty = serializedObject.FindProperty(CardSetNamePropertyName);
            CardNumberProperty = serializedObject.FindProperty(CardNumberPropertyName);
            RarityProperty = serializedObject.FindProperty(RarityPropertyName);
            CostProperty = serializedObject.FindProperty(CostPropertyName);
            CardTypeDataProperty = serializedObject.FindProperty(CardTypeDataPropertyName);
            CardTypeData  = CardTypeDataProperty.objectReferenceValue as CardTypeDataSO;
            if (CardTypeData != null)
            {
                CardTypeNameProperty = CardTypeData.CardTypeName;
                HasStatsProperty = CardTypeData.HasStats;
                HasKeywordsProperty = CardTypeData.HasKeywords;
                HasCostProperty = CardTypeData.HasCost;
                HasCardTextProperty = CardTypeData.HasCardText;
            }
            CardDataSO cardDataSO = target as CardDataSO;
            CardStats = cardDataSO.Stats;
            CardNameProperty = serializedObject.FindProperty(CardNamePropertyName);
            ArtWorkProperty = serializedObject.FindProperty(ArtWorkPropertyName);
            KeywordsProperty = serializedObject.FindProperty(KeywordsPropertyName);
            CardTextProperty = serializedObject.FindProperty(CardTextPropertyName);
            StatsProperty = serializedObject.FindProperty(StatsPropertyName);
           
            
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawCardInspectorGUI();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawCardInspectorGUI() 
        {           
            CardDataSO cardData = target as CardDataSO;
            GUILayout.BeginVertical(GUILayout.Width(300));
            EditorGUIUtility.labelWidth = 80;
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            DrawOpenCardEditorButton(cardData);
            DrawOpenCostCalculatorButton(cardData);
            GUILayout.EndHorizontal();
            
            DrawProperties();

            GUILayout.EndVertical();
        }

        private void DrawOpenCardEditorButton(CardDataSO cardData) 
        {
            if (IsOpenInCardEditorButtonPressed) 
            {
               _editorWindowChannel.RaiseCardEditorWindowRequestedEvent(cardData);
            }
        }

        private void DrawOpenCostCalculatorButton(CardDataSO cardData)
        {
            if (IsOpenInCostCalculatorButtonPressed)
            {
               _editorWindowChannel.RaiseCostCalculatorWindowRequestedEvent(cardData);
            }
        }
        

        private void DrawProperties() 
        {
            GUI.enabled = true;
            DrawLabel(_cardInspectorLogic.FormatPropertyLabel($"#{CardNumberProperty.intValue} in ",$"{CardSetNameProperty.stringValue}"));
            CardRarity cardRarity = (CardRarity) RarityProperty.enumValueIndex;
            DrawLabel(_cardInspectorLogic.FormatPropertyLabel($"Rarity: ",$"{cardRarity.GetDescription()}"));
            if (HasCostProperty)
            {
                DrawLabel(_cardInspectorLogic.FormatPropertyLabel($"Card Cost: ",$"{CostProperty.intValue}"));
            }
           
            DrawLabel(_cardInspectorLogic.FormatPropertyLabel($"Card Type: ",$"{CardTypeNameProperty}"));

            DrawLabel(_cardInspectorLogic.FormatPropertyLabel($"Card Name: ", $"{CardNameProperty.stringValue}"));

            DrawArtWorkProperty();

            if (_cardInspectorLogic.ShouldDrawKeywordData(HasKeywordsProperty))
            {
                DrawKeywordArrayProperty();
            }
            
            if (_cardInspectorLogic.ShouldDrawCardTextData(HasCardTextProperty))
            {
                DrawCardTextLabel();
            }
            
            if (_cardInspectorLogic.ShouldDrawCardStatData(HasStatsProperty))
            {
               DrawStatsProperties();
            }
        }

        private void DrawArtWorkProperty() 
        {
            if (ArtWorkProperty.objectReferenceValue is not Texture2D artworkTexture) 
            {
                return;
            }
            GUILayout.Label("Artwork Preview:");
            Rect rect = GUILayoutUtility.GetRect(400 * 0.75f, 225 * 0.75f);
            EditorGUI.DrawPreviewTexture(rect, artworkTexture);
        }

        private void DrawKeywordArrayProperty()
        {
            GUILayout.Label("Keywords:",EditorStyles.boldLabel, GUILayout.ExpandWidth(true));
            GUILayout.BeginHorizontal(EditorStyles.boldLabel);
            for (int i = 0; i < KeywordsProperty.arraySize; i++)
            {
                
                SerializedProperty keywordProperty = KeywordsProperty.GetArrayElementAtIndex(i);
                SerializedProperty keywordNameProperty = keywordProperty.FindPropertyRelative("keywordName");
                string keywordName = keywordNameProperty.stringValue;
                if (!string.IsNullOrEmpty(keywordName))
                {
                    DrawLabel(_cardInspectorLogic.FormatPropertyLabel("",keywordNameProperty.stringValue));
                }
            }
            GUILayout.EndHorizontal();
        }
        
        private void DrawLabel((string, string) labelText) 
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.LabelField(labelText.Item1,EditorStyles.boldLabel, GUILayout.Width(10), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.LabelField(labelText.Item2,EditorStyles.label, GUILayout.Width(100), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.EndHorizontal();
        }

        private void DrawCardTextLabel()
        {
            EditorGUILayout.LabelField("Card Text: ",EditorStyles.boldLabel, GUILayout.Width(10), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.LabelField($"{CardTextProperty.stringValue}",EditorStyles.wordWrappedLabel, GUILayout.Width(100), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        }

        private void DrawStatsProperties() 
        {
            if (CardTypeDataProperty != null && CardTypeDataProperty.objectReferenceValue != null)
            {
                DrawLabel(_cardInspectorLogic.FormatPropertyLabel($"STATS:",""));
                foreach (CardStat stat in CardStats)
                {
                    DrawLabel(_cardInspectorLogic.FormatPropertyLabel("",$"{stat.statName}"));
                    DrawLabel(_cardInspectorLogic.FormatPropertyLabel("",$"{stat.statValue}"));
                    DrawLabel(_cardInspectorLogic.FormatPropertyLabel("",$"{stat.statDescription}"));;
                    DrawLabel(_cardInspectorLogic.FormatPropertyLabel("          ",""));
                }
            }
        }
    }
}