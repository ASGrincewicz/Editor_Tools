using Editor.Channels;
using UnityEditor;
using UnityEngine;

namespace Editor.CardData.CardTypes
{
    [CustomEditor(typeof(CardTypeDataSO))]
    public class CardTypeSOInspector : UnityEditor.Editor
    {
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

        private void OnEnable()
        {
            CardTypeNameProperty = serializedObject.FindProperty(CardTypeName);
            CardTypeIconProperty = serializedObject.FindProperty(CardTypeIconName);
            CardTypeColorProperty = serializedObject.FindProperty(CardTypeColorName);
            HasStatsProperty = serializedObject.FindProperty(HasStatsName);
            HasCostProperty = serializedObject.FindProperty(HasCostName);
            HasKeywordsProperty = serializedObject.FindProperty(HasKeywordsName);
            HasCardTextProperty = serializedObject.FindProperty(HasCardTextName);
            CardStatDataProperty = serializedObject.FindProperty(CardStatDataName);
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawCardTypeSOInspectorGUI();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawCardTypeSOInspectorGUI()
        {
            GUI.enabled = true;
            CardTypeDataSO cardTypeSO = (CardTypeDataSO) target;
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            DrawOpenInCardTypeEditorButton(cardTypeSO);
            GUILayout.EndHorizontal();
            GUI.enabled = false;
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
            EditorGUILayout.PropertyField(CardTypeNameProperty);
            DrawArtWorkProperty();
            EditorGUILayout.PropertyField(CardTypeColorProperty);
            EditorGUILayout.PropertyField(HasStatsProperty);
            EditorGUILayout.PropertyField(HasCostProperty);
            EditorGUILayout.PropertyField(HasKeywordsProperty);
            EditorGUILayout.PropertyField(HasCardTextProperty);
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
            if (HasStatsProperty.boolValue)
            {
                GUI.enabled = true;
                DrawCardStatDataProperty();
            }
        }

        private void DrawCardStatDataProperty()
        {
            EditorGUILayout.PropertyField(CardStatDataProperty);
        }
    }
}