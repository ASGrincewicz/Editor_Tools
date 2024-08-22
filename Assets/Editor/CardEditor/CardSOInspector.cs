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
        private const string CardTextPropertyName = "_cardText";
        
        private SerializedProperty WeightDataProperty;
        private SerializedProperty CardTypeProperty;
        private SerializedProperty CardNameProperty;
        private SerializedProperty ArtWorkProperty;
        private SerializedProperty AttackProperty;
        private SerializedProperty ExploreProperty;
        private SerializedProperty HitPointsProperty;
        private SerializedProperty SpeedProperty;
        private SerializedProperty FocusProperty;
        private SerializedProperty UpgradeSlotsProperty;
        private SerializedProperty CardTextProperty;

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
            CardTextProperty = serializedObject.FindProperty(CardTextPropertyName);
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawCardEditorGUI();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawCardEditorGUI() 
        {           
            CardSO card = target as CardSO;
            GUILayout.BeginVertical(GUILayout.Width(300));
            EditorGUIUtility.labelWidth = 80;

            DrawOpenButton(card);
            DrawProperties();

            GUILayout.EndVertical();
        }

        private void DrawOpenButton(CardSO card) 
        {
            if (!GUILayout.Button("Open in Card Editor")) 
            {
                return;
            }
            CardEditor instance = EditorWindow.GetWindow<CardEditor>();
            instance.OpenCardInEditor(card);
        }

        private void DrawProperties() 
        {
            DrawProperty(WeightDataProperty);
            CardTypes cardTypes = (CardTypes)CardTypeProperty.enumValueIndex;
            DrawLabel($"Card Type: {cardTypes.GetDescription()}", EditorStyles.boldLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            DrawLabel($"Card Name: {CardNameProperty.stringValue}", EditorStyles.boldLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            DrawArtWorkProperty();

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
            EditorGUILayout.LabelField(artworkTexture.name);
        }

        private void DrawStatsProperties(CardTypes cardTypes) 
        {
            //Debug.Log($"Card Type: {cardTypes.GetDescription()}");
            switch (cardTypes)
            {
                case CardTypes.TBD:
                case CardTypes.Action:
                case CardTypes.Environment:
                    DrawProperty(ExploreProperty);
                    break;
                case CardTypes.Gear_Equipment:
                case CardTypes.Gear_Upgrade:
                    // These cases don't require any properties to be drawn.
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
            EditorGUILayout.PropertyField(property, GUILayout.Height(15));
        }

        private void DrawLabel(string text, GUIStyle editorStyles, params GUILayoutOption[] options) 
        {
            EditorGUILayout.LabelField(text, editorStyles, options);
        }
    }
}