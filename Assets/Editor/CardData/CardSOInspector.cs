using Editor.Channels;
using Editor.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Editor.CardData
{
    [CustomEditor(typeof(CardDataSO))]
    public class CardSoInspector: UnityEditor.Editor
    {
        [FormerlySerializedAs("_cardEditorChannel")] [SerializeField] private EditorWindowChannel _editorWindowChannel;
        private const string WeightDataPropertyName = "_weightData";
        private const string CardSetNamePropertyName = "_cardSetName";
        private const string CardNumberPropertyName = "_cardNumber";
        private const string RarityPropertyName = "_rarity";
        private const string CostPropertyName = "_cost";
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
        
        
        
        private SerializedProperty WeightDataProperty { get; set; }
        private SerializedProperty CardSetNameProperty { get; set; }
        private SerializedProperty CardNumberProperty { get; set; }
        private SerializedProperty RarityProperty { get; set; }
        private SerializedProperty CostProperty { get; set; }
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
        
        
        
        // GUI Properties
        private bool IsOpenInCardEditorButtonPressed => GUILayout.Button("Open in Card Editor", EditorStyles.toolbarButton);
        private bool IsOpenInCostCalculatorButtonPressed => GUILayout.Button("Open in Cost Calculator", EditorStyles.toolbarButton);

        private void OnEnable()
        {
            WeightDataProperty = serializedObject.FindProperty(WeightDataPropertyName);
            CardSetNameProperty = serializedObject.FindProperty(CardSetNamePropertyName);
            CardNumberProperty = serializedObject.FindProperty(CardNumberPropertyName);
            RarityProperty = serializedObject.FindProperty(RarityPropertyName);
            CostProperty = serializedObject.FindProperty(CostPropertyName);
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
            GUI.enabled = false;
            DrawProperty(WeightDataProperty);
            GUI.enabled = true;
            DrawLabel($"#{CardNumberProperty.intValue} in {CardSetNameProperty.stringValue}");
            CardRarity cardRarity = (CardRarity) RarityProperty.enumValueIndex;
            DrawLabel($"Rarity: ",$"{cardRarity.GetDescription()}");
            DrawLabel($"Card Cost: ",$"{CostProperty.intValue}");
            CardTypes cardTypes = (CardTypes)CardTypeProperty.enumValueIndex;
            DrawLabel($"Card Type: ", $"{cardTypes.GetDescription()}");

            DrawLabel($"Card Name: ", $"{CardNameProperty.stringValue}");

            DrawArtWorkProperty();
            
            DrawKeywordArrayProperty();

            DrawCardTextLabel();

            DrawLabel($"STATS:");

            DrawStatsProperties(cardTypes);
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
                    DrawLabel("",keywordNameProperty.stringValue);
                }
            }
            GUILayout.EndHorizontal();
        }
        
        private void DrawLabel(string labelText = "", string fieldText = "") 
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.LabelField(labelText,EditorStyles.boldLabel, GUILayout.Width(10), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.LabelField(fieldText,EditorStyles.label, GUILayout.Width(100), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.EndHorizontal();
        }

        private void DrawCardTextLabel()
        {
            EditorGUILayout.LabelField("Card Text: ",EditorStyles.boldLabel, GUILayout.Width(10), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.LabelField($"{CardTextProperty.stringValue}",EditorStyles.wordWrappedLabel, GUILayout.Width(100), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        }

        private void DrawStatsProperties(CardTypes cardTypes) 
        {
            switch (cardTypes)
            {
                case CardTypes.TBD:
                case CardTypes.Action:
                case CardTypes.Starship:
                    break;
                case CardTypes.Environment:
                    DrawProperty(ExploreProperty);
                    break;
                case CardTypes.Boss:
                case CardTypes.Character_Ally:
                case CardTypes.Character_Hunter:
                case CardTypes.Creature:
                case CardTypes.Gear_Equipment:
                case CardTypes.Gear_Upgrade:
                
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