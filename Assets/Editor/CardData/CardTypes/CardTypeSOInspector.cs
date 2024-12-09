using System.Collections.Generic;
using Editor.CardData.Stats;
using Editor.Channels;
using UnityEditor;
using UnityEngine;

namespace Editor.CardData.CardTypes
{
    [CustomEditor(typeof(CardTypeDataSO))]
    public class CardTypeSOInspector : UnityEditor.Editor
    {
        
        // Logic helper
        private CardTypeInspectorLogic _cardTypeInspectorLogic;
        // Property Names
        private const string CardTypeName = "_cardTypeName";
        private const string CardTypeIconName = "_cardTypeIcon";
        private const string CardTypeColorName = "_cardTypeColor";
        private const string HasStatsName = "_hasStats";
        private const string HasCostName = "_hasCost";
        private const string HasKeywordsName = "_hasKeywords";
        private const string HasCardTextName = "_hasCardText";
        private const string CardStatDataName = "_cardStatData";

        [SerializeField] private EditorWindowChannel _editorWindowChannel;
        // Serialized Properties
        private SerializedProperty CardTypeNameProperty { get; set; }
        private SerializedProperty CardTypeIconProperty { get; set; }
        
        private SerializedProperty CardTypeColorProperty { get; set; }
        private SerializedProperty HasStatsProperty { get; set; }
        private SerializedProperty HasCostProperty { get; set; }
        private SerializedProperty HasKeywordsProperty { get; set; }
        private SerializedProperty HasCardTextProperty { get; set; }
        private SerializedProperty CardStatDataProperty { get; set; }
        
        private List<CardStatData> _cardStatData;

        private void OnEnable()
        {
            _cardTypeInspectorLogic = new CardTypeInspectorLogic();
            CardTypeNameProperty = serializedObject.FindProperty(CardTypeName);
            CardTypeIconProperty = serializedObject.FindProperty(CardTypeIconName);
            CardTypeColorProperty = serializedObject.FindProperty(CardTypeColorName);
            HasStatsProperty = serializedObject.FindProperty(HasStatsName);
            HasCostProperty = serializedObject.FindProperty(HasCostName);
            HasKeywordsProperty = serializedObject.FindProperty(HasKeywordsName);
            HasCardTextProperty = serializedObject.FindProperty(HasCardTextName);
            CardStatDataProperty = serializedObject.FindProperty(CardStatDataName);
            CardTypeDataSO cardTypeSO = (CardTypeDataSO) target;
            _cardStatData = cardTypeSO.CardStats;
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawCardTypeSOInspectorGUI();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawCardTypeSOInspectorGUI()
        {
            CardTypeDataSO cardTypeSO = (CardTypeDataSO) target;
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            DrawOpenInCardTypeEditorButton(cardTypeSO);
            GUILayout.EndHorizontal();
            DrawRequiredProperties();
            DrawOptionalProperties();
        }

        private void DrawOpenInCardTypeEditorButton(CardTypeDataSO cardTypeSO)
        {
            if (GUILayout.Button("Open Card Type Editor", EditorStyles.toolbarButton))
            {
                _editorWindowChannel.RaiseCardTypeEditorWindowRequestedEvent(cardTypeSO);
            }
        }

        private void DrawRequiredProperties()
        {
            DrawBoldLabel(_cardTypeInspectorLogic.FormatPropertyLabel("Card Type Name", CardTypeNameProperty.stringValue));
            DrawColorPreview();
            DrawArtWorkProperty();
            DrawBoldLabel(_cardTypeInspectorLogic.FormatPropertyLabel("Has Stats",HasStatsProperty.boolValue.ToString()));
            DrawBoldLabel(_cardTypeInspectorLogic.FormatPropertyLabel("Has Cost",HasCostProperty.boolValue.ToString() ));
            DrawBoldLabel(_cardTypeInspectorLogic.FormatPropertyLabel("Has Keywords",HasKeywordsProperty.boolValue.ToString()));
            DrawBoldLabel(_cardTypeInspectorLogic.FormatPropertyLabel("Has Card Text",HasCardTextProperty.boolValue.ToString()));
            DrawLabel("          ");
        }
        
        private void DrawColorPreview()
        {
            GUILayout.Label("Color Preview:");
            Rect rect = GUILayoutUtility.GetRect(50, 25, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
            EditorGUI.DrawRect(rect, CardTypeColorProperty.colorValue);
        }
        
        private void DrawArtWorkProperty() 
        {
            if (CardTypeIconProperty.objectReferenceValue is not Texture2D iconTexture) 
            {
                return;
            }
            GUILayout.Label("Icon Preview:");
            Rect rect = GUILayoutUtility.GetRect(100, 100, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
            EditorGUI.DrawPreviewTexture(rect, iconTexture);
        }

        private void DrawOptionalProperties()
        {
            if (_cardTypeInspectorLogic.ShouldDrawCardStatData(HasStatsProperty.boolValue))
            {
                DrawCardStatDataProperty();
            }
        }

        private void DrawCardStatDataProperty()
        {
            DrawBoldLabel("Stats");
            foreach (CardStatData stat in _cardStatData)
            {
                DrawLabel(_cardTypeInspectorLogic.FormatPropertyLabel("Stat Name",$"{stat.statName}"));
                DrawLabel(_cardTypeInspectorLogic.FormatPropertyLabel("Description",$"{stat.statDescription}"));
                DrawLabel(_cardTypeInspectorLogic.FormatPropertyLabel("Weight",$"{stat.statWeight}"));
                DrawLabel("          ");
            }
        }
        
        private void DrawBoldLabel(string labelText = "") 
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.LabelField(labelText,EditorStyles.boldLabel, GUILayout.Width(10), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.EndHorizontal();
        }

        private void DrawLabel(string labelText = "")
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.LabelField(labelText, EditorStyles.label, GUILayout.Width(10), GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true));
            GUILayout.EndHorizontal();
        }
    }
}