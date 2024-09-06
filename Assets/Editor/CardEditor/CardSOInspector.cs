using Editor.CardData;
using Editor.CostCalculator;
using Editor.Utilities;
using UnityEditor;
using UnityEngine;
namespace Editor.CardEditor
{
    [CustomEditor(typeof(CardSO))]
    public class CardSOInspector: UnityEditor.Editor
    {
        private const string WeightDataPropertyName = "_weightData";
        private const string CardTypePropertyName = "_cardType";
        private const string CardNamePropertyName = "_cardName";
        private const string ArtWorkPropertyName = "_artWork";
        private const string AttackStatPropertyName = "_attack";
        private const string ExploreStatPropertyName = "_explore";
        private const string HitPointsStatPropertyName = "_hitPoints";
        private const string SpeedStatPropertyName = "_speed";
        private const string FocusStatPropertyName = "_focus";
        private const string UpgradeSlotsPropertyName = "_upgradeSlots";
        private const string KeywordsPropertyName = "_keywords";
        private const string CardTextPropertyName = "_cardText";
        private const string CostPropertyName = "_cost";
        private const string RarityPropertyName = "_rarity";
        
        private SerializedProperty _weightDataProperty;
        private SerializedProperty _cardTypeProperty;
        private SerializedProperty _cardNameProperty;
        private SerializedProperty _artWorkProperty;
        private SerializedProperty _attackProperty;
        private SerializedProperty _exploreProperty;
        private SerializedProperty _hitPointsProperty;
        private SerializedProperty _speedProperty;
        private SerializedProperty _focusProperty;
        private SerializedProperty _upgradeSlotsProperty;
        private SerializedProperty _keywordsProperty;
        private SerializedProperty _cardTextProperty;
        private SerializedProperty _costProperty;
        private SerializedProperty _rarityProperty;

        private void OnEnable()
        {
            _weightDataProperty = serializedObject.FindProperty(WeightDataPropertyName);
            _cardTypeProperty = serializedObject.FindProperty(CardTypePropertyName);
            _cardNameProperty = serializedObject.FindProperty(CardNamePropertyName);
            _artWorkProperty = serializedObject.FindProperty(ArtWorkPropertyName);
            _attackProperty = serializedObject.FindProperty(AttackStatPropertyName);
            _exploreProperty = serializedObject.FindProperty(ExploreStatPropertyName);
            _hitPointsProperty = serializedObject.FindProperty(HitPointsStatPropertyName);
            _speedProperty = serializedObject.FindProperty(SpeedStatPropertyName);
            _focusProperty = serializedObject.FindProperty(FocusStatPropertyName);
            _upgradeSlotsProperty = serializedObject.FindProperty(UpgradeSlotsPropertyName);
            _keywordsProperty = serializedObject.FindProperty(KeywordsPropertyName);
            _cardTextProperty = serializedObject.FindProperty(CardTextPropertyName);
            _costProperty = serializedObject.FindProperty(CostPropertyName);
            _rarityProperty = serializedObject.FindProperty(RarityPropertyName);
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawCardInspectorGUI();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawCardInspectorGUI() 
        {           
            CardSO card = target as CardSO;
            GUILayout.BeginVertical(GUILayout.Width(300));
            EditorGUIUtility.labelWidth = 80;

            DrawOpenCardEditorButton(card);
            DrawOpenCostCalculatorButton(card);
            DrawProperties();

            GUILayout.EndVertical();
        }

        private void DrawOpenCardEditorButton(CardSO card) 
        {
            if (GUILayout.Button("Open in Card Editor")) 
            {
                CardEditorWindow instance = EditorWindow.GetWindow<CardEditorWindow>();
                instance.OpenCardInEditor(card);
            }
        }

        private void DrawOpenCostCalculatorButton(CardSO card)
        {
            if (GUILayout.Button("Open Cost Calculator"))
            {
                CostCalculatorWindow instance = EditorWindow.GetWindow<CostCalculatorWindow>();
                instance.OpenInCostCalculatorWindow(card);
            }
        }
        

        private void DrawProperties() 
        {
            GUI.enabled = false;
            DrawProperty(_weightDataProperty);
            GUI.enabled = true;
            CardRarity cardRarity = (CardRarity) _rarityProperty.enumValueIndex;
            DrawLabel($"Rarity: {cardRarity.GetDescription()}",EditorStyles.boldLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            DrawLabel($"Card Cost: {_costProperty.intValue}",EditorStyles.boldLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            CardTypes cardTypes = (CardTypes)_cardTypeProperty.enumValueIndex;
            DrawLabel($"Card Type: {cardTypes.GetDescription()}", EditorStyles.boldLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            DrawLabel($"Card Name: {_cardNameProperty.stringValue}", EditorStyles.boldLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            DrawArtWorkProperty();
            
            DrawKeywordArrayProperty();

            DrawLabel($"Card Text: {_cardTextProperty.stringValue}", EditorStyles.wordWrappedLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            DrawLabel($"STATS:", EditorStyles.whiteLargeLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            DrawStatsProperties(cardTypes);
        }

        private void DrawArtWorkProperty() 
        {
            if (!(_artWorkProperty.objectReferenceValue is Texture2D artworkTexture)) 
            {
                return;
            }
            GUILayout.Label("Artwork Preview:");
            Rect rect = GUILayoutUtility.GetRect(400 * 0.75f, 225 * 0.75f);
            EditorGUI.DrawPreviewTexture(rect, artworkTexture);
        }

        private void DrawKeywordArrayProperty()
        {
            GUILayout.Label("Keywords:",EditorStyles.boldLabel);
            for (int i = 0; i < _keywordsProperty.arraySize; i++)
            {
                
                SerializedProperty keywordProperty = _keywordsProperty.GetArrayElementAtIndex(i);
                SerializedProperty keywordNameProperty = keywordProperty.FindPropertyRelative("keywordName");
                string keywordName = keywordNameProperty.stringValue;
                if (!string.IsNullOrEmpty(keywordName))
                {
                    DrawLabel(keywordNameProperty.stringValue, EditorStyles.boldLabel);
                }
            }
        }

        private void DrawStatsProperties(CardTypes cardTypes) 
        {
            switch (cardTypes)
            {
                case CardTypes.TBD:
                case CardTypes.Action:
                case CardTypes.Environment:
                    DrawProperty(_exploreProperty);
                    break;
                case CardTypes.Gear_Equipment:
                case CardTypes.Gear_Upgrade:
                default:
                    DrawCommonStatsProperties();
                    if (cardTypes == CardTypes.Character_Hunter)
                    {
                        DrawProperty(_upgradeSlotsProperty);
                    }
                    break;
            }
        }

        private void DrawCommonStatsProperties()
        {
            DrawProperty(_attackProperty);
            DrawProperty(_hitPointsProperty);
            DrawProperty(_speedProperty);
            DrawProperty(_focusProperty);
        }

        private void DrawProperty(SerializedProperty property) 
        {
            EditorGUILayout.PropertyField(property, GUILayout.Height(15), GUILayout.ExpandHeight(true));
        }

        private void DrawLabel(string text, GUIStyle editorStyles, params GUILayoutOption[] options) 
        {
            EditorGUILayout.LabelField(text, editorStyles, options);
        }
    }
}