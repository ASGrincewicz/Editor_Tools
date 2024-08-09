using System;
using UnityEditor;
using UnityEngine;

namespace Editor.CardEditor
{
    [CustomEditor(typeof(CardSO))]
    public class CardSOInspector: UnityEditor.Editor
    {
        private const string CardTypePropertyName = "_cardType";
        private const string CardNamePropertyName = "_cardName";
        private const string ArtWorkPropertyName = "_artWork";
        private const string AttackStatPropertyName = "_attack";
        private const string HitPointsStatPropertyName = "_hitPoints";
        private const string SpeedStatPropertyName = "_speed";
        private const string FocusStatPropertyName = "_focus";
        private const string UpgradeSlotsPropertyName = "_upgradeSlots";
        private const string CardTextPropertyName = "_cardText";
        private SerializedProperty CardType;
        private SerializedProperty CardName;
        private SerializedProperty ArtWork;
        private SerializedProperty Attack;
        private SerializedProperty HitPoints;
        private SerializedProperty Speed;
        private SerializedProperty Focus;
        private SerializedProperty UpgradeSlots;
        private SerializedProperty CardText;
        private void OnEnable()
        {
            CardType = serializedObject.FindProperty(CardTypePropertyName);
            CardName = serializedObject.FindProperty(CardNamePropertyName);
            ArtWork = serializedObject.FindProperty(ArtWorkPropertyName);
            Attack = serializedObject.FindProperty(AttackStatPropertyName);
            HitPoints = serializedObject.FindProperty(HitPointsStatPropertyName);
            Speed = serializedObject.FindProperty(SpeedStatPropertyName);
            Focus = serializedObject.FindProperty(FocusStatPropertyName);
            UpgradeSlots = serializedObject.FindProperty(UpgradeSlotsPropertyName);
            CardText = serializedObject.FindProperty(CardTextPropertyName);
        }
        
        public override void OnInspectorGUI()
        {
            CardSO card = (CardSO)target;
            serializedObject.Update();
            GUILayout.BeginVertical(GUILayout.Width(300));
            EditorGUIUtility.labelWidth = 80;
            if (GUILayout.Button("Open in Card Editor"))
            {
                Editor.CardEditor.CardEditor instance = (Editor.CardEditor.CardEditor)EditorWindow.GetWindow(typeof(Editor.CardEditor.CardEditor));
                instance.OpenCardInEditor(card);
            }
            CardTypes cardTypes = (CardTypes)CardType.enumValueIndex;
            EditorGUILayout.LabelField($"Card Type: {(CardTypes)CardType.enumValueIndex}",EditorStyles.boldLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));

            
            EditorGUILayout.LabelField($"Card Name: {CardName.stringValue}",EditorStyles.boldLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
            //ArtWork.objectReferenceValue = EditorGUILayout.ObjectField("Artwork",ArtWork.objectReferenceValue, typeof(Texture2D),false);
            if (ArtWork.objectReferenceValue is Texture2D artworkTexture)
            {
                GUILayout.Label("Artwork Preview:");
                Rect rect = GUILayoutUtility.GetRect(400 * 0.75f,225 * 0.75f);
                EditorGUI.DrawPreviewTexture(rect, artworkTexture);
                EditorGUILayout.LabelField(artworkTexture.name);
            }
            
            EditorGUILayout.LabelField($"Card Text: {CardText.stringValue}",EditorStyles.wordWrappedLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
            EditorGUILayout.LabelField($"STATS:",EditorStyles.whiteLargeLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
            switch (cardTypes)
            {
                case CardTypes.TBD:
                    break;
                case CardTypes.Action:
                    break;
                case CardTypes.Environment:
                    break;
                case CardTypes.Equipment:
                    break;
                case CardTypes.Boss:
                case CardTypes.Character:
                case CardTypes.Creature:
                case CardTypes.Hunter:
                    EditorGUILayout.PropertyField(Attack, GUILayout.Height(15));
                    //EditorGUILayout.Space(10, true);
                    EditorGUILayout.PropertyField(HitPoints, GUILayout.Height(15));
                    //EditorGUILayout.Space(10, true);
                    EditorGUILayout.PropertyField(Speed, GUILayout.Height(15));
                    //EditorGUILayout.Space(10, true);
                    EditorGUILayout.PropertyField(Focus, GUILayout.Height(15));
                    //EditorGUILayout.Space(10, true);
                    if (cardTypes == CardTypes.Hunter)
                    {
                        EditorGUILayout.PropertyField(UpgradeSlots, GUILayout.Height(15));
                        //EditorGUILayout.Space(10, true);
                    }
                    break;
                case CardTypes.Upgrade:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
           
            
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }
    }
}