using Editor.CardData;
using Editor.CostCalculator;
using Editor.Utilities;
using UnityEditor;
using UnityEngine;
namespace Editor.CardEditor
{
    [CustomEditor(typeof(CardSO))]
    public class CardSoInspector: UnityEditor.Editor
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
        
        private SerializedProperty WeightDataProperty { get; set; }
        private SerializedProperty CardTypeProperty { get; set; }
        private SerializedProperty CardNameProperty { get; set; }
        private SerializedProperty ArtWorkProperty { get; set; }
        private SerializedProperty AttackProperty { get; set; }
        private SerializedProperty ExploreProperty { get; set; }
        private SerializedProperty HitPointsProperty { get; set; }
        private SerializedProperty SpeedProperty { get; set; }
        private SerializedProperty FocusProperty { get; set; }
        private SerializedProperty UpgradeSlotsProperty { get; set; }
        private SerializedProperty KeywordsProperty { get; set; }
        private SerializedProperty CardTextProperty { get; set; }
        private SerializedProperty CostProperty { get; set; }
        private SerializedProperty RarityProperty { get; set; }
        
        // GUI Properties
        private bool IsOpenInCardEditorButtonPressed => GUILayout.Button("Open in Card Editor");
        private bool IsOpenInCostCalculatorButtonPressed => GUILayout.Button("Open in Cost Calculator");

        private void OnEnable()
        {
            WeightDataProperty = serializedObject.FindProperty(WeightDataPropertyName);
            CardTypeProperty = serializedObject.FindProperty(CardTypePropertyName);
            CardNameProperty = serializedObject.FindProperty(CardNamePropertyName);
            ArtWorkProperty = serializedObject.FindProperty(ArtWorkPropertyName);
            AttackProperty = serializedObject.FindProperty(AttackStatPropertyName);
            ExploreProperty = serializedObject.FindProperty(ExploreStatPropertyName);
            HitPointsProperty = serializedObject.FindProperty(HitPointsStatPropertyName);
            SpeedProperty = serializedObject.FindProperty(SpeedStatPropertyName);
            FocusProperty = serializedObject.FindProperty(FocusStatPropertyName);
            UpgradeSlotsProperty = serializedObject.FindProperty(UpgradeSlotsPropertyName);
            KeywordsProperty = serializedObject.FindProperty(KeywordsPropertyName);
            CardTextProperty = serializedObject.FindProperty(CardTextPropertyName);
            CostProperty = serializedObject.FindProperty(CostPropertyName);
            RarityProperty = serializedObject.FindProperty(RarityPropertyName);
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
            if (IsOpenInCardEditorButtonPressed) 
            {
                CardEditorWindow instance = EditorWindow.GetWindow<CardEditorWindow>();
                instance.OpenCardInEditor(card);
            }
        }

        private void DrawOpenCostCalculatorButton(CardSO card)
        {
            if (IsOpenInCostCalculatorButtonPressed)
            {
                CostCalculatorWindow instance = EditorWindow.GetWindow<CostCalculatorWindow>();
                instance.OpenInCostCalculatorWindow(card);
            }
        }
        

        private void DrawProperties() 
        {
            GUI.enabled = false;
            DrawProperty(WeightDataProperty);
            GUI.enabled = true;
            CardRarity cardRarity = (CardRarity) RarityProperty.enumValueIndex;
            DrawLabel($"Rarity: {cardRarity.GetDescription()}",EditorStyles.boldLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            DrawLabel($"Card Cost: {CostProperty.intValue}",EditorStyles.boldLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            CardTypes cardTypes = (CardTypes)CardTypeProperty.enumValueIndex;
            DrawLabel($"Card Type: {cardTypes.GetDescription()}", EditorStyles.boldLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            DrawLabel($"Card Name: {CardNameProperty.stringValue}", EditorStyles.boldLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            DrawArtWorkProperty();
            
            DrawKeywordArrayProperty();

            DrawLabel($"Card Text: {CardTextProperty.stringValue}", EditorStyles.wordWrappedLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            DrawLabel($"STATS:", EditorStyles.whiteLargeLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            DrawStatsProperties(cardTypes);
        }

        private void DrawArtWorkProperty() 
        {
            if (!(ArtWorkProperty.objectReferenceValue is Texture2D artworkTexture)) 
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
            for (int i = 0; i < KeywordsProperty.arraySize; i++)
            {
                
                SerializedProperty keywordProperty = KeywordsProperty.GetArrayElementAtIndex(i);
                SerializedProperty keywordNameProperty = keywordProperty.FindPropertyRelative("keywordName");
                string keywordName = keywordNameProperty.stringValue;
                if (!string.IsNullOrEmpty(keywordName))
                {
                    DrawLabel(keywordNameProperty.stringValue, EditorStyles.boldLabel);
                }
            }
        }
        
        private void DrawLabel(string text, GUIStyle editorStyles, params GUILayoutOption[] options) 
        {
            EditorGUILayout.LabelField(text, editorStyles, options);
        }

        private void DrawStatsProperties(CardTypes cardTypes) 
        {
            switch (cardTypes)
            {
                case CardTypes.TBD:
                case CardTypes.Action:
                case CardTypes.Environment:
                    DrawProperty(ExploreProperty);
                    break;
                default:
                    DrawCommonStatsProperties();
                    if (cardTypes == CardTypes.Character_Hunter)
                    {
                        DrawProperty(UpgradeSlotsProperty);
                    }
                    break;
            }
        }

        private void DrawCommonStatsProperties()
        {
            DrawProperty(AttackProperty);
            DrawProperty(HitPointsProperty);
            DrawProperty(SpeedProperty);
            DrawProperty(FocusProperty);
        }

        private void DrawProperty(SerializedProperty property) 
        {
            EditorGUILayout.PropertyField(property, GUILayout.Height(15), GUILayout.ExpandHeight(true));
        }
    }
}